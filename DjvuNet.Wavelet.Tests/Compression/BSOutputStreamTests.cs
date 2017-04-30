﻿using Xunit;
using DjvuNet.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DjvuNet.Tests;
using DjvuNet.Tests.Xunit;

namespace DjvuNet.Compression.Tests
{
    public class BSOutputStreamTests
    {

        public static IEnumerable<object[]> WriteTestData
        {
            get
            {
                List<object[]> retVal = new List<object[]>();

                string[] files = Directory.GetFiles(Util.ArtifactsPath, "*.json");
                foreach(string f in files)
                {
                    string fileName = Path.GetFileName(f);
                    if (fileName == "test003C.json" || fileName == "test032C.json" || fileName == "test049C.json")
                        continue;

                    retVal.Add(new object[] 
                    {
                        fileName,
                        f,
                        Path.Combine(Util.ArtifactsDataPath, Path.GetFileName(f) + ".bz3")
                    });
                }

                return retVal;
            }
        }

        [Fact()]
        public void BSOutputStreamTest001()
        {
            using(BSOutputStream outStream = new BSOutputStream())
            {
                Assert.NotNull(outStream.BaseStream);
                Assert.IsType<MemoryStream>(outStream.BaseStream);
                Assert.True(outStream.CanWrite);
                Assert.False(outStream.CanRead);
                Assert.False(outStream.CanSeek);
            }
        }

        [Fact()]
        public void BSOutputStreamTest002()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BSOutputStream outStream = new BSOutputStream(stream))
            {
                Assert.NotNull(outStream.BaseStream);
                Assert.IsType<MemoryStream>(outStream.BaseStream);
                Assert.Same(stream, outStream.BaseStream);
                Assert.True(outStream.CanWrite);
                Assert.False(outStream.CanRead);
                Assert.False(outStream.CanSeek);
            }
        }

        [Fact()]
        public void BSOutputStreamTest003()
        {
            using (MemoryStream stream = new MemoryStream())
            Assert.Throws<ArgumentException>("blockSize", () => new BSOutputStream(stream, 8192));
        }

        [Fact(Skip = "Not implemented"), Trait("Category", "Skip")]
        public void CloseTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact(Skip = "Not implemented"), Trait("Category", "Skip")]
        public void EncodeRawTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact(Skip = "Not implemented"), Trait("Category", "Skip")]
        public void EncodeBinaryTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact(Skip = "Not implemented"), Trait("Category", "Skip")]
        public void EncodeTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void InitTest001()
        {
            byte[] buffer = new byte[16];
            using (MemoryStream stream = new MemoryStream(buffer, false))
                Assert.Throws<ArgumentException>("dataStream", () => new BSOutputStream(stream).Init(stream));
        }

        [Fact()]
        public void InitTest002()
        {
            byte[] buffer = new byte[16];
            using (MemoryStream stream = new MemoryStream(buffer))
            using (BSOutputStream outStream = (BSOutputStream) new BSOutputStream().Init(stream))
            {
                Assert.NotNull(outStream.BaseStream);
                Assert.IsType<MemoryStream>(outStream.BaseStream);
                Assert.Same(stream, outStream.BaseStream);
                Assert.True(outStream.CanWrite);
                Assert.False(outStream.CanRead);
                Assert.False(outStream.CanSeek);
            }
        }

        [Fact(Skip = "Not implemented"), Trait("Category", "Skip")]
        public void FlushTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void WriteTest001()
        {
            UTF8Encoding encoding = new UTF8Encoding(false);
            string filePath = Path.Combine(Util.ArtifactsDataPath, "testhello.obz");
            byte[] buffer = Util.ReadFileToEnd(filePath);
            filePath = filePath.Replace(".obz", ".bz3");

            string testText = "Hello bzz! \r\n";
            string sourceText = new UTF8Encoding(false).GetString(buffer);
            Assert.Equal(testText, sourceText);

            byte[] data = new byte[buffer.Length + 32];
            Buffer.BlockCopy(buffer, 0, data, 0, buffer.Length);
            long bytesWritten = 0;

            using (Stream stream = new FileStream(
                filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (BSOutputStream writer = new BSOutputStream(stream, 4096))
            {
                writer.Write(data, 0, buffer.Length);
                bytesWritten = stream.Position;
            }

            byte[] testBuffer = Util.ReadFileToEnd(filePath);

            using (MemoryStream readStream = new MemoryStream(testBuffer))
            using (BzzReader reader = new BzzReader(new BSInputStream(readStream)))
            {

                string testResult = reader.ReadUTF8String(testText.Length);
                Assert.False(String.IsNullOrWhiteSpace(testResult));
                Assert.Equal(testText, testResult);
            }
        }

        [Fact()]
        public void WriteTest002()
        {
            string filePath = Path.Combine(Util.ArtifactsDataPath, "DjvuNet.pdb");
            string outFilePath = filePath + ".bz3";

            try
            {
                byte[] buffer = Util.ReadFileToEnd(filePath);

                byte[] data = new byte[buffer.Length + 32];
                Buffer.BlockCopy(buffer, 0, data, 0, buffer.Length);
                long bytesWritten = 0;

                using (Stream stream = new FileStream(
                    outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (BSOutputStream writer = new BSOutputStream(stream, 4096))
                {
                    writer.Write(data, 0, buffer.Length);
                    bytesWritten = stream.Position;
                }

                byte[] testBuffer = Util.ReadFileToEnd(outFilePath);

                using (MemoryStream readStream = new MemoryStream(testBuffer))
                using (BzzReader reader = new BzzReader(new BSInputStream(readStream)))
                {

                    byte[] testResult = reader.ReadBytes(buffer.Length);
                    Assert.NotNull(testResult);
                    Assert.Equal(buffer.Length, testResult.Length);
                    for (int i = 0; i < buffer.Length; i++)
                        Assert.Equal(buffer[i], testResult[i]);
                }
            }
            finally
            {
                if (File.Exists(outFilePath))
                    File.Delete(outFilePath);
            }
        }

        [DjvuTheory]
        [MemberData(nameof(WriteTestData))]
        public void Write_Theory(string fileName, string filePath, string outFilePath)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding(false);
                byte[] buffer = Util.ReadFileToEnd(filePath);

                string sourceText = new UTF8Encoding(false).GetString(buffer);

                byte[] data = new byte[buffer.Length + 32];
                Buffer.BlockCopy(buffer, 0, data, 0, buffer.Length);
                long bytesWritten = 0;

                using (Stream stream = new FileStream(
                    outFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (BSOutputStream writer = new BSOutputStream(stream, 4096))
                {
                    writer.Write(data, 0, buffer.Length);
                    bytesWritten = stream.Position;
                }

                byte[] testBuffer = Util.ReadFileToEnd(outFilePath);

                using (MemoryStream readStream = new MemoryStream(testBuffer))
                using (BzzReader reader = new BzzReader(new BSInputStream(readStream)))
                {

                    string testResult = reader.ReadUTF8String(sourceText.Length);
                    Assert.False(String.IsNullOrWhiteSpace(testResult));
                    Assert.Equal(sourceText, testResult);
                }
            }
            finally
            {
                if (File.Exists(outFilePath))
                    File.Delete(outFilePath);
            }
        }
    }
}