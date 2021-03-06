﻿using Xunit;
using DjvuNet.DataChunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace DjvuNet.DataChunks.Tests
{
    public class DjbzChunkTests
    {
        [Fact()]
        public void DjbzChunkTest()
        {
            Mock<IDjvuReader> readerMock = new Mock<IDjvuReader>();
            readerMock.Setup(x => x.Position).Returns(1024);

            DjbzChunk unk = new DjbzChunk(readerMock.Object, null, null, null, 0);
            Assert.Equal<ChunkType>(ChunkType.Djbz, unk.ChunkType);
            Assert.Equal(ChunkType.Djbz.ToString(), unk.Name);
            Assert.Equal<long>(1024, unk.DataOffset);
        }

        [Fact()]
        public void ShapeDictionaryTest()
        {
            Mock<IDjvuReader> readerMock = new Mock<IDjvuReader>();
            readerMock.Setup(x => x.Position).Returns(1024);

            DjbzChunk unk = new DjbzChunk(readerMock.Object, null, null, null, 0);
            Assert.Equal<ChunkType>(ChunkType.Djbz, unk.ChunkType);
            Assert.Equal(ChunkType.Djbz.ToString(), unk.Name);
            Assert.Equal<long>(1024, unk.DataOffset);

            JB2.JB2Dictionary dictionary = new JB2.JB2Dictionary();
            unk.ShapeDictionary = dictionary;
            Assert.NotNull(unk.ShapeDictionary);
            Assert.Same(dictionary, unk.ShapeDictionary);
        }
    }
}
