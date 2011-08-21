using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using ludumdare.src;

namespace ludumdare
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice pDevice;
        SpriteBatch spriteBatch;
        SpriteFont font;

        BasicEffect basicEffect;
        VertexPositionColor[] vertices;

        MultiAnimSprite multiAnimTest;

        SpriteBase boundingTest;
        List<SpriteBase> spritesToDraw;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferMultiSampling = true;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "XNA LD Framework";

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, 0, 0, 1);

            vertices = new VertexPositionColor[4];

            spritesToDraw = new List<SpriteBase>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("calibri");

            // TODO: use this.Content to load your game content here
            // Pointer to Graphics Device
            pDevice = graphics.GraphicsDevice;

            SpriteBase slimeCrouch = new SpriteBase(new Vector2(100, 100), 85, 2);
            SpriteBase slimeDeath = new SpriteBase(new Vector2(100, 200), 85, 2);
            SpriteBase slimeWalk = new SpriteBase(new Vector2(100, 300), 85, 2);

            slimeCrouch.LoadContent(Content, "slimesheet-crouch", OriginPos.TOP_LEFT);
            slimeDeath.LoadContent(Content, "slimesheet-death", OriginPos.TOP_LEFT);
            slimeWalk.LoadContent(Content, "slimesheet", OriginPos.TOP_LEFT);


            //spritesToDraw.Add(slimeCrouch);
            //spritesToDraw.Add(slimeDeath);
            //spritesToDraw.Add(slimeWalk);

            multiAnimTest = new MultiAnimSprite(new Vector2(200, 200));

            multiAnimTest.AddAnimation("WALK", slimeWalk);
            multiAnimTest.AddAnimation("CROUCH", slimeCrouch);
            multiAnimTest.AddAnimation("DEATH", slimeDeath);

            multiAnimTest.PlayAnimation("WALK");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState kbState = Keyboard.GetState();

            /*
            Vector2 currPos = boundingTest.Position;

            if (kbState.IsKeyDown(Keys.Left))
            {
                currPos.X -= (15.0f * (gameTime.ElapsedGameTime.Milliseconds / 100.0f));
            }
            if (kbState.IsKeyDown(Keys.Right))
            {
                currPos.X += (15.0f * (gameTime.ElapsedGameTime.Milliseconds / 100.0f));
            }

            boundingTest.Position = currPos;
            */

            if (kbState.IsKeyDown(Keys.C))
            {
                multiAnimTest.PlayAnimation("CROUCH");
            }

            if (kbState.IsKeyDown(Keys.D))
            {
                multiAnimTest.PlayAnimation("DEATH");
            }

            if (kbState.IsKeyDown(Keys.Left))
            {
                multiAnimTest.PlayAnimation("WALK");
            }

            foreach (SpriteBase sprite in spritesToDraw)
            {
                sprite.Update(gameTime);
            }

            multiAnimTest.Update( gameTime );

            /*
            // Update vertex positioning for the bounding box of boundingTest
            Rectangle currentAABB = boundingTest.m_AABBs[boundingTest.CurrentFrame];
            Vector2 currentPos = boundingTest.Position;

            vertices[0].Position = new Vector3(currentPos.X + currentAABB.X, currentPos.Y + currentAABB.Y, 0);
            vertices[1].Position = new Vector3(currentPos.X + currentAABB.Width, currentPos.Y + currentAABB.Y, 0);
            vertices[2].Position = new Vector3(currentPos.X + currentAABB.Width, currentPos.Y + currentAABB.Height, 0);
            vertices[3].Position = new Vector3(currentPos.X + currentAABB.X, currentPos.Y + currentAABB.Height, 0);
            */

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            /*
            short[] indices = new short[5] { 0, 1, 2, 3, 0 };

            for (int i = 0; i < 4; i++)
            {
                vertices[i].Color = Color.Red;
            }

            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineStrip,
                                                                                    vertices,
                                                                                    0,
                                                                                    4,
                                                                                    indices,
                                                                                    0,
                                                                                    4
                                                                                    );
             */

            spriteBatch.Begin();

            //spriteBatch.DrawString(font, "Current Frame: " + boundingTest.CurrentFrame, new Vector2(0, 100), Color.White);
            //spriteBatch.DrawString(font, "Current BB#: " + boundingTest.CurrentFrame, new Vector2(0, 120), Color.White);

            foreach (SpriteBase sprite in spritesToDraw)
            {
                sprite.Draw(spriteBatch, null);
            }

            multiAnimTest.Draw(spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
