using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Castle_Knight
{
    public class TitleScreen : screen
    {
        int select = 0;
        bool noLoad = false;
        Texture2D buttonStart;
        Texture2D buttonLoad;
        Texture2D buttonExit;
        Texture2D bg_mainmenu;
        Texture2D noLoadPic;
        Texture2D BG1_1;
        Texture2D BG1_2;
        Texture2D BG1_3;
        Texture2D credit;
        Texture2D buttonSoundOn;
        Texture2D buttonSoundOff;
        Vector2[] BG1_1Pos = new Vector2[2];
        Vector2[] BG1_2Pos = new Vector2[2];
        private AnimatedTexture p_title;
        private AnimatedTexture buttonSelect;

        string loadingText;

        SoundEffect soundEffects;

        KeyboardState keyboardState;

        Vector2 select_Pos;
        bool stopPress = false;
        bool Credit = false;

        int mAlphaValue = 1;
        int mFadeIncrement = 8;
        double mFadeDelay = .035;

        Texture2D logo;
        bool logoOn = false;
        bool Title = false;

        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;

        private static readonly TimeSpan intervalBetweenSelect = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimeSelect;

        private static readonly TimeSpan intervalBetweenLoad = TimeSpan.FromMilliseconds(1000);
        private TimeSpan lastTimeLoad;

        private static readonly TimeSpan DelayBack = TimeSpan.FromMilliseconds(2500);
        private TimeSpan lastTimeBack;

        private TimeSpan lastTimeLogo;

        Game1 game;
        public TitleScreen(Game1 game, EventHandler theScreenEvent)
        : base(theScreenEvent)
        {
            p_title = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            buttonSelect = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            BG1_1 = game.Content.Load<Texture2D>("bg_level1");
            BG1_2 = game.Content.Load<Texture2D>("bg_level1_1");
            BG1_3 = game.Content.Load<Texture2D>("bg_level1_2");
            credit = game.Content.Load<Texture2D>("Credit");
            buttonSoundOn = game.Content.Load<Texture2D>("sound1");
            buttonSoundOff = game.Content.Load<Texture2D>("sound2");
            bg_mainmenu = game.Content.Load<Texture2D>("gameTitle");
            noLoadPic = game.Content.Load<Texture2D>("noLoad");
            buttonStart = game.Content.Load<Texture2D>("Start");
            buttonLoad = game.Content.Load<Texture2D>("Load");
            buttonExit = game.Content.Load<Texture2D>("Exit");
            logo = game.Content.Load<Texture2D>("Logo_game");
            p_title.Load(game.Content, "p_Title", 2, 1, 4);
            buttonSelect.Load(game.Content, "Select", 4, 1, 5);

            soundEffects = game.Content.Load<SoundEffect>("button-15");

            select_Pos = new Vector2(130, 195);

            #region Loadflie
            string fileName = @"Content\data.txt";
            if (File.Exists(fileName))
            {
                string filepath = Path.Combine(@"Content\data.txt");
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string tmpStr = sr.ReadLine();
                loadingText = tmpStr;
                sr.Close();
            }
            else
            {
                string filepath = Path.Combine(@"Content\data.txt");
                FileStream fc = new FileStream(filepath, FileMode.CreateNew);
                fc.Close();
            }

            #endregion

            BG1_1Pos[0] = new Vector2(0, 0);
            BG1_2Pos[0] = new Vector2(0, 0);
            BG1_1Pos[1] = new Vector2(3678, 0);
            BG1_2Pos[1] = new Vector2(3678, 0);

            this.game = game;
        }
        public override void Update(GameTime theTime)
        {
            if (Game1.BackMenu)
            {
                lastTimeBack = theTime.TotalGameTime;
                Game1.BackMenu = false;
            }
            float elapsed = (float)theTime.ElapsedGameTime.TotalSeconds;

            if (!logoOn)
            {
                lastTimeLogo = theTime.TotalGameTime;
                logoOn = true;
            }

            if (logoOn && lastTimeLogo + TimeSpan.FromSeconds(2.5) < theTime.TotalGameTime)
            {
                mFadeDelay -= theTime.ElapsedGameTime.TotalSeconds;
                if (mFadeDelay <= 0)
                {
                    mFadeDelay = .035;
                    mAlphaValue -= mFadeIncrement;
                    if (mAlphaValue <= 0)
                    {
                        mAlphaValue = 0;
                    }
                }
            }
            else if (logoOn)
            {
                mFadeDelay -= theTime.ElapsedGameTime.TotalSeconds;
                if (mFadeDelay <= 0)
                {
                    mFadeDelay = .035;
                    mAlphaValue += mFadeIncrement;
                    if (mAlphaValue >= 255)
                    {
                        mAlphaValue = 255;
                    }
                }
            }
            if (logoOn && lastTimeLogo + TimeSpan.FromSeconds(5.0) < theTime.TotalGameTime)
            {
                logoOn = false;
                Title = true;
            }

            if (Title)
            {
                keyboardState = Keyboard.GetState();
                p_title.UpdateFrame(elapsed);
                buttonSelect.UpdateFrame(elapsed);

                if (Game1.State == "Title")
                {
                    if (!Game1.SFXOn)
                    {
                        Game1.soundOn = false;
                    }
                    else if (Game1.SFXOn)
                    {
                        Game1.soundOn = true;
                    }
                }
                if (Game1.MusicOn)
                {
                    MediaPlayer.IsMuted = true;
                }
                else if (!Game1.MusicOn)
                {
                    MediaPlayer.IsMuted = true;
                }
                if (Game1.SFXOn)
                {
                    SoundEffect.MasterVolume = 0.5f;
                }
                else if (!Game1.SFXOn)
                {
                    SoundEffect.MasterVolume = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.I))
                {
                    if (lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                    {
                        if (!Credit)
                        {
                            Credit = true;
                        }
                        else if (Credit)
                        {
                            Credit = false;
                        }

                        lastTimeSelect = theTime.TotalGameTime;
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    if (lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                    {
                        if (Game1.soundOn)
                        {
                            Game1.SFXOn = false;
                            select = 0;
                        }
                        else if (!Game1.soundOn)
                        {
                            Game1.SFXOn = true;
                            select = 0;
                        }

                        lastTimeSelect = theTime.TotalGameTime;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Down) && stopPress == false && !Credit)
                {
                    if (lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                    {
                        if (!(select_Pos.Y == 315))
                        {
                            soundEffects.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                        }
                        if (select_Pos.Y <= 255)
                        {
                            select_Pos.Y += 60;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Up) && stopPress == false && !Credit)
                {
                    if (lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                    {
                        if (!(select_Pos.Y == 195))
                        {
                            soundEffects.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                        }
                        if (select_Pos.Y >= 255)
                        {
                            select_Pos.Y -= 60;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.A) && stopPress == false && !Credit && lastTimeBack + DelayBack < theTime.TotalGameTime)
                {
                    stopPress = true;
                    soundEffects.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                    if (select_Pos.Y == 195)
                    {
                        select = 1;
                    }
                    else if (select_Pos.Y == 255)
                    {
                        select = 2;
                    }
                    else if (select_Pos.Y == 315)
                    {
                        select = 3;
                    }

                    if (select == 1)
                    {
                        string filepath = Path.Combine(@"Content\data.txt");
                        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine("InGame1");
                        sw.Flush();
                        sw.Close();

                        string _filepath = Path.Combine(@"Content\data.txt");
                        FileStream _fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                        StreamReader sr = new StreamReader(_fs);
                        string tmpStr = sr.ReadLine();
                        loadingText = tmpStr;
                        sr.Close();

                        if (loadingText == "InGame1")
                        {
                            ScreenEvent.Invoke(game.mGameplayScreen, new EventArgs());
                            return;
                        }
                    }
                    else if (select == 2)
                    {
                        string filepath = Path.Combine(@"Content\data.txt");
                        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                        StreamReader sr = new StreamReader(fs);
                        string tmpStr = sr.ReadLine();
                        loadingText = tmpStr;
                        sr.Close();

                        if (loadingText == "InGame1")
                        {
                            ScreenEvent.Invoke(game.mGameplayScreen, new EventArgs());
                            return;
                        }
                        else if (loadingText == "InGame2")
                        {
                            ScreenEvent.Invoke(game.mGameplayScreen2, new EventArgs());
                            return;
                        }
                        else if (loadingText == "InGame3")
                        {
                            ScreenEvent.Invoke(game.mGameplayScreen3, new EventArgs());
                            return;
                        }
                        else
                        {
                            noLoad = true;

                            lastTimeLoad = theTime.TotalGameTime;
                        }
                    }
                    else if (select == 3)
                    {
                        game.Exit();
                    }
                }

                BG1_1Pos[0].X -= 1 * 0.4f;
                BG1_2Pos[0].X -= 1 * 0.6f;
                BG1_1Pos[1].X -= 1 * 0.4f;
                BG1_2Pos[1].X -= 1 * 0.6f;
                if (BG1_1Pos[0].X <= -3682)
                {
                    BG1_1Pos[0].X = 3676;
                }
                if (BG1_2Pos[0].X <= -3680)
                {
                    BG1_2Pos[0].X = 3678;
                }
                if (BG1_1Pos[1].X <= -3680)
                {
                    BG1_1Pos[1].X = 3678;
                }
                if (BG1_2Pos[1].X <= -3680)
                {
                    BG1_2Pos[1].X = 3678;
                }
            }

            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch, GameTime theTime)
        {
            theBatch.Begin();
            if (logoOn)
            {
                theBatch.Draw(logo, new Vector2(0, 0), new Color((byte)255, (byte)255, (byte)255, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
            }
            if (Title)
            {
                theBatch.Draw(BG1_1, BG1_1Pos[0], Color.White);
                theBatch.Draw(BG1_1, BG1_1Pos[1], Color.White);
                theBatch.Draw(BG1_2, BG1_2Pos[1], Color.White);
                theBatch.Draw(BG1_2, BG1_2Pos[0], Color.White);
                theBatch.Draw(BG1_3, new Vector2(0, 0), Color.White);
                if (Credit)
                {
                    theBatch.Draw(credit, new Vector2(0, 0), Color.White);
                }
                else if (!Credit)
                {
                    theBatch.Draw(bg_mainmenu, new Vector2(0, 0), Color.White);
                    if (Game1.soundOn)
                    {
                        theBatch.Draw(buttonSoundOn, new Vector2(10, 10), Color.White);
                    }
                    else if (!Game1.soundOn)
                    {
                        theBatch.Draw(buttonSoundOff, new Vector2(10, 10), Color.White);
                    }
                    if (select_Pos.Y == 195)
                    {
                        theBatch.Draw(buttonStart, new Vector2(225, 195), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        theBatch.Draw(buttonStart, new Vector2(230, 200), Color.White);
                    }
                    if (select_Pos.Y == 255)
                    {
                        theBatch.Draw(buttonLoad, new Vector2(225, 255), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        theBatch.Draw(buttonLoad, new Vector2(230, 260), Color.White);
                    }
                    if (select_Pos.Y == 315)
                    {
                        theBatch.Draw(buttonExit, new Vector2(225, 315), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        theBatch.Draw(buttonExit, new Vector2(230, 320), Color.White);
                    }
                    buttonSelect.DrawFrame(theBatch, select_Pos);
                    p_title.DrawFrame(theBatch, new Vector2(560, 95));
                }
                if (noLoad == true)
                {
                    theBatch.Draw(noLoadPic, new Vector2(0, 0), Color.White);
                }
                if (lastTimeLoad + intervalBetweenLoad < theTime.TotalGameTime)
                {
                    noLoad = false;
                    stopPress = false;
                }

            }

            theBatch.End();

            base.Draw(theBatch, theTime);
        }

    }

}