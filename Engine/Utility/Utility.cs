using System;
using System.Collections;
using CenterSpace.Free;

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
	}
}
