using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace ludumdare.src
{
    public static class TexHelper
    {

        public static Color[,] Tex2DArray(Texture2D tex, Nullable<Rectangle> sourceRect )
        {
            // Create 1D array to hold all Color values in texture
            // Calculate proper 1D size
            int tex1DSize = sourceRect.Value.Width * sourceRect.Value.Height;

            Color[] tex1D = new Color[tex1DSize];
            tex.GetData<Color>(0, sourceRect, tex1D, 0, tex1D.Length);

            // 2D texture array
            Color[,] tex2D = new Color[sourceRect.Value.Width, sourceRect.Value.Height];

            for (int x = 0; x < sourceRect.Value.Width; x++)
            {
                for (int y = 0; y < sourceRect.Value.Height; y++)
                {
                    tex2D[x, y] = tex1D[x + y * sourceRect.Value.Width];
                }
            }

            return tex2D;
        }

    }
}
