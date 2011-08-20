﻿using System;
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

        #endregion

        #region Constructors

        public SpriteBase()
        {
            m_vecPos = new Vector2(0, 0);
            m_fScale = 1.0f;
            m_fRotation = 0.0f;
        }


        public SpriteBase(Vector2 pos)
        {
            m_vecPos = pos;
            m_fScale = 1.0f;
            m_fRotation = 0.0f;
        }

        #endregion
       

        public void LoadContent(ContentManager contentManager, string texLocation, OriginPos origin )
        {
            m_texBase = contentManager.Load<Texture2D>(texLocation);
            SetOrigin(origin);
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

        
    }
}
