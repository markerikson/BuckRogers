using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Nodes;
using UMD.HCIL.Piccolo.Util;
using UMD.HCIL.PiccoloX;
using UMD.HCIL.PiccoloX.Nodes;
using UMD.HCIL.PiccoloX.Components;

using BuckRogers;
using BuckRogers.Interface;

namespace BuckRogers
{
	/// <summary>
	/// Summary description for BuckRogersForm.
	/// </summary>
	public class BuckRogersForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button m_btnZoomIn;
		private System.Windows.Forms.Button m_btnZoomOut;
		private System.Windows.Forms.Button m_btnDefaultZoom;

		private MapControl m_map;
		private MovePanel m_movePanel;
		private System.Windows.Forms.Button m_btnCenterCamera;
		private System.Windows.Forms.ComboBox m_cbCenterLocations;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage m_pgAction;
		private System.Windows.Forms.TabPage m_pgTerritory;
		private TerritoryPanel territoryPanel1;
		private BuckRogers.Interface.MovePanel movePanel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private float[] m_zoomFactors;


		public BuckRogersForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//m_movePanel = new MovePanel();

			m_zoomFactors = new float[]{0.1f, 0.175f, 0.25f, 0.5f, 0.75f, 1.0f, 1.5f, 2.0f, 3.0f, 4.0f, 5.0f};
																		   

			//this.Size = new Size(1024, 730);
			m_map = new MapControl();

			m_map.Size = new Size(this.ClientSize.Width - tabControl1.Right - 4, this.ClientSize.Height - 30);

			//this.SuspendLayout();

			m_map.Location = new Point(tabControl1.Right + 4, 0);
			m_map.ScrollControl.Size = m_map.ClientSize;
			m_map.Canvas.Size = m_map.ClientSize;
			this.Controls.Add(m_map);

			m_map.Anchor = 
				AnchorStyles.Bottom |
				AnchorStyles.Top |
				AnchorStyles.Left |
				AnchorStyles.Right;

			m_map.Canvas.Camera.ViewScale = 0.25f;

			Size viewSize = m_map.ScrollControl.ViewSize;

			int centerX = (int)(viewSize.Width / 2);
			int centerY = 0;//(int)(viewSize.Height / 2);

			//Point center = new Point(, );

			Point ulCorner = new Point(centerX, centerY);
			ulCorner.X -= this.Width /  2;
			//ulCorner.Y -= this.Height / 2;

			m_map.ScrollControl.ViewPosition = ulCorner;

			m_cbCenterLocations.SelectedIndex = 0;

