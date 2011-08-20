using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ludumdare.src
{

    enum OriginPos
    {
        TOP_LEFT = 0,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT,
        CENTER,
    }

    class SpriteBase
    {
        #region Member Variables
        
        Vector2 m_vecPos;
        Vector2 m_vecOrigin;
        Texture2D m_texBase;

        float m_fScale;
        float m_fRotation; // Rotation: IN DEGREES

        public List<Rectangle> m_AABBs;

        #endregion

        #region Accessors

        public float Scale
        {
            get { return m_fScale; }
            set { m_fScale = value; }
        }

        public float Rotation
        {
            get { return m_fRotation; }
            set { m_fRotation = value; }
        }

        public Vector2 Position
        {
            get { return m_vecPos; }
            set { m_vecPos = value; }
        }

        #endregion

        #region Constructors

        public SpriteBase()
        {
            m_vecPos = new Vector2(0, 0);
            m_fScale = 1.0f;
            m_fRotation = 0.0f;

            m_AABBs = new List<Rectangle>();
        }


        public SpriteBase(Vector2 pos)
        {
            m_vecPos = pos;
            m_fScale = 1.0f;
            m_fRotation = 0.0f;

            m_AABBs = new List<Rectangle>();
        }

        #endregion
       

        public void LoadContent(ContentManager contentManager, string texLocation, OriginPos origin )
        {
            m_texBase = contentManager.Load<Texture2D>(texLocation);
            SetOrigin(origin);
            CalculateBoundingBoxes();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_texBase, m_vecPos, null, Color.White, MathHelper.ToRadians(m_fRotation), m_vecOrigin, m_fScale, SpriteEffects.None, 1);
        }


        #region Private Functions
        private void SetOrigin(OriginPos origin)
        {
            switch (origin)
            {
                case OriginPos.TOP_LEFT:
                    m_vecOrigin = new Vector2(0, 0);
                    break;
                case OriginPos.TOP_RIGHT:
                    m_vecOrigin = new Vector2(m_texBase.Width, 0);
                    break;
                case OriginPos.BOTTOM_LEFT:
                    m_vecOrigin = new Vector2(0, m_texBase.Height);
                    break;
                case OriginPos.BOTTOM_RIGHT:
                    m_vecOrigin = new Vector2(m_texBase.Width, m_texBase.Height);
                    break;
                case OriginPos.CENTER:
                    m_vecOrigin = new Vector2((m_texBase.Width / 2), (m_texBase.Height / 2));
                    break;
                default:
                    m_vecOrigin = new Vector2(0, 0); // TOP_LEFT by default
                    break;
            }
        }
        #endregion


        private void CalculateBoundingBoxes()
        {
            Color[,] baseTex2D = TexHelper.Tex2DArray(m_texBase, null);

            int minX = m_texBase.Width;
            int minY = m_texBase.Height;
            int maxX = 0;
            int maxY = 0;

            for (int x = 0; x < m_texBase.Width; x++)
            {
                for (int y = 0; y < m_texBase.Height; y++)
                {
                    if (baseTex2D[x, y].A == 255 )
                    {
                        if (x < minX)
                        {
                            minX = x;    
                        }
                        if (x > maxX)
                        {
                            maxX = x;
                        }
                        if (y < minY)
                        {
                            minY = y;
                        }
                        if (y > maxY)
                        {
                            maxY = y;
                        }
                    }
                }
            }

            Console.WriteLine("Max X: " + maxX + " Max Y: " + maxY + " MinX: " + minX + " MinY: " + minY);
            
            // Calculate middle position
            int midX = ((maxX - minX) / 2) + minX;
            int midY = ((maxY - minY) / 2) + minY;

            Console.WriteLine("MidX: " + midX + " MidY: " + midY);

            Rectangle bbox = new Rectangle(minX, minY, maxX, maxY);

            m_AABBs.Add(bbox);

            Console.WriteLine(bbox.ToString());


        }

    }
}
