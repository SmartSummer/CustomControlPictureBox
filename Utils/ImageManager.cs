using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace CustomPictureBox
{
    public class ImageManager
    {

        public static Bitmap GetModelBMP(Bitmap srcBmp, Point start, Point endPoint)
        {

            ////start.X = 0;
            ////start.Y = 0;
            ////endPoint.X = 189;
            ////endPoint.Y = 189;

            int width = endPoint.X - start.X + 1;
            int height = endPoint.Y - start.Y + 1;

            byte[] buffer = GetBufferFromPoint(srcBmp, start, endPoint);

            Bitmap des = new Bitmap(width, height, PixelFormat.Format8bppIndexed);


            Rectangle rect = new Rectangle(0, 0, width, height);

            BitmapData bd = des.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            int stride = bd.Stride;  // 扫描线的宽度  
            int offset = stride - bd.Width;  // 显示宽度与扫描线宽度的间隙  
            IntPtr iptr = bd.Scan0;  // 获取bmpData的内存起始位置  
            int scanBytes = stride * bd.Height;// 用stride宽度，表示这是内存区域的大小  
            byte[] desBuffer = new byte[stride * bd.Height];
            for (int j = 0; j < height; j++)
            {

                for (int i = 0; i < stride; i++)
                {

                    if (i < width)
                    {

                        desBuffer[j * stride + i] = buffer[j * width + i];
                    }
                }
            }


            Marshal.Copy(desBuffer, 0, iptr, scanBytes);

            des.UnlockBits(bd);

            Bitmap monoBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            ColorPalette myMonoColorPalette = monoBitmap.Palette;

            for (int i = 0; i < 256; i++)
                myMonoColorPalette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);

            des.Palette = myMonoColorPalette;

            return des;
        }


        public static int WIDTH(int width, int bitcount)
        {

            int nwBytePerLine;
            nwBytePerLine = ((width) * (bitcount) + 31) / 32 * 4;
            return nwBytePerLine;
        }


        public static byte[] GetBufferFromPoint(Bitmap srcBmp, Point start, Point endPoint)
        {


            int width = endPoint.X - start.X + 1;
            int height = endPoint.Y - start.Y + 1;

            Rectangle rect = new Rectangle(0, 0, srcBmp.Width, srcBmp.Height);

            BitmapData bmData = srcBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            int strid = bmData.Stride;
            byte[] buffer = new byte[srcBmp.Width * srcBmp.Height];

            Marshal.Copy(bmData.Scan0, buffer, 0, buffer.Length);

            srcBmp.UnlockBits(bmData);


            int count = 0;
            byte[] newBuffer = new byte[width * height];
            for (int hei = start.Y; hei < endPoint.Y + 1; hei++)
                for (int wid = start.X; wid < endPoint.X + 1; wid++)
                {
                    int index = wid + hei * strid;

                    newBuffer[count] = buffer[index];
                    count++;
                }

            return newBuffer;
        }
    }

}
