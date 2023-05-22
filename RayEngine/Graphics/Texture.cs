﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayEngine.Graphics
{
    public abstract class Texture : IDisposable
    {
        private string Filepath { get; init; }

        public Texture(string filepath)
        {
            Filepath = filepath;
        }

        public abstract int GetWidth();
        public abstract int GetHeight();

        public abstract void Dispose();
    }
}
