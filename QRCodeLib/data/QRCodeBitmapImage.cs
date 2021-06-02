using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ThoughtWorks.QRCode.Codec.Data
{
    public class QRCodeBitmapImage : QRCodeImage
    {
        Bitmap _image;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image">Bitmap image/param>
        public QRCodeBitmapImage(Bitmap image)
        {
            this._image = image;
        }

        virtual public int Width
        {
            get
            {
                return _image.Width;
            }

        }
        virtual public int Height
        {
            get
            {
                return _image.Height;
            }

        }


        public virtual int getPixel(int x, int y)
        {
            return _image.GetPixel(x, y).ToArgb();
        }
    }
}
