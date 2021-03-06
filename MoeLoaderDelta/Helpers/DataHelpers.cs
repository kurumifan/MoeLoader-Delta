﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace MoeLoaderDelta
{
    public class DataHelpers
    {

        /// <summary>
        /// 查找字节数组,失败未找到返回-1
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static int SearchBytes(byte[] bytes, byte[] search)
        {
            try
            {
                var i = bytes.Select((t, index) =>
                new { t = t, index = index }).FirstOrDefault(t =>
                bytes.Skip(t.index).Take(search.Length).SequenceEqual(search)).index;
                return i;
            }
            catch
            {
                return -1;
            }
        }


        /// <summary> 
        /// MemoryStream 保存到文件
        /// </summary> 
        public static void MemoryStreamToFile(MemoryStream stream, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(stream.ToArray());
            bw.Close();
            fs.Close();
        }

        #region image数据保存
        public enum ImageFormat { JPG, BMP, PNG, GIF }
        /// <summary>
        /// 将图片保存到文件
        /// </summary>
        /// <param name="bitmap">BitmapSource</param>
        /// <param name="format">图像类型</param>
        /// <param name="fileName">保存文件名</param>
        public static void ImageToFile(BitmapSource bitmap, ImageFormat format, string fileName)
        {
            BitmapEncoder encoder;

            switch (format)
            {
                case ImageFormat.JPG:
                    encoder = new JpegBitmapEncoder();
                    break;
                case ImageFormat.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
                case ImageFormat.BMP:
                    encoder = new BmpBitmapEncoder();
                    break;
                case ImageFormat.GIF:
                    encoder = new GifBitmapEncoder();
                    break;
                default:
                    throw new InvalidOperationException();
            }

            FileStream fs = new FileStream(fileName, FileMode.Create);
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(fs);
            fs.Dispose();
            fs.Close();
        }

        /// <summary>
        /// 将图片保存到文件
        /// </summary>
        /// <param name="bitmap">BitmapSource</param>
        /// <param name="format">图像类型</param>
        /// <param name="fileName">保存文件名</param>
        public static void ImageToFile(BitmapSource bitmap, string format, string fileName)
        {
            ImageFormat ifo = ImageFormat.JPG;
            switch(format)
            {
                case "jpg":
                    break;
                case "png":
                    ifo = ImageFormat.PNG;
                    break;
                case "bmp":
                    ifo = ImageFormat.BMP;
                    break;
                case "gif":
                    ifo = ImageFormat.GIF;
                    break;
                default:
                    throw new Exception("ImageFormat incorrect type");
            }
            ImageToFile(bitmap, ifo, fileName);
        }
        #endregion
    }
}
