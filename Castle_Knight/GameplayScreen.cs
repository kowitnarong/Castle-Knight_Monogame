﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Castle_Knight
{
    public class GameplayScreen : screen
    {
        struct DisplayMessage
        {
            public string Message;
            public TimeSpan DisplayTime;
            public int CurentIndex;
            public Vector2 Position;
            public string DrawnMessage;
            public Color DrawColor;
            public DisplayMessage(string message, TimeSpan displayTime, Vector2 position, Color color)
            {
                Message = message;
                DisplayTime = displayTime;
                CurentIndex = 0;
                Position = position;
                DrawnMessage = string.Empty;
                DrawColor = color;
            }
        }
        List<DisplayMessage> messages = new List<DisplayMessage>();
        bool resetValue = false;

        Player Player = new Player();
        Enemy enemyBlack = new Enemy();
        Enemy enemyBlack2 = new Enemy();
        Enemy enemyGold = new Enemy();
        Enemy enemyWizard = new Enemy();
        Enemy enemyRed = new Enemy();

        #region Factor
        bool w_left = false, w_right = false;
        bool def = false;
        bool special = false;
        bool special_ani = false;
        bool devMode = false;
        bool loadOn = false;

        // Menu
        bool load = false;
        string Switch;
        SpriteFont ArialFont;
        int dead_count;
        bool menuLoading = false;
        bool gamePause = false;
        Texture2D ButtonGuide;
        Texture2D ButtonMenu;
        Texture2D pausePic;
        Texture2D gameOver;
        Texture2D Cloud1;
        Texture2D Cloud2;
        int potion_Count = 0;
        float[] speedCloud = new float[10];
        float[] speedCloud2 = new float[5];
        private AnimatedTexture loading;
        private AnimatedTexture glass;
        private AnimatedTexture effect1;
        private AnimatedTexture effect2;
        bool ChatOn = true;
        int text = 1;
        string _text;
        Texture2D sChat;

        Texture2D buttonBack;
        Texture2D buttonRetry;
        Texture2D buttonSoundOn;
        Texture2D buttonSoundOff;
        Texture2D buttonSetting;
        Texture2D buttonMusicOn;
        Texture2D buttonMusicOFF;
        Texture2D buttonExit;
        private AnimatedTexture buttonSelect;
        Vector2 select_Pos;
        int select = 0;
        bool stopPress = false;
        bool setting = false;

        // Sound
        Song bgSong;
        List<SoundEffect> soundEffects;
        SoundEffectInstance walkSoundInstance;
        SoundEffectInstance SwordHit;
        SoundEffectInstance SwordWhoosh;
        SoundEffectInstance Dead;
        bool bg1Song = false;

        Texture2D BG1_1;
        Texture2D BG1_2;
        Texture2D BG1_3;
        Texture2D Heart;
        Texture2D HeartBlack;
        Texture2D potion;
        // Special attack
        Texture2D special1;
        Texture2D special2;
        Texture2D special3;
        Texture2D special4;
        Texture2D special5;
        Vector2[] potion_Pos = new Vector2[2];
        Vector2[] glassPos = new Vector2[70];
        Vector2[] Cloud_Pos = new Vector2[7];
        Vector2[] Cloud_Pos2 = new Vector2[5];
        bool[] potion_Ena = new bool[2];
        bool[] potion_Use = new bool[2];

        KeyboardState keyboardState;
        KeyboardState old_keyboardState;

        Camera camera = new Camera();

        // Ai
        Random r = new Random();
        private AnimatedTexture Cat_idle;
        private AnimatedTexture rabbit_idle;

        // Ai 3
        Texture2D eWaveAtk;
        bool ai3_Wave = false;
        bool ai3_Use = false;
        Vector2 Ai3WavePos = new Vector2(0, 600);
        // Ai 4
        Texture2D fireball;
        Texture2D bigFireball;
        int fireBallCount = 0;
        bool ai4_Fireball = false;
        bool ai4_bigFireball = false;
        bool ai4_Use = false;
        Vector2 Ai4FirePos = new Vector2(0, 600);

        bool rab_direction = false;
        bool _rab_direction = false;

        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;
        #endregion

        #region Time

        // time select
        private static readonly TimeSpan intervalBetweenSelect = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimeSelect;

        private static readonly TimeSpan intervalBetweenPause = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimePause;
        private TimeSpan lastTimePauseOn;
        private TimeSpan lastTimePauseOff;
        private TimeSpan PauseTime;
        private TimeSpan PauseTime2;

        // time wait loading
        private static readonly TimeSpan intervalBetweenLoad = TimeSpan.FromMilliseconds(2500);
        private TimeSpan lastTimeLoad;

        // time died
        private static readonly TimeSpan intervalBetweenDied = TimeSpan.FromMilliseconds(700);
        private TimeSpan lastTimeDied;

        // time effec1
        private static readonly TimeSpan effectfadeOut = TimeSpan.FromMilliseconds(30);
        private TimeSpan lasttimeEffect;

        // time chat
        private static readonly TimeSpan delayChat = TimeSpan.FromMilliseconds(2500);
        private TimeSpan lasttimeChat;

        // Enemy time
        private static readonly TimeSpan eDelayAtk = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan eCoolDownAtk = TimeSpan.FromMilliseconds(1500);
        private TimeSpan eAtkTime;

        private static readonly TimeSpan eDelayAtk2 = TimeSpan.FromMilliseconds(2200);
        private static readonly TimeSpan eCoolDownAtk2 = TimeSpan.FromMilliseconds(800);
        private TimeSpan eAtkTime2;

        private TimeSpan eDelayAtk3 = TimeSpan.FromMilliseconds(1650);
        private static readonly TimeSpan eCoolDownAtk3 = TimeSpan.FromMilliseconds(400);
        private TimeSpan eAtkTime3;

        private static readonly TimeSpan DelayAttackWave3 = TimeSpan.FromMilliseconds(2000);
        private TimeSpan AttackWave3;

        private TimeSpan eDelayAtk4 = TimeSpan.FromMilliseconds(2500);
        private static readonly TimeSpan eCoolDownAtk4 = TimeSpan.FromMilliseconds(400);
        private TimeSpan eAtkTime4;

        private static readonly TimeSpan DelayAttackWave4 = TimeSpan.FromMilliseconds(800);
        private TimeSpan AttackWave4;

        private TimeSpan eDelayAtk5 = TimeSpan.FromMilliseconds(1200);
        private static readonly TimeSpan eCoolDownAtk5 = TimeSpan.FromMilliseconds(300);
        private TimeSpan eAtkTime5;
        #endregion

        #region Vector and Frame
        private const int Frames = 2;
        private const int FramesPerSec = 4;
        private const int FramesRow = 1;

        // Pos Camera
        private Vector2 Camera_Pos = new Vector2(700, 255);

        // effect pos
        private Vector2 effect1_Pos = new Vector2(700, 600);
        private Vector2 effect2_Pos = new Vector2(700, 600);

        // Cat Pos
        private Vector2 Cat_Pos = new Vector2(1040, 55);

        // rabbit Pos
        private Vector2 rabbit_Pos = new Vector2(500, 368);

        // Special atk
        private Vector2 special_Pos = new Vector2(500, 600);
        private const int special_Frames = 6;
        private const int special_FramesPerSec = 12;
        private const int special_FramesRow = 1;

        // Walk frame
        private const int w_Frames = 4;
        private const int w_FramesPerSec = 7;
        private const int w_FramesRow = 1;

        // Attack frame
        private const int atk_Frames = 8;
        private const int atk_FramesPerSec = 28;
        private const int atk_FramesRow = 1;

        // def frame
        private const int def_Frames = 12;
        private const int def_FramesPerSec = 20;
        private const int def_FramesRow = 1;

        // died frame
        private const int died_Frames = 4;
        private const int died_FramesPerSec = 4;
        private const int died_FramesRow = 1;


        // Enemy frame
        private const int e_walk_Frames = 4;
        private const int e_walk_FramesPerSec = 4;
        private const int e_walk_FramesRow = 1;

        private const int e_atk_Frames = 9;
        private const int e_atk_FramesPerSec = 18;
        private const int e_atk_FramesRow = 1;

        #endregion

        Game1 game;
        public GameplayScreen(Game1 game, EventHandler theScreenEvent)
        : base(theScreenEvent)
        {

            #region AnimatedTexture
            soundEffects = new List<SoundEffect>();
            effect1 = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            effect2 = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            buttonSelect = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.defAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack2.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack2.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack2.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyGold.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyGold.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyWizard.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyRed.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyRed.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyWizard.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Cat_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            loading = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            glass = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.diedAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            rabbit_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.specialAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.specialAtkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            #endregion

            #region Asset
            // Sound Effect
            soundEffects.Add(game.Content.Load<SoundEffect>("WalkSound")); //[0]
            soundEffects.Add(game.Content.Load<SoundEffect>("button-15")); //[1]
            soundEffects.Add(game.Content.Load<SoundEffect>("SwordHit")); //[2]
            soundEffects.Add(game.Content.Load<SoundEffect>("SwordWhoosh")); //[3]
            soundEffects.Add(game.Content.Load<SoundEffect>("SwordBlock")); //[4]
            soundEffects.Add(game.Content.Load<SoundEffect>("Drink")); //[5]
            soundEffects.Add(game.Content.Load<SoundEffect>("SwordKillpower")); //[6]
            soundEffects.Add(game.Content.Load<SoundEffect>("PlayerDead")); //[7]

            walkSoundInstance = soundEffects[0].CreateInstance();
            SwordHit = soundEffects[2].CreateInstance();
            SwordWhoosh = soundEffects[3].CreateInstance();
            Dead = soundEffects[7].CreateInstance();

            bgSong = game.Content.Load<Song>("BackgroundLevel1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume -= 0.7f;

            ButtonMenu = game.Content.Load<Texture2D>("ButtonMenu");
            ButtonGuide = game.Content.Load<Texture2D>("ButtonGuide");
            effect1.Load(game.Content, "effect1", 3, 1, 15);
            effect2.Load(game.Content, "effect2", 3, 1, 15);
            eWaveAtk = game.Content.Load<Texture2D>("goldEneAtkWave");
            fireball = game.Content.Load<Texture2D>("fireBall");
            bigFireball = game.Content.Load<Texture2D>("bigFireBall");
            pausePic = game.Content.Load<Texture2D>("pause");
            ArialFont = game.Content.Load<SpriteFont>("ArialFont");
            BG1_1 = game.Content.Load<Texture2D>("bg_level1");
            BG1_2 = game.Content.Load<Texture2D>("bg_level1_1");
            BG1_3 = game.Content.Load<Texture2D>("bg_level1_2");
            Heart = game.Content.Load<Texture2D>("Heart");
            HeartBlack = game.Content.Load<Texture2D>("HeartBlack");
            gameOver = game.Content.Load<Texture2D>("Game over");
            Cloud1 = game.Content.Load<Texture2D>("Cloud1");
            Cloud2 = game.Content.Load<Texture2D>("Cloud2");
            potion = game.Content.Load<Texture2D>("hp_Potion");
            special1 = game.Content.Load<Texture2D>("special_atk");
            special2 = game.Content.Load<Texture2D>("special_atk2");
            special3 = game.Content.Load<Texture2D>("special_atk3");
            special4 = game.Content.Load<Texture2D>("special_atk4");
            special5 = game.Content.Load<Texture2D>("special_atk5");
            loading.Load(game.Content, "loadScreen", 3, 1, 6);
            Player.idleAni.Load(game.Content, "p_test", Frames, FramesRow, FramesPerSec);
            Player.walkAni.Load(game.Content, "p_Walk_Test", w_Frames, w_FramesRow, w_FramesPerSec);
            Player.atkAni.Load(game.Content, "player_atk", atk_Frames, atk_FramesRow, atk_FramesPerSec);
            Player.specialAni.Load(game.Content, "player_special", atk_Frames, atk_FramesRow, atk_FramesPerSec);
            Player.defAni.Load(game.Content, "player_def", def_Frames, def_FramesRow, def_FramesPerSec);
            Player.diedAni.Load(game.Content, "p_died", died_Frames, died_FramesRow, died_FramesPerSec);
            Player.specialAtkAni.Load(game.Content, "atk_special", special_Frames, special_FramesRow, special_FramesPerSec);

            // Ai
            enemyBlack.walkAni.Load(game.Content, "enemy_walk", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            enemyBlack.idleAni.Load(game.Content, "enemy_idle", Frames, FramesRow, FramesPerSec);
            enemyBlack.atkAni.Load(game.Content, "enemy_atk", e_atk_Frames, e_atk_FramesRow, e_atk_FramesPerSec);
            enemyBlack2.walkAni.Load(game.Content, "enemy_walk", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            enemyBlack2.idleAni.Load(game.Content, "enemy_idle", Frames, FramesRow, FramesPerSec);
            enemyBlack2.atkAni.Load(game.Content, "enemy_atk", e_atk_Frames, e_atk_FramesRow, e_atk_FramesPerSec);
            enemyGold.walkAni.Load(game.Content, "goldEnemy", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            enemyGold.atkAni.Load(game.Content, "goldEnemyAtk", 8, e_atk_FramesRow, 16);
            enemyWizard.atkAni.Load(game.Content, "wizard", 4, 1, 8);
            enemyRed.walkAni.Load(game.Content, "redEnemy", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            enemyRed.atkAni.Load(game.Content, "redEnemyAtk", 8, e_atk_FramesRow, 12);
            enemyWizard.idleAni.Load(game.Content, "wizard_idle", Frames, FramesRow, FramesPerSec);
            Cat_idle.Load(game.Content, "cat", 3, 1, 3);
            rabbit_idle.Load(game.Content, "rabbit", 3, 1, 3);
            glass.Load(game.Content, "Glass", 2, 1, 2);

            buttonBack = game.Content.Load<Texture2D>("BackButton");
            buttonRetry = game.Content.Load<Texture2D>("RetryButton");
            buttonSoundOn = game.Content.Load<Texture2D>("SfxOn");
            buttonSoundOff = game.Content.Load<Texture2D>("SfxOff");
            buttonMusicOn = game.Content.Load<Texture2D>("MusicON");
            buttonMusicOFF = game.Content.Load<Texture2D>("MusicOFF");
            buttonSetting = game.Content.Load<Texture2D>("Setting");
            buttonExit = game.Content.Load<Texture2D>("ExitButton");
            buttonSelect.Load(game.Content, "Select", 4, 1, 5);
            sChat = game.Content.Load<Texture2D>("StartChat");

            Player.walkAni.Pause();
            #endregion

            game.IsMouseVisible = true;
            Player.hp = 5;
            enemyBlack.hp = 3;
            enemyBlack2.hp = 5;
            enemyGold.hp = 5;
            enemyWizard.hp = 3;
            enemyRed.hp = 5;

            // Cloud
            for (int i = 0; i < 7; i++)
            {
                Cloud_Pos[i].X = 50 + (i * 500);
                Cloud_Pos[i].Y = r.Next(0, 60);
                speedCloud[i] = (float)0.1 * (r.Next(4, 6));
            }
            for (int i = 0; i < 5; i++)
            {
                Cloud_Pos2[i].X = r.Next(500, 2700 + ((i + 2) * 20));
                Cloud_Pos2[i].Y = r.Next(0, 70);
                speedCloud2[i] = (float)0.1 * r.Next(6, 10);
            }

            potion_Ena[0] = false;
            potion_Use[0] = false;
            potion_Pos[0] = new Vector2(700, 390);

            potion_Ena[1] = false;
            potion_Use[1] = false;
            potion_Pos[1] = new Vector2(2200, 390);

            select_Pos = new Vector2(340, 185);

            Player.Position = new Vector2(50, 255);

            enemyBlack.Position = new Vector2(800, 255);

            enemyBlack2.Position = new Vector2(1500, 255);

            enemyGold.Position = new Vector2(3000, 255);

            enemyWizard.Position = new Vector2(2600, 255);

            enemyRed.Position = new Vector2(3500, 255);

            this.game = game;
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.7f;
            MediaPlayer.Play(bgSong);
        }

        public override void Update(GameTime theTime)
        {
            float elapsed = (float)theTime.ElapsedGameTime.TotalSeconds;

            GameplayUpdate(theTime, elapsed);
            if (!resetValue)
            {
                ResetValue(theTime);
                resetValue = true;
            }

            base.Update(theTime);
        }

        public override void Draw(SpriteBatch theBatch, GameTime theTime)
        {
            game.GraphicsDevice.Clear(Color.Black);

            DrawGameplay(theBatch, theTime);
            
            base.Draw(theBatch, theTime);
        }

        public void ResetValue(GameTime theTime)
        {
            string fileName = @"Content\Dead.txt";
            if (File.Exists(fileName))
            {
                string filepathDead = Path.Combine(@"Content\Dead.txt");
                FileStream fsDead = new FileStream(filepathDead, FileMode.Open, FileAccess.Read);
                StreamReader srDead = new StreamReader(fsDead);
                string tmpStrDead = srDead.ReadLine();
                dead_count = Convert.ToInt32(tmpStrDead);
                srDead.Close();
            }
            else
            {
                string filepath = Path.Combine(@"Content\Dead.txt");
                FileStream fc = new FileStream(filepath, FileMode.CreateNew);
                fc.Close();
            }

            load = true;
            loadOn = false;
            Switch = "loading";
            setting = false;

            MediaPlayer.MediaStateChanged -= MediaPlayer_MediaStateChanged;
            PauseTime = TimeSpan.FromMilliseconds(0);
            PauseTime2 = TimeSpan.FromMilliseconds(0);
            bg1Song = false;
            select = 0;
            gamePause = false;
            enemyBlack.died = false;
            enemyBlack2.died = false;
            enemyGold.died = false;
            enemyWizard.died = false;
            enemyRed.died = false;
            fireBallCount = 0;
            ai4_bigFireball = false;
            enemyBlack.Position = new Vector2(800, 255);
            enemyBlack2.Position = new Vector2(1500, 255);
            enemyGold.Position = new Vector2(3000, 255);
            enemyWizard.Position = new Vector2(2600, 255);
            enemyRed.Position = new Vector2(3500, 255);
            Player.Position = new Vector2(50, 255);
            Camera_Pos = new Vector2(700, 255);
            special_Pos = new Vector2(500, 600);
            enemyBlack.hp = 3;
            enemyBlack2.hp = 5;
            enemyGold.hp = 5;
            enemyWizard.hp = 3;
            enemyRed.hp = 5;
            potion_Count = 0;
            Player.SpeCount = 0;
            potion_Pos[0] = new Vector2(700, 390);
            potion_Pos[1] = new Vector2(2200, 390);
            potion_Ena[0] = false;
            potion_Use[0] = false;
            potion_Ena[1] = false;
            potion_Use[1] = false;
            Player.diedAni.Pause(0, 0);
            Player.hp = 5;
            Player.died = false;

        }

        private void GameplayUpdate(GameTime theTime, float elapsed)
        {
            Game1.State = "Game1";
            if (load)
            {
                menuLoading = true;

                if (!loadOn) { lastTimeLoad = theTime.TotalGameTime; loadOn = true; }
            }
            if (Switch == "InGame1")
            {
                if (!bg1Song)
                {
                    MediaPlayer.Play(bgSong);
                    bg1Song = true;
                }
                if (Game1.MusicOn)
                {
                    MediaPlayer.IsMuted = false;
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
                    if (walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                }

                // Key Pause
                GameKeyPause(theTime);

                if (!gamePause)
                {
                    if (!Player.died)
                    {
                        // Time Pause
                        if (lastTimePauseOn + TimeSpan.FromMilliseconds(2500) < theTime.TotalGameTime)
                        {
                            PauseTime = TimeSpan.FromMilliseconds(0);
                        }
                        if (lastTimePauseOn + TimeSpan.FromMilliseconds(800) < theTime.TotalGameTime)
                        {
                            PauseTime2 = TimeSpan.FromMilliseconds(0);
                        }

                        #region PlayerHeartPos
                        // Player heart
                        for (int i = 0; i < 5; i++)
                        {
                            Player.Heart_Pos[i].X = (Player.Position.X - 43) - (i + 1) * -22;
                            Player.Heart_Pos[i].Y = Player.Position.Y - 25;
                        }

                        // Enemy heart
                        for (int i = 0; i < 3; i++)
                        {
                            enemyBlack.Heart_Pos[i].X = (enemyBlack.Position.X + 43) - (i + 1) * -22;
                            enemyBlack.Heart_Pos[i].Y = enemyBlack.Position.Y - 15;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            enemyBlack2.Heart_Pos[i].X = (enemyBlack2.Position.X + 20) - (i + 1) * -22;
                            enemyBlack2.Heart_Pos[i].Y = enemyBlack2.Position.Y - 15;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            enemyGold.Heart_Pos[i].X = (enemyGold.Position.X + 20) - (i + 1) * -22;
                            enemyGold.Heart_Pos[i].Y = enemyGold.Position.Y - 15;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            enemyWizard.Heart_Pos[i].X = (enemyWizard.Position.X + 20) - (i + 1) * -22;
                            enemyWizard.Heart_Pos[i].Y = enemyWizard.Position.Y - 15;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            enemyRed.Heart_Pos[i].X = (enemyRed.Position.X + 20) - (i + 1) * -22;
                            enemyRed.Heart_Pos[i].Y = enemyRed.Position.Y - 15;
                        }

                        #endregion
                        // Glass
                        for (int i = 0; i < 70; i++)
                        {
                            glassPos[i].X = i * 64;
                            glassPos[i].Y = 320;
                        }

                        // Cloud
                        for (int i = 0; i < 7; i++)
                        {
                            Cloud_Pos[i].X -= speedCloud[i];
                            if (Cloud_Pos[i].X <= -160)
                            {
                                Cloud_Pos[i].X = r.Next(3680, 3750) + ((i + 2) * 20);
                            }
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            Cloud_Pos2[i].X -= speedCloud2[i];
                            if (Cloud_Pos2[i].X <= -100)
                            {
                                Cloud_Pos2[i].X = r.Next(3680, 3750) + ((i + 2) * 20);
                            }
                        }

                        // KeyDown
                        GameKeyDown();

                        // Key Attack
                        GameKeyAttack(theTime);

                        Rectangle[] charPotion = new Rectangle[2];
                        charPotion[0] = new Rectangle((int)potion_Pos[0].X, (int)potion_Pos[0].Y, 32, 32);
                        charPotion[1] = new Rectangle((int)potion_Pos[1].X, (int)potion_Pos[1].Y, 32, 32);
                        enemyBlack.charBlock = new Rectangle((int)enemyBlack.Position.X + 64, (int)enemyBlack.Position.Y, 32, 160);
                        enemyBlack2.charBlock = new Rectangle((int)enemyBlack2.Position.X + 64, (int)enemyBlack2.Position.Y, 32, 160);
                        enemyGold.charBlock = new Rectangle((int)enemyGold.Position.X + 64, (int)enemyGold.Position.Y, 32, 160);
                        enemyWizard.charBlock = new Rectangle((int)enemyWizard.Position.X + 30, (int)enemyWizard.Position.Y, 32, 160);
                        enemyRed.charBlock = new Rectangle((int)enemyRed.Position.X + 64, (int)enemyRed.Position.Y, 32, 160);
                        Rectangle blockEnemy3Wave;
                        Rectangle blockEnemy4Fire;
                        Rectangle blockEnemy4BigFire;

                        #region PlayerAbility
                        if (special == true)
                        {
                            Player.charBlock = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 125, 160);
                            Player.charSpecialAtk = new Rectangle((int)special_Pos.X, (int)special_Pos.Y, 20, 32);
                            special_Pos.X += 15;
                            if (Player.charSpecialAtk.Intersects(enemyBlack.charBlock) && enemyBlack.died == false)
                            {
                                enemyBlack.PlayerSpecialAtk(theTime);
                            }
                            else if (Player.charSpecialAtk.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                            {
                                enemyBlack2.PlayerSpecialAtk(theTime);
                            }
                            else if (Player.charSpecialAtk.Intersects(enemyGold.charBlock) && enemyGold.died == false)
                            {
                                if (enemyGold.lastTimeKnockBack + enemyGold.DelayKnockBack < theTime.TotalGameTime)
                                {
                                    enemyGold.knockBack = true;

                                    enemyGold.lastTimeKnockBack = theTime.TotalGameTime;
                                }
                                if (enemyGold.lastTimeHp + enemyGold.DelayHp < theTime.TotalGameTime)
                                {
                                    enemyGold.hp -= 2;

                                    enemyGold.lastTimeHp = theTime.TotalGameTime;
                                }
                            }
                            else if (Player.charSpecialAtk.Intersects(enemyWizard.charBlock) && enemyWizard.died == false)
                            {
                                if (enemyWizard.lastTimeKnockBack + enemyWizard.DelayKnockBack < theTime.TotalGameTime)
                                {
                                    enemyWizard.knockBack = true;

                                    enemyWizard.lastTimeKnockBack = theTime.TotalGameTime;
                                }
                                if (enemyWizard.lastTimeHp + enemyWizard.DelayHp < theTime.TotalGameTime)
                                {
                                    enemyWizard.hp -= 1;

                                    enemyWizard.lastTimeHp = theTime.TotalGameTime;
                                }
                            }
                            else if (Player.charSpecialAtk.Intersects(enemyRed.charBlock) && enemyRed.died == false)
                            {
                                if (enemyRed.lastTimeKnockBack + enemyRed.DelayKnockBack < theTime.TotalGameTime)
                                {
                                    enemyRed.knockBack = true;
                                    special_Pos = new Vector2(500, 600);
                                    enemyRed.lastTimeKnockBack = theTime.TotalGameTime;
                                }
                                if (enemyRed.lastTimeHp + enemyRed.DelayHp < theTime.TotalGameTime)
                                {
                                    enemyRed.hp -= 3;

                                    enemyRed.lastTimeHp = theTime.TotalGameTime;
                                }
                            }
                        }
                        if (Player.atk == true)
                        {
                            Player.charBlock = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 125, 160);
                            enemyWizard.charBlock = new Rectangle((int)enemyWizard.Position.X, (int)enemyWizard.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyBlack.charBlock) && enemyBlack.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyBlack.PlayerAtk(theTime, SwordHit);
                            }
                            else if (Player.charBlock.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyBlack2.PlayerAtk(theTime, SwordHit);
                            }
                            else if (Player.charBlock.Intersects(enemyGold.charBlock) && enemyGold.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyGold.PlayerAtk(theTime, SwordHit);
                            }
                            else if (Player.charBlock.Intersects(enemyWizard.charBlock) && enemyWizard.died == false)
                            {
                                effect2.Play();
                                effect2_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyWizard.PlayerAtk(theTime, SwordHit);
                            }
                            else if (Player.charBlock.Intersects(enemyRed.charBlock) && enemyRed.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyRed.PlayerAtk(theTime, SwordHit);
                            }
                        }
                        else
                        {
                            if (def == true)
                            {
                                Player.charBlock = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 100, 160);
                            }
                            else
                            {
                                Player.charBlock = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 80, 160);
                                if (potion_Ena[0] == false)
                                {
                                    if (Player.charBlock.Intersects(charPotion[0]))
                                    {
                                        potion_Count += 1;
                                        potion_Ena[0] = true;
                                    }
                                }
                                if (potion_Ena[1] == false)
                                {
                                    if (Player.charBlock.Intersects(charPotion[1]))
                                    {
                                        potion_Count += 1;
                                        potion_Use[0] = false;
                                        potion_Ena[1] = true;
                                    }
                                }
                            }
                        }
                        if (lasttimeEffect + effectfadeOut < theTime.TotalGameTime && !Player.atk)
                        {
                            effect1_Pos = new Vector2(700, 600);
                            effect1.Reset();
                            effect1.Pause(2, 0);
                            effect2_Pos = new Vector2(700, 600);
                            effect2.Reset();
                            effect2.Pause(2, 0);
                        }
                        #endregion

                        #region EnemyAbility
                        // Enemy 1
                        if (enemyBlack.atk == true && enemyBlack.died == false)
                        {
                            enemyBlack.charBlock = new Rectangle((int)enemyBlack.Position.X + 18, (int)enemyBlack.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyBlack.charBlock) && enemyBlack.died == false)
                            {
                                if (def == true)
                                {
                                    Player.WhenDefTrue(theTime, soundEffects);
                                }
                                else
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }
                        else
                        {
                            if (enemyBlack.died == false)
                            {
                                enemyBlack.charBlock = new Rectangle((int)enemyBlack.Position.X + 96, (int)enemyBlack.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyBlack.charBlock) && enemyBlack.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }

                        // Enemy 2 atk
                        if (enemyBlack2.atk == true && enemyBlack.died == true)
                        {
                            enemyBlack2.charBlock = new Rectangle((int)enemyBlack2.Position.X + 18, (int)enemyBlack2.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                            {
                                if (def == true)
                                {
                                    Player.WhenDefTrue(theTime, soundEffects);
                                }
                                else
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }
                        else
                        {
                            if (enemyBlack2.died == false && enemyBlack.died == true)
                            {
                                enemyBlack2.charBlock = new Rectangle((int)enemyBlack2.Position.X + 96, (int)enemyBlack2.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }

                        // Enemy 3 atk
                        if (ai3_Wave == true && enemyBlack.died == true && enemyBlack2.died == true)
                        {
                            if (!ai3_Use)
                            {
                                Ai3WavePos = new Vector2(enemyGold.Position.X, enemyGold.Position.Y);
                                ai3_Use = true;
                            }
                            Ai3WavePos.X -= 7;
                            blockEnemy3Wave = new Rectangle((int)Ai3WavePos.X, (int)Ai3WavePos.Y, 50, 160);
                            if (Player.charBlock.Intersects(blockEnemy3Wave) && enemyGold.died == false)
                            {
                                if (def == true)
                                {
                                    Player.WhenDefTrue(theTime, soundEffects);

                                    Ai3WavePos.Y = 600;
                                    ai3_Wave = false;
                                }
                                else
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);

                                    Ai3WavePos.Y = 600;
                                    ai3_Wave = false;
                                }
                            }
                        }
                        if (enemyGold.atk == true && enemyBlack.died == true && enemyBlack2.died == true)
                        {
                            enemyGold.charBlock = new Rectangle((int)enemyGold.Position.X + 18, (int)enemyGold.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyGold.charBlock) && enemyGold.died == false && ai3_Wave == false)
                            {
                                if (def == true)
                                {
                                    Player.WhenDefTrue(theTime, soundEffects);
                                }
                                else
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }
                        else
                        {
                            if (enemyGold.died == false && enemyBlack.died == true && enemyBlack2.died == true)
                            {
                                enemyGold.charBlock = new Rectangle((int)enemyGold.Position.X + 96, (int)enemyGold.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyGold.charBlock) && enemyGold.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }

                        // Enemy 4 atk
                        if (ai4_Fireball == true && enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true)
                        {
                            if (ai4_bigFireball == false)
                            {
                                if (fireBallCount == 5)
                                {
                                    ai4_bigFireball = true;
                                    fireBallCount = 0;
                                }
                            }
                            if (ai4_bigFireball == true)
                            {
                                if (!ai4_Use)
                                {
                                    Ai4FirePos = new Vector2(enemyWizard.Position.X + 15, enemyWizard.Position.Y + 20);
                                    ai4_Use = true;
                                }
                                Ai4FirePos.X -= 7;
                                blockEnemy4BigFire = new Rectangle((int)Ai4FirePos.X, (int)Ai4FirePos.Y, 80, 80);
                                if (Player.charBlock.Intersects(blockEnemy4BigFire) && enemyWizard.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);

                                    Ai4FirePos.Y = 600;
                                    ai4_Fireball = false;
                                }
                            }
                            else if (!ai4_bigFireball)
                            {
                                if (!ai4_Use)
                                {
                                    Ai4FirePos = new Vector2(enemyWizard.Position.X + 15, enemyWizard.Position.Y + 20);
                                    fireBallCount += 1;
                                    ai4_Use = true;
                                }
                                Ai4FirePos.X -= 10;
                                blockEnemy4Fire = new Rectangle((int)Ai4FirePos.X, (int)Ai4FirePos.Y, 40, 40);
                                if (Player.charBlock.Intersects(blockEnemy4Fire) && enemyWizard.died == false)
                                {
                                    if (def == true)
                                    {
                                        Player.WhenDefTrue(theTime, soundEffects);

                                        Ai4FirePos.Y = 600;
                                        ai4_Fireball = false;
                                    }
                                    else
                                    {
                                        Player.WhenDefFalse(theTime, soundEffects);

                                        Ai4FirePos.Y = 600;
                                        ai4_Fireball = false;
                                    }
                                }
                            }
                        }
                        if (enemyWizard.atk == true && enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true)
                        {
                            enemyWizard.charBlock = new Rectangle((int)enemyWizard.Position.X + 35, (int)enemyWizard.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyWizard.charBlock) && enemyWizard.died == false && ai4_Fireball == false)
                            {
                                Player.WhenDefFalse(theTime, soundEffects);
                            }
                        }
                        else
                        {
                            if (enemyWizard.died == false && enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true)
                            {
                                enemyWizard.charBlock = new Rectangle((int)enemyWizard.Position.X + 35, (int)enemyWizard.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyWizard.charBlock) && enemyWizard.died == false && ai4_Fireball == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }

                        // Enemy 5 atk
                        if (enemyRed.atk == true && enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true && enemyWizard.died == true)
                        {
                            enemyRed.charBlock = new Rectangle((int)enemyRed.Position.X + 18, (int)enemyRed.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyRed.charBlock) && enemyRed.died == false)
                            {
                                if (def == true)
                                {
                                    Player.WhenDefTrue(theTime, soundEffects);

                                }
                                else
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }
                        else
                        {
                            if (enemyRed.died == false && enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true && enemyWizard.died == true)
                            {
                                enemyRed.charBlock = new Rectangle((int)enemyRed.Position.X + 96, (int)enemyRed.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyRed.charBlock) && enemyRed.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }

                        if (Player.died == false)
                        {
                            // Enemy 1
                            if (enemyBlack.died == false)
                            {
                                if (enemyBlack.lastTimeHp + enemyBlack.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyBlack.Position.X - Player.Position.X >= 61)
                                    {
                                        if (eAtkTime + eDelayAtk + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack.walkAni.Play();
                                            enemyBlack.atk = false;
                                            enemyBlack.Position.X -= 1;
                                            enemyBlack.atkAni.Pause(0, 0);
                                        }
                                        else if (eAtkTime + eCoolDownAtk + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack.Position.X -= 1;
                                            enemyBlack.walkAni.Play();
                                            enemyBlack.atk = false;
                                        }
                                    }
                                    else
                                    {
                                        enemyBlack.atkAni.Play();
                                        if (eAtkTime + eDelayAtk + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack.atk = true;

                                            eAtkTime = theTime.TotalGameTime;
                                        }
                                        if (eAtkTime + eCoolDownAtk + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack.walkAni.Play();
                                            enemyBlack.atk = false;
                                        }
                                    }
                                }
                            }

                            // Enemy 2
                            if (enemyBlack.died == true)
                            {
                                if (enemyBlack2.lastTimeHp + enemyBlack2.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyBlack2.Position.X - Player.Position.X >= 61)
                                    {
                                        if (eAtkTime2 + eDelayAtk2 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.walkAni.Play();
                                            enemyBlack2.atk = false;
                                            enemyBlack2.Position.X -= 2;
                                            enemyBlack2.atkAni.Pause(0, 0);
                                        }
                                        else if (eAtkTime2 + eCoolDownAtk2 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.Position.X -= 2;
                                            enemyBlack2.walkAni.Play();
                                            enemyBlack2.atk = false;
                                        }
                                    }
                                    else
                                    {
                                        enemyBlack2.atkAni.Play();
                                        if (eAtkTime2 + eDelayAtk2 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.atk = true;

                                            eAtkTime2 = theTime.TotalGameTime;
                                        }
                                        if (eAtkTime2 + eCoolDownAtk2 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.walkAni.Play();
                                            enemyBlack2.atk = false;
                                        }
                                    }
                                }
                            }

                            // Enemy 3
                            if (enemyBlack.died == true && enemyBlack2.died == true)
                            {
                                if (enemyGold.lastTimeHp + enemyGold.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyGold.Position.X - Player.Position.X >= 120)
                                    {
                                        eDelayAtk3 = TimeSpan.FromMilliseconds(1500);
                                        if (eAtkTime3 + eDelayAtk3 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyGold.atk = true;
                                            ai3_Wave = true;
                                            ai3_Use = false;
                                            AttackWave3 = theTime.TotalGameTime;
                                            eAtkTime3 = theTime.TotalGameTime;
                                        }
                                        else if (eAtkTime3 + eCoolDownAtk3 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            enemyGold.Position.X -= 2;
                                            enemyGold.walkAni.Play();
                                            enemyGold.atk = false;
                                        }
                                    }
                                    else if (enemyGold.Position.X - Player.Position.X >= 61 && enemyGold.Position.X - Player.Position.X < 120)
                                    {
                                        if (enemyGold.atk == false)
                                        {
                                            enemyGold.Position.X -= 2;
                                        }
                                        if (eAtkTime3 + eCoolDownAtk3 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            enemyGold.walkAni.Play();
                                            enemyGold.Position.X -= 1;
                                            enemyGold.atk = false;
                                        }
                                    }
                                    else if (enemyGold.Position.X - Player.Position.X < 61)
                                    {
                                        eDelayAtk3 = TimeSpan.FromMilliseconds(1200);
                                        if (eAtkTime3 + eDelayAtk3 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyGold.atkAni.Play();
                                            enemyGold.atk = true;

                                            eAtkTime3 = theTime.TotalGameTime;
                                        }
                                        if (eAtkTime3 + eCoolDownAtk3 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            enemyGold.atkAni.Pause(0, 0);
                                            enemyGold.walkAni.Play();
                                            enemyGold.Position.X -= 1;
                                            enemyGold.atk = false;
                                        }
                                    }
                                }
                            }
                            if (AttackWave3 + DelayAttackWave3 + PauseTime < theTime.TotalGameTime)
                            {
                                ai3_Wave = false;
                                Ai3WavePos.Y = 600;
                            }

                            // Enemy 4
                            if (enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true)
                            {
                                if (enemyWizard.lastTimeHp + enemyWizard.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyWizard.Position.X - Player.Position.X < 500)
                                    {
                                        enemyWizard.atkAni.Play();
                                        if (eAtkTime4 + eDelayAtk4 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyWizard.idle = false;
                                            enemyWizard.atk = true;
                                            ai4_Fireball = true;
                                            ai4_Use = false;

                                            AttackWave4 = theTime.TotalGameTime;
                                            eAtkTime4 = theTime.TotalGameTime;
                                        }
                                        else if (eAtkTime4 + eCoolDownAtk4 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            enemyWizard.atk = false;
                                            enemyWizard.idle = true;
                                        }
                                        if (AttackWave4 + DelayAttackWave4 + PauseTime2 < theTime.TotalGameTime)
                                        {
                                            ai4_Fireball = false;
                                            if (ai4_bigFireball)
                                            {
                                                fireBallCount = 0;
                                                ai4_bigFireball = false;
                                            }
                                            Ai4FirePos.Y = 600;
                                        }
                                    }
                                }
                            }

                            // Enemy 5
                            if (enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true && enemyWizard.died == true)
                            {
                                if (enemyRed.Position.X - Player.Position.X >= 61)
                                {
                                    if (eAtkTime5 + eDelayAtk5 + PauseTime < theTime.TotalGameTime)
                                    {
                                        enemyRed.walkAni.Play();
                                        enemyRed.atk = false;
                                        enemyRed.Position.X -= 5;
                                        enemyRed.atkAni.Pause(0, 0);
                                    }
                                    else if (eAtkTime5 + eCoolDownAtk5 + PauseTime2 < theTime.TotalGameTime)
                                    {
                                        enemyRed.Position.X -= 5;
                                        enemyRed.walkAni.Play();
                                        enemyRed.atk = false;
                                    }
                                }
                                else
                                {
                                    if (eAtkTime5 + eDelayAtk5 + PauseTime < theTime.TotalGameTime)
                                    {
                                        enemyRed.atkAni.Play();
                                        enemyRed.atk = true;

                                        eAtkTime5 = theTime.TotalGameTime;
                                    }
                                    if (eAtkTime5 + eCoolDownAtk5 + PauseTime2 < theTime.TotalGameTime)
                                    {
                                        enemyRed.walkAni.Play();
                                        enemyRed.atk = false;
                                        enemyRed.atkAni.Pause(0, 0);
                                    }
                                }
                            }
                            #endregion

                            #region KnockBack

                            Player.PlayerKnockBack(theTime);

                            // Enemy 1 knockBack
                            if (enemyBlack.died == false)
                            {
                                enemyBlack.EnemyKnockBack(theTime);
                            }
                            // Enemy 2 knockBack
                            if (enemyBlack.died == true && enemyBlack2.died == false)
                            {
                                enemyBlack2.EnemyKnockBack(theTime);
                            }
                            // Enemy 3 knockBack
                            if (enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == false)
                            {
                                enemyGold.EnemyKnockBack(theTime);
                            }
                            // Enemy 4 knockBack
                            if (enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true && enemyWizard.died == false)
                            {
                                enemyWizard.EnemyKnockBack(theTime);
                            }
                            // Enemy 5 knockBack
                            if (enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true && enemyWizard.died == true && enemyRed.died == false)
                            {
                                if (enemyRed.knockBack == true)
                                {
                                    enemyRed.Position.X += 22;
                                    enemyRed.Position.Y -= 6;
                                }
                                else
                                {
                                    if (enemyRed.Position.Y < 255)
                                    {
                                        enemyRed.Position.Y += 12;
                                    }
                                    if (enemyRed.Position.Y > 255)
                                    {
                                        enemyRed.Position.Y = 255;
                                    }
                                }
                            }
                            if (enemyRed.lastTimeKnockBack + enemyRed.DelayKnockBack < theTime.TotalGameTime)
                            {
                                enemyRed.knockBack = false;
                            }
                        }
                        else
                        {
                            enemyBlack.atkAni.Pause(0, 0);
                            enemyBlack.walkAni.Pause(0, 0);
                            enemyBlack.idleAni.Pause(0, 0);
                            enemyBlack2.atkAni.Pause(0, 0);
                            enemyBlack2.walkAni.Pause(0, 0);
                            enemyBlack2.idleAni.Pause(0, 0);
                            enemyGold.atkAni.Pause(0, 0);
                            enemyGold.walkAni.Pause(0, 0);
                            enemyWizard.atkAni.Pause(0, 0);
                            enemyRed.atkAni.Pause(0, 0);
                            enemyRed.walkAni.Pause(0, 0);
                        }
                        #endregion

                        #region EnemyDiedCheck
                        if (enemyBlack.hp <= 0)
                        {
                            enemyBlack.died = true;
                            enemyBlack.atk = false;
                            enemyBlack.atkAni.Pause(0, 0);
                            enemyBlack.walkAni.Pause(0, 0);
                            enemyBlack.idleAni.Pause(0, 0);
                        }
                        if (enemyBlack2.hp <= 0)
                        {
                            enemyBlack2.died = true;
                            enemyBlack2.atk = false;
                            enemyBlack2.atkAni.Pause(0, 0);
                            enemyBlack2.walkAni.Pause(0, 0);
                            enemyBlack2.idleAni.Pause(0, 0);
                        }
                        if (enemyGold.hp <= 0)
                        {
                            enemyGold.died = true;
                            enemyGold.atk = false;
                            enemyGold.atkAni.Pause(0, 0);
                            enemyGold.walkAni.Pause(0, 0);
                        }
                        if (enemyWizard.hp <= 0)
                        {
                            enemyWizard.died = true;
                            enemyWizard.atkAni.Pause(0, 0);
                            enemyWizard.idleAni.Pause(0, 0);
                        }
                        if (enemyRed.hp <= 0)
                        {
                            enemyRed.died = true;
                            enemyRed.atk = false;
                            enemyRed.atkAni.Pause(0, 0);
                            enemyRed.walkAni.Pause(0, 0);
                        }
                        #endregion

                        #region Check
                        // Potion
                        if (potion_Count >= 1) { potion_Count = 1; }
                        if (potion_Count == 1)
                        {
                            if (keyboardState.IsKeyDown(Keys.E))
                            {
                                soundEffects[5].Play(volume: 1.0f, pitch: 0.0f, pan: 0.0f);
                                potion_Count = 0;
                                if (Player.hp < 5) { Player.hp += 2; }
                                if (Player.hp > 5) { Player.hp = 5; }
                                potion_Use[0] = true;
                            }
                        }
                        #endregion

                        if (Camera_Pos.X - Player.Position.X >= 80 && menuLoading == false)
                        {
                            Camera_Pos.X -= 2;
                        }
                        else if (Camera_Pos.X - Player.Position.X <= -80 && menuLoading == false)
                        {
                            if (3680 - Camera_Pos.X > 625)
                            {
                                Camera_Pos.X += 2;
                            }
                        }

                        #region Visible When GetHit
                        if (Player.GetHitTime + Player.GetHitDelay < theTime.TotalGameTime)
                        {
                            Player.getHit = false;
                        }
                        if (Player.GetHitTime + Player.GetHitDelay < theTime.TotalGameTime && Player.playerInvi && Player.GetHitTime + Player._GetHitDelay < theTime.TotalGameTime)
                        {
                            Player.getHit = true;

                            if (Player.GetHitTime + Player.GetHitDelay + Player._GetHitDelay < theTime.TotalGameTime)
                            {
                                Player.getHit = false;

                                if (Player.GetHitTime + Player.GetHitDelay + Player._GetHitDelay2 < theTime.TotalGameTime)
                                {
                                    Player.getHit = true;

                                    Player.GetHitTime = theTime.TotalGameTime;
                                    Player.playerInvi = false;
                                }
                            }
                        }
                        #endregion

                        // Rabbit
                        if (rabbit_Pos.X < 1100 && rab_direction == false)
                        {
                            rabbit_Pos.X += 1;
                            _rab_direction = true;
                            if (rabbit_Pos.X >= 1100)
                            {
                                rab_direction = true;
                            }
                        }
                        if (rab_direction == true)
                        {
                            rabbit_Pos.X -= 1;
                            _rab_direction = false;
                            if (rabbit_Pos.X <= 500)
                            {
                                rab_direction = false;
                            }
                        }

                        if (Player.Position.X >= 3610)
                        {
                            Player.Position.X -= 2;
                        }
                        else if (Player.Position.X < 0)
                        {
                            Player.Position.X += 2;
                        }

                        camera.Update(Camera_Pos);
                    }
                }

                if (ChatOn)
                {
                    gamePause = true;
                }

                if (ChatOn)
                {
                    if (lasttimeChat + delayChat < theTime.TotalGameTime)
                    {
                        if (text == 1)
                        {
                            messages.Add(new DisplayMessage("   A wandering knight passed out in the forest. He couldn't remember anything.\nBut he had a memory that He had to help his brother in a mysterious castle.\nArriving at a point near the castle met someone who was trying to get rid of him.\nHe must fight and overcome obstacles to save his brother.", TimeSpan.FromSeconds(18.0), new Vector2
                                (75 - camera.ViewMatrix.Translation.X, 150 + messages.Count * 30 - camera.ViewMatrix.Translation.Y), Color.White));
                            text += 1;

                            _text = "F";

                            lasttimeChat = theTime.TotalGameTime;
                        }
                        else if (text == 2 && lasttimeChat + TimeSpan.FromSeconds(18.0) < theTime.TotalGameTime)
                        {
                            ChatOn = false;
                            gamePause = false;

                            lasttimeChat = theTime.TotalGameTime;
                        }
                    }
                }
            }

            UpdateMessages(theTime);

            #region DiedPlayer

            if (Player.Position.X >= 3600)
            {    
                {
                    if (walkSoundInstance.State != SoundState.Stopped)
                    {
                        walkSoundInstance.Stop();
                    }
                }
                string filepath = Path.Combine(@"Content\data.txt");
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                if (Switch == "InGame1")
                { sw.WriteLine("InGame2"); }
                sw.Flush();
                sw.Close();
                MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
                resetValue = false;
                Game1.State = "Title";
                ScreenEvent.Invoke(game.mGameplayScreen2, new EventArgs());
            }

            if (!devMode)
            {
                if (Player.hp <= 0 && Player.died == false)
                {
                    if (walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    if (Dead.State != SoundState.Playing) { Dead.Play(); }
                    Player.died = true;
                    Player.stop_move = true;
                    Player.diedAni.Play();
                    enemyBlack.atkAni.Pause(0, 0);
                    enemyBlack2.atkAni.Pause(0, 0);
                    enemyBlack2.walkAni.Pause(0, 0);
                    enemyBlack.walkAni.Pause(0, 0);
                    enemyGold.atkAni.Pause(0, 0);
                    enemyGold.walkAni.Pause(0, 0);
                    enemyWizard.atkAni.Pause(0, 0);
                    enemyWizard.idleAni.Pause(0, 0);
                    enemyRed.atkAni.Pause(0, 0);
                    enemyRed.walkAni.Pause(0, 0);

                    lastTimeDied = theTime.TotalGameTime;
                }
                else if (Player.died == true)
                {
                    Player.stop_move = true;
                }
            }

            if (lastTimeDied + intervalBetweenDied < theTime.TotalGameTime)
            {
                Player.diedAni.Pause();
                if (Player.died == true && lastTimeDied + intervalBetweenDied + intervalBetweenDied < theTime.TotalGameTime)
                {
                    string filepath = Path.Combine(@"Content\data.txt");
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    if (Switch == "InGame1")
                    { sw.WriteLine("InGame1"); }
                    sw.Flush();
                    sw.Close();
                    dead_count += 1;
                    string filepathDead = Path.Combine(@"Content\Dead.txt");
                    FileStream fsDead = new FileStream(filepathDead, FileMode.Open, FileAccess.Write);
                    StreamWriter swDead = new StreamWriter(fsDead);
                    if (Switch == "InGame1")
                    { swDead.WriteLine(dead_count.ToString()); }
                    swDead.Flush();
                    swDead.Close();
                    MediaPlayer.IsMuted = true;
                    if (Dead.State != SoundState.Stopped) { Dead.Stop(); }
                    ScreenEvent.Invoke(game.mTitleScreen, new EventArgs());
                    resetValue = false;
                }
            }
            #endregion

            // UpdateFrame
            UpdateFrame(elapsed);
        }

        private void DrawGameplay(SpriteBatch theBatch, GameTime theTime)
        {
            if (Switch == "InGame1" && menuLoading == false)
            {
                theBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.ViewMatrix);
                theBatch.Draw(BG1_1, new Vector2(0 - camera.ViewMatrix.Translation.X * 0.8f, 0), Color.White);
                for (int i = 0; i < 70; i++)
                {
                    glass.DrawFrame(theBatch, glassPos[i]);
                }

                // Cloud
                for (int i = 0; i < 7; i++)
                {
                    theBatch.Draw(Cloud1, Cloud_Pos[i], Color.White);
                }
                for (int i = 0; i < 5; i++)
                {
                    theBatch.Draw(Cloud2, Cloud_Pos2[i], Color.White);
                }
                theBatch.Draw(BG1_2, new Vector2(0, 0), Color.White);
                theBatch.Draw(BG1_3, new Vector2(0, 0), Color.White);

                // Potion
                if (potion_Ena[0] == true && potion_Use[0] == false)
                {
                    theBatch.Draw(potion, new Vector2(15 - camera.ViewMatrix.Translation.X, 35 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                else if (potion_Ena[0] == false)
                {
                    theBatch.Draw(potion, potion_Pos[0], Color.White);
                }
                if (potion_Ena[1] == false)
                {
                    theBatch.Draw(potion, potion_Pos[1], Color.White);
                }

                // Special
                switch (Player.SpeCount)
                {
                    case 0:
                        theBatch.Draw(special1, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 1:
                        theBatch.Draw(special2, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 2:
                        theBatch.Draw(special3, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 3:
                        theBatch.Draw(special4, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 4:
                        theBatch.Draw(special5, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                }

                #region HeartDraw
                for (int i = 0; i < Player.hp; i++)
                {
                    theBatch.Draw(Heart, Player.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.White);
                }
                for (int i = 0; i < enemyBlack.hp; i++)
                {
                    theBatch.Draw(HeartBlack, enemyBlack.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.White);
                }
                for (int i = 0; i < enemyBlack2.hp; i++)
                {
                    theBatch.Draw(HeartBlack, enemyBlack2.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.White);
                }
                for (int i = 0; i < enemyGold.hp; i++)
                {
                    theBatch.Draw(Heart, enemyGold.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Brown);
                }
                for (int i = 0; i < enemyWizard.hp; i++)
                {
                    theBatch.Draw(Heart, enemyWizard.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Black);
                }
                for (int i = 0; i < enemyRed.hp; i++)
                {
                    theBatch.Draw(Heart, enemyRed.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Black);
                }
                #endregion

                #region PlayerDraw
                if (Player.died == false)
                {
                    Cat_idle.DrawFrame(theBatch, Cat_Pos);
                    rabbit_idle.DrawFrame(theBatch, rabbit_Pos, _rab_direction);
                    if (!Player.getHit)
                    {
                        if (special == true)
                        {
                            Player.specialAtkAni.DrawFrame(theBatch, special_Pos);
                        }
                        if (special_ani == true)
                        {
                            Player.specialAni.DrawFrame(theBatch, Player.Position);
                        }
                        else if (Player.atk == true)
                        {
                            Player.atkAni.DrawFrame(theBatch, Player.Position);
                        }
                        else if (def == true)
                        {
                            Player.defAni.DrawFrame(theBatch, Player.Position);
                        }
                        else
                        {
                            if ((w_left == false && w_right == false) || (w_left == true && w_right == true))
                            {
                                Player.idleAni.Play();
                                if (gamePause) { Player.idleAni.Pause(); }
                                Player.idleAni.DrawFrame(theBatch, Player.Position);
                            }
                            else
                            {
                                if (w_left == true)
                                {
                                    if (Player.walkAni.IsPaused)
                                    {
                                        Player.walkAni.Play();
                                    }
                                    if (gamePause) { Player.walkAni.Pause(); }
                                    Player.walkAni.DrawFrame(theBatch, Player.Position);
                                }
                                if (w_right == true)
                                {
                                    if (Player.walkAni.IsPaused)
                                    {
                                        Player.walkAni.Play();
                                    }
                                    if (gamePause) { Player.walkAni.Pause(); }
                                    Player.walkAni.DrawFrame(theBatch, Player.Position);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Player.atkAni.Pause(0, 0);
                    Player.defAni.Pause(0, 0);
                    Player.specialAni.Pause(0, 0);
                    Player.specialAtkAni.Pause(0, 0);
                    Player.walkAni.Pause(0, 0);
                    Player.idleAni.Pause(0, 0);
                    Player.diedAni.DrawFrame(theBatch, Player.Position);
                    Cat_idle.DrawFrame(theBatch, Cat_Pos);
                    rabbit_idle.DrawFrame(theBatch, rabbit_Pos, _rab_direction);
                    theBatch.Draw(gameOver, new Vector2(350 - camera.ViewMatrix.Translation.X, 120 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                #endregion

                #region EnemyDraw
                // Enemy
                if (enemyBlack.died == false)
                {
                    if (enemyBlack.atk == true)
                    {
                        if (gamePause) { enemyBlack.atkAni.Pause(); }
                        if (Player.died) { enemyBlack.atkAni.Pause(0, 0); }
                        enemyBlack.atkAni.DrawFrame(theBatch, enemyBlack.Position);
                    }
                    else if (enemyBlack.atk == false)
                    {
                        if (gamePause) { enemyBlack.walkAni.Pause(); }
                        if (Player.died) { enemyBlack.walkAni.Pause(0, 0); }
                        enemyBlack.walkAni.DrawFrame(theBatch, enemyBlack.Position);
                    }
                }
                if (enemyBlack2.died == false && enemyBlack.died == true)
                {
                    if (enemyBlack2.atk == true)
                    {
                        if (gamePause) { enemyBlack2.atkAni.Pause(); }
                        if (Player.died) { enemyBlack2.atkAni.Pause(0, 0); }
                        enemyBlack2.atkAni.DrawFrame(theBatch, enemyBlack2.Position);
                    }
                    else if (enemyBlack2.atk == false)
                    {
                        if (gamePause) { enemyBlack2.walkAni.Pause(); }
                        if (Player.died) { enemyBlack2.walkAni.Pause(0, 0); }
                        enemyBlack2.walkAni.DrawFrame(theBatch, enemyBlack2.Position);
                    }
                }
                if (enemyGold.died == false && enemyBlack.died == true && enemyBlack2.died == true)
                {
                    if (ai3_Wave == true)
                    {
                        theBatch.Draw(eWaveAtk, Ai3WavePos, Color.White);
                    }
                    if (enemyGold.atk == true)
                    {
                        enemyGold.atkAni.Play();
                        if (gamePause) { enemyGold.atkAni.Pause(); }
                        if (Player.died) { enemyGold.atkAni.Pause(0, 0); }
                        enemyGold.atkAni.DrawFrame(theBatch, enemyGold.Position);
                    }
                    else if (enemyGold.atk == false)
                    {
                        enemyGold.walkAni.Play();
                        if (gamePause) { enemyGold.walkAni.Pause(); }
                        if (Player.died) { enemyGold.walkAni.Pause(0, 0); }
                        enemyGold.walkAni.DrawFrame(theBatch, enemyGold.Position);
                    }
                }
                if (enemyWizard.died == false && enemyGold.died == true && enemyBlack.died == true && enemyBlack2.died == true)
                {
                    if (ai4_bigFireball == true)
                    {
                        theBatch.Draw(bigFireball, Ai4FirePos, Color.White);
                    }
                    else if (ai4_Fireball == true)
                    {
                        theBatch.Draw(fireball, Ai4FirePos, Color.White);
                    }
                    if (enemyWizard.atk == true)
                    {
                        if (gamePause) { enemyWizard.atkAni.Pause(); }
                        if (Player.died) { enemyWizard.atkAni.Pause(0, 0); }
                        enemyWizard.atkAni.DrawFrame(theBatch, enemyWizard.Position);
                    }
                    else if (enemyWizard.idle == true)
                    {
                        if (gamePause) { enemyWizard.idleAni.Pause(); }
                        if (Player.died) { enemyWizard.idleAni.Pause(0, 0); }
                        enemyWizard.idleAni.DrawFrame(theBatch, enemyWizard.Position);
                    }
                }
                if (enemyRed.died == false && enemyBlack.died == true && enemyBlack2.died == true && enemyGold.died == true && enemyWizard.died == true)
                {
                    if (enemyRed.atk == true)
                    {
                        enemyRed.atkAni.Play();
                        if (gamePause) { enemyRed.atkAni.Pause(); }
                        if (Player.died) { enemyRed.atkAni.Pause(0, 0); }
                        enemyRed.atkAni.DrawFrame(theBatch, enemyRed.Position);
                    }
                    else if (enemyRed.atk == false)
                    {
                        enemyRed.walkAni.Play();
                        if (gamePause) { enemyRed.walkAni.Pause(); }
                        if (Player.died) { enemyRed.walkAni.Pause(0, 0); }
                        enemyRed.walkAni.DrawFrame(theBatch, enemyRed.Position);
                    }
                }
                #endregion

                // effect1
                if (!Player.died)
                {
                    effect2.DrawFrame(theBatch, effect2_Pos);
                    effect1.DrawFrame(theBatch, effect1_Pos);
                }

                string strDead = "Dead = " + dead_count;
                string strDevMode = "DevMode";
                if (Switch == "InGame1")
                {
                    theBatch.DrawString(ArialFont, strDead, new Vector2(470 - camera.ViewMatrix.Translation.X, 435 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                if (devMode)
                {
                    theBatch.DrawString(ArialFont, strDevMode, new Vector2(880 - camera.ViewMatrix.Translation.X, 435 - camera.ViewMatrix.Translation.Y), Color.Red);
                }

                if (gamePause && Switch == "InGame1" && !ChatOn)
                {
                    theBatch.Draw(pausePic, new Vector2(0 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                    if (setting)
                    {
                        buttonSelect.DrawFrame(theBatch, new Vector2(select_Pos.X - 10 - camera.ViewMatrix.Translation.X, select_Pos.Y - camera.ViewMatrix.Translation.Y));
                        if (select_Pos.Y == 185)
                        {
                            if (Game1.MusicOn)
                            {
                                theBatch.Draw(buttonMusicOn, new Vector2(427 - camera.ViewMatrix.Translation.X, 175 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                            }
                            else if (!Game1.MusicOn)
                            {
                                theBatch.Draw(buttonMusicOFF, new Vector2(427 - camera.ViewMatrix.Translation.X, 175 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                            }
                        }
                        else
                        {
                            if (Game1.MusicOn)
                            {
                                theBatch.Draw(buttonMusicOn, new Vector2(435 - camera.ViewMatrix.Translation.X, 180 - camera.ViewMatrix.Translation.Y), Color.White);
                            }
                            else if (!Game1.MusicOn)
                            {
                                theBatch.Draw(buttonMusicOFF, new Vector2(435 - camera.ViewMatrix.Translation.X, 180 - camera.ViewMatrix.Translation.Y), Color.White);
                            }
                        }
                        if (select_Pos.Y == 255)
                        {
                            if (Game1.SFXOn)
                            {
                                theBatch.Draw(buttonSoundOn, new Vector2(427 - camera.ViewMatrix.Translation.X, 245 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                            }
                            else if (!Game1.SFXOn)
                            {
                                theBatch.Draw(buttonSoundOff, new Vector2(427 - camera.ViewMatrix.Translation.X, 245 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                            }                        
                        }
                        else
                        {
                            if (Game1.SFXOn)
                            {
                                theBatch.Draw(buttonSoundOn, new Vector2(435 - camera.ViewMatrix.Translation.X, 250 - camera.ViewMatrix.Translation.Y), Color.White);
                            }
                            else if (!Game1.SFXOn)
                            {
                                theBatch.Draw(buttonSoundOff, new Vector2(435 - camera.ViewMatrix.Translation.X, 250 - camera.ViewMatrix.Translation.Y), Color.White);
                            }
                        }
                        if (select_Pos.Y == 325)
                        {
                            theBatch.Draw(buttonBack, new Vector2(427 - camera.ViewMatrix.Translation.X, 315 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            theBatch.Draw(buttonBack, new Vector2(435 - camera.ViewMatrix.Translation.X, 320 - camera.ViewMatrix.Translation.Y), Color.White);

                        }
                    }
                    else if (!setting)
                    {
                        if (select_Pos.Y == 185)
                        {
                            theBatch.Draw(buttonRetry, new Vector2(427 - camera.ViewMatrix.Translation.X, 175 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            theBatch.Draw(buttonRetry, new Vector2(435 - camera.ViewMatrix.Translation.X, 180 - camera.ViewMatrix.Translation.Y), Color.White);
                        }
                        if (select_Pos.Y == 325)
                        {
                            theBatch.Draw(buttonExit, new Vector2(427 - camera.ViewMatrix.Translation.X, 315 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            theBatch.Draw(buttonExit, new Vector2(435 - camera.ViewMatrix.Translation.X, 320 - camera.ViewMatrix.Translation.Y), Color.White);

                        }
                        theBatch.Draw(ButtonGuide, new Vector2(860 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                        buttonSelect.DrawFrame(theBatch, new Vector2(select_Pos.X - 10 - camera.ViewMatrix.Translation.X, select_Pos.Y - camera.ViewMatrix.Translation.Y));
                        if (select_Pos.Y == 255)
                        {
                            theBatch.Draw(buttonSetting, new Vector2(427 - camera.ViewMatrix.Translation.X, 245 - camera.ViewMatrix.Translation.Y), null, Color.White, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 0f);
                        }
                        else
                        {
                            theBatch.Draw(buttonSetting, new Vector2(435 - camera.ViewMatrix.Translation.X, 250 - camera.ViewMatrix.Translation.Y), Color.White);
                        }
                    }
                }
                else if (!gamePause && !ChatOn)
                {
                    theBatch.Draw(ButtonMenu, new Vector2(860 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                else if (gamePause && ChatOn && _text == "F")
                {
                    theBatch.Draw(sChat, new Vector2(0 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                    DrawMessages(theBatch);
                }
                theBatch.End();
            }
            if (menuLoading == true)
            {
                theBatch.Begin();
                loading.DrawFrame(theBatch, new Vector2(0, 0));
                Switch = "InGame1";
                if (lastTimeLoad + intervalBetweenLoad < theTime.TotalGameTime)
                {
                    load = false;
                    menuLoading = false;
                }
                theBatch.End();
            }
        }

        private void UpdateFrame(float Elapsed)
        {
            // Enemy Update frame
            enemyBlack.walkAni.UpdateFrame(Elapsed);
            enemyBlack.idleAni.UpdateFrame(Elapsed);
            enemyBlack.atkAni.UpdateFrame(Elapsed);
            enemyBlack2.walkAni.UpdateFrame(Elapsed);
            enemyBlack2.idleAni.UpdateFrame(Elapsed);
            enemyBlack2.atkAni.UpdateFrame(Elapsed);
            enemyGold.walkAni.UpdateFrame(Elapsed);
            enemyGold.atkAni.UpdateFrame(Elapsed);
            enemyWizard.atkAni.UpdateFrame(Elapsed);
            enemyRed.walkAni.UpdateFrame(Elapsed);
            enemyRed.atkAni.UpdateFrame(Elapsed);
            enemyWizard.idleAni.UpdateFrame(Elapsed);

            // Player Update frame
            Player.diedAni.UpdateFrame(Elapsed);
            Player.walkAni.UpdateFrame(Elapsed);
            Player.idleAni.UpdateFrame(Elapsed);
            Player.atkAni.UpdateFrame(Elapsed);
            Player.defAni.UpdateFrame(Elapsed);
            Player.specialAni.UpdateFrame(Elapsed);
            Player.specialAtkAni.UpdateFrame(Elapsed);

            // ฉาก Update frame
            glass.UpdateFrame(Elapsed);
            loading.UpdateFrame(Elapsed);
            Cat_idle.UpdateFrame(Elapsed);
            rabbit_idle.UpdateFrame(Elapsed);
            buttonSelect.UpdateFrame(Elapsed);
            effect1.UpdateFrame(Elapsed);
            effect2.UpdateFrame(Elapsed);
        }

        private void GameKeyDown()
        {
            keyboardState = Keyboard.GetState();
            if (!menuLoading)
            {
                if (keyboardState.IsKeyDown(Keys.Left) && Player.stop_move == false && !gamePause)
                {
                    Player.Position.X -= 2;
                    w_left = true;
                    if (keyboardState.IsKeyDown(Keys.Left) && walkSoundInstance.State != SoundState.Playing) { walkSoundInstance.Play(); }
                }
                else if (old_keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Left))
                {
                    Player.walkAni.Pause(0, 0);
                    w_left = false;
                    if (old_keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Left) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                }

                if (keyboardState.IsKeyDown(Keys.Right) && Player.stop_move == false && !gamePause)
                {
                    Player.Position.X += 2;
                    w_right = true;
                    if (keyboardState.IsKeyDown(Keys.Right) && walkSoundInstance.State != SoundState.Playing) { walkSoundInstance.Play(); }
                }
                else if (old_keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Right))
                {
                    Player.walkAni.Pause(0, 0);
                    w_right = false;
                    if (old_keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Right) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                }
                if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Left) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                old_keyboardState = keyboardState;
            }

        }

        private void GameKeyAttack(GameTime theTime)
        {
            keyboardState = Keyboard.GetState();
            if (!menuLoading)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A) && Player.lastTimeAttack + Player.DelayAttack < theTime.TotalGameTime && Player.lastTimeBlock + Player.CDBlock < theTime.TotalGameTime && Player.lastTimeSpecial + Player.DelaySAttack < theTime.TotalGameTime)
                {
                    if (keyboardState.IsKeyDown(Keys.A) && SwordWhoosh.State != SoundState.Playing) { SwordWhoosh.Play(); }
                    def = false;
                    special_ani = false;
                    Player.atk = true;
                    Player.stop_move = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.A) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    Player.atkAni.Play();
                    Player.walkAni.Pause(0, 0);

                    Player.lastTimeAttack = theTime.TotalGameTime;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && Player.lastTimeBlock + Player.DelayBlock < theTime.TotalGameTime && Player.lastTimeAttack + Player.CDAttack < theTime.TotalGameTime && Player.lastTimeSpecial + Player.DelaySAttack < theTime.TotalGameTime)
                {
                    Player.atk = false;
                    special_ani = false;
                    def = true;
                    Player.stop_move = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.S) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    Player.defAni.Play();
                    Player.walkAni.Pause(0, 0);

                    Player.lastTimeBlock = theTime.TotalGameTime;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.F) && Player.SpeCount == 4 && Player.lastTimeSpecial + Player.DelaySAttack < theTime.TotalGameTime && Player.lastTimeBlock + Player.CDBlock < theTime.TotalGameTime && Player.lastTimeAttack + Player.CDAttack < theTime.TotalGameTime)
                {
                    Player.atk = false;
                    def = false;
                    special = true;
                    special_ani = true;
                    Player.stop_move = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.F) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    soundEffects[6].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                    Player.specialAtkAni.Play();
                    Player.specialAni.Play();
                    Player.walkAni.Pause(0, 0);
                    special_Pos = new Vector2(Player.Position.X + 120, Player.Position.Y);
                    Player.SpeCount = 0;

                    Player.lastTimeSpecial = theTime.TotalGameTime;
                    Player.lastTimeSpecialDuring = theTime.TotalGameTime;
                }

                // ก่ารทำงาน
                if (Player.lastTimeAttack + Player.CDAttack < theTime.TotalGameTime && Player.lastTimeBlock + Player.CDBlock < theTime.TotalGameTime && Player.lastTimeSpecialDuring + Player.CDSAttack < theTime.TotalGameTime)
                {
                    if (SwordWhoosh.State != SoundState.Stopped) { SwordWhoosh.Stop(); }
                    Player.atk = false;
                    Player.stop_move = false;
                    def = false;
                    special_ani = false;
                    Player.specialAni.Pause(0, 0);
                    Player.atkAni.Pause(0, 0);
                    Player.defAni.Pause(0, 0);
                }
                if (Player.lastTimeSpecial + Player.TimeDuringSAttack + PauseTime < theTime.TotalGameTime)
                {
                    special = false;
                    special_Pos = new Vector2(500, 600);
                    Player.specialAtkAni.Pause(0, 0);
                }
            }
        }

        private void GameKeyPause(GameTime theTime)
        {
            keyboardState = Keyboard.GetState();
            if (lastTimePause + intervalBetweenPause < theTime.TotalGameTime)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !Player.died && !ChatOn)
                {
                    if (gamePause == false)
                    {
                        lastTimePauseOff = theTime.TotalGameTime;

                        enemyBlack.walkAni.Pause();
                        enemyBlack.atkAni.Pause();
                        enemyBlack2.atkAni.Pause();
                        enemyBlack2.walkAni.Pause();
                        enemyGold.atkAni.Pause();
                        enemyGold.walkAni.Pause();
                        enemyWizard.atkAni.Pause();
                        enemyWizard.idleAni.Pause();
                        enemyRed.atkAni.Pause();
                        enemyRed.walkAni.Pause();
                        Player.diedAni.Pause();
                        Player.walkAni.Pause();
                        Player.idleAni.Pause();
                        Player.atkAni.Pause();
                        Player.defAni.Pause();
                        Player.specialAni.Pause();
                        Player.specialAtkAni.Pause();

                        setting = false;
                        select_Pos = new Vector2(340, 185);
                        if (walkSoundInstance.State != SoundState.Paused) { walkSoundInstance.Pause(); }

                        gamePause = true;
                    }
                    else
                    {
                        lastTimePauseOn = theTime.TotalGameTime;
                        PauseTime = lastTimePauseOn - lastTimePauseOff;
                        PauseTime2 = lastTimePauseOn - lastTimePauseOff;

                        enemyBlack.walkAni.Play();
                        enemyBlack.atkAni.Play();
                        enemyBlack2.atkAni.Play();
                        enemyBlack2.walkAni.Play();
                        enemyGold.atkAni.Play();
                        enemyGold.walkAni.Play();
                        enemyWizard.atkAni.Play();
                        enemyWizard.idleAni.Play();
                        enemyRed.atkAni.Play();
                        enemyRed.walkAni.Play();
                        Player.diedAni.Play();
                        Player.walkAni.Play();
                        Player.idleAni.Play();
                        Player.atkAni.Play();
                        Player.defAni.Play();
                        Player.specialAni.Play();
                        Player.specialAtkAni.Play();

                        gamePause = false;
                    }
                    lastTimePause = theTime.TotalGameTime;
                }
            }
            if (lastTimePause + intervalBetweenPause < theTime.TotalGameTime)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.F1))
                {
                    if (devMode == false)
                    {
                        devMode = true;
                    }
                    else
                    {
                        devMode = false;
                    }
                    lastTimePause = theTime.TotalGameTime;
                }
            }
            if (gamePause && !ChatOn)
            {
                if (keyboardState.IsKeyDown(Keys.Down) && stopPress == false)
                {
                    if (lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                    {
                        if (!(select_Pos.Y == 325))
                        {
                            soundEffects[1].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                        }
                        if (select_Pos.Y <= 255)
                        {
                            select_Pos.Y += 70;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Up) && stopPress == false)
                {
                    if (lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                    {
                        if (!(select_Pos.Y == 185))
                        {
                            soundEffects[1].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                        }
                        if (select_Pos.Y >= 255)
                        {
                            select_Pos.Y -= 70;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.A) && stopPress == false && lastTimeSelect + intervalBetweenSelect < theTime.TotalGameTime)
                {
                    stopPress = true;
                    soundEffects[1].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                    if (setting)
                    {
                        if (select_Pos.Y == 185)
                        {
                            select = 1;
                        }
                        else if (select_Pos.Y == 255)
                        {
                            select = 2;
                        }
                        else if (select_Pos.Y == 325)
                        {
                            select = 3;
                        }

                        if (select == 1)
                        {
                            if (Game1.MusicOn)
                            {
                                stopPress = false;
                                Game1.MusicOn = false;
                                select = 0;

                                lastTimeSelect = theTime.TotalGameTime;
                            }
                            else if (!Game1.MusicOn)
                            {
                                stopPress = false;
                                Game1.MusicOn = true;
                                select = 0;

                                lastTimeSelect = theTime.TotalGameTime;
                            }
                        }
                        else if (select == 2)
                        {
                            if (Game1.SFXOn)
                            {
                                stopPress = false;
                                Game1.SFXOn = false;
                                select = 0;

                                lastTimeSelect = theTime.TotalGameTime;
                            }
                            else if (!Game1.SFXOn)
                            {
                                stopPress = false;
                                Game1.SFXOn = true;
                                select = 0;

                                lastTimeSelect = theTime.TotalGameTime;
                            }
                        }
                        else if (select == 3)
                        {
                            lastTimeSelect = theTime.TotalGameTime;

                            stopPress = false;
                            select_Pos.Y = 185;
                            setting = false;
                        }
                    }
                    else if (!setting)
                    {
                        if (select_Pos.Y == 185)
                        {
                            select = 1;
                        }
                        else if (select_Pos.Y == 255)
                        {
                            select = 2;
                        }
                        else if (select_Pos.Y == 325)
                        {
                            select = 3;
                        }

                        if (select == 1)
                        {
                            resetValue = false;
                            stopPress = false;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                        else if (select == 2)
                        {
                            lastTimeSelect = theTime.TotalGameTime;

                            stopPress = false;
                            select_Pos.Y = 185;
                            setting = true;
                        }
                        else if (select == 3)
                        {
                            string filepath = Path.Combine(@"Content\data.txt");
                            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                            StreamWriter sw = new StreamWriter(fs);
                            if (Switch == "InGame1")
                            { sw.WriteLine("InGame1"); }
                            sw.Flush();
                            sw.Close();

                            string filepathDead = Path.Combine(@"Content\Dead.txt");
                            FileStream fsDead = new FileStream(filepathDead, FileMode.Open, FileAccess.Write);
                            StreamWriter swDead = new StreamWriter(fsDead);
                            if (Switch == "InGame1")
                            { swDead.WriteLine(dead_count.ToString()); }
                            swDead.Flush();
                            swDead.Close();
                            stopPress = false;
                            resetValue = false;
                            Game1.BackMenu = true;
                            Game1.State = "Title";
                            ScreenEvent.Invoke(game.mTitleScreen, new EventArgs());
                        }
                    } 
                }
            }
        }

        void UpdateMessages(GameTime gameTime)
        {
            if (messages.Count > 0)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    DisplayMessage dm = messages[i];
                    dm.DisplayTime -= gameTime.ElapsedGameTime;
                    if (dm.DisplayTime <= TimeSpan.Zero)
                    {
                        messages.RemoveAt(i);
                    }
                    else
                    {
                        messages[i] = dm;
                    }
                }
            }
        }
        void DrawMessages(SpriteBatch theBatch)
        {
            if (messages.Count > 0)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    DisplayMessage dm = messages[i];
                    dm.DrawnMessage += dm.Message[dm.CurentIndex].ToString();
                    theBatch.DrawString(ArialFont, dm.DrawnMessage, dm.Position, dm.DrawColor);
                    if (dm.CurentIndex != dm.Message.Length - 1)
                    {
                        dm.CurentIndex++;
                        messages[i] = dm;
                    }
                }
            }
        }
    }
}
