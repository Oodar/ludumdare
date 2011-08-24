using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ludumdare.src
{
    interface IGameObject
    {
        void Draw(SpriteBatch spriteBatch, GraphicsDevice pDevice);
        void Update(GameTime gameTime);
    }
}
