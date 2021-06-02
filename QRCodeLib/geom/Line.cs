using System;
using QRCodeUtility = ThoughtWorks.QRCode.Codec.Util.QRCodeUtility;

namespace ThoughtWorks.QRCode.Geom
{	
	public class Line
	{
        internal int _x1, _y1, _x2, _y2;

		virtual public bool Horizontal
		{
			get
			{
				if (_y1 == _y2)
					return true;
				else
					return false;
			}
			
		}
		virtual public bool Vertical
		{
			get
			{
				if (_x1 == _x2)
					return true;
				else
					return false;
			}
			
		}
		virtual public Point Center
		{
			get
			{
				int x = (_x1 + _x2) / 2;
				int y = (_y1 + _y2) / 2;
				return new Point(x, y);
			}
			
		}
		virtual public int Length
		{
			get
			{
				int x = System.Math.Abs(_x2 - _x1);
				int y = System.Math.Abs(_y2 - _y1);
				int r = QRCodeUtility.sqrt(x * x + y * y);
				return r;
			}
			
		}		
		
		public Line()
		{
			_x1 = _y1 = _x2 = _y2 = 0;
		}

		public Line(int x1, int y1, int x2, int y2)
		{
			this._x1 = x1;
			this._y1 = y1;
			this._x2 = x2;
			this._y2 = y2;
		}
		public Line(Point p1, Point p2)
		{
			_x1 = p1.X;
			_y1 = p1.Y;
			_x2 = p2.X;
			_y2 = p2.Y;
		}
		public virtual Point getP1()
		{
			return new Point(_x1, _y1);
		}
		
		public virtual Point getP2()
		{
			return new Point(_x2, _y2);
		}
		
		public virtual void  setLine(int x1, int y1, int x2, int y2)
		{
			this._x1 = x1;
			this._y1 = y1;
			this._x2 = x2;
			this._y2 = y2;
		}
		public virtual void  setP1(Point p1)
		{
			_x1 = p1.X;
			_y1 = p1.Y;
		}
		public virtual void  setP1(int x1, int y1)
		{
			this._x1 = x1;
			this._y1 = y1;
		}
		public virtual void  setP2(Point p2)
		{
			_x2 = p2.X;
			_y2 = p2.Y;
		}
		public virtual void  setP2(int x2, int y2)
		{
			this._x2 = x2;
			this._y2 = y2;
		}
		
		public virtual void  translate(int dx, int dy)
		{
			this._x1 += dx;
			this._y1 += dy;
			this._x2 += dx;
			this._y2 += dy;
		}
		
		//check if two lines are neighboring. allow only 1 dot difference 
		public static bool isNeighbor(Line line1, Line line2)
		{
			if ((System.Math.Abs(line1.getP1().X - line2.getP1().X) < 2 && System.Math.Abs(line1.getP1().Y - line2.getP1().Y) < 2) && (System.Math.Abs(line1.getP2().X - line2.getP2().X) < 2 && System.Math.Abs(line1.getP2().Y - line2.getP2().Y) < 2))
				return true;
			else
				return false;
		}
		
		public static bool isCross(Line line1, Line line2)
		{
			if (line1.Horizontal && line2.Vertical)
			{
				if (line1.getP1().Y > line2.getP1().Y && line1.getP1().Y < line2.getP2().Y && line2.getP1().X > line1.getP1().X && line2.getP1().X < line1.getP2().X)
					return true;
			}
			else if (line1.Vertical && line2.Horizontal)
			{
				if (line1.getP1().X > line2.getP1().X && line1.getP1().X < line2.getP2().X && line2.getP1().Y > line1.getP1().Y && line2.getP1().Y < line1.getP2().Y)
					return true;
			}
			
			return false;
		}
		public static Line getLongest(Line[] lines)
		{
			Line longestLine = new Line();
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Length > longestLine.Length)
				{
					longestLine = lines[i];
				}
			}
			return longestLine;
		}
		public override string ToString()
		{
			return "(" + System.Convert.ToString(_x1) + "," + System.Convert.ToString(_y1) + ")-(" + System.Convert.ToString(_x2) + "," + System.Convert.ToString(_y2) + ")";
		}
	}
}