using Microsoft.Xna.Framework;
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

        bool w_left = false, w_right = false;
        bool atk = false;
        bool def = false;
        bool special = false;
        bool special_ani = false;
        bool stop_move = false;
        bool p_knockBack = false;
        bool knockBack = false;
        bool knockBack2 = false;
        bool knockBack3 = false;

        bool e1_died = false;
        bool e2_died = false;
        bool e3_died = false;
        bool p_died = false;

        // Menu
        string Switch;
        SpriteFont loadingFont;
        string loadingText;
        bool menuLoading = false;
        bool noLoad = false;
        bool stopPress = false;
        bool gamePause = false;
        Texture2D noLoadPic;
        Texture2D pausePic;
        Texture2D buttonStart;
        Texture2D buttonLoad;
        Texture2D buttonExit;
        Texture2D bg_mainmenu;
        Texture2D buttonSelect;
        Texture2D gameOver;
        Texture2D Cloud1;
        Texture2D Cloud2;
        Vector2 select_Pos;
        int select;
        int potion_Count = 0;
        float[] speedCloud = new float[10];
        float[] speedCloud2 = new float[5];
        private AnimatedTexture loading;
        private AnimatedTexture glass;
        private AnimatedTexture p_title;

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
        Texture2D potion;
        // Special attack
        Texture2D special1;
        Texture2D special2;
        Texture2D special3;
        Texture2D special4;
        Texture2D special5;
        Vector2[] potion_Pos = new Vector2[2];
        Vector2[] Heart_Pos = new Vector2[5];
        Vector2[] e1_HeartPos = new Vector2[3];
        Vector2[] e2_HeartPos = new Vector2[5];
        Vector2[] e3_HeartPos = new Vector2[5];
        Vector2[] glassPos = new Vector2[70];
        Vector2[] Cloud_Pos = new Vector2[7];
        Vector2[] Cloud_Pos2 = new Vector2[5];
        bool[] potion_Ena = new bool[2];
        bool[] potion_Use = new bool[2];

        KeyboardState keyboardState;
        KeyboardState old_keyboardState;

        Camera camera = new Camera();

        // Player
        int special_Count = 0;
        Status p_Status = new Status();
        Status e1_Status = new Status();
        Status e2_Status = new Status();
        Status e3_Status = new Status();
        private AnimatedTexture player_idle;
        private AnimatedTexture player_walk;
        private AnimatedTexture player_atk;
        private AnimatedTexture player_def;
        private AnimatedTexture player_died;
        private AnimatedTexture player_special;
        private AnimatedTexture atk_special;

        // Ai
        Random r = new Random();
        private AnimatedTexture Cat_idle;
        private AnimatedTexture rabbit_idle;
        // Ai 1
        private AnimatedTexture Ai1_walk;
        private AnimatedTexture Ai1_idle;
        private AnimatedTexture Ai1_atk;
        bool ai1_idle = false;
        bool ai1_atk = false;
        // Ai 2
        private AnimatedTexture Ai2_walk;
        private AnimatedTexture Ai2_idle;
        private AnimatedTexture Ai2_atk;
        bool ai2_idle = false;
        bool ai2_atk = false;
        // Ai 3
        Texture2D eWaveAtk;
        private AnimatedTexture Ai3_walk;
        private AnimatedTexture Ai3_atk;
        bool ai3_atk = false;
        bool ai3_Wave = false;
        bool ai3_Use = false;
        Vector2 Ai3WavePos = new Vector2(0, 600);

        bool rab_direction = false;
        bool _rab_direction = false;

        private const float Rotation = 0;
        private const float Scale = 1.0f;
        private const float Depth = 0.5f;

        // time wait
        private static readonly TimeSpan intervalBetweenAttack = TimeSpan.FromMilliseconds(1000);
        private static readonly TimeSpan _intervalBetweenAttack = TimeSpan.FromMilliseconds(300);
        private static readonly TimeSpan intervalBetweenBlock = TimeSpan.FromMilliseconds(1500);
        private static readonly TimeSpan intervalBetweenSpecialCount = TimeSpan.FromMilliseconds(800);
        private static readonly TimeSpan _intervalBetweenBlock = TimeSpan.FromMilliseconds(610);
        private static readonly TimeSpan sIntervalBetweenAttackAni = TimeSpan.FromMilliseconds(300);
        private static readonly TimeSpan sIntervalBetweenAttack = TimeSpan.FromMilliseconds(1200);
        private static readonly TimeSpan s_IntervalBetweenAttack = TimeSpan.FromMilliseconds(500);
        private TimeSpan lastTimeAttack;
        private TimeSpan lastTimeBlock;
        private TimeSpan lastTimeSpecial;
        private TimeSpan lastTimeSpecialAni;
        private TimeSpan lastTimeSpecialCount;

        // time hp
        private static readonly TimeSpan intervalBetweenHp = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimeHp;
        private static readonly TimeSpan intervalBetweenHp2 = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimeHp2;

        // time animation
        private static readonly TimeSpan intervalBetweenAni = TimeSpan.FromMilliseconds(100);
        private TimeSpan lastTimeAni;
        private static readonly TimeSpan intervalBetweenAni2 = TimeSpan.FromMilliseconds(100);
        private TimeSpan lastTimeAni2;

        // time select
        private static readonly TimeSpan intervalBetweenSelect = TimeSpan.FromMilliseconds(250);
        private TimeSpan lastTimeSelect;

        // time wait loading
        private static readonly TimeSpan intervalBetweenLoad = TimeSpan.FromMilliseconds(2500);
        private TimeSpan lastTimeLoad;

        // time died
        private static readonly TimeSpan intervalBetweenDied = TimeSpan.FromMilliseconds(800);
        private TimeSpan lastTimeDied;

        // Enemy time
        private static readonly TimeSpan e_intervalBetweenAttack = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan _intervalBetweenAttack1 = TimeSpan.FromMilliseconds(1500);
        private TimeSpan e_lastTimeAttack;

        private static readonly TimeSpan e_intervalBetweenAttack2 = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan _intervalBetweenAttack2 = TimeSpan.FromMilliseconds(1500);
        private TimeSpan e_lastTimeAttack2;

        private TimeSpan e_intervalBetweenAttack3 = TimeSpan.FromMilliseconds(2850);
        private static readonly TimeSpan _intervalBetweenAttack3 = TimeSpan.FromMilliseconds(400);
        private TimeSpan e_lastTimeAttack3;

        private static readonly TimeSpan DelayAttackWave3 = TimeSpan.FromMilliseconds(2000);
        private TimeSpan AttackWave3;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            soundEffects = new List<SoundEffect>();
            player_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            player_walk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            player_atk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            player_def = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai1_walk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai1_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai1_atk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai2_walk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai2_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai2_atk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai3_walk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Ai3_atk = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Cat_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            loading = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            glass = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            p_title = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            player_died = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            rabbit_idle = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            player_special = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            atk_special = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            string filepath = Path.Combine(@"Content\data.txt");
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string tmpStr = sr.ReadLine();
            loadingText = tmpStr;
            sr.Close();

            select_Pos = new Vector2(130, 195);
            IsMouseVisible = true;
            Switch = "Mainmenu";
            p_Status.hp = 5;
            e1_Status.hp = 3;
            e2_Status.hp = 5;
            e3_Status.hp = 5;

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


            base.Initialize();
        }

        // Pos Player
        private Vector2 player_Pos = new Vector2(50, 255);
        private const int Frames = 2;
        private const int FramesPerSec = 4;
        private const int FramesRow = 1;

        // Pos Camera
        private Vector2 Camera_Pos = new Vector2(700, 255);

        // Pos Enemy1
        private Vector2 enemy1_Pos = new Vector2(800, 255);

        // Pos Enemy2
        private Vector2 enemy2_Pos = new Vector2(1800, 255);

        // Pos Enemy3
        private Vector2 enemy3_Pos = new Vector2(3000, 255);

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
        private const int atk_FramesPerSec = 22;
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

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Sound Effect
            soundEffects.Add(Content.Load<SoundEffect>("WalkSound")); //[0]
            soundEffects.Add(Content.Load<SoundEffect>("button-15")); //[1]
            soundEffects.Add(Content.Load<SoundEffect>("SwordHit")); //[2]
            soundEffects.Add(Content.Load<SoundEffect>("SwordWhoosh")); //[3]
            soundEffects.Add(Content.Load<SoundEffect>("SwordBlock")); //[4]
            soundEffects.Add(Content.Load<SoundEffect>("Drink")); //[5]
            soundEffects.Add(Content.Load<SoundEffect>("SwordKillpower")); //[6]
            soundEffects.Add(Content.Load<SoundEffect>("PlayerDead")); //[7]

            walkSoundInstance = soundEffects[0].CreateInstance();
            SwordHit = soundEffects[2].CreateInstance();
            SwordWhoosh = soundEffects[3].CreateInstance();
            Dead = soundEffects[7].CreateInstance();

            bgSong = Content.Load<Song>("BackgroundLevel1");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            SoundEffect.MasterVolume = 0.5f;

            eWaveAtk = Content.Load<Texture2D>("goldEneAtkWave");
            noLoadPic = Content.Load<Texture2D>("noLoad");
            pausePic = Content.Load<Texture2D>("pause");
            loadingFont = Content.Load<SpriteFont>("ArialFont");
            BG1_1 = Content.Load<Texture2D>("bg_level1");
            BG1_2 = Content.Load<Texture2D>("bg_level1_1");
            BG1_3 = Content.Load<Texture2D>("bg_level1_2");
            bg_mainmenu = Content.Load<Texture2D>("gameTitle");
            buttonStart = Content.Load<Texture2D>("Start");
            buttonLoad = Content.Load<Texture2D>("Load");
            buttonExit = Content.Load<Texture2D>("Exit");
            buttonSelect = Content.Load<Texture2D>("Select");
            Heart = Content.Load<Texture2D>("Heart");
            gameOver = Content.Load<Texture2D>("Game over");
            Cloud1 = Content.Load<Texture2D>("Cloud1");
            Cloud2 = Content.Load<Texture2D>("Cloud2");
            potion = Content.Load<Texture2D>("hp_Potion");
            special1 = Content.Load<Texture2D>("special_atk");
            special2 = Content.Load<Texture2D>("special_atk2");
            special3 = Content.Load<Texture2D>("special_atk3");
            special4 = Content.Load<Texture2D>("special_atk4");
            special5 = Content.Load<Texture2D>("special_atk5");
            loading.Load(Content, "loadScreen", 3, 1, 6);
            player_idle.Load(Content, "p_test", Frames, FramesRow, FramesPerSec);
            player_walk.Load(Content, "p_Walk_Test", w_Frames, w_FramesRow, w_FramesPerSec);
            player_atk.Load(Content, "player_atk", atk_Frames, atk_FramesRow, atk_FramesPerSec);
            player_special.Load(Content, "player_special", atk_Frames, atk_FramesRow, atk_FramesPerSec);
            player_def.Load(Content, "player_def", def_Frames, def_FramesRow, def_FramesPerSec);
            player_died.Load(Content, "p_died", died_Frames, died_FramesRow, died_FramesPerSec);
            atk_special.Load(Content, "atk_special", special_Frames, special_FramesRow, special_FramesPerSec);

            // Ai
            Ai1_walk.Load(Content, "enemy_walk", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            Ai1_idle.Load(Content, "enemy_idle", Frames, FramesRow, FramesPerSec);
            Ai1_atk.Load(Content, "enemy_atk", e_atk_Frames, e_atk_FramesRow, e_atk_FramesPerSec);
            Ai2_walk.Load(Content, "enemy_walk", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            Ai2_idle.Load(Content, "enemy_idle", Frames, FramesRow, FramesPerSec);
            Ai2_atk.Load(Content, "enemy_atk", e_atk_Frames, e_atk_FramesRow, e_atk_FramesPerSec);
            Ai3_walk.Load(Content, "goldEnemy", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            Ai3_atk.Load(Content, "goldEnemyAtk", 8, e_atk_FramesRow, 16);
            Cat_idle.Load(Content, "cat", 3, 1, 3);
            rabbit_idle.Load(Content, "rabbit", 3, 1, 3);
            glass.Load(Content, "Glass", 2, 1, 2);
            p_title.Load(Content, "p_Title", 2, 1, 4);
            player_walk.Pause();

        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(bgSong);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Switch == "Mainmenu")
            {
                p_died = false;
                UpdateMenu(gameTime);
            }
            else if (Switch == "Loading")
            {
                menuLoading = true;
                string filepath = Path.Combine(@"Content\data.txt");
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string tmpStr = sr.ReadLine();
                loadingText = tmpStr;
                sr.Close();
                if (noLoad == true)
                {
                    Switch = "Mainmenu";
                }
                else if (noLoad == false)
                {
                    Switch = loadingText;
                }
            }
            else if (Switch == "InGame1")
            {
                if (!bg1Song)
                {
                    MediaPlayer.Play(bgSong);
                    MediaPlayer.IsMuted = false;
                    bg1Song = true;
                }
                // Player Update frame
                player_died.UpdateFrame(elapsed);
                player_walk.UpdateFrame(elapsed);
                player_idle.UpdateFrame(elapsed);
                player_atk.UpdateFrame(elapsed);
                player_def.UpdateFrame(elapsed);
                player_special.UpdateFrame(elapsed);
                atk_special.UpdateFrame(elapsed);

                // Enemy Update frame
                Ai1_walk.UpdateFrame(elapsed);
                Ai1_idle.UpdateFrame(elapsed);
                Ai1_atk.UpdateFrame(elapsed);
                Ai2_walk.UpdateFrame(elapsed);
                Ai2_idle.UpdateFrame(elapsed);
                Ai2_atk.UpdateFrame(elapsed);
                Ai3_walk.UpdateFrame(elapsed);
                Ai3_atk.UpdateFrame(elapsed);
                Cat_idle.UpdateFrame(elapsed);
                rabbit_idle.UpdateFrame(elapsed);

                // ฉาก Update frame
                glass.UpdateFrame(elapsed);
                loading.UpdateFrame(elapsed);

                // Key Pause
                gameKeyPause(gameTime);
                if (gamePause == false)
                {
                    if (!p_died)
                    {
                        // Player heart
                        for (int i = 0; i < 5; i++)
                        {
                            Heart_Pos[i].X = (player_Pos.X - 43) - (i + 1) * -22;
                            Heart_Pos[i].Y = player_Pos.Y - 25;
                        }
                        // Enemy heart
                        for (int i = 0; i < 3; i++)
                        {
                            e1_HeartPos[i].X = (enemy1_Pos.X + 43) - (i + 1) * -22;
                            e1_HeartPos[i].Y = enemy1_Pos.Y - 15;

                        }
                        for (int i = 0; i < 5; i++)
                        {
                            e2_HeartPos[i].X = (enemy2_Pos.X + 20) - (i + 1) * -22;
                            e2_HeartPos[i].Y = enemy2_Pos.Y - 15;

                        }
                        for (int i = 0; i < 5; i++)
                        {
                            e3_HeartPos[i].X = (enemy3_Pos.X + 20) - (i + 1) * -22;
                            e3_HeartPos[i].Y = enemy3_Pos.Y - 15;

                        }

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
                        gameKeyDown();

                        // Key Attack
                        gameKeyAttack(gameTime);

                        Rectangle charRectangle;
                        Rectangle charPlayerSpecialAtk;
                        Rectangle[] charPotion = new Rectangle[2];
                        charPotion[0] = new Rectangle((int)potion_Pos[0].X, (int)potion_Pos[0].Y, 32, 32);
                        Rectangle blockEnemy1 = new Rectangle((int)enemy1_Pos.X + 64, (int)enemy1_Pos.Y, 32, 160);
                        Rectangle blockEnemy2 = new Rectangle((int)enemy2_Pos.X + 64, (int)enemy2_Pos.Y, 32, 160);
                        Rectangle blockEnemy3 = new Rectangle((int)enemy3_Pos.X + 64, (int)enemy3_Pos.Y, 32, 160);
                        Rectangle blockEnemy3Wave;

                        if (special == true)
                        {
                            charRectangle = new Rectangle((int)player_Pos.X, (int)player_Pos.Y, 125, 160);
                            charPlayerSpecialAtk = new Rectangle((int)special_Pos.X, (int)special_Pos.Y, 20, 32);
                            special_Pos.X += 15;
                            if (charPlayerSpecialAtk.Intersects(blockEnemy1) && e1_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack = true;
                                    ai1_idle = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e1_Status.hp -= 3;

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                            else if (charPlayerSpecialAtk.Intersects(blockEnemy2) && e2_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack2 = true;
                                    ai2_idle = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e2_Status.hp -= 3;

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                            else if (charPlayerSpecialAtk.Intersects(blockEnemy3) && e3_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack3 = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e3_Status.hp -= 2;

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                        }
                        if (special_ani == true)
                        {
                            charRectangle = new Rectangle((int)player_Pos.X, (int)player_Pos.Y, 125, 160);
                            if (charRectangle.Intersects(blockEnemy1) && e1_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack = true;
                                    ai1_idle = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e1_Status.hp -= 1;

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                            else if (charRectangle.Intersects(blockEnemy2) && e2_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack2 = true;
                                    ai2_idle = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e2_Status.hp -= 1;

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                            else if (charRectangle.Intersects(blockEnemy3) && e3_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack3 = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e3_Status.hp -= 1;

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                        }
                        if (atk == true)
                        {
                            charRectangle = new Rectangle((int)player_Pos.X, (int)player_Pos.Y, 125, 160);
                            if (charRectangle.Intersects(blockEnemy1) && e1_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack = true;
                                    ai1_idle = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e1_Status.hp -= 1;
                                    if (SwordHit.State != SoundState.Playing) { SwordHit.Play(); }

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                            else if (charRectangle.Intersects(blockEnemy2) && e2_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack2 = true;
                                    ai2_idle = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e2_Status.hp -= 1;
                                    if (SwordHit.State != SoundState.Playing) { SwordHit.Play(); }

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                            else if (charRectangle.Intersects(blockEnemy3) && e3_died == false)
                            {
                                if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                                {
                                    knockBack3 = true;

                                    lastTimeAni2 = gameTime.TotalGameTime;
                                }
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    e3_Status.hp -= 1;
                                    if (SwordHit.State != SoundState.Playing) { SwordHit.Play(); }

                                    lastTimeHp2 = gameTime.TotalGameTime;
                                }
                            }
                        }
                        else
                        {
                            if (def == true)
                            {
                                charRectangle = new Rectangle((int)player_Pos.X, (int)player_Pos.Y, 100, 160);
                            }
                            else
                            {
                                charRectangle = new Rectangle((int)player_Pos.X, (int)player_Pos.Y, 80, 160);
                                if (potion_Ena[0] == false)
                                {
                                    if (charRectangle.Intersects(charPotion[0]))
                                    {
                                        potion_Count += 1;
                                        potion_Ena[0] = true;
                                    }
                                }
                            }
                        }

                        if (ai1_atk == true && e1_died == false)
                        {
                            blockEnemy1 = new Rectangle((int)enemy1_Pos.X + 18, (int)enemy1_Pos.Y, 32, 160);
                            if (charRectangle.Intersects(blockEnemy1) && e1_died == false)
                            {
                                if (def == true)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime && lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        soundEffects[4].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;
                                        if (special_Count < 4 && lastTimeSpecialCount + intervalBetweenSpecialCount < gameTime.TotalGameTime)
                                        {
                                            special_Count += 1;

                                            lastTimeSpecialCount = gameTime.TotalGameTime;
                                        }
                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                }
                                else
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        soundEffects[2].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (e1_died == false)
                            {
                                blockEnemy1 = new Rectangle((int)enemy1_Pos.X + 96, (int)enemy1_Pos.Y, 32, 160);
                                if (charRectangle.Intersects(blockEnemy1) && e1_died == false)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                }
                            }
                        }

                        // Enemy 2 atk
                        if (ai2_atk == true && e1_died == true)
                        {
                            blockEnemy2 = new Rectangle((int)enemy2_Pos.X + 18, (int)enemy2_Pos.Y, 32, 160);
                            if (charRectangle.Intersects(blockEnemy2) && e2_died == false)
                            {
                                if (def == true)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime && lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        soundEffects[4].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;
                                        if (special_Count < 4 && lastTimeSpecialCount + intervalBetweenSpecialCount < gameTime.TotalGameTime)
                                        {
                                            special_Count += 1;

                                            lastTimeSpecialCount = gameTime.TotalGameTime;
                                        }
                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                }
                                else
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        soundEffects[2].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (e2_died == false && e1_died == true)
                            {
                                blockEnemy2 = new Rectangle((int)enemy2_Pos.X + 96, (int)enemy2_Pos.Y, 32, 160);
                                if (charRectangle.Intersects(blockEnemy2) && e2_died == false)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                }
                            }
                        }

                        // Enemy 3 atk
                        if (ai3_Wave == true && e1_died == true && e2_died == true)
                        {
                            if (!ai3_Use)
                            {
                                Ai3WavePos = new Vector2(enemy3_Pos.X, enemy3_Pos.Y);
                                ai3_Use = true;
                            }
                            Ai3WavePos.X -= 7;
                            blockEnemy3Wave = new Rectangle((int)Ai3WavePos.X, (int)Ai3WavePos.Y, 50, 160);
                            if(charRectangle.Intersects(blockEnemy3Wave) && e3_died == false)
                            {
                                if (def == true)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime && lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        soundEffects[4].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;
                                        if (special_Count < 4 && lastTimeSpecialCount + intervalBetweenSpecialCount < gameTime.TotalGameTime)
                                        {
                                            special_Count += 1;

                                            lastTimeSpecialCount = gameTime.TotalGameTime;
                                        }
                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    Ai3WavePos.Y = 600;
                                    ai3_Wave = false;
                                }
                                else
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        soundEffects[2].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                    Ai3WavePos.Y = 600;
                                    ai3_Wave = false;
                                }
                            }
                        }

                        if (ai3_atk == true && e1_died == true && e2_died == true)
                        {
                            blockEnemy3 = new Rectangle((int)enemy3_Pos.X + 18, (int)enemy3_Pos.Y, 32, 160);
                            if (charRectangle.Intersects(blockEnemy3) && e3_died == false && ai3_Wave == false)
                            {
                                if (def == true)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime && lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        soundEffects[4].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;
                                        if (special_Count < 4 && lastTimeSpecialCount + intervalBetweenSpecialCount < gameTime.TotalGameTime)
                                        {
                                            special_Count += 1;

                                            lastTimeSpecialCount = gameTime.TotalGameTime;
                                        }
                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                }
                                else
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        soundEffects[2].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (e3_died == false && e1_died == true && e2_died == true)
                            {
                                blockEnemy3 = new Rectangle((int)enemy3_Pos.X + 96, (int)enemy3_Pos.Y, 32, 160);
                                if (charRectangle.Intersects(blockEnemy3) && e3_died == false)
                                {
                                    if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                                    {
                                        p_knockBack = true;
                                        stop_move = true;

                                        lastTimeAni = gameTime.TotalGameTime;
                                    }
                                    if (lastTimeHp + intervalBetweenHp < gameTime.TotalGameTime)
                                    {
                                        p_Status.hp -= 1;

                                        lastTimeHp = gameTime.TotalGameTime;
                                    }
                                }
                            }
                        }


                        if (p_died == false)
                        {
                            // Enemy 1
                            if (e1_died == false)
                            {
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    if (enemy1_Pos.X - player_Pos.X >= 61)
                                    {
                                        if (e_lastTimeAttack + e_intervalBetweenAttack < gameTime.TotalGameTime)
                                        {
                                            Ai1_walk.Play();
                                            ai1_atk = false;
                                            ai1_idle = false;
                                            enemy1_Pos.X -= 1;
                                            Ai1_atk.Pause(0, 0);
                                        }
                                        else if (e_lastTimeAttack + _intervalBetweenAttack1 < gameTime.TotalGameTime)
                                        {
                                            enemy1_Pos.X -= 1;
                                            Ai1_walk.Play();
                                            ai1_atk = false;
                                        }
                                    }
                                    else
                                    {
                                        Ai1_atk.Play();
                                        if (e_lastTimeAttack + e_intervalBetweenAttack < gameTime.TotalGameTime)
                                        {
                                            ai1_atk = true;

                                            e_lastTimeAttack = gameTime.TotalGameTime;
                                        }
                                        if (e_lastTimeAttack + _intervalBetweenAttack1 < gameTime.TotalGameTime)
                                        {
                                            Ai1_walk.Play();
                                            ai1_atk = false;
                                        }
                                    }
                                }
                            }

                            // Enemy 2
                            if (e1_died == true)
                            {
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    if (enemy2_Pos.X - player_Pos.X >= 61)
                                    {
                                        if (e_lastTimeAttack2 + e_intervalBetweenAttack2 < gameTime.TotalGameTime)
                                        {
                                            Ai2_walk.Play();
                                            ai2_atk = false;
                                            ai2_idle = false;
                                            enemy2_Pos.X -= 2;
                                            Ai2_atk.Pause(0, 0);
                                        }
                                        else if (e_lastTimeAttack2 + _intervalBetweenAttack2 < gameTime.TotalGameTime)
                                        {
                                            enemy2_Pos.X -= 2;
                                            Ai2_walk.Play();
                                            ai2_atk = false;
                                        }
                                    }
                                    else
                                    {
                                        Ai2_atk.Play();
                                        if (e_lastTimeAttack2 + e_intervalBetweenAttack2 < gameTime.TotalGameTime)
                                        {
                                            ai2_atk = true;

                                            e_lastTimeAttack2 = gameTime.TotalGameTime;
                                        }
                                        if (e_lastTimeAttack2 + _intervalBetweenAttack2 < gameTime.TotalGameTime)
                                        {
                                            Ai2_walk.Play();
                                            ai2_atk = false;
                                        }
                                    }
                                }
                            }

                            // Enemy 3
                            if (e1_died == true && e2_died == true)
                            {
                                if (lastTimeHp2 + intervalBetweenHp2 < gameTime.TotalGameTime)
                                {
                                    if (enemy3_Pos.X - player_Pos.X >= 120)
                                    {
                                        e_intervalBetweenAttack3 = TimeSpan.FromMilliseconds(2850);
                                        if (e_lastTimeAttack3 + e_intervalBetweenAttack3 < gameTime.TotalGameTime)
                                        {
                                            ai3_atk = true;
                                            ai3_Wave = true;
                                            ai3_Use = false;
                                            AttackWave3 = gameTime.TotalGameTime;
                                            e_lastTimeAttack3 = gameTime.TotalGameTime;
                                        }
                                        else if (e_lastTimeAttack3 + _intervalBetweenAttack3 < gameTime.TotalGameTime)
                                        {
                                            enemy3_Pos.X -= 1;
                                            Ai3_walk.Play();
                                            ai3_atk = false;
                                        }
                                    }
                                    else if (enemy3_Pos.X - player_Pos.X >= 61 && enemy3_Pos.X - player_Pos.X < 120)
                                    {
                                        if (ai3_atk == false)
                                        {
                                            enemy3_Pos.X -= 1;
                                        }
                                        if (e_lastTimeAttack3 + _intervalBetweenAttack3 < gameTime.TotalGameTime)
                                        {
                                            Ai3_walk.Play();
                                            enemy3_Pos.X -= 1;
                                            ai3_atk = false;
                                        }
                                    }
                                    else if (enemy3_Pos.X - player_Pos.X < 61)
                                    {
                                        e_intervalBetweenAttack3 = TimeSpan.FromMilliseconds(1200);
                                        Ai3_atk.Play();
                                        if (e_lastTimeAttack3 + e_intervalBetweenAttack3 < gameTime.TotalGameTime)
                                        {
                                            ai3_atk = true;

                                            e_lastTimeAttack3 = gameTime.TotalGameTime;
                                        }
                                        if (e_lastTimeAttack3 + _intervalBetweenAttack3 < gameTime.TotalGameTime)
                                        {
                                            Ai3_walk.Play();
                                            enemy3_Pos.X -= 1;
                                            ai3_atk = false;
                                        }
                                    }
                                }
                            }
                            if (AttackWave3 + DelayAttackWave3 < gameTime.TotalGameTime)
                            {
                                ai3_Wave = false;
                                Ai3WavePos.Y = 600;
                            }


                            if (p_knockBack == true)
                            {
                                player_Pos.X -= 8;
                                player_Pos.Y -= 2;
                            }
                            else
                            {
                                if (player_Pos.Y < 255)
                                {
                                    player_Pos.Y += 4;
                                }
                                if (player_Pos.Y > 255)
                                {
                                    player_Pos.Y = 255;
                                }

                            }
                            if (lastTimeAni + intervalBetweenAni < gameTime.TotalGameTime)
                            {
                                p_knockBack = false;
                            }
                            // Enemy 1 knockBack
                            if (e1_died == false)
                            {
                                if (knockBack == true)
                                {
                                    enemy1_Pos.X += 6;
                                    enemy1_Pos.Y -= 2;
                                }
                                else
                                {
                                    if (enemy1_Pos.Y < 255)
                                    {
                                        enemy1_Pos.Y += 4;
                                    }
                                    if (enemy1_Pos.Y > 255)
                                    {
                                        enemy1_Pos.Y = 255;
                                    }
                                }
                            }
                            // Enemy 2 knockBack
                            if (e1_died == true && e2_died == false)
                            {
                                if (knockBack2 == true)
                                {
                                    enemy2_Pos.X += 6;
                                    enemy2_Pos.Y -= 2;
                                }
                                else
                                {
                                    if (enemy2_Pos.Y < 255)
                                    {
                                        enemy2_Pos.Y += 4;
                                    }
                                    if (enemy2_Pos.Y > 255)
                                    {
                                        enemy2_Pos.Y = 255;
                                    }
                                }
                            }
                            if (e1_died == true && e2_died == true && e3_died == false)
                            {
                                if (knockBack3 == true)
                                {
                                    enemy3_Pos.X += 3;
                                    enemy3_Pos.Y -= 2;
                                }
                                else
                                {
                                    if (enemy3_Pos.Y < 255)
                                    {
                                        enemy3_Pos.Y += 4;
                                    }
                                    if (enemy3_Pos.Y > 255)
                                    {
                                        enemy3_Pos.Y = 255;
                                    }
                                }
                            }

                            if (lastTimeAni2 + intervalBetweenAni2 < gameTime.TotalGameTime)
                            {
                                knockBack = false;
                                knockBack2 = false;
                                knockBack3 = false;
                            }
                        }
                        else
                        {
                            Ai1_atk.Pause(0, 0);
                            Ai1_walk.Pause(0, 0);
                            Ai1_idle.Pause(0, 0);
                            Ai2_atk.Pause(0, 0);
                            Ai2_walk.Pause(0, 0);
                            Ai2_idle.Pause(0, 0);
                            Ai3_atk.Pause(0, 0);
                            Ai3_walk.Pause(0, 0);
                        }

                        if (e1_Status.hp <= 0)
                        {
                            e1_died = true;
                            ai1_atk = false;
                            Ai1_atk.Pause(0, 0);
                            Ai1_walk.Pause(0, 0);
                            Ai1_idle.Pause(0, 0);
                        }
                        if (e2_Status.hp <= 0)
                        {
                            e2_died = true;
                            ai2_atk = false;
                            Ai2_atk.Pause(0, 0);
                            Ai2_walk.Pause(0, 0);
                            Ai2_idle.Pause(0, 0);
                        }
                        if (e3_Status.hp <= 0)
                        {
                            e3_died = true;
                            ai3_atk = false;
                            Ai3_atk.Pause(0, 0);
                            Ai3_walk.Pause(0, 0);
                        }

                        // Potion
                        if (potion_Count == 1)
                        {
                            if (keyboardState.IsKeyDown(Keys.E))
                            {
                                soundEffects[5].Play(volume: 1.0f, pitch: 0.0f, pan: 0.0f);
                                potion_Count -= 1;
                                if (p_Status.hp < 5) { p_Status.hp += 2; }
                                if (p_Status.hp > 5) { p_Status.hp = 5; }
                                potion_Use[0] = true;
                            }
                        }

                        if (Camera_Pos.X - player_Pos.X >= 80 && menuLoading == false)
                        {
                            Camera_Pos.X -= 2;
                        }
                        else if (Camera_Pos.X - player_Pos.X <= -80 && menuLoading == false)
                        {
                            if (3680 - Camera_Pos.X > 625)
                            {
                                Camera_Pos.X += 2;
                            }
                        }

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

                        if (player_Pos.X >= 3610)
                        {
                            player_Pos.X -= 2;
                        }
                        else if (player_Pos.X < 0)
                        {
                            player_Pos.X += 2;
                        }

                        camera.Update(Camera_Pos);
                    }
                }
            }


            if (p_Status.hp <= 0 && p_died == false)
            {
                if (walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                if (Dead.State != SoundState.Playing) { Dead.Play(); }
                p_died = true;
                stop_move = true;
                player_died.Play();
                Ai1_atk.Pause(0,0);
                Ai2_atk.Pause(0, 0);
                Ai3_atk.Pause(0, 0);
                Ai1_walk.Pause(0, 0);
                Ai2_walk.Pause(0, 0);
                Ai3_walk.Pause(0, 0);
                lastTimeDied = gameTime.TotalGameTime;
            }
            else if (p_died == true)
            {
                stop_move = true;
            }

            if (lastTimeDied + intervalBetweenDied < gameTime.TotalGameTime)
            {
                player_died.Pause();
                if (p_died == true && lastTimeDied + intervalBetweenDied + intervalBetweenDied < gameTime.TotalGameTime)
                {
                    select_Pos = new Vector2(130, 195);
                    string filepath = Path.Combine(@"Content\data.txt");
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    if (Switch == "InGame1")
                    { sw.WriteLine("InGame1"); }
                    sw.Flush();
                    sw.Close();
                    MediaPlayer.IsMuted = true;
                    if (Dead.State != SoundState.Stopped) { Dead.Stop(); }
                    Switch = "Mainmenu";
                }
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (Switch == "Mainmenu")
            {
                spriteBatch.Begin();
                spriteBatch.Draw(bg_mainmenu, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(buttonStart, new Vector2(230, 200), Color.White);
                spriteBatch.Draw(buttonLoad, new Vector2(230, 260), Color.White);
                spriteBatch.Draw(buttonExit, new Vector2(230, 320), Color.White);
                spriteBatch.Draw(buttonSelect, select_Pos, Color.White);
                p_title.DrawFrame(spriteBatch, new Vector2(560, 80));
                spriteBatch.End();
            }
            else if (Switch == "InGame1" && menuLoading == false)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.ViewMatrix);
                spriteBatch.Draw(BG1_1, new Vector2(0 - camera.ViewMatrix.Translation.X * 0.8f, 0), Color.White);
                for (int i = 0; i < 70; i++)
                {
                    glass.DrawFrame(spriteBatch, glassPos[i]);
                }

                // Cloud
                for (int i = 0; i < 7; i++)
                {
                    spriteBatch.Draw(Cloud1, Cloud_Pos[i], Color.White);
                }
                for (int i = 0; i < 5; i++)
                {
                    spriteBatch.Draw(Cloud2, Cloud_Pos2[i], Color.White);
                }
                spriteBatch.Draw(BG1_2, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(BG1_3, new Vector2(0, 0), Color.White);

                // Potion
                if (potion_Ena[0] == true && potion_Use[0] == false)
                {
                    spriteBatch.Draw(potion, new Vector2(15 - camera.ViewMatrix.Translation.X, 35 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                else if (potion_Ena[0] == false)
                {
                    spriteBatch.Draw(potion, potion_Pos[0], Color.White);
                }

                // Special
                switch (special_Count)
                {
                    case 0:
                        spriteBatch.Draw(special1, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(special2, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(special3, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(special4, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                    case 4:
                        spriteBatch.Draw(special5, new Vector2(0 - camera.ViewMatrix.Translation.X, 5 - camera.ViewMatrix.Translation.Y), Color.White);
                        break;
                }


                for (int i = 0; i < p_Status.hp; i++)
                {
                    spriteBatch.Draw(Heart, Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.White);
                }
                for (int i = 0; i < e1_Status.hp; i++)
                {
                    spriteBatch.Draw(Heart, e1_HeartPos[i], new Rectangle(0, 0, 32, 32), Color.Black);
                }
                for (int i = 0; i < e2_Status.hp; i++)
                {
                    spriteBatch.Draw(Heart, e2_HeartPos[i], new Rectangle(0, 0, 32, 32), Color.Black);
                }
                for (int i = 0; i < e3_Status.hp; i++)
                {
                    spriteBatch.Draw(Heart, e3_HeartPos[i], new Rectangle(0, 0, 32, 32), Color.LightGoldenrodYellow);
                }

                if (p_died == false)
                {
                    Cat_idle.DrawFrame(spriteBatch, Cat_Pos);
                    rabbit_idle.DrawFrame(spriteBatch, rabbit_Pos, _rab_direction);
                    if (special == true)
                    {
                        atk_special.DrawFrame(spriteBatch, special_Pos);
                    }
                    if (special_ani == true)
                    {
                        player_special.DrawFrame(spriteBatch, player_Pos);
                    }
                    else if (atk == true)
                    {
                        player_atk.DrawFrame(spriteBatch, player_Pos);
                    }
                    else if (def == true)
                    {
                        player_def.DrawFrame(spriteBatch, player_Pos);
                    }
                    else
                    {
                        if ((w_left == false && w_right == false) || (w_left == true && w_right == true))
                        {
                            player_idle.DrawFrame(spriteBatch, player_Pos);
                        }
                        else
                        {
                            if (w_left == true)
                            {
                                if (player_walk.IsPaused)
                                {
                                    player_walk.Play();
                                }
                                player_walk.DrawFrame(spriteBatch, player_Pos);
                            }
                            if (w_right == true)
                            {
                                if (player_walk.IsPaused)
                                {
                                    player_walk.Play();
                                }
                                player_walk.DrawFrame(spriteBatch, player_Pos);
                            }
                        }
                    }
                }
                else
                {
                    player_atk.Pause(0, 0);
                    player_def.Pause(0, 0);
                    player_special.Pause(0, 0);
                    atk_special.Pause(0, 0);
                    player_walk.Pause(0, 0);
                    player_idle.Pause(0, 0);
                    player_died.DrawFrame(spriteBatch, player_Pos);
                    Cat_idle.DrawFrame(spriteBatch, Cat_Pos);
                    rabbit_idle.DrawFrame(spriteBatch, rabbit_Pos, _rab_direction);
                    spriteBatch.Draw(gameOver, new Vector2(350 - camera.ViewMatrix.Translation.X, 120 - camera.ViewMatrix.Translation.Y), Color.White);
                }

                // Enemy
                if (e1_died == false)
                {
                    if (ai1_atk == true)
                    {
                        Ai1_atk.DrawFrame(spriteBatch, enemy1_Pos);
                    }
                    else if (ai1_atk == false)
                    {
                        Ai1_walk.DrawFrame(spriteBatch, enemy1_Pos);
                    }
                }
                if (e2_died == false && e1_died == true)
                {
                    if (ai2_atk == true)
                    {
                        Ai2_atk.DrawFrame(spriteBatch, enemy2_Pos);
                    }
                    else if (ai2_atk == false)
                    {
                        Ai2_walk.DrawFrame(spriteBatch, enemy2_Pos);
                    }
                }
                if (e3_died == false && e1_died == true && e2_died == true)
                {
                    if (ai3_Wave == true)
                    {
                        spriteBatch.Draw(eWaveAtk, Ai3WavePos, Color.White);
                    }
                    if (ai3_atk == true)
                    {
                        Ai3_atk.DrawFrame(spriteBatch, enemy3_Pos);
                    }
                    else if (ai2_atk == false)
                    {
                        Ai3_walk.DrawFrame(spriteBatch, enemy3_Pos);
                    }
                }
                if (gamePause && Switch == "InGame1")
                {
                    spriteBatch.Draw(pausePic, new Vector2(0 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                spriteBatch.End();
            }
            if (menuLoading == true)
            {
                spriteBatch.Begin();
                if (noLoad == true)
                {
                    spriteBatch.Draw(noLoadPic, new Vector2(0, 0), Color.White);
                }
                else if (!noLoad)
                {
                    loading.DrawFrame(spriteBatch, new Vector2(0, 0));
                }
                if (lastTimeLoad + intervalBetweenLoad < gameTime.TotalGameTime)
                {
                    noLoad = false;
                    menuLoading = false;
                    stopPress = false;
                }
                spriteBatch.End();
            }
                
            base.Draw(gameTime);
        }

        private void UpdateMenu(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            p_title.UpdateFrame(elapsed);
            p_Status.hp = 5;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Down) && stopPress == false)
            {
                if (lastTimeSelect + intervalBetweenSelect < gameTime.TotalGameTime)
                {
                    if (!(select_Pos.Y == 315))
                    {
                        soundEffects[1].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                    }
                    if (select_Pos.Y <= 255)
                    {
                        select_Pos.Y += 60;

                        lastTimeSelect = gameTime.TotalGameTime;
                    }
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Up) && stopPress == false)
            {
                if (lastTimeSelect + intervalBetweenSelect < gameTime.TotalGameTime)
                {
                    if (!(select_Pos.Y == 195))
                    {
                        soundEffects[1].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                    }
                    if (select_Pos.Y >= 255)
                    {
                        select_Pos.Y -= 60;

                        lastTimeSelect = gameTime.TotalGameTime;
                    }

                }
            }
            else if (keyboardState.IsKeyDown(Keys.A) && stopPress == false)
            {
                stopPress = true;
                soundEffects[1].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
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
                    bg1Song = false;
                    noLoad = false;
                    string filepath = Path.Combine(@"Content\data.txt");
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("InGame1");
                    sw.Flush();
                    sw.Close();
                    e1_died = false;
                    e2_died = false;
                    e3_died = false;
                    enemy1_Pos = new Vector2(800, 255);
                    enemy2_Pos = new Vector2(1800, 255);
                    enemy3_Pos = new Vector2(3000, 255);
                    player_Pos = new Vector2(50, 255);
                    Camera_Pos = new Vector2(700, 255);
                    special_Pos = new Vector2(500, 600);
                    e1_Status.hp = 3;
                    e2_Status.hp = 5;
                    e3_Status.hp = 5;
                    potion_Count = 0;
                    special_Count = 0;
                    potion_Pos[0] = new Vector2(700, 390);
                    potion_Ena[0] = false;
                    potion_Use[0] = false;
                    player_died.Pause(0, 0);

                    lastTimeLoad = gameTime.TotalGameTime;

                    Switch = "Loading";
                }
                else if (select == 2)
                {
                    if (loadingText == "InGame1")
                    {
                        bg1Song = false;
                        noLoad = false;
                        e1_died = false;
                        e2_died = false;
                        e3_died = false;
                        enemy1_Pos = new Vector2(800, 255);
                        enemy2_Pos = new Vector2(1800, 255);
                        enemy3_Pos = new Vector2(3000, 255);
                        player_Pos = new Vector2(50, 255);
                        Camera_Pos = new Vector2(700, 255);
                        special_Pos = new Vector2(500, 600);
                        e1_Status.hp = 3;
                        e2_Status.hp = 5;
                        e3_Status.hp = 5;
                        potion_Count = 0;
                        special_Count = 0;
                        potion_Pos[0] = new Vector2(700, 390);
                        potion_Ena[0] = false;
                        potion_Use[0] = false;
                        player_died.Pause(0, 0);
                    }
                    else
                    {
                        noLoad = true;
                    }
                    lastTimeLoad = gameTime.TotalGameTime;

                    Switch = "Loading";
                }
                else if (select == 3)
                {
                    Exit();
                }
            }
        }

        private void gameKeyDown()
        {
            keyboardState = Keyboard.GetState();
            if (!menuLoading)
            {
                var walk = soundEffects[0].CreateInstance();
                if (keyboardState.IsKeyDown(Keys.Left) && stop_move == false)
                {
                    player_Pos.X -= 2;
                    w_left = true;
                    if (keyboardState.IsKeyDown(Keys.Left) && walkSoundInstance.State != SoundState.Playing) { walkSoundInstance.Play(); }
                }
                else if (old_keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Left))
                {
                    player_walk.Pause(0, 0);
                    w_left = false;
                    if (old_keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Left) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                }

                if (keyboardState.IsKeyDown(Keys.Right) && stop_move == false)
                {
                    player_Pos.X += 2;
                    w_right = true;
                    if (keyboardState.IsKeyDown(Keys.Right) && walkSoundInstance.State != SoundState.Playing) { walkSoundInstance.Play(); }
                }
                else if (old_keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Right))
                {
                    player_walk.Pause(0, 0);
                    w_right = false;
                    if (old_keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Right) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                }
                if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Left) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                old_keyboardState = keyboardState;
            }
           
        }

        private void gameKeyAttack(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (!menuLoading)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.A) && lastTimeAttack + intervalBetweenAttack < gameTime.TotalGameTime && lastTimeBlock + _intervalBetweenBlock < gameTime.TotalGameTime && lastTimeSpecial + sIntervalBetweenAttack < gameTime.TotalGameTime)
                {
                    if (keyboardState.IsKeyDown(Keys.A) && SwordWhoosh.State != SoundState.Playing) { SwordWhoosh.Play(); }
                    def = false;
                    special_ani = false;
                    atk = true;
                    stop_move = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.A) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    player_atk.Play();
                    player_walk.Pause(0, 0);

                    lastTimeAttack = gameTime.TotalGameTime;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && lastTimeBlock + intervalBetweenBlock < gameTime.TotalGameTime && lastTimeAttack + _intervalBetweenAttack < gameTime.TotalGameTime && lastTimeSpecial + sIntervalBetweenAttack < gameTime.TotalGameTime)
                {
                    atk = false;
                    special_ani = false;
                    def = true;
                    stop_move = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.S) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    player_def.Play();
                    player_walk.Pause(0, 0);

                    lastTimeBlock = gameTime.TotalGameTime;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.F) && special_Count == 4 && lastTimeSpecial + sIntervalBetweenAttack < gameTime.TotalGameTime && lastTimeBlock + _intervalBetweenBlock < gameTime.TotalGameTime && lastTimeAttack + _intervalBetweenAttack < gameTime.TotalGameTime)
                {
                    atk = false;
                    def = false;
                    special = true;
                    special_ani = true;
                    stop_move = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.F) && walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                    soundEffects[6].Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                    atk_special.Play();
                    player_special.Play();
                    player_walk.Pause(0, 0);
                    special_Pos = new Vector2(player_Pos.X + 120, player_Pos.Y);
                    special_Count = 0;

                    lastTimeSpecial = gameTime.TotalGameTime;
                    lastTimeSpecialAni = gameTime.TotalGameTime;
                }

                // ก่ารทำงาน
                if (lastTimeAttack + _intervalBetweenAttack < gameTime.TotalGameTime && lastTimeBlock + _intervalBetweenBlock < gameTime.TotalGameTime && lastTimeSpecialAni + sIntervalBetweenAttackAni < gameTime.TotalGameTime)
                {
                    if (SwordWhoosh.State != SoundState.Stopped) { SwordWhoosh.Stop(); }
                    if (SwordHit.State != SoundState.Stopped) { SwordHit.Stop(); }
                    atk = false;
                    stop_move = false;
                    def = false;
                    special_ani = false;
                    player_special.Pause(0, 0);
                    player_atk.Pause(0, 0);
                    player_def.Pause(0, 0);
                }
                if (lastTimeSpecial + s_IntervalBetweenAttack < gameTime.TotalGameTime)
                {
                    special = false;
                    special_Pos = new Vector2(500, 600);
                    atk_special.Pause(0, 0);
                }
            } 
        }

        private void gameKeyPause(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (lastTimeSelect + intervalBetweenSelect < gameTime.TotalGameTime)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    if (gamePause == false)
                    {
                        Ai1_walk.Pause();
                        Ai1_walk.Pause();
                        Ai2_atk.Pause();
                        Ai2_walk.Pause();
                        Ai3_atk.Pause();
                        Ai3_walk.Pause();
                        player_died.Pause();
                        player_walk.Pause();
                        player_idle.Pause();
                        player_atk.Pause();
                        player_def.Pause();
                        player_special.Pause();
                        atk_special.Pause();
                        gamePause = true;
                    }
                    else
                    {
                        Ai1_walk.Play();
                        Ai1_walk.Play();
                        Ai2_atk.Play();
                        Ai2_walk.Play();
                        Ai3_atk.Play();
                        Ai3_walk.Play();
                        player_died.Play();
                        player_walk.Play();
                        player_idle.Play();
                        player_atk.Play();
                        player_def.Play();
                        player_special.Play();
                        atk_special.Play();
                        gamePause = false;
                    }
                    lastTimeSelect = gameTime.TotalGameTime;
                }
            }
        }
    }
}