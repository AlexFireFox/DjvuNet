﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DjvuNet.DataChunks.Enums;
using DjvuNet.DataChunks.Navigation;
using DjvuNet.DataChunks.Navigation.Interfaces;

namespace DjvuNet.DataChunks
{

    /// <summary>
    /// CIDa chunk was supported till version 3.23 - 2002 July.
    /// Function is unknown and all it's content is skipped.
    /// </summary>
    public class CidaChunk : IFFChunk
    {
        #region Private Variables

        #endregion Private Variables

        #region Public Properties

        #region ChunkType

        public override ChunkType ChunkType
        {
            get { return ChunkType.Cida; }
        }

        #endregion ChunkType

        #endregion Public Properties

        #region Constructors

        public CidaChunk(DjvuReader reader, IFFChunk parent, DjvuDocument document,
            string chunkID = "", long length = 0)
            : base(reader, parent, document, chunkID, length)
        {
        }

        #endregion Constructors

        #region Protected Methods

        /// <summary>
        /// Skip the data bytes per IFF specification
        /// </summary>
        /// <param name="reader"></param>
        protected override void ReadChunkData(DjvuReader reader)
        {
            reader.Position += Length;
        }

        #endregion ProtectedMethods
    }
}