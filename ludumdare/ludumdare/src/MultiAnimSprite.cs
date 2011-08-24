using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace ludumdare.src
{
    class MultiAnimSprite : IGameObject
    {

        // Dictionary used to look up for animations
        Dictionary<string, SpriteBase> m_SpriteAnims;

        // Use to hold current animation
        string m_strCurrAnim;

        // Current position - MUST SYNC ALL ANIMS TO THIS
        Vector2 m_vecPos;

        bool m_bShowDebug;

        #region Accessors
        public string CurrentAnimation
        {
            get { return m_strCurrAnim;  }
        }

        public Vector2 Position
        {
            get { return m_vecPos; }
            set { m_vecPos = value; }
        }

        public bool DisplayAABBs
        {
            get { return m_bShowDebug;}
            set { m_bShowDebug = value; }
        }

        #endregion
        
        public MultiAnimSprite( Vector2 pos )
        {
            m_SpriteAnims = new Dictionary<string, SpriteBase>();
            m_strCurrAnim = "NO_ANIMS_LOADED";

            m_bShowDebug = false;

            m_vecPos = pos;
        }

        public void AddAnimation(string animName, SpriteBase sprite)
        {
            // sync sprite's position to this MultiAnimSprites'
            SpriteBase spriteCopy = new SpriteBase(sprite);
            spriteCopy.Position = m_vecPos;
            m_SpriteAnims.Add(animName, spriteCopy);
        }

        public void PlayAnimation(string animName, Nullable<PlaybackOptions> playbackOptions)
        {

            // Reset previous animation
            if (m_SpriteAnims.ContainsKey(m_strCurrAnim))
            {
                m_SpriteAnims[m_strCurrAnim].CurrentFrame = 0;
            }

            if (m_SpriteAnims.ContainsKey(animName))
            {
                m_strCurrAnim = animName;

                // Set AnimationOptions to playbackOptions passed in this function
                if (playbackOptions != null)
                {
                    m_SpriteAnims[m_strCurrAnim].AnimationOptions = playbackOptions.Value;
                }
                

                if (m_SpriteAnims[m_strCurrAnim].AnimationOptions.HasFlag(PlaybackOptions.Reverse))
                {
                    // Set anims with Reverse flag to start from last frame
                    m_SpriteAnims[m_strCurrAnim].CurrentFrame = (m_SpriteAnims[m_strCurrAnim].MaxFrameNum - 1);
                }
            }
        }
        
        // Sets PlaybackOptions for currently selected animation
        public void SetPlaybackOptions( PlaybackOptions playbackOptions )
        {
            if (m_SpriteAnims.ContainsKey(m_strCurrAnim))
            {
                m_SpriteAnims[m_strCurrAnim].AnimationOptions = playbackOptions;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!(m_strCurrAnim.Equals("NO_ANIMS_LOADED")))
            {
                m_SpriteAnims[m_strCurrAnim].Position = m_vecPos;
                m_SpriteAnims[m_strCurrAnim].Update(gameTime);
            }
        }

        public void Draw( SpriteBatch spriteBatch, GraphicsDevice pDevice )
        {
            if (!(m_strCurrAnim.Equals("NO_ANIMS_LOADED")))
            {
                if (m_bShowDebug)
                {
                    m_SpriteAnims[m_strCurrAnim].DisplayAABBs = true;
                }
                else
                {
                    m_SpriteAnims[m_strCurrAnim].DisplayAABBs = false;
                }
                m_SpriteAnims[m_strCurrAnim].Draw(spriteBatch, pDevice);
            }

        }
    }
}
