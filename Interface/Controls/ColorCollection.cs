using System;
using System.Drawing;
using System.Collections;

namespace Azuria.Controls.ColorPicker
{
	/// <summary>D�claration de l'interface IColorCollection</summary>
	public interface ColorCollection : IEnumerable
	{
		/// <summary>Obtient le nombre d'objets <see cref="Color"/>Color de la collection</summary>
		int Count { get; }
		/// <summary>Retourne l'objet couleur situ� � l'emplacement sp�cifi� dans la collection</summary>
		Color this[int i] { get; }
		/// <summary>Retourne l'objet couleur d�termin� par son nom</summary>
		Color this[string s] { get; }
		/// <summary>Obtient l'�num�rateur associ� � la collection</summary>
		/// <returns>Retourne un objet de type <seealso cref="IEnumerator"/>IEnumerator</see></returns>
		new IEnumerator GetEnumerator();
		/// <summary>Obtient l'index de la couleur sp�cifi�e dans la collection</summary>
		int IndexOf(string ColorName);
	}
}
