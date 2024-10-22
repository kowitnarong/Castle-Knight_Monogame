﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using System.IO;

namespace Castle_Knight
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public GameplayScreen mGameplayScreen;
        public GameplayScreen2 mGameplayScreen2;
        public GameplayScreen3 mGameplayScreen3;
        public TitleScreen mTitleScreen;
        public screen mCurrentScreen;

        public static bool soundOn = true;
        public static bool MusicOn = true;
        public static bool SFXOn = true;
        public static string State = "Title";
        public static bool BackMenu = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mGameplayScreen = new GameplayScreen(this, new EventHandler(GameplayScreenEvent));
            mGameplayScreen2 = new GameplayScreen2(this, new EventHandler(GameplayScreenEvent));
            mGameplayScreen3 = new GameplayScreen3(this, new EventHandler(GameplayScreenEvent));
            mTitleScreen = new TitleScreen(this, new EventHandler(GameplayScreenEvent));
            mCurrentScreen = mTitleScreen;

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            mCurrentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            mCurrentScreen.Draw(spriteBatch , gameTime);
            
            base.Draw(gameTime);
        }
        public void GameplayScreenEvent(object obj, EventArgs e)
        {
            mCurrentScreen = (screen)obj;
        }
    }
}