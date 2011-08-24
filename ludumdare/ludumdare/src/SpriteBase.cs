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

    [Flags]
    enum PlaybackOptions
    {
        None = 0, // Defaults to looped animation
        Once = 1,
        Reverse = 2,
        FlipHorizontal = 4,
        FlipVertical = 8
    }

    #endregion

    class SpriteBase : IGameObject
    {

        PlaybackOptions m_playbackOptions;

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

        // Debug drawing for BBs:
        BasicEffect basicEffect;
        VertexPositionColor[] vertices;
        short[] indices = new short[5] { 0, 1, 2, 3, 0 };

        bool m_bDisplayAABBs;


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
            set { m_iCurrFrame = value; }
        }

        public bool DisplayAABBs
        {
            set { m_bDisplayAABBs = value; }
        }

        public PlaybackOptions AnimationOptions
        {
            get { return m_playbackOptions; }
            set { m_playbackOptions = value; }
        }

        public int MaxFrameNum
        {
            get { return m_iMaxFrames; }
        }

        #endregion

        #region Constructor

        public SpriteBase( SpriteBase o )
        {
            this.m_vecPos = o.Position;
            this.m_vecOrigin = o.m_vecOrigin;
            this.m_playbackOptions = o.AnimationOptions;

            this.m_texBase = o.m_texBase;

            this.m_fScale = o.m_fScale;
            this.m_fRotation = o.m_fRotation;

            this.m_iCurrFrame = o.m_iCurrFrame;
            this.m_iMaxFrames = o.m_iMaxFrames;
            this.m_iFrameWidth = o.m_iFrameWidth;

            this.m_fFrameTime = o.m_fFrameTime;
            this.m_fTimeAccumulator = o.m_fTimeAccumulator;

            this.m_frameRect = o.m_frameRect;

            this.m_AABBs = o.m_AABBs;

            this.basicEffect = o.basicEffect;
            this.vertices = o.vertices;

            this.m_bDisplayAABBs = o.m_bDisplayAABBs;
        }

        public SpriteBase(Vector2 pos, int frameWidth, float frameRate, GraphicsDevice pDevice )
        {
            m_playbackOptions = PlaybackOptions.None;

            m_vecPos = pos;
            m_fScale = 1.0f;
            m_fRotation = 0.0f;

            m_iCurrFrame = 0;
            m_iFrameWidth = frameWidth;

            m_fFrameTime = (1.0f / frameRate);
            
            m_AABBs = new List<Rectangle>();

            m_bDisplayAABBs = false;

            // Set rendering of debug BBs
            vertices = new VertexPositionColor[4];
            basicEffect = new BasicEffect(pDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, pDevice.Viewport.Width, pDevice.Viewport.Height, 0, 0, 1);

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
                // Potention next frame counter
                int nextFrame = 0;

                if (m_playbackOptions.HasFlag(PlaybackOptions.Reverse))
                {
                    nextFrame = m_iCurrFrame - 1;

                    // If PlaybackOptions.Once is set, we shouldn't wrap around from 0
                    if (m_playbackOptions.HasFlag(PlaybackOptions.Once) && nextFrame < 0 )
                    {
                        m_iCurrFrame = 0;
                    }
                    else
                    {
                        if (nextFrame < 0)
                        {
                            m_iCurrFrame = (m_iMaxFrames - 1);
                        }
                        else
                        {
                            m_iCurrFrame = nextFrame;
                        }
                    }
                }
                else
                { 
                    // Not going in reverse, so increment counter forwards
                    nextFrame = m_iCurrFrame + 1;

                    // If we're only playing once, we want to stop the counter going forwards
                    if (m_playbackOptions.HasFlag(PlaybackOptions.Once) && nextFrame >= (m_iMaxFrames - 1))
                    {
                        m_iCurrFrame = (m_iMaxFrames - 1);
                    }
                    else
                    {
                        m_iCurrFrame = nextFrame % m_iMaxFrames;
                    }

                }

                m_frameRect.X = m_iCurrFrame * m_iFrameWidth;
                m_frameRect.Y = 0;

                m_frameRect.Width = m_iFrameWidth;
                m_frameRect.Height = m_texBase.Height;
                
              
                m_fTimeAccumulator = 0.0f;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice pDevice )
        {

            if (m_bDisplayAABBs)
            {

                Rectangle currentAABB = m_AABBs[m_iCurrFrame];

                vertices[0].Position = new Vector3(m_vecPos.X + currentAABB.X, m_vecPos.Y + currentAABB.Y, 0);
                vertices[1].Position = new Vector3(m_vecPos.X + currentAABB.Width, m_vecPos.Y + currentAABB.Y, 0);
                vertices[2].Position = new Vector3(m_vecPos.X + currentAABB.Width, m_vecPos.Y + currentAABB.Height, 0);
                vertices[3].Position = new Vector3(m_vecPos.X + currentAABB.X, m_vecPos.Y + currentAABB.Height, 0);

                for (int i = 0; i < 4; i++)
                {
                    vertices[i].Color = Color.Red;
                }


                basicEffect.CurrentTechnique.Passes[0].Apply();
                pDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip,
                                                                        vertices,
                                                                        0,
                                                                        4,
                                                                        indices,
                                                                        0,
                                                                        4
                                                                        );

            }


            SpriteEffects effects = 0;

            if (m_playbackOptions.HasFlag(PlaybackOptions.FlipHorizontal))
            {
                effects |= SpriteEffects.FlipHorizontally;
            }
            if (m_playbackOptions.HasFlag(PlaybackOptions.FlipVertical))
            {
                effects |= SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw(m_texBase, m_vecPos, m_frameRect, Color.White, MathHelper.ToRadians(m_fRotation), m_vecOrigin, m_fScale, effects, 1);
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
