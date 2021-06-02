using System;
using QRCodeImageReader = ThoughtWorks.QRCode.Codec.Reader.QRCodeImageReader;
namespace ThoughtWorks.QRCode.Geom
{
    /// <summary> This class designed to move target point based on independent axis.
    /// It allows move target coodinate on rotated, scaled and gauche QR Code image
    /// </summary>
    public class Axis
    {

        internal int _sin, _cos;
        internal int _modulePitch;
        internal Point _origin;

        virtual public Point Origin
        {
            set
            {
                this._origin = value;
            }

        }
        virtual public int ModulePitch
        {
            set
            {
                this._modulePitch = value;
            }

        }

        public Axis(int[] angle, int modulePitch)
        {
            this._sin = angle[0];
            this._cos = angle[1];
            this._modulePitch = modulePitch;
            this._origin = new Point();
        }

        public virtual Point translate(Point offset)
        {
            int moveX = offset.X;
            int moveY = offset.Y;
            return this.translate(moveX, moveY);
        }

        public virtual Point translate(Point origin, Point offset)
        {
            Origin = origin;
            int moveX = offset.X;
            int moveY = offset.Y;
            return this.translate(moveX, moveY);
        }

        public virtual Point translate(Point origin, int moveX, int moveY)
        {
            Origin = origin;
            return this.translate(moveX, moveY);
        }

        public virtual Point translate(Point origin, int modulePitch, int moveX, int moveY)
        {
            Origin = origin;
            this._modulePitch = modulePitch;
            return this.translate(moveX, moveY);
        }

        public virtual Point translate(int moveX, int moveY)
        {
            long dp = QRCodeImageReader.DECIMAL_POINT;
            Point point = new Point();
            int dx = (moveX == 0) ? 0 : (_modulePitch * moveX) >> (int)dp;
            int dy = (moveY == 0) ? 0 : (_modulePitch * moveY) >> (int)dp;
            point.translate((dx * _cos - dy * _sin) >> (int)dp, (dx * _sin + dy * _cos) >> (int)dp);
            point.translate(_origin.X, _origin.Y);
            return point;
        }
    }
}