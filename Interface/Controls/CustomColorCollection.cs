using System;
using System.Drawing;
using System.Collections;

namespace Azuria.Controls.ColorPicker
{
	/// <summary>Description r�sum�e de CustomColorCollection</summary>
	public class CustomColorCollection : ColorCollection
	{
		/// <summary>Liste contenant les objets de la collection</summary>
		private ArrayList m_Array;

		/// <summary>Constructeur unique</summary>
		public CustomColorCollection() {
			m_Array = new ArrayList();
		}

		/// <summary>Impl�mente la fonction GetEnumerator de l'interface ColorCollection</summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() {
			return m_Array.GetEnumerator();
		}
		/// <summary>Obtient le nombre d'objets <see cref="Color"/>Color de la collection</summary>
		public int Count {
			get { return m_Array.Count; }
		}
		/// <summary>Retourne l'objet couleur d�termin� par son index dans la collection</summary>
		public Color this[int iColor] {
			get { return (Color) m_Array[iColor]; }
		}
		/// <summary>Retourne l'objet couleur d�termin� par son nom</summary>
		public Color this[string szColor] {
			get { 
				if(szColor.Length == 0) throw new ArgumentNullException();
				return Color.FromName(szColor);
			}
		}
		/// <summary>Ajoute un objet Color � la collection</summary>
		/// <param name="color">Objet KnownColor � ajouter</param>
		public void Add(Color color) {
			m_Array.Add(color);
		}
		/// <summary>Supprime un objet Color de la collection</summary>
		/// <param name="color">Objet KnownColor � supprimer</param>
		public void Remove(Color color)  {
			m_Array.Remove(color);
		}
		/// <summary>Supprime un objet Color de la collection</summary>
		/// <param name="index">Rang dans la collection de l'objet Color � supprimer</param>
		public void RemoveAt(int index)  {
			if(index < 0 || index >= m_Array.Count) throw new ArgumentOutOfRangeException();
			m_Array.RemoveAt(index);
		}

		/// <summary>Obtient le rang dans la collection d'une couleur de la collection</summary>
		/// <param name="ColorName">Nom de la couleur</param>
		/// <returns>Retourne le rang si la couleur se trouve dans la collection et -1 sinon</returns>
		public int IndexOf(string ColorName) {
			return m_Array.IndexOf(ColorName);
		}

	}
}
