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
            Color[] tex1D = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(0, sourceRect, tex1D, 0, tex1D.Length);

            // 2D texture array
            Color[,] tex2D = new Color[tex.Width, tex.Height];

            for (int x = 0; x < tex.Width; x++)
            {
                for (int y = 0; y < tex.Height; y++)
                {
                    tex2D[x, y] = tex1D[x + y * tex.Width];
                }
            }

            return tex2D;
        }

    }
}
