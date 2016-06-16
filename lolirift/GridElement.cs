﻿using fun.Core;
using Environment = fun.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace lolirift
{
    public sealed class GridElement : Element
    {
        private GridField[] grid;

        public int Height;
        public int Width;
        public string Heightmap;

        public GridElement(Environment environment, Entity entity) 
            : base(environment, entity)
        {
        }

        public override void Initialize()
        {
            grid = new GridField[Height * Width];

            var hm = new Bitmap(Heightmap);
            var data = hm.LockBits(new Rectangle(0, 0, hm.Width, hm.Height), 
                ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            var length = data.Stride * data.Height;
            var pixels = new byte[length];
            Marshal.Copy(data.Scan0, pixels, 0, length);
            hm.UnlockBits(data);

            for (int i = 0; i < grid.Length; i++)
            {
                grid[i].Height = pixels[i];
            }
        }

        public void Set(UnitElement unit, int x, int y)
        {
            grid[x + Width * y].Unit = unit;
        }

        public GridField Get(int x, int y)
        {
            return grid[x + Width * y];
        }
    }

    public struct GridField
    {
        public UnitElement Unit;
        public byte Height;
    }
}
