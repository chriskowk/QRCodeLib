using System;
using QRCodeUtility = ThoughtWorks.QRCode.Codec.Util.QRCodeUtility;

namespace ThoughtWorks.QRCode.Geom
{  
	public class Point
	{
        public const int RIGHT = 1;
        public const int BOTTOM = 2;
        public const int LEFT = 4;
        public const int TOP = 8;

        internal int _x;
        internal int _y;
	

		virtual public int X
		{
			get
			{
				return _x;
			}
			
			set
			{
				this._x = value;
			}
			
		}
		virtual public int Y
		{
			get
			{
				return _y;
			}
			
			set
			{
				this._y = value;
			}
			
		}
		
		
		public Point()
		{
			_x = 0;
			_y = 0;
		}
		public Point(int x, int y)
		{
			this._x = x;
			this._y = y;
		}
		
		public virtual void  translate(int dx, int dy)
		{
			this._x += dx;
			this._y += dy;
		}
		
		public virtual void  set_Renamed(int x, int y)
		{
			this._x = x;
			this._y = y;
		}
		
		public override string ToString()
		{
			return "(" + System.Convert.ToString(_x) + "," + System.Convert.ToString(_y) + ")";
		}
		
		public static Point getCenter(Point p1, Point p2)
		{
			return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
		}
		
		public bool equals(Point compare)
		{
			if (_x == compare._x && _y == compare._y)
				return true;
			else
				return false;
		}
		
		public virtual int distanceOf(Point other)
		{
			int x2 = other.X;
			int y2 = other.Y;
			return QRCodeUtility.sqrt((_x - x2) * (_x - x2) + (_y - y2) * (_y - y2));
		}
	}
}