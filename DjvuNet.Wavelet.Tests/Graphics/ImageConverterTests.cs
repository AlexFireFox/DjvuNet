﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DjvuNet.Graphics;
using DjvuNet.Tests.Xunit;
using Xunit;

namespace DjvuNet.Wavelet.Tests.Graphics
{
    public class ImageConverterTests
    {
        public static IEnumerable<object[]> ReadHeaderData
        {
            get
            {
                List<object[]> retVal = new List<object[]>();

                retVal.Add(new object[] { "JPEG", new byte[] { 0xff, 0xd8, 0xff, 0x00, 0x11, 0x22, 0x56, 0xef } });
                retVal.Add(new object[] { "PNG", new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "GIF v1", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "GIF v2", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "TIFF v1", new byte[] { 0x49, 0x49, 0x2a, 0x00, 0x37, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "TIFF v2", new byte[] { 0x4d, 0x4d, 0x00, 0x2a, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "JPEG 2000", new byte[] { 0xff, 0x4f, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "JPEG XR", new byte[] { 0x57, 0x4d, 0x50, 0x48, 0x4f, 0x54, 0x4f, 0x00, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "OpenEXR", new byte[] { 0x76, 0x2f, 0x31, 0x01, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "BMP", new byte[] { 0x42, 0x4d, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "BA - OS / 2 Bitmap Array", new byte[] { 0x42, 0x41, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "CI - OS / 2 Color Icon", new byte[] { 0x43, 0x49, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "CP - OS / 2 Color Pointer", new byte[] { 0x43, 0x50, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "IC - OS / 2 Icon", new byte[] { 0x49, 0x43, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PT - OS / 2 Pointer", new byte[] { 0x50, 0x54, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "WebP", new byte[] { 0x52, 0x49, 0x46, 0x46, 0x4f, 0x54, 0x4f, 0x00, 0x57, 0x45, 0x42, 0x50, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "ICO | CUR v1", new byte[] { 0x00, 0x00, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "ICO | CUR v2", new byte[] { 0x00, 0x00, 0x02, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PBM P4", new byte[] { 0x50, 0x34, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PBM P1", new byte[] { 0x50, 0x31, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PGM P5", new byte[] { 0x50, 0x35, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PGM P2", new byte[] { 0x50, 0x32, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PPM P6", new byte[] { 0x50, 0x36, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "PPM P3", new byte[] { 0x50, 0x33, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "RLE", new byte[] { 0x52, 0x34, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "BPG", new byte[] { 0x42, 0x50, 0x47, 0xfb, 0x50, 0x33, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "BPG", new byte[] { 0xab, 0x4b, 0x54, 0x58, 0x20, 0x31, 0x31, 0xbb, 0x0d, 0x0a, 0x1a, 0x0a, 0x50, 0x33, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });

                return retVal;
            }
        }

        public static IEnumerable<object[]> ReadHeaderDataWithErrors
        {
            get
            {
                List<object[]> retVal = new List<object[]>();

                retVal.Add(new object[] { "JPEG", new byte[] { 0xff, 0xd8, 0xf1, 0x00, 0x11, 0x22, 0x56, 0xef } });
                retVal.Add(new object[] { "PNG", new byte[] { 0x89, 0x50, 0x4e, 0x41, 0x0d, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "GIF v1", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x31, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "GIF v2", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x32, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "TIFF v1", new byte[] { 0x49, 0x49, 0x2f, 0x00, 0x37, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "TIFF v2", new byte[] { 0x4d, 0x4d, 0x01, 0x2a, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "JPEG 2000", new byte[] { 0xff, 0x4f, 0xff, 0x5e, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "JPEG XR", new byte[] { 0x57, 0x4d, 0x50, 0x49, 0x4f, 0x54, 0x4f, 0x00, 0xff, 0x51, 0xff, 0x52, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "OpenEXR", new byte[] { 0x76, 0x2f, 0x31, 0x02, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "WebP", new byte[] { 0x52, 0x49, 0x46, 0x47, 0x4f, 0x54, 0x4f, 0x00, 0x57, 0x45, 0x42, 0x50, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "ICO | CUR v1", new byte[] { 0x00, 0x00, 0x03, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "ICO | CUR v2", new byte[] { 0x00, 0x00, 0x04, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "BPG", new byte[] { 0x42, 0x50, 0x43, 0xfb, 0x50, 0x33, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "BPG", new byte[] { 0xab, 0x4b, 0x51, 0x58, 0x20, 0x31, 0x31, 0xbb, 0x0d, 0x0a, 0x1a, 0x0a, 0x50, 0x33, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });
                retVal.Add(new object[] { "Unsupported", new byte[] { 0xee, 0xdd, 0x51, 0x58, 0x20, 0x31, 0x31, 0xbb, 0x0d, 0x0a, 0x1a, 0x0a, 0x50, 0x33, 0x01, 0x00, 0x39, 0x0a, 0x1a, 0x0a } });

                return retVal;
            }
        }

        [DjvuTheory]
        [MemberData(nameof(ReadHeaderData))]
        public void ReadHeader_Theory(string fileFormat, byte[] headerBuffer)
        {
            using (MemoryStream ms = new MemoryStream(headerBuffer))
            using (DjvuReader reader = new DjvuReader(ms))
            {
                Assert.Throws<NotImplementedException>(() => ImageConverter.Read(reader));
            }
        }

        [DjvuTheory]
        [MemberData(nameof(ReadHeaderDataWithErrors))]
        public void ReadHeaderWithError_Theory(string fileFormat, byte[] headerBuffer)
        {
            using (MemoryStream ms = new MemoryStream(headerBuffer))
            using (DjvuReader reader = new DjvuReader(ms))
            {
                Assert.Throws<DjvuFormatException>(() => ImageConverter.Read(reader));
            }
        }
    }
}