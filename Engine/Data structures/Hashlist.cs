using System;
using System.Collections;

namespace BuckRogers
{
	/// <summary>
	/// 
	/// </summary>
	public /*abstract*/ class Hashlist : IEnumerable, IDictionary
	{
		//array list that contains all the keys 
		//as they are inserted, the index is associated with
		//a key so when pulling out values by index
		//we can get the key for the index, pull from the 
		//hashtable the proper value with the corresponding 
		//key
		//This is basically the same as a sorted list but
		//does not sort the items, rather it leaves them in the
		//order they were inserted - like a list
		/// <summary>
		/// 
		/// </summary>
		protected ArrayList m_oKeys = new ArrayList();
		/// <summary>
		/// 
		/// </summary>
		protected Hashtable m_oValues = new Hashtable();		

		#region ICollection implementation
		//ICollection implementation
		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get{return m_oValues.Count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get{return m_oValues.IsSynchronized;}
		}
		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get{return m_oValues.SyncRoot;}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oArray"></param>
		/// <param name="iArrayIndex"></param>
		public void CopyTo(System.Array oArray, int iArrayIndex)
		{
			m_oValues.CopyTo(oArray, iArrayIndex);
		}
		#endregion

		#region IDictionary implementation
		//IDictionary implementation
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oKey"></param>
		/// <param name="oValue"></param>
		public void Add(object oKey, object oValue)
		{
			m_oKeys.Add(oKey);
			m_oValues.Add(oKey, oValue);
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize
		{
			get{return m_oKeys.IsFixedSize;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly
		{
			get{return m_oKeys.IsReadOnly;}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICollection Keys
		{
			get{return m_oValues.Keys;}
		}
		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			m_oValues.Clear();
			m_oKeys.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oKey"></param>
		/// <returns></returns>
		public bool Contains(object oKey)
		{
			return m_oValues.Contains(oKey);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oKey"></param>
		/// <returns></returns>
		public bool ContainsKey(object oKey)
		{
			return m_oValues.ContainsKey(oKey);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return m_oValues.GetEnumerator();
		}	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oKey"></param>
		public void Remove(object oKey)
		{
			m_oValues.Remove(oKey);
			m_oKeys.Remove(oKey);
		}

		public void Remove(int idx)
		{
			object oKey = m_oKeys[idx];

			m_oValues.Remove(oKey);
			m_oKeys.Remove(oKey);
		}
		/// <summary>
		/// 
		/// </summary>
		public object this[object oKey]
		{
			get{return m_oValues[oKey];}
			set
			{
				if(!m_oValues.ContainsKey(value))
				{
					m_oKeys.Add(value);
				}
				m_oValues[oKey] = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICollection Values
		{
			get{return m_oValues.Values;}
		}
		#endregion

		#region IEnumerable implementation

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			 //return ((IEnumerable)m_oValues).GetEnumerator();
			return new HashlistEnumerator(this);
		}
		
		#endregion
		
		#region Hashlist specialized implementation
		//specialized indexer routines
		/// <summary>
		/// 
		/// </summary>
		public object this[string Key]
		{
			get{return m_oValues[Key];}
		}
		/// <summary>
		/// 
		/// </summary>
		public object this[int Index]
		{
			get{return m_oValues[m_oKeys[Index]];}
		}
		#endregion


	}

	// Declare the enumerator and implement the IEnumerator 
	// and IDisposable interfaces
	public class HashlistEnumerator: IEnumerator, IDisposable
	{
		int nIndex;
		Hashlist collection;
		public HashlistEnumerator(Hashlist coll) 
		{
			collection = coll;
			nIndex = -1;
		}

		public void Reset() 
		{
			nIndex = -1;
		}

		public bool MoveNext()
		{
			nIndex++;
			return (nIndex < collection.Count);
		}

		public object Current 
		{
			get 
			{
				object o = collection[nIndex];
				return o;
			}
		}

		// The current property on the IEnumerator interface:
		object IEnumerator.Current 
		{
			get 
			{
				return (Current);
			}
		}
   
		public void Dispose()
		{
			collection = null;
		}
	}

	/*
	class PlanetCollection : Hashlist
	{
		public new Planet this[string Key]
		{
			get{return (Planet)base[Key];}
		}        

		public new Planet this[int Index]
		{
			get
			{
				object oTemp = base[Index];
				return (Planet)oTemp;
			}
		}
		public PlanetCollection()
		{
                    
		}
	}
	*/

}