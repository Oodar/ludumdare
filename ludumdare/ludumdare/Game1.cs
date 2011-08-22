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

        MultiAnimSprite multiAnimTest;

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

            SpriteBase slimeCrouch =    new SpriteBase(new Vector2(300, 100), 85, 2.0f, pDevice);
            SpriteBase slimeDeath =     new SpriteBase(new Vector2(300, 200), 85, 2.0f, pDevice);
            SpriteBase slimeWalk =      new SpriteBase(new Vector2(300, 300), 85, 2.0f, pDevice);
            SpriteBase slimeIdle =      new SpriteBase(new Vector2(300, 400), 85, 0.5f, pDevice);

            slimeCrouch.LoadContent(Content, "slimesheet-crouch", OriginPos.TOP_LEFT);
            slimeDeath.LoadContent(Content, "slimesheet-death", OriginPos.TOP_LEFT);
            slimeWalk.LoadContent(Content, "slimesheet", OriginPos.TOP_LEFT);
            slimeIdle.LoadContent(Content, "slimesheet-idle", OriginPos.TOP_LEFT);

            slimeCrouch.DisplayAABBs = true;
            slimeDeath.DisplayAABBs = true;
            slimeWalk.DisplayAABBs = true;
            slimeIdle.DisplayAABBs = true;

            //spritesToDraw.Add(slimeCrouch);
            //spritesToDraw.Add(slimeDeath);
            //spritesToDraw.Add(slimeWalk);

            multiAnimTest = new MultiAnimSprite(new Vector2(200, 200));

            multiAnimTest.AddAnimation("IDLE", slimeIdle);
            multiAnimTest.AddAnimation("WALK", slimeWalk);
            multiAnimTest.AddAnimation("CROUCH", slimeCrouch);
            multiAnimTest.AddAnimation("DEATH", slimeDeath);

            multiAnimTest.PlayAnimation("IDLE");


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

            if (kbState.IsKeyDown(Keys.I))
            {
                multiAnimTest.PlayAnimation("IDLE");
            }

            foreach (SpriteBase sprite in spritesToDraw)
            {
                sprite.Update(gameTime);
            }

            multiAnimTest.Update( gameTime );


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
           
            spriteBatch.Begin();

            foreach (SpriteBase sprite in spritesToDraw)
            {
                sprite.Draw(spriteBatch, null, pDevice);
            }

            multiAnimTest.Draw(spriteBatch, pDevice, true);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
