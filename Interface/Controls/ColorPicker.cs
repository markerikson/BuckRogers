using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Azuria.Controls.ColorPicker
{

	/// <summary>Contrôle permettant de sélectionner une couleur parmi toute une collection</summary>
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
		/// <summary>Contient la collection de couleurs à afficher</summary>
		private ColorCollection m_ColorCollection;
	#endregion Variables

	#region Construction
		/// <summary>Constructeur par défaut</summary>
		public ColorPicker() {
			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.DropDownStyle = ComboBoxStyle.DropDownList;
		}
		/// <summary>Nettoyage des ressources utilisées - managées et non managées</summary>
		/// <param name="disposing">Préciser false pour libérer uniquement les ressources non managées</param>
		protected override void Dispose(bool disposing) {
			if(disposing) {
			}
			base.Dispose(disposing);
		}
	#endregion Construction

	#region Propriétés
		/// <summary>Obtient ou définit la collection de couleurs à afficher</summary>
		/// <remarks>Masque la collection Items de l'objet parent ComboBox</remarks>
		public new ColorCollection Items {
			get { return m_ColorCollection; }
			set {
				if(m_ColorCollection != value && value != null ) {
					m_ColorCollection = value;
					foreach(Color color in value) base.Items.Add(color.Name);
					// Redessiner le contrôle
					Refresh();
				}
			}
		}
		/// <summary>Obtient ou définit le nom de la couleur sélectionnée</summary>
		/// <remarks>Masque la propriété SelectedText de l'objet parent ComboBox</remarks>
		public new string SelectedText {
			get { return Items[SelectedIndex].Name; }
			set {
				int selidx = Items.IndexOf(value);
				if(selidx > 0) SelectedIndex = selidx;
			}
		}
	#endregion Propriétés

	#region Méthodes
	#endregion Méthodes

	#region Evènements
		/// <summary>Appelée en cas de modification de l'apparence visuelle du Picker, redessine un item</summary>
		/// <param name="e">Contient les paramètres de l'évènement nécessaires au dessin d'un item</param>
		protected override void OnDrawItem(DrawItemEventArgs e) {
			Graphics Grphcs = e.Graphics;
			Color BlockColor = Color.Empty;
			int left = RECTCOLOR_LEFT;
			// Dessiner l'arrière-plan de l'item en fonction de son état
			if(e.State == DrawItemState.Selected || e.State == DrawItemState.None) e.DrawBackground();
			// Récupérer la couleur à afficher
			if(e.Index == -1) BlockColor = SelectedIndex < 0 ? BackColor : Color.FromName(SelectedText);
			else BlockColor = Color.FromName((string)base.Items[e.Index]);
			// Peindre le rectangle représentant la couleur
			Grphcs.FillRectangle(new SolidBrush(BlockColor),left,e.Bounds.Top+RECTCOLOR_TOP,RECTCOLOR_WIDTH,ItemHeight - 2 * RECTCOLOR_TOP);
			// Dessiner un cadre noir autour du rectangle
			Grphcs.DrawRectangle(Pens.Black,left,e.Bounds.Top+RECTCOLOR_TOP,RECTCOLOR_WIDTH,ItemHeight - 2 * RECTCOLOR_TOP);
			// Dessiner le nom de la couleur
			Grphcs.DrawString(BlockColor.Name,e.Font,new SolidBrush(ForeColor),new Rectangle(RECTTEXT_LEFT,e.Bounds.Top,e.Bounds.Width-RECTTEXT_LEFT,ItemHeight));
			// Appeller la méthode de base
			base.OnDrawItem(e);
		}

		/// <summary>Appelée lorsque la propriété DropDownStyle a été modifiée</summary>
		/// <param name="e">Contient les paramètres de l'évènement nécessaires</param>
		/// <remarks>Cette surcharge garantit que la propriété DropDownStylle restera à DropDownList</remarks>
		protected override void OnDropDownStyleChanged(EventArgs e) {
			if(this.DropDownStyle != ComboBoxStyle.DropDownList) this.DropDownStyle = ComboBoxStyle.DropDownList;
		}
	#endregion Evènements

	}
}
