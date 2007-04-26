using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Azuria.Controls.ColorPicker
{

	/// <summary>Contr�le permettant de s�lectionner une couleur parmi toute une collection</summary>
	public class ColorPicker : ComboBox
	{

	#region Constantes
		private const int RECTCOLOR_LEFT = 4;
		private const int RECTCOLOR_TOP = 2;
		private const int RECTCOLOR_WIDTH = 40;
		private const int RECTTEXT_MARGIN = 10;
		private const int RECTTEXT_LEFT = RECTCOLOR_LEFT + RECTCOLOR_WIDTH + RECTTEXT_MARGIN;
	#endregion Constantes

	#region Variables
		/// <summary>Contient la collection de couleurs � afficher</summary>
		private ColorCollection m_ColorCollection;
	#endregion Variables

	#region Construction
		/// <summary>Constructeur par d�faut</summary>
		public ColorPicker() {
			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.DropDownStyle = ComboBoxStyle.DropDownList;
		}
		/// <summary>Nettoyage des ressources utilis�es - manag�es et non manag�es</summary>
		/// <param name="disposing">Pr�ciser false pour lib�rer uniquement les ressources non manag�es</param>
		protected override void Dispose(bool disposing) {
			if(disposing) {
			}
			base.Dispose(disposing);
		}
	#endregion Construction

	#region Propri�t�s
		/// <summary>Obtient ou d�finit la collection de couleurs � afficher</summary>
		/// <remarks>Masque la collection Items de l'objet parent ComboBox</remarks>
		public new ColorCollection Items {
			get { return m_ColorCollection; }
			set {
				if(m_ColorCollection != value && value != null ) {
					m_ColorCollection = value;
					foreach(Color color in value) base.Items.Add(color.Name);
					// Redessiner le contr�le
					Refresh();
				}
			}
		}
		/// <summary>Obtient ou d�finit le nom de la couleur s�lectionn�e</summary>
		/// <remarks>Masque la propri�t� SelectedText de l'objet parent ComboBox</remarks>
		public new string SelectedText {
			get { return Items[SelectedIndex].Name; }
			set {
				int selidx = Items.IndexOf(value);
				if(selidx > 0) SelectedIndex = selidx;
			}
		}
	#endregion Propri�t�s

	#region M�thodes
	#endregion M�thodes

	#region Ev�nements
		/// <summary>Appel�e en cas de modification de l'apparence visuelle du Picker, redessine un item</summary>
		/// <param name="e">Contient les param�tres de l'�v�nement n�cessaires au dessin d'un item</param>
		protected override void OnDrawItem(DrawItemEventArgs e) {
			Graphics Grphcs = e.Graphics;
			Color BlockColor = Color.Empty;
			int left = RECTCOLOR_LEFT;
			// Dessiner l'arri�re-plan de l'item en fonction de son �tat
			if(e.State == DrawItemState.Selected || e.State == DrawItemState.None) e.DrawBackground();
			// R�cup�rer la couleur � afficher
			if(e.Index == -1) BlockColor = SelectedIndex < 0 ? BackColor : Color.FromName(SelectedText);
			else BlockColor = Color.FromName((string)base.Items[e.Index]);
			// Peindre le rectangle repr�sentant la couleur
			Grphcs.FillRectangle(new SolidBrush(BlockColor),left,e.Bounds.Top+RECTCOLOR_TOP,RECTCOLOR_WIDTH,ItemHeight - 2 * RECTCOLOR_TOP);
			// Dessiner un cadre noir autour du rectangle
			Grphcs.DrawRectangle(Pens.Black,left,e.Bounds.Top+RECTCOLOR_TOP,RECTCOLOR_WIDTH,ItemHeight - 2 * RECTCOLOR_TOP);
			// Dessiner le nom de la couleur
			Grphcs.DrawString(BlockColor.Name,e.Font,new SolidBrush(ForeColor),new Rectangle(RECTTEXT_LEFT,e.Bounds.Top,e.Bounds.Width-RECTTEXT_LEFT,ItemHeight));
			// Appeller la m�thode de base
			base.OnDrawItem(e);
		}

		/// <summary>Appel�e lorsque la propri�t� DropDownStyle a �t� modifi�e</summary>
		/// <param name="e">Contient les param�tres de l'�v�nement n�cessaires</param>
		/// <remarks>Cette surcharge garantit que la propri�t� DropDownStylle restera � DropDownList</remarks>
		protected override void OnDropDownStyleChanged(EventArgs e) {
			if(this.DropDownStyle != ComboBoxStyle.DropDownList) this.DropDownStyle = ComboBoxStyle.DropDownList;
		}
	#endregion Ev�nements

	}
}
