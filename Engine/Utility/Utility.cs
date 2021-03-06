using System;
using System.Collections;
using CenterSpace.Free;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for Utility.
	/// </summary>
	public class Utility
	{

		public Utility()
		{
		}

		static Random rng = new Random();
		private static MersenneTwister m_twister = new MersenneTwister();
		private static MersenneTwister m_twister2 = new MersenneTwister();

		public static void RandomizeList(IList list)
		{
			for (int i=list.Count-1; i > 0; i--)

			{
				int swapIndex = rng.Next(i+1);
				if (swapIndex != i)
				{
					object tmp = list[swapIndex];
					list[swapIndex] = list[i];
					list[i] = tmp;
				}
			}
		}

		public static ArrayList RandomizeArrayList(ArrayList original)
		{
			ArrayList rand = new ArrayList();
			while(original.Count > 0)
			{
				Object o = original[m_twister.Next(original.Count - 1)];
				rand.Add(o);
				original.Remove(o);
			}

			return rand;

		}

		public static int RollD10()
		{
			return m_twister.Next(1, 10);
		}

		public static MersenneTwister Twister
		{
			get
			{
				return m_twister;
			}
		}

		public static float GetSin(float degAngle)
		{
			return (float) Math.Sin(Math.PI * degAngle / 180);
		}

		public static float GetCos(float degAngle)
		{
			return (float) Math.Cos(Math.PI * degAngle / 180);
		}

		public static CenterSpace.Free.MersenneTwister Twister2
		{
			get { return m_twister2; }
			set { m_twister2 = value; }
		}

		/*
        public static string GetEnumValueDescription(object value)
        {
            string result= string.Empty;

            if (value != null)
            {
                result=value.ToString();
                // Get the type from the object.
                Type type = value.GetType();

                try
                {
                    result=Enum.GetName(type,value);
                    // Get the member on the type that corresponds to the value passed in.
                    FieldInfo fieldInfo = type.GetField(result);
                    // Now get the attribute on the field.
                    object[] attributeArray=fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute),false);
                    DescriptionAttribute attribute=null;

                    if (attributeArray.Length > 0)
                    {
                        attribute = (DescriptionAttribute)attributeArray[0];
                    }
                    if (attribute!=null)
                    {
                        result=attribute.Description;
                    }
                }
                catch (ArgumentNullException)
                {
                    //We shouldn't ever get here, but it means that value was null, so we'll just go with the default.
                    result = string.Empty;
                }
                catch (ArgumentException)
                {
                    //we didn't have an enum.
                    result=value.ToString();
                }
            }
            // Return the description.
            return result;
        }
		*/


        public static string GetDescriptionOf(Enum enumType) 
        {
            // By default, the result is just ToString with
            // a space in front of each capital letter

            Regex capitalLetterMatch = new Regex("\\B[A-Z]", RegexOptions.Compiled);
            string enumDescription = capitalLetterMatch.Replace(enumType.ToString(), " $&");  
            MemberInfo[] memberInfo = enumType.GetType().GetMember(enumType.ToString());                     

            if (memberInfo != null && memberInfo.Length == 1) 
            {
                object[] customAttributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (customAttributes.Length == 1) 
                {
                    enumDescription = ((DescriptionAttribute)customAttributes[0]).Description;
                }
            }

            return enumDescription;

        }

		public static Enum GetEnumFromDescription(string description, Type enumType)
		{
			Hashtable ht = new Hashtable();
			Enum result = null;

			foreach (Enum e in Enum.GetValues(enumType))
			{
				string enumDescription = GetDescriptionOf(e);
				
				if(description.Equals(enumDescription))
				{
					result = e;
					break;
				}
			}

			return result;
		}

		public static DateTime RetrieveLinkerTimestamp()
		{
			string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;
			byte[] b = new byte[2048];
			System.IO.Stream s = null;

			try
			{
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
				{
					s.Close();
				}
			}

			int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
			return dt;
		}

		public static int MakeLong(int LoWord, int HiWord)
		{
			return (HiWord << 16) | (LoWord & 0xffff);
		}

		public static int HiWord(int Number)
		{
			return (Number >> 16) & 0xffff;
		}

		public static int LoWord(int Number)
		{
			return Number & 0xffff;
		}


		public static int MakeReallyLong(int loWord, int hiWord)
		{
			return (hiWord << 8) | (loWord & 0xff);
		}

		public static int High24Bits(int Number)
		{
			return (Number >> 8) & 0xffffff;
		}

		public static int Low8Bits(int Number)
		{
			return Number & 0xff;
		}
    }
}