			//this.ResumeLayout(false);
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BuckRogersForm));
			this.m_btnZoomIn = new System.Windows.Forms.Button();
			this.m_btnZoomOut = new System.Windows.Forms.Button();
			this.m_btnDefaultZoom = new System.Windows.Forms.Button();
			this.m_btnCenterCamera = new System.Windows.Forms.Button();
			this.m_cbCenterLocations = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.m_pgAction = new System.Windows.Forms.TabPage();
			this.movePanel1 = new BuckRogers.Interface.MovePanel();
			this.m_pgTerritory = new System.Windows.Forms.TabPage();
			this.territoryPanel1 = new BuckRogers.Interface.TerritoryPanel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tabControl1.SuspendLayout();
			this.m_pgAction.SuspendLayout();
			this.m_pgTerritory.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_btnZoomIn
			// 
			this.m_btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnZoomIn.Location = new System.Drawing.Point(424, 672);
			this.m_btnZoomIn.Name = "m_btnZoomIn";
			this.m_btnZoomIn.TabIndex = 0;
			this.m_btnZoomIn.Text = "Zoom In";
			this.m_btnZoomIn.Click += new System.EventHandler(this.m_btnZoomIn_Click);
			// 
			// m_btnZoomOut
			// 
			this.m_btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnZoomOut.Location = new System.Drawing.Point(504, 672);
			this.m_btnZoomOut.Name = "m_btnZoomOut";
			this.m_btnZoomOut.TabIndex = 1;
			this.m_btnZoomOut.Text = "Zoom Out";
			this.m_btnZoomOut.Click += new System.EventHandler(this.m_btnZoomOut_Click);
			// 
			// m_btnDefaultZoom
			// 
			this.m_btnDefaultZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnDefaultZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_btnDefaultZoom.Location = new System.Drawing.Point(584, 672);
			this.m_btnDefaultZoom.Name = "m_btnDefaultZoom";
			this.m_btnDefaultZoom.Size = new System.Drawing.Size(80, 23);
			this.m_btnDefaultZoom.TabIndex = 2;
			this.m_btnDefaultZoom.Text = "Default Zoom";
			this.m_btnDefaultZoom.Click += new System.EventHandler(this.m_btnDefaultZoom_Click);
			// 
			// m_btnCenterCamera
			// 
			this.m_btnCenterCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnCenterCamera.Location = new System.Drawing.Point(920, 672);
			this.m_btnCenterCamera.Name = "m_btnCenterCamera";
			this.m_btnCenterCamera.TabIndex = 3;
			this.m_btnCenterCamera.Text = "Center";
			this.m_btnCenterCamera.Click += new System.EventHandler(this.m_btnCenterCamera_Click);
			// 
			// m_cbCenterLocations
			// 
			this.m_cbCenterLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_cbCenterLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cbCenterLocations.Items.AddRange(new object[] {
																	 "Sun",
																	 "Mercury",
																	 "Venus",
																	 "Earth",
																	 "Mars",
																	 "Asteroids"});
			this.m_cbCenterLocations.Location = new System.Drawing.Point(792, 672);
			this.m_cbCenterLocations.Name = "m_cbCenterLocations";
			this.m_cbCenterLocations.Size = new System.Drawing.Size(121, 21);
			this.m_cbCenterLocations.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(688, 676);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Center camera on:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(276, 672);
			this.button1.Name = "button1";
			this.button1.TabIndex = 6;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.m_pgAction);
			this.tabControl1.Controls.Add(this.m_pgTerritory);
			this.tabControl1.Location = new System.Drawing.Point(4, 160);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(240, 536);
			this.tabControl1.TabIndex = 7;
			// 
			// m_pgAction
			// 
			this.m_pgAction.Controls.Add(this.movePanel1);
			this.m_pgAction.Location = new System.Drawing.Point(4, 22);
			this.m_pgAction.Name = "m_pgAction";
			this.m_pgAction.Size = new System.Drawing.Size(232, 510);
			this.m_pgAction.TabIndex = 0;
			this.m_pgAction.Text = "Actions";
			// 
			// movePanel1
			// 
			this.movePanel1.Location = new System.Drawing.Point(0, 0);
			this.movePanel1.Name = "movePanel1";
			this.movePanel1.Size = new System.Drawing.Size(240, 400);
			this.movePanel1.TabIndex = 0;
			// 
			// m_pgTerritory
			// 
			this.m_pgTerritory.Controls.Add(this.territoryPanel1);
			this.m_pgTerritory.Location = new System.Drawing.Point(4, 22);
			this.m_pgTerritory.Name = "m_pgTerritory";
			this.m_pgTerritory.Size = new System.Drawing.Size(232, 510);
			this.m_pgTerritory.TabIndex = 1;
			this.m_pgTerritory.Text = "Territory";
			// 
			// territoryPanel1
			// 
			this.territoryPanel1.Location = new System.Drawing.Point(0, 0);
			this.territoryPanel1.Name = "territoryPanel1";
			this.territoryPanel1.Size = new System.Drawing.Size(240, 400);
			this.territoryPanel1.TabIndex = 0;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(4, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(240, 144);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// BuckRogersForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 696);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_cbCenterLocations);
			this.Controls.Add(this.m_btnCenterCamera);
			this.Controls.Add(this.m_btnDefaultZoom);
			this.Controls.Add(this.m_btnZoomOut);
			this.Controls.Add(this.m_btnZoomIn);
			this.Name = "BuckRogersForm";
			this.Text = "BuckRogersForm";
			this.Load += new System.EventHandler(this.BuckRogersForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.m_pgAction.ResumeLayout(false);
			this.m_pgTerritory.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new BuckRogersForm());
		}

		private void m_btnZoomIn_Click(object sender, System.EventArgs e)
		{
			float currentZoom = m_map.Canvas.Camera.ViewScale;
			Point originalPosition = m_map.ScrollControl.ViewPosition;
			Size originalSize = m_map.ScrollControl.ViewSize;

			float previousZoom = 0.0f;
			float newZoom = 0.0f;
			bool foundZoom = false;

			foreach(float zoom in m_zoomFactors)
			{
				if(zoom > currentZoom)
				{
					newZoom = zoom;
					foundZoom = true;
					break;
				}
				previousZoom = zoom;
			}

			if(foundZoom)
			{
				m_map.Canvas.Camera.ViewScale = newZoom;
			}
			else
			{
				//m_map.Canvas.Camera.ViewScale += 1.0f;
			}
		
			CenterZoomedMap(originalPosition, originalSize);
		}

		private void m_btnZoomOut_Click(object sender, System.EventArgs e)
		{
			Point originalPosition = m_map.ScrollControl.ViewPosition;
			Size originalSize = m_map.ScrollControl.ViewSize;
			float currentZoom = m_map.Canvas.Camera.ViewScale;

			float previousZoom = 0.0f;
			float newZoom = 0.0f;
			bool foundZoom = false;

			
			for(int i = m_zoomFactors.Length - 1; i >= 0; i--)
			{
				float zoom = m_zoomFactors[i];

				if(currentZoom > zoom)
				{			
					newZoom = zoom;
					foundZoom = true;
					break;
				}
			}
			
/*
			foreach(float zoom in m_zoomFactors)
			{
				if(currentZoom > zoom)
				{
					newZoom = zoom;
					foundZoom = true;
					break;
				}
				previousZoom = zoom;
			}
*/
			if(!foundZoom)
			{
				//newZoom -= 1.0f;
				newZoom = currentZoom;
			}
			/*
			else
			{
				m_map.Canvas.Camera.ViewScale -= 1.0f;
			}
			*/

			m_map.Canvas.Camera.ViewScale = newZoom;
			
			CenterZoomedMap(originalPosition, originalSize);

			//StringBuilder sb = new StringBuilder();
			//sb.AppendFormat("{0}:{1} = {2}:{3}", originalPosition.X, originalSize.Width, newXPos, newSize.Width);

			//MessageBox.Show(sb.ToString());
		}

		private void CenterZoomedMap(Point originalPosition, Size originalSize)
		{
			Size mapSize = m_map.ClientSize;
			
			float oldCenterX = originalPosition.X  + (float)(mapSize.Width / 2);
			float oldXPercent = (float)(oldCenterX / (float)originalSize.Width);
			//(originalPosition.X  + mapSize.Width / 2)/ (float)originalSize.Width;
			float oldCenterY = originalPosition.Y + (float)(mapSize.Height / 2);
			float oldYPercent = (float)(oldCenterY / (float)originalSize.Height);
			//float oldYPercent = (float)(originalPosition.Y + mapSize.Height / 2)/ (float)originalSize.Height;

			Size newSize = m_map.ScrollControl.ViewSize;

			int newCenterX = (int)(oldXPercent * newSize.Width);
			int newXPos = newCenterX - (mapSize.Width / 2);
			int newCenterY = (int)(oldYPercent * newSize.Height);
			int newYPos = newCenterY - (mapSize.Height / 2);
			//int newXPos = (int)(oldXPercent * newSize.Width) - (mapSize.Width / 2);
			//int newYPos = (int)(oldYPercent * newSize.Height) - (mapSize.Height / 2);

			
			//newXPos += (int)((mapSize.Width / 2) * oldXPercent);
			//newYPos += (int)((mapSize.Height / 2) * oldYPercent);

			m_map.ScrollControl.ViewPosition = new Point(newXPos, newYPos);
			//m_map.ScrollControl.UpdateScrollbars();

			Point newPosition = m_map.ScrollControl.ViewPosition;
		}

		private void m_btnDefaultZoom_Click(object sender, System.EventArgs e)
		{
			m_map.Canvas.Camera.ViewScale = 0.25f;
		}

		private void m_btnCenterCamera_Click(object sender, System.EventArgs e)
		{
			//MessageBox.Show(m_map.ScrollControl.ViewPosition.ToString());
			//return;

			string target = (string)m_cbCenterLocations.SelectedItem;

			PointF targetLocation = new Point();
			Size viewSize = m_map.ScrollControl.ViewSize;
			float zoom = m_map.Canvas.Camera.ViewScale;
			float originalWidth = viewSize.Width / zoom;
			float originalHeight = viewSize.Height / zoom;

			switch(target)
			{
				case "Sun":
				{
					targetLocation.X = 0.52f * viewSize.Width;
					targetLocation.Y = 0.34f * viewSize.Height;//0.0f;
					break;
				}
				case "Mercury":
				{
					targetLocation.X = viewSize.Width * 0.085f;//0;//-2180;
					targetLocation.Y = viewSize.Height * 0.222f;//-380;
					break;
				}
				case "Venus":
				{
					break;
				}
				case "Earth":
				{
					break;
				}
				case "Mars":
				{
					break;
				}
				case "Asteroids":
				{
					break;
				}
			}

			


			//targetLocation.X *= zoom;
			//targetLocation.Y *= zoom;

			Size mapSize = m_map.ClientSize;
			targetLocation.X -= mapSize.Width / 2;
			targetLocation.Y -= mapSize.Height / 2;

			Point ulCorner = new Point((int)targetLocation.X, (int)targetLocation.Y);

			//PointF transformedPoint = m_map.Canvas.Camera.ViewToLocal(ulCorner);
			//ulCorner.X -= m_map.Width /  2;
			//ulCorner.Y -= m_map.Height / 2;

			m_map.ScrollControl.ViewPosition = ulCorner;// new Point((int)transformedPoint.X, (int)transformedPoint.Y);

		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			m_map.DrawScreenshot();
		}

		private void BuckRogersForm_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}