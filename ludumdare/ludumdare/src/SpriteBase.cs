using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ludumdare.src
{
    #region enum
    enum OriginPos
    {
        TOP_LEFT = 0,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT,
        CENTER,
    }
    #endregion
  
    class SpriteBase
    {
        #region Member Variables
        
        Vector2 m_vecPos;
        Vector2 m_vecOrigin;
        Texture2D m_texBase;

        float m_fScale;
        float m_fRotation; // Rotation: IN DEGREES

        int m_iCurrFrame;
        int m_iMaxFrames;
        int m_iFrameWidth;

        float m_fFrameTime;
        float m_fTimeAccumulator;

        Rectangle m_frameRect;
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

        public int CurrentFrame
        {
            get { return m_iCurrFrame; }
        }

        #endregion

        #region Constructors

        public SpriteBase(Vector2 pos, int frameWidth, int frameRate )
        {
            m_vecPos = pos;
            m_fScale = 1.0f;
            m_fRotation = 0.0f;

            m_iCurrFrame = 0;
            m_iFrameWidth = frameWidth;

            m_fFrameTime = (float)(1.0f / frameRate);
            
            m_AABBs = new List<Rectangle>();
        }

        #endregion

        #region LoadContent
        public void LoadContent(ContentManager contentManager, string texLocation, OriginPos origin)
        {
            m_texBase = contentManager.Load<Texture2D>(texLocation);
            SetOrigin(origin);

            m_iMaxFrames = m_texBase.Width / m_iFrameWidth;

            // Construct initial frame Rect
            m_frameRect = new Rectangle(0, 0, m_iFrameWidth, m_texBase.Height);

            CalculateBoundingBoxes();
        }
        #endregion


        public void Update( GameTime gameTime )
        {  
            m_fTimeAccumulator += (gameTime.ElapsedGameTime.Milliseconds / 100.0f);

            if (m_fTimeAccumulator > m_fFrameTime )
            {
                // Increment frame counter (but loop around when max frames are reached)
                m_iCurrFrame = (m_iCurrFrame + 1) % m_iMaxFrames;

                m_frameRect.X = m_iCurrFrame * m_iFrameWidth;
                m_frameRect.Y = 0;

                m_frameRect.Width = m_iFrameWidth;
                m_frameRect.Height = m_texBase.Height;
                
              
                m_fTimeAccumulator = 0.0f;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, Nullable<Vector2> overridePos )
        {
            if (overridePos != null)
            {
                spriteBatch.Draw(m_texBase, overridePos.Value, m_frameRect, Color.White, MathHelper.ToRadians(m_fRotation), m_vecOrigin, m_fScale, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(m_texBase, m_vecPos, m_frameRect, Color.White, MathHelper.ToRadians(m_fRotation), m_vecOrigin, m_fScale, SpriteEffects.None, 1);
            }
            
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
            //Color[,] baseTex2D = TexHelper.Tex2DArray(m_texBase, null);

            Rectangle currentFrameRect = new Rectangle();

            // Calculate AABBs for each frame of the animation
            for (int x = 0; x < m_iMaxFrames; x++)
            {
                // Calculate rect for the the frame # x
                currentFrameRect.X = x * m_iFrameWidth;
                currentFrameRect.Y = 0;

                currentFrameRect.Width = m_iFrameWidth;
                currentFrameRect.Height = m_texBase.Height;

                // Now grab the 2d texture array
                Color[,] baseTex2D = TexHelper.Tex2DArray(m_texBase, currentFrameRect);

                int minX = m_texBase.Width;
                int minY = m_texBase.Height;
                int maxX = 0;
                int maxY = 0;

                for (int i = 0; i < currentFrameRect.Width; i++)
                {
                    for (int j = 0; j < currentFrameRect.Height; j++)
                    {
                        if (baseTex2D[i, j].A == 255)
                        {
                            if (i < minX)
                            {
                                minX = i;
                            }
                            if (i > maxX)
                            {
                                maxX = i;
                            }
                            if (j < minY)
                            {
                                minY = j;
                            }
                            if (j > maxY)
                            {
                                maxY = j;
                            }
                        }
                    }
                }

                // Create new bounding box + push onto m_AABBs
                Rectangle bbox = new Rectangle(minX, minY, maxX, maxY);
                m_AABBs.Add(bbox);


                //Console.WriteLine("x: " + x + " " + bbox.ToString());

            }
        }
    }
}
