﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace MobileCms.Common
{
    public class ImageHelp
    {
        public void CreateCheckImage(string checkCode)
        {
            if ((checkCode != null) && !(checkCode.Trim() == string.Empty))
            {
                //Bitmap image = new Bitmap((int) Math.Ceiling((double) (checkCode.Length * 12.5)), 0x16);
                //Bitmap image = new Bitmap((int)Math.Ceiling((double)(checkCode.Length * 12.5)), 0x20);
                Bitmap image = new Bitmap((int)Math.Ceiling((double)(checkCode.Length * 18)), 0x20);
                Graphics g = Graphics.FromImage(image);
                try
                {
                    int i;
                    Random random = new Random();
                    g.Clear(Color.White);
                    for (i = 0; i < 0x19; i++)
                    {
                        int x1 = random.Next(image.Width);
                        int x2 = random.Next(image.Width);
                        int y1 = random.Next(image.Height);
                        int y2 = random.Next(image.Height);
                        //g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                        g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                    }
                    //Font font = new Font("Arial", 12f, FontStyle.Italic | FontStyle.Bold);
                    Font font = new Font("Arial", 15f, FontStyle.Italic | FontStyle.Bold);
                    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                    //g.DrawString(checkCode, font, brush, (float) 2f, (float) 2f);
                    g.DrawString(checkCode, font, brush, (float)12f, (float)7f);
                    for (i = 0; i < 100; i++)
                    {
                        int x = random.Next(image.Width);
                        int y = random.Next(image.Height);
                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }
                    g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Gif);
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.ContentType = "image/Gif";
                    HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                }
                finally
                {
                    g.Dispose();
                    image.Dispose();
                }
            }
        }
    }
}
