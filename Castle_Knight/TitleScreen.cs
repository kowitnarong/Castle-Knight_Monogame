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
        Texture2D buttonSelect;
        Texture2D noLoadPic;
        private AnimatedTexture p_title;

        SpriteFont ArialFont;
        string loadingText;

        SoundEffect soundEffects;

        KeyboardState keyboardState;

        Vector2 select_Pos;
        bool stopPress = false;

        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;

        private static readonly TimeSpan intervalBetweenSelect = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimeSelect;

        private static readonly TimeSpan intervalBetweenLoad = TimeSpan.FromMilliseconds(2500);
        private TimeSpan lastTimeLoad;

        Game1 game;
        public TitleScreen(Game1 game, EventHandler theScreenEvent)
        : base(theScreenEvent)
        {
            p_title = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            bg_mainmenu = game.Content.Load<Texture2D>("gameTitle");
            noLoadPic = game.Content.Load<Texture2D>("noLoad");
            buttonStart = game.Content.Load<Texture2D>("Start");
            buttonLoad = game.Content.Load<Texture2D>("Load");
            buttonExit = game.Content.Load<Texture2D>("Exit");
            buttonSelect = game.Content.Load<Texture2D>("Select");
            p_title.Load(game.Content, "p_Title", 2, 1, 4);

            soundEffects = game.Content.Load<SoundEffect>("button-15");

            select_Pos = new Vector2(130, 195);

            #region Loadflie
            string filepath = Path.Combine(@"Content\data.txt");
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string tmpStr = sr.ReadLine();
            loadingText = tmpStr;
            sr.Close();
            #endregion

            this.game = game;
        }
        public override void Update(GameTime theTime)
        {
            float elapsed = (float)theTime.ElapsedGameTime.TotalSeconds;
            keyboardState = Keyboard.GetState();
            p_title.UpdateFrame(elapsed);

            if (keyboardState.IsKeyDown(Keys.Down) && stopPress == false)
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
            else if (keyboardState.IsKeyDown(Keys.Up) && stopPress == false)
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
            else if (keyboardState.IsKeyDown(Keys.A) && stopPress == false)
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

            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch, GameTime theTime)
        {
            theBatch.Begin();
            theBatch.Draw(bg_mainmenu, new Vector2(0, 0), Color.White);
            theBatch.Draw(buttonStart, new Vector2(230, 200), Color.White);
            theBatch.Draw(buttonLoad, new Vector2(230, 260), Color.White);
            theBatch.Draw(buttonExit, new Vector2(230, 320), Color.White);
            theBatch.Draw(buttonSelect, select_Pos, Color.White);
            p_title.DrawFrame(theBatch, new Vector2(560, 80));
            if (noLoad == true)
            {
                theBatch.Draw(noLoadPic, new Vector2(0, 0), Color.White);
            }
            if (lastTimeLoad + intervalBetweenLoad < theTime.TotalGameTime)
            {
                noLoad = false;
                stopPress = false;
            }

            theBatch.End();

            base.Draw(theBatch, theTime);
        }

    }

}