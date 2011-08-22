using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace ludumdare.src
{
    class MultiAnimSprite
    {

        // Dictionary used to look up for animations
        Dictionary<string, SpriteBase> m_SpriteAnims;

        // Use to hold current animation
        string m_strCurrAnim;

        // Current position - MUST SYNC ALL ANIMS TO THIS
        Vector2 m_vecPos;

        #region Accessors
        public string CurrentAnimation
        {
            get { return m_strCurrAnim;  }
        }

        #endregion
        
        public MultiAnimSprite( Vector2 pos )
        {
            m_SpriteAnims = new Dictionary<string, SpriteBase>();
            m_strCurrAnim = "NO_ANIMS_LOADED";

            m_vecPos = pos;
        }

        public void AddAnimation(string animName, SpriteBase sprite)
        {
            m_SpriteAnims.Add(animName, sprite);
        }

        public void PlayAnimation(string animName)
        {
            if (m_SpriteAnims.ContainsKey(animName))
            {
                m_strCurrAnim = animName;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!(m_strCurrAnim.Equals("NO_ANIMS_LOADED")))
            {
                m_SpriteAnims[m_strCurrAnim].Update(gameTime);
            }
        }

        public void Draw( SpriteBatch spriteBatch, GraphicsDevice pDevice, bool debug )
        {
            if (!(m_strCurrAnim.Equals("NO_ANIMS_LOADED")))
            {
                if (debug)
                {
                    m_SpriteAnims[m_strCurrAnim].DisplayAABBs = true;
                }
                else
                {
                    m_SpriteAnims[m_strCurrAnim].DisplayAABBs = false;
                }
                m_SpriteAnims[m_strCurrAnim].Draw(spriteBatch, m_vecPos, pDevice);
            }

        }
    }
}
