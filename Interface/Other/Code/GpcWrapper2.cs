using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace GpcWrapper
{
	public struct Vertex
	{
		public double X;
		public double Y;
		
		public Vertex( double x, double y )
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return "(" + X.ToString() + "," + Y.ToString() + ")";
		}
	}

	public class VertexList
	{
		public int          NofVertices;
		public Vertex[]     Vertex;
	
		public VertexList()
		{
		}
		
		public VertexList( PointF[] p )
		{
			NofVertices = p.Length;
			Vertex = new Vertex[NofVertices];
			for ( int i=0 ; i<p.Length ; i++ )
				Vertex[i] = new Vertex( (double)p[i].X, (double)p[i].Y );
		}

		public GraphicsPath ToGraphicsPath()
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddLines( ToPoints() );
			return graphicsPath;
		}
		
		public PointF[] ToPoints()
		{
			PointF[] vertexArray = new PointF[NofVertices];
			for ( int i=0 ; i<NofVertices ; i++ ) 
			{
				vertexArray[i] = new PointF( (float)Vertex[i].X, (float)Vertex[i].Y );
			}
			return vertexArray;
		}

		public override string ToString()
		{
			string s = "Polygon with " + NofVertices + " vertices: ";
			
			for ( int i=0 ; i<NofVertices ; i++ ) 
			{
				s += Vertex[i].ToString();
				if ( i!=NofVertices-1 )
					s += ",";
			}
			return s;
		}
	}

	public class Polygon
	{
		public int          NofContours;
		public bool[]       ContourIsHole;
		public VertexList[] Contour;

		public Polygon()
		{
		}

		// path should contain only polylines ( use Flatten )
		// furthermore the constructor assumes that all Subpathes of path except the first one are holes
		public Polygon( GraphicsPath path )  
		{
			NofContours = 0;
			foreach ( byte b in path.PathTypes ) 
			{
				if ( ( b&((byte)PathPointType.CloseSubpath) ) != 0 )
					NofContours++;
			}
			
			ContourIsHole = new bool[NofContours];
			Contour       = new VertexList[NofContours];
			for ( int i=0 ; i<NofContours ; i++ )
				ContourIsHole[i] = (i==0);

			int contourNr = 0;
			ArrayList contour = new ArrayList();
			for ( int i=0 ; i<path.PathPoints.Length ; i++ ) 
			{
				contour.Add( path.PathPoints[i] );
				if ( ( path.PathTypes[i]&((byte)PathPointType.CloseSubpath) ) != 0 ) 
				{
					PointF[] pointArray = (PointF[])contour.ToArray(typeof(PointF)); 
					VertexList vl = new VertexList( pointArray );
					Contour[contourNr++] = vl;
					contour.Clear();
				}				
			}
		}

		
		public void AddContour( VertexList contour, bool contourIsHole )
		{
			bool[]       hole = new bool[NofContours+1];
			VertexList[] cont = new VertexList[NofContours+1];
			
			for ( int i=0 ; i<NofContours ; i++ ) 
			{
				hole[i] = ContourIsHole[i];
				cont[i] = Contour[i];
			}
			hole[NofContours]   = contourIsHole;
			cont[NofContours++] = contour;
			
			ContourIsHole = hole;
			Contour       = cont;
		}

		public GraphicsPath ToGraphicsPath()
		{
			GraphicsPath path = new GraphicsPath();
			
			for ( int i=0 ; i<NofContours ; i++ ) 
			{
				PointF[] points = Contour[i].ToPoints();
				if ( ContourIsHole[i] )
					Array.Reverse( points );
				path.AddPolygon( points );
			}
			return path;
		}

		public override string ToString()
		{
			string s = "Polygon with " + NofContours.ToString() + " contours." + "\r\n";
			for ( int i=0 ; i<NofContours ; i++ ) 
			{
				if ( ContourIsHole[i] )
					s += "Hole: ";
				else
					s += "Contour: ";
				s += Contour[i].ToString();
			}
			return s;
		}

	}
}