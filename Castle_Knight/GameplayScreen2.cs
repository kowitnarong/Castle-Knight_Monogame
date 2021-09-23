using System;
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
    public class GameplayScreen2 : screen
    {
        bool resetValue = false;

        Player Player = new Player();
        Enemy enemyArcher = new Enemy();
        Enemy enemyGold = new Enemy();
        Enemy enemyGhost = new Enemy();
        Enemy enemyGhost2 = new Enemy();
        Enemy enemyArcherR = new Enemy();
        Enemy enemyBlack2 = new Enemy();

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
        int potion_Count = 0;
        private AnimatedTexture loading;
        private AnimatedTexture effect1;

        Texture2D buttonRetry;
        Texture2D buttonSoundOn;
        Texture2D buttonSoundOff;
        Texture2D buttonExit;
        private AnimatedTexture buttonSelect;
        Vector2 select_Pos;
        int select = 0;
        bool stopPress = false;

        // Sound
        Song bgSong2;
        List<SoundEffect> soundEffects;
        SoundEffectInstance walkSoundInstance;
        SoundEffectInstance SwordHit;
        SoundEffectInstance SwordWhoosh;
        SoundEffectInstance Dead;
        bool bg2Song = false;

        Texture2D BG1_1;
        Texture2D BG1_2;
        Texture2D Heart;
        Texture2D potion;
        // Special attack
        Texture2D special1;
        Texture2D special2;
        Texture2D special3;
        Texture2D special4;
        Texture2D special5;
        Vector2[] potion_Pos = new Vector2[2];
        bool[] potion_Ena = new bool[2];
        bool[] potion_Use = new bool[2];

        KeyboardState keyboardState;
        KeyboardState old_keyboardState;

        Camera camera = new Camera();

        // Ai
        Random r = new Random();

        // Ai1
        Texture2D eWaveAtk;
        bool ai1_Wave = false;
        bool ai1_Use = false;
        bool arrowOn = false;
        Vector2 Ai1WavePos = new Vector2(0, 600);
        int speedArrow = 0;

        // Ai2
        Texture2D eWaveAtk2;
        bool ai2_Wave = false;
        bool ai2_Use = false;
        Vector2 Ai2WavePos = new Vector2(0, 600);

        // Ai5
        Texture2D eWaveAtk5;
        bool ai5_Wave = false;
        bool ai5_Use = false;
        bool arrowOn5 = false;
        Vector2 Ai5WavePos = new Vector2(0, 600);
        int speedArrow5 = 0;

        // ฉาก
        private AnimatedTexture Touch;

        bool batMove = false;
        bool batMove2 = false;
        private AnimatedTexture Bat;

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

        // time wait loading
        private static readonly TimeSpan intervalBetweenLoad = TimeSpan.FromMilliseconds(2500);
        private TimeSpan lastTimeLoad;

        // time died
        private static readonly TimeSpan intervalBetweenDied = TimeSpan.FromMilliseconds(700);
        private TimeSpan lastTimeDied;

        // time effec1
        private static readonly TimeSpan effectfadeOut = TimeSpan.FromMilliseconds(30);
        private TimeSpan lasttimeEffect;

        // Enemy time
        private TimeSpan eDelayAtk1 = TimeSpan.FromMilliseconds(2050);
        private static readonly TimeSpan eCoolDownAtk1 = TimeSpan.FromMilliseconds(450);
        private TimeSpan eAtkTime1;
        private static readonly TimeSpan DelayAttackWave1 = TimeSpan.FromMilliseconds(2000);
        private TimeSpan AttackWave1;

        private TimeSpan eDelayAtk2 = TimeSpan.FromMilliseconds(1650);
        private static readonly TimeSpan eCoolDownAtk2 = TimeSpan.FromMilliseconds(400);
        private TimeSpan eAtkTime2;

        private static readonly TimeSpan DelayAttackWave2 = TimeSpan.FromMilliseconds(2000);
        private TimeSpan AttackWave2;

        private static readonly TimeSpan DelayAttack3 = TimeSpan.FromMilliseconds(2000);
        private TimeSpan lasttimeDelay3;

        private static readonly TimeSpan DelayAttack4 = TimeSpan.FromMilliseconds(2000);
        private TimeSpan lasttimeDelay4;

        private TimeSpan eDelayAtk5 = TimeSpan.FromMilliseconds(1900);
        private static readonly TimeSpan eCoolDownAtk5 = TimeSpan.FromMilliseconds(450);
        private TimeSpan eAtkTime5;
        private static readonly TimeSpan DelayAttackWave5 = TimeSpan.FromMilliseconds(1600);
        private TimeSpan AttackWave5;

        private static readonly TimeSpan eDelayAtk6 = TimeSpan.FromMilliseconds(2200);
        private static readonly TimeSpan eCoolDownAtk6 = TimeSpan.FromMilliseconds(800);
        private TimeSpan eAtkTime6;
        #endregion

        #region Vector and Frame
        private const int Frames = 2;
        private const int FramesPerSec = 4;
        private const int FramesRow = 1;

        // effect pos
        private Vector2 effect1_Pos = new Vector2(700, 600);

        // Pos Camera
        private Vector2 Camera_Pos = new Vector2(700, 255);

        // Bat Pos
        private Vector2 Bat_Pos = new Vector2(450, 80);
        private Vector2 Bat2_Pos = new Vector2(1450, 80);

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
        public GameplayScreen2(Game1 game, EventHandler theScreenEvent)
        : base(theScreenEvent)
        {
            #region AnimatedTexture
            soundEffects = new List<SoundEffect>();
            buttonSelect = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.defAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            loading = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.diedAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.specialAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Player.specialAtkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);

            enemyArcher.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyArcher.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyGold.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyGold.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyGhost.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyGhost2.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyArcherR.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyArcherR.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack2.walkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack2.idleAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            enemyBlack2.atkAni = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);

            Touch = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            effect1 = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);
            Bat = new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth);

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
            soundEffects.Add(game.Content.Load<SoundEffect>("WalkSound5")); //[8]
            soundEffects.Add(game.Content.Load<SoundEffect>("ArrowSound")); //[9]

            walkSoundInstance = soundEffects[8].CreateInstance();
            SwordHit = soundEffects[2].CreateInstance();
            SwordWhoosh = soundEffects[3].CreateInstance();
            Dead = soundEffects[7].CreateInstance();

            bgSong2 = game.Content.Load<Song>("BackgroundLevel2");
            MediaPlayer.IsRepeating = true;

            ButtonGuide = game.Content.Load<Texture2D>("ButtonGuide");
            ButtonMenu = game.Content.Load<Texture2D>("ButtonMenu");
            effect1.Load(game.Content, "effect1", 3, 1, 15);
            Bat.Load(game.Content, "Bat", 2, 1, 6);
            pausePic = game.Content.Load<Texture2D>("pause");
            ArialFont = game.Content.Load<SpriteFont>("ArialFont");
            BG1_1 = game.Content.Load<Texture2D>("Map1");
            BG1_2 = game.Content.Load<Texture2D>("Map1_2");
            Heart = game.Content.Load<Texture2D>("Heart");
            gameOver = game.Content.Load<Texture2D>("Game over");
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
            enemyArcher.atkAni.Load(game.Content, "ArcherAni", 8, e_atk_FramesRow, 10);
            enemyArcher.idleAni.Load(game.Content, "Archer_idle", Frames, FramesRow, FramesPerSec);
            eWaveAtk = game.Content.Load<Texture2D>("Arrow");
            enemyGold.walkAni.Load(game.Content, "goldEnemy", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            enemyGold.atkAni.Load(game.Content, "goldEnemyAtk", 8, e_atk_FramesRow, 16);
            eWaveAtk2 = game.Content.Load<Texture2D>("goldEneAtkWave");
            enemyGhost.walkAni.Load(game.Content, "Ghost", 2, 1, 4);
            enemyGhost2.walkAni.Load(game.Content, "Ghost", 2, 1, 4);
            enemyArcherR.atkAni.Load(game.Content, "ArcherAni2", 8, e_atk_FramesRow, 10);
            enemyArcherR.idleAni.Load(game.Content, "Archer_idle2", Frames, FramesRow, FramesPerSec);
            eWaveAtk5 = game.Content.Load<Texture2D>("Arrow2");
            enemyBlack2.walkAni.Load(game.Content, "enemy_walk", e_walk_Frames, e_walk_FramesRow, e_walk_FramesPerSec);
            enemyBlack2.idleAni.Load(game.Content, "enemy_idle", Frames, FramesRow, FramesPerSec);
            enemyBlack2.atkAni.Load(game.Content, "enemy_atk", e_atk_Frames, e_atk_FramesRow, e_atk_FramesPerSec);

            buttonRetry = game.Content.Load<Texture2D>("RetryButton");
            buttonSoundOn = game.Content.Load<Texture2D>("SfxOn");
            buttonSoundOff = game.Content.Load<Texture2D>("SfxOff");
            buttonExit = game.Content.Load<Texture2D>("ExitButton");
            buttonSelect.Load(game.Content, "Select", 4, 1, 5);

            Player.walkAni.Pause();

            Touch.Load(game.Content, "Touch", 9, 1, 5);

            #endregion

            game.IsMouseVisible = true;
            Player.hp = 5;
            enemyArcher.hp = 3;
            enemyGold.hp = 5;
            enemyGhost.hp = 1;
            enemyGhost2.hp = 1;
            enemyArcherR.hp = 5;
            enemyBlack2.hp = 5;


            potion_Ena[0] = false;
            potion_Use[0] = false;
            potion_Pos[0] = new Vector2(700, 390);

            potion_Ena[1] = false;
            potion_Use[1] = false;
            potion_Pos[1] = new Vector2(2200, 390);

            select_Pos = new Vector2(340, 185);

            Player.Position = new Vector2(50, 255);

            enemyArcher.Position = new Vector2(800, 255);

            enemyGold.Position = new Vector2(2000, 255);

            enemyGhost.Position = new Vector2(-600, 255);
            enemyGhost2.Position = new Vector2(-600, 255);

            enemyArcherR.Position = new Vector2(2600, 255);

            enemyBlack2.Position = new Vector2(3600, 255);

            this.game = game;
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.5f;
            MediaPlayer.Play(bgSong2);
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

            PauseTime = TimeSpan.FromMilliseconds(0);
            bg2Song = false;
            select = 0;
            gamePause = false;
            arrowOn = false;
            arrowOn5 = false;
            effect1_Pos = new Vector2(700, 600);
            Player.Position = new Vector2(50, 255);
            Camera_Pos = new Vector2(700, 255);
            special_Pos = new Vector2(500, 600);
            potion_Count = 0;
            Player.SpeCount = 0;
            enemyArcher.Position = new Vector2(800, 255);
            enemyGold.Position = new Vector2(2000, 255);
            enemyGhost.Position = new Vector2(-600, 255);
            enemyGhost2.Position = new Vector2(-600, 255);
            enemyArcherR.Position = new Vector2(2600, 255);
            enemyBlack2.Position = new Vector2(3600, 255);
            enemyArcher.hp = 3;
            enemyGold.hp = 5;
            enemyGhost.hp = 1;
            enemyGhost2.hp = 1;
            enemyArcherR.hp = 5;
            enemyBlack2.hp = 5;
            enemyArcher.died = false;
            enemyGold.died = false;
            enemyGhost.died = false;
            enemyGhost2.died = false;
            enemyArcherR.died = false;
            enemyBlack2.died = false;
            speedArrow = 0;
            speedArrow5 = 0;
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
            if (load)
            {
                menuLoading = true;

                if (!loadOn) { lastTimeLoad = theTime.TotalGameTime; loadOn = true; }
            }
            if (Switch == "InGame2")
            {
                if (!bg2Song)
                {
                    MediaPlayer.Play(bgSong2);
                    bg2Song = true;
                }
                if (Game1.soundOn)
                {
                    MediaPlayer.IsMuted = false;
                    SoundEffect.MasterVolume = 0.5f;
                }
                else if (!Game1.soundOn)
                {
                    MediaPlayer.IsMuted = true;
                    SoundEffect.MasterVolume = 0f;
                    if (walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                }

                // Key Pause
                GameKeyPause(theTime);
                if (gamePause == false)
                {
                    if (!Player.died)
                    {
                        // Time Pause
                        if (lastTimePauseOn + TimeSpan.FromMilliseconds(2200) < theTime.TotalGameTime)
                        {
                            PauseTime = TimeSpan.FromMilliseconds(0);
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
                            enemyArcher.Heart_Pos[i].X = (enemyArcher.Position.X + 12) - (i + 1) * -22;
                            enemyArcher.Heart_Pos[i].Y = enemyArcher.Position.Y - 15;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            enemyGold.Heart_Pos[i].X = (enemyGold.Position.X + 20) - (i + 1) * -22;
                            enemyGold.Heart_Pos[i].Y = enemyGold.Position.Y - 15;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            enemyArcherR.Heart_Pos[i].X = (enemyArcherR.Position.X - 12) - (i + 1) * -22;
                            enemyArcherR.Heart_Pos[i].Y = enemyArcherR.Position.Y - 15;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            enemyBlack2.Heart_Pos[i].X = (enemyBlack2.Position.X + 20) - (i + 1) * -22;
                            enemyBlack2.Heart_Pos[i].Y = enemyBlack2.Position.Y - 15;
                        }

                        #endregion

                        // KeyDown
                        GameKeyDown();

                        // Key Attack
                        GameKeyAttack(theTime);

                        Rectangle[] charPotion = new Rectangle[2];
                        charPotion[0] = new Rectangle((int)potion_Pos[0].X, (int)potion_Pos[0].Y, 32, 32);
                        charPotion[1] = new Rectangle((int)potion_Pos[1].X, (int)potion_Pos[1].Y, 32, 32);
                        enemyArcher.charBlock = new Rectangle((int)enemyArcher.Position.X + 18, (int)enemyArcher.Position.Y, 32, 160);
                        enemyGold.charBlock = new Rectangle((int)enemyGold.Position.X + 64, (int)enemyGold.Position.Y, 32, 160);
                        enemyGhost.charBlock = new Rectangle((int)enemyGhost.Position.X + 36, (int)enemyGhost.Position.Y, 81, 135);
                        enemyGhost2.charBlock = new Rectangle((int)enemyGhost2.Position.X + 36, (int)enemyGhost2.Position.Y, 81, 135);
                        enemyArcherR.charBlock = new Rectangle((int)enemyArcherR.Position.X + 18, (int)enemyArcherR.Position.Y, 32, 160);
                        enemyBlack2.charBlock = new Rectangle((int)enemyBlack2.Position.X + 64, (int)enemyBlack2.Position.Y, 32, 160);
                        Rectangle blockEnemy1Wave;
                        Rectangle blockEnemy2Wave;
                        Rectangle blockEnemy5Wave;

                        #region PlayerAbility
                        if (special == true)
                        {
                            Player.charBlock = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 125, 160);
                            Player.charSpecialAtk = new Rectangle((int)special_Pos.X, (int)special_Pos.Y, 20, 32);
                            special_Pos.X += 15;
                            if (Player.charSpecialAtk.Intersects(enemyArcher.charBlock) && enemyArcher.died == false)
                            {
                                if (enemyArcher.lastTimeKnockBack + enemyArcher.DelayKnockBack < theTime.TotalGameTime)
                                {
                                    enemyArcher.knockBack = true;

                                    enemyArcher.lastTimeKnockBack = theTime.TotalGameTime;
                                }
                                if (enemyArcher.lastTimeHp + enemyArcher.DelayHp < theTime.TotalGameTime)
                                {
                                    enemyArcher.hp -= 2;

                                    enemyArcher.lastTimeHp = theTime.TotalGameTime;
                                }
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
                            else if (Player.charSpecialAtk.Intersects(enemyArcherR.charBlock) && enemyArcherR.died == false)
                            {
                                if (enemyArcherR.lastTimeKnockBack + enemyArcherR.DelayKnockBack < theTime.TotalGameTime)
                                {
                                    enemyArcherR.knockBack = true;

                                    enemyArcherR.lastTimeKnockBack = theTime.TotalGameTime;
                                }
                                if (enemyArcherR.lastTimeHp + enemyArcherR.DelayHp < theTime.TotalGameTime)
                                {
                                    enemyArcherR.hp -= 2;

                                    enemyArcherR.lastTimeHp = theTime.TotalGameTime;
                                }
                            }
                            else if (Player.charSpecialAtk.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                            {
                                enemyBlack2.PlayerSpecialAtk(theTime);
                            }
                        }
                        if (Player.atk == true)
                        {
                            Player.charBlock = new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 125, 160);
                            if (Player.charBlock.Intersects(enemyArcher.charBlock) && enemyArcher.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyArcher.PlayerAtk(theTime, SwordHit);                   
                            }
                            else if (Player.charBlock.Intersects(enemyGold.charBlock) && enemyGold.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyGold.PlayerAtk(theTime, SwordHit);
                            }
                            else if (Player.charBlock.Intersects(enemyGhost.charBlock) && enemyGhost.died == false)
                            {
                                
                                if (enemyGhost.lastTimeHp + enemyGhost.DelayHp < theTime.TotalGameTime)
                                {
                                    if (SwordWhoosh.State != SoundState.Stopped) { SwordWhoosh.Stop(); }
                                    if (SwordWhoosh.State != SoundState.Playing) { SwordWhoosh.Play(); }
                                    enemyGhost.hp -= 1;

                                    enemyGhost.lastTimeHp = theTime.TotalGameTime;
                                }
                            }
                            else if (Player.charBlock.Intersects(enemyGhost2.charBlock) && enemyGhost2.died == false)
                            {
                                
                                if (enemyGhost2.lastTimeHp + enemyGhost2.DelayHp < theTime.TotalGameTime)
                                {
                                    if (SwordWhoosh.State != SoundState.Stopped) { SwordWhoosh.Stop(); }
                                    if (SwordWhoosh.State != SoundState.Playing) { SwordWhoosh.Play(); }
                                    enemyGhost2.hp -= 1;

                                    enemyGhost2.lastTimeHp = theTime.TotalGameTime;
                                }
                            }
                            else if (Player.charBlock.Intersects(enemyArcherR.charBlock) && enemyArcherR.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyArcherR.PlayerAtk(theTime, SwordHit);
                            }
                            else if (Player.charBlock.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                            {
                                effect1.Play();
                                effect1_Pos = new Vector2(Player.Position.X + 110, Player.Position.Y + 57);

                                lasttimeEffect = theTime.TotalGameTime;
                                enemyBlack2.PlayerAtk(theTime, SwordHit);
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
                        }

                        #endregion

                        #region EnemyAbility
                        // Enemy 1
                        if (!enemyArcher.died)
                        {
                            if (ai1_Wave == true)
                            {
                                if (enemyArcher.atkAni.Frame >= 4)
                                {
                                    arrowOn = true;
                                }
                                if (!ai1_Use && arrowOn)
                                {
                                    Ai1WavePos = new Vector2(enemyArcher.Position.X - 15, enemyArcher.Position.Y + 15);
                                    ai1_Use = true;
                                }
                                if (arrowOn)
                                {
                                    if (enemyArcher.Position.X < 1200)
                                    {
                                        if (speedArrow % 2 == 0)
                                        {
                                            Ai1WavePos.X -= 20;
                                        }
                                        else if (speedArrow % 2 != 0)
                                        {
                                            Ai1WavePos.X -= 8;
                                        }
                                    }
                                    else if (enemyArcher.Position.X >= 1200)
                                    {
                                        Ai1WavePos.X -= 30;
                                    }
                                    blockEnemy1Wave = new Rectangle((int)Ai1WavePos.X, (int)Ai1WavePos.Y, 144, 144);
                                    if (Player.charBlock.Intersects(blockEnemy1Wave) && enemyArcher.died == false)
                                    {
                                        if (def == true)
                                        {
                                            Player.WhenDefTrue(theTime, soundEffects);

                                            Ai1WavePos.Y = 600;
                                        }
                                        else
                                        {
                                            Player.WhenDefFalse(theTime, soundEffects);

                                            Ai1WavePos.Y = 600;
                                        }
                                    }
                                }           
                            }
                            if (enemyArcher.atk == true)
                            {
                                enemyArcher.atkAni.Play();
                                if (enemyArcher.Position.X < 1400)
                                {
                                    if (enemyArcher.atkAni.Frame >= 4 && enemyArcher.atkAni.Frame <= 6) { enemyArcher.Position.X += 8; }
                                }
                                enemyArcher.charBlock = new Rectangle((int)enemyArcher.Position.X + 18, (int)enemyArcher.Position.Y, 32, 160);
                            }
                            else
                            {
                                if (enemyArcher.died == false)
                                {
                                    enemyArcher.charBlock = new Rectangle((int)enemyArcher.Position.X + 18, (int)enemyArcher.Position.Y, 32, 160);
                                    if (Player.charBlock.Intersects(enemyArcher.charBlock) && enemyArcher.died == false)
                                    {
                                        if (!Player.atk)
                                        {
                                            Player.WhenDefFalse(theTime, soundEffects);
                                        }
                                    }
                                }
                            }
                        }

                        // Enemy 2
                        if (ai2_Wave == true && enemyArcher.died == true)
                        {
                            if (!ai2_Use)
                            {
                                Ai2WavePos = new Vector2(enemyGold.Position.X, enemyGold.Position.Y);
                                ai2_Use = true;
                            }
                            Ai2WavePos.X -= 7;
                            blockEnemy2Wave = new Rectangle((int)Ai2WavePos.X, (int)Ai2WavePos.Y, 50, 160);
                            if (Player.charBlock.Intersects(blockEnemy2Wave) && enemyGold.died == false)
                            {
                                if (def == true)
                                {
                                    Player.WhenDefTrue(theTime, soundEffects);

                                    Ai2WavePos.Y = 600;
                                    ai2_Wave = false;
                                }
                                else
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);

                                    Ai2WavePos.Y = 600;
                                    ai2_Wave = false;
                                }
                            }
                        }
                        if (enemyGold.atk == true && enemyArcher.died == true)
                        {
                            enemyGold.charBlock = new Rectangle((int)enemyGold.Position.X + 18, (int)enemyGold.Position.Y, 32, 160);
                            if (Player.charBlock.Intersects(enemyGold.charBlock) && enemyGold.died == false && ai2_Wave == false)
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
                            if (enemyGold.died == false && enemyArcher.died == true)
                            {
                                enemyGold.charBlock = new Rectangle((int)enemyGold.Position.X + 96, (int)enemyGold.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyGold.charBlock) && enemyGold.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }
                        #endregion
                        // Enemy 3
                        if(enemyGhost.atk && enemyArcher.died && enemyGold.died)
                        {
                            enemyGhost.Position.X -= 8;
                            enemyGhost.charBlock = new Rectangle((int)enemyGhost.Position.X + 36, (int)enemyGhost.Position.Y, 81, 135);
                            if (Player.charBlock.Intersects(enemyGhost.charBlock) && enemyGhost.died == false && !Player.atk)
                            {
                                if (lasttimeDelay3 + DelayAttack3 < theTime.TotalGameTime)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }

                                lasttimeDelay3 = theTime.TotalGameTime;
                            }
                        }

                        // Enemy 4
                        if (enemyGhost2.atk && enemyArcher.died && enemyGold.died)
                        {
                            enemyGhost2.Position.X -= 8;
                            enemyGhost2.charBlock = new Rectangle((int)enemyGhost2.Position.X + 36, (int)enemyGhost2.Position.Y, 81, 135);
                            if (Player.charBlock.Intersects(enemyGhost2.charBlock) && enemyGhost2.died == false && !Player.atk)
                            {
                                if (lasttimeDelay4 + DelayAttack4 < theTime.TotalGameTime)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }

                                lasttimeDelay4 = theTime.TotalGameTime;
                            }
                        }

                        // Enemy 5
                        if (!enemyArcherR.died && enemyGhost.died && enemyGhost2.died && enemyArcher.died && enemyGold.died)
                        {
                            if (ai5_Wave == true)
                            {
                                if (enemyArcherR.atkAni.Frame >= 4)
                                {
                                    arrowOn5 = true;
                                }
                                if (!ai5_Use && arrowOn5)
                                {
                                    Ai5WavePos = new Vector2(enemyArcherR.Position.X - 15, enemyArcherR.Position.Y + 15);
                                    ai5_Use = true;
                                }
                                if (arrowOn5)
                                {
                                    if (enemyArcherR.Position.X < 3200)
                                    {
                                        if (speedArrow5 % 3 == 0)
                                        {
                                            Ai5WavePos.X -= 15;
                                        }
                                        else if (speedArrow5 % 3 != 0)
                                        {
                                            Ai5WavePos.X -= 30;
                                        }
                                    }
                                    else if (enemyArcherR.Position.X >= 3200)
                                    {
                                        Ai5WavePos.X -= 40;
                                    }
                                    blockEnemy5Wave = new Rectangle((int)Ai5WavePos.X, (int)Ai5WavePos.Y, 144, 144);
                                    if (Player.charBlock.Intersects(blockEnemy5Wave) && enemyArcherR.died == false)
                                    {
                                        if (def == true)
                                        {
                                            Player.WhenDefTrue(theTime, soundEffects);

                                            Ai5WavePos.Y = 600;
                                        }
                                        else
                                        {
                                            Player.WhenDefFalse(theTime, soundEffects);

                                            Ai5WavePos.Y = 600;
                                        }
                                    }
                                }
                            }
                            if (enemyArcherR.atk == true)
                            {
                                if (enemyArcherR.Position.X < 3000)
                                {
                                    if (enemyArcherR.atkAni.Frame >= 4 && enemyArcherR.atkAni.Frame <= 6) { enemyArcherR.Position.X += 4; }
                                }
                                enemyArcherR.charBlock = new Rectangle((int)enemyArcherR.Position.X + 18, (int)enemyArcherR.Position.Y, 32, 160);
                            }
                            else
                            {
                                if (enemyArcherR.died == false)
                                {
                                    enemyArcherR.charBlock = new Rectangle((int)enemyArcherR.Position.X + 18, (int)enemyArcherR.Position.Y, 32, 160);
                                    if (Player.charBlock.Intersects(enemyArcherR.charBlock) && enemyArcherR.died == false)
                                    {
                                        if (!Player.atk)
                                        {
                                            Player.WhenDefFalse(theTime, soundEffects);
                                        }
                                    }
                                }
                            }
                        }
                        
                        // Enemy 6
                        if (enemyBlack2.atk == true && enemyArcherR.died && enemyGhost.died && enemyGhost2.died && enemyArcher.died && enemyGold.died)
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
                            if (enemyBlack2.died == false && enemyArcherR.died && enemyGhost.died && enemyGhost2.died && enemyArcher.died && enemyGold.died)
                            {
                                enemyBlack2.charBlock = new Rectangle((int)enemyBlack2.Position.X + 96, (int)enemyBlack2.Position.Y, 32, 160);
                                if (Player.charBlock.Intersects(enemyBlack2.charBlock) && enemyBlack2.died == false)
                                {
                                    Player.WhenDefFalse(theTime, soundEffects);
                                }
                            }
                        }

                        if (Player.died == false)
                        {
                            // Enemy 1
                            if (!enemyArcher.died)
                            {
                                enemyArcher.idleAni.Play();
                                if (enemyArcher.lastTimeHp + enemyArcher.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyArcher.Position.X - Player.Position.X < 600)
                                    {
                                        if (eAtkTime1 + eCoolDownAtk1 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyArcher.atk = false;
                                            enemyArcher.idle = true;
                                            enemyArcher.atkAni.Pause(0, 0);
                                        }
                                        if (eAtkTime1 + eDelayAtk1 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyArcher.atkAni.Play();
                                            enemyArcher.idle = false;
                                            enemyArcher.atk = true;
                                            ai1_Wave = true;
                                            ai1_Use = false;
                                            speedArrow += 1;
                                            soundEffects[9].Play(volume: 1f, pitch: 0.0f, pan: 0.0f);

                                            AttackWave1 = theTime.TotalGameTime;
                                            eAtkTime1 = theTime.TotalGameTime;
                                        }
                                        if (AttackWave1 + DelayAttackWave1 + PauseTime < theTime.TotalGameTime)
                                        {
                                            arrowOn = false;
                                            ai1_Wave = false;
                                            Ai1WavePos.Y = 600;
                                        }
                                    }
                                    else if (enemyArcher.Position.X - Player.Position.X >= 600)
                                    {
                                        enemyArcher.idle = true;
                                        enemyArcher.atk = false;
                                    }
                                }
                            }

                            // Enemy 2
                            if (enemyArcher.died == true && !enemyGold.died)
                            {
                                if (enemyGold.lastTimeHp + enemyGold.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyGold.Position.X - Player.Position.X >= 120)
                                    {
                                        eDelayAtk2 = TimeSpan.FromMilliseconds(1500);
                                        if (eAtkTime2 + eDelayAtk2 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyGold.atk = true;
                                            ai2_Wave = true;
                                            ai2_Use = false;
                                            AttackWave2 = theTime.TotalGameTime;
                                            eAtkTime2 = theTime.TotalGameTime;
                                        }
                                        else if (eAtkTime2 + eCoolDownAtk2 + PauseTime < theTime.TotalGameTime)
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
                                        if (eAtkTime2 + eCoolDownAtk2 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyGold.walkAni.Play();
                                            enemyGold.Position.X -= 1;
                                            enemyGold.atk = false;
                                        }
                                    }
                                    else if (enemyGold.Position.X - Player.Position.X < 61)
                                    {
                                        eDelayAtk2 = TimeSpan.FromMilliseconds(1200);
                                        if (eAtkTime2 + eDelayAtk2 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyGold.atkAni.Play();
                                            enemyGold.atk = true;

                                            eAtkTime2 = theTime.TotalGameTime;
                                        }
                                        if (eAtkTime2 + eCoolDownAtk2 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyGold.atkAni.Pause(0, 0);
                                            enemyGold.walkAni.Play();
                                            enemyGold.Position.X -= 1;
                                            enemyGold.atk = false;
                                        }
                                    }
                                }
                            }
                            if (AttackWave2 + DelayAttackWave2 + PauseTime < theTime.TotalGameTime)
                            {
                                ai2_Wave = false;
                                Ai2WavePos.Y = 600;
                            }

                            // Enemy 3
                            if (enemyArcher.died && enemyGold.died && !enemyGhost.died)
                            {
                                enemyGhost.atk = true;
                                if (Player.Position.X - enemyGhost.Position.X >= 800)
                                {
                                    enemyGhost.Position.X = Player.Position.X + 1500;
                                }

                            }

                            // Enemy 4
                            if (enemyArcher.died && enemyGold.died && !enemyGhost2.died)
                            {
                                enemyGhost2.atk = true;
                                if (Player.Position.X - enemyGhost2.Position.X >= 400)
                                {
                                    enemyGhost2.Position.X = Player.Position.X + 1900;
                                }

                            }

                            // Enemy 5
                            if (enemyArcher.died && enemyGold.died && enemyGhost.died && enemyGhost2.died && !enemyArcherR.died)
                            {
                                enemyArcherR.idleAni.Play();
                                if (enemyArcherR.lastTimeHp + enemyArcherR.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyArcherR.Position.X - Player.Position.X < 800)
                                    {
                                        if (eAtkTime5 + eDelayAtk5 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyArcherR.atkAni.Play();
                                            enemyArcherR.idle = false;
                                            enemyArcherR.atk = true;
                                            ai5_Wave = true;
                                            ai5_Use = false;
                                            speedArrow5 += 1;
                                            soundEffects[9].Play(volume: 1f, pitch: 0.0f, pan: 0.0f);

                                            AttackWave5 = theTime.TotalGameTime;
                                            eAtkTime5 = theTime.TotalGameTime;
                                        }
                                        else if (eAtkTime5 + eCoolDownAtk5 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyArcherR.atk = false;
                                            enemyArcherR.idle = true;
                                            enemyArcherR.atkAni.Pause(0, 0);
                                        }
                                        if (AttackWave5 + DelayAttackWave5 + PauseTime < theTime.TotalGameTime)
                                        {
                                            arrowOn5 = false;
                                            ai5_Wave = false;
                                            Ai5WavePos.Y = 600;
                                        }
                                    }
                                    else if (enemyArcherR.Position.X - Player.Position.X >= 600)
                                    {
                                        enemyArcherR.idle = true;
                                        enemyArcherR.atk = false;
                                    }
                                }
                            }

                            if (enemyArcher.died && enemyGold.died && enemyGhost.died && enemyGhost2.died && enemyArcherR.died && !enemyBlack2.died)
                            {
                                if (enemyBlack2.lastTimeHp + enemyBlack2.DelayHp + PauseTime < theTime.TotalGameTime)
                                {
                                    if (enemyBlack2.Position.X - Player.Position.X >= 61)
                                    {
                                        if (eAtkTime6 + eDelayAtk6 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.walkAni.Play();
                                            enemyBlack2.atk = false;
                                            enemyBlack2.Position.X -= 2;
                                            enemyBlack2.atkAni.Pause(0, 0);
                                        }
                                        else if (eAtkTime6 + eCoolDownAtk6 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.Position.X -= 2;
                                            enemyBlack2.walkAni.Play();
                                            enemyBlack2.atk = false;
                                        }
                                    }
                                    else
                                    {
                                        enemyBlack2.atkAni.Play();
                                        if (eAtkTime6 + eDelayAtk6 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.atk = true;

                                            eAtkTime6 = theTime.TotalGameTime;
                                        }
                                        if (eAtkTime6 + eCoolDownAtk6 + PauseTime < theTime.TotalGameTime)
                                        {
                                            enemyBlack2.walkAni.Play();
                                            enemyBlack2.atk = false;
                                        }
                                    }
                                }
                            }

                            #region KnockBack

                            Player.PlayerKnockBack(theTime);

                            // Enemy knockBack
                            if (enemyArcher.died == false)
                            {
                                enemyArcher.EnemyKnockBack(theTime);
                            }
                            if (enemyArcher.died == true && enemyGold.died == false)
                            {
                                enemyGold.EnemyKnockBack(theTime);
                            }
                            if (enemyArcherR.died == false && enemyArcher.died && enemyGold.died)
                            {
                                enemyArcherR.EnemyKnockBack(theTime);
                            }
                            if (enemyBlack2.died == false && enemyArcher.died && enemyGold.died && enemyArcherR.died)
                            {
                                enemyBlack2.EnemyKnockBack(theTime);
                            }
                        }
                        else
                        {
                            enemyArcher.atkAni.Pause(0, 0);
                            enemyArcher.idleAni.Pause(0, 0);
                            enemyGold.atkAni.Pause(0, 0);
                            enemyGold.walkAni.Pause(0, 0);
                            enemyGhost.walkAni.Pause(0, 0);
                            enemyGhost2.walkAni.Pause(0, 0);
                            enemyArcherR.atkAni.Pause(0, 0);
                            enemyArcherR.idleAni.Pause(0, 0);
                            enemyBlack2.atkAni.Pause(0, 0);
                            enemyBlack2.walkAni.Pause(0, 0);
                            enemyBlack2.idleAni.Pause(0, 0);
                        }

                        #endregion

                        if (enemyArcher.hp <= 0)
                        {
                            enemyArcher.died = true;
                            enemyArcher.atk = false;
                            enemyArcher.atkAni.Pause(0, 0);
                            enemyArcher.idleAni.Pause(0, 0);
                        }
                        if (enemyGold.hp <= 0)
                        {
                            enemyGold.died = true;
                            enemyGold.atk = false;
                            enemyGold.atkAni.Pause(0, 0);
                            enemyGold.walkAni.Pause(0, 0);
                        }
                        if (enemyGhost.hp <= 0)
                        {
                            enemyGhost.died = true;
                            enemyGhost.atk = false;
                            enemyGhost.walkAni.Pause(0, 0);
                        }
                        if (enemyGhost2.hp <= 0)
                        {
                            enemyGhost2.died = true;
                            enemyGhost2.atk = false;
                            enemyGhost2.walkAni.Pause(0, 0);
                        }
                        if (enemyArcherR.hp <= 0)
                        {
                            enemyArcherR.died = true;
                            enemyArcherR.atk = false;
                            enemyArcherR.atkAni.Pause(0, 0);
                            enemyArcherR.idleAni.Pause(0, 0);
                        }
                        if (enemyBlack2.hp <= 0)
                        {
                            enemyBlack2.died = true;
                            enemyBlack2.atk = false;
                            enemyBlack2.atkAni.Pause(0, 0);
                            enemyBlack2.walkAni.Pause(0, 0);
                            enemyBlack2.idleAni.Pause(0, 0);
                        }

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

                        #region Bat movement
                        // Bat 
                        if (Bat_Pos.Y >= 75)
                        {
                            Bat_Pos.Y += 1;
                        }
                        if (Bat_Pos.Y > 120)
                        {
                            batMove = true;
                        }
                        if (batMove)
                        {
                            Bat_Pos.Y -= 2;
                            if(Bat_Pos.Y < 80)
                            {
                                batMove = false;
                            }
                        }

                        if (Bat2_Pos.Y >= 75)
                        {
                            Bat2_Pos.Y += 1;
                        }
                        if (Bat2_Pos.Y > 120)
                        {
                            batMove2 = true;
                        }
                        if (batMove2)
                        {
                            Bat2_Pos.Y -= 2;
                            if (Bat2_Pos.Y < 80)
                            {
                                batMove2 = false;
                            }
                        }
                        #endregion

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
            }
        

                #region DiedPlayer

                if (!devMode)
                {
                    if (Player.hp <= 0 && Player.died == false)
                    {
                        if (walkSoundInstance.State != SoundState.Stopped) { walkSoundInstance.Stop(); }
                        if (Dead.State != SoundState.Playing) { Dead.Play(); }
                        Player.died = true;
                        Player.stop_move = true;
                        Player.diedAni.Play();
                        enemyArcher.atkAni.Pause(0, 0);
                        enemyArcher.idleAni.Pause(0, 0);
                        enemyGold.atkAni.Pause(0, 0);
                        enemyGold.walkAni.Pause(0, 0);
                        enemyGhost.walkAni.Pause(0, 0);
                        enemyGhost2.walkAni.Pause(0, 0);
                        enemyArcherR.atkAni.Pause(0, 0);
                        enemyArcherR.idleAni.Pause(0, 0);
                        enemyBlack2.atkAni.Pause(0, 0);
                        enemyBlack2.walkAni.Pause(0, 0);

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
                        if (Switch == "InGame2")
                        { sw.WriteLine("InGame2"); }
                        sw.Flush();
                        sw.Close();
                        dead_count += 1;
                        string filepathDead = Path.Combine(@"Content\Dead.txt");
                        FileStream fsDead = new FileStream(filepathDead, FileMode.Open, FileAccess.Write);
                        StreamWriter swDead = new StreamWriter(fsDead);
                        if (Switch == "InGame2")
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
            if (Switch == "InGame2" && menuLoading == false)
            {
                theBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.ViewMatrix);
                theBatch.Draw(BG1_1, new Vector2(0 - camera.ViewMatrix.Translation.X * 0.65f, 0), Color.White);
                theBatch.Draw(BG1_2, new Vector2(0, 0), Color.White);
                Touch.DrawFrame(theBatch, new Vector2(550 - camera.ViewMatrix.Translation.X * 0.65f, 100));
                Touch.DrawFrame(theBatch, new Vector2(1550 - camera.ViewMatrix.Translation.X * 0.65f, 100));
                Bat.DrawFrame(theBatch, new Vector2(Bat_Pos.X - camera.ViewMatrix.Translation.X * 0.65f, Bat_Pos.Y - camera.ViewMatrix.Translation.Y));
                Bat.DrawFrame(theBatch, new Vector2(Bat2_Pos.X - camera.ViewMatrix.Translation.X * 0.65f, Bat2_Pos.Y - camera.ViewMatrix.Translation.Y));

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
                for (int i = 0; i < enemyArcher.hp; i++)
                {
                    theBatch.Draw(Heart, enemyArcher.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Black);
                }
                for (int i = 0; i < enemyGold.hp; i++)
                {
                    theBatch.Draw(Heart, enemyGold.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Brown);
                }
                for (int i = 0; i < enemyArcherR.hp; i++)
                {
                    theBatch.Draw(Heart, enemyArcherR.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Brown);
                }
                for (int i = 0; i < enemyBlack2.hp; i++)
                {
                    theBatch.Draw(Heart, enemyBlack2.Heart_Pos[i], new Rectangle(0, 0, 32, 32), Color.Black);
                }

                #endregion

                #region PlayerDraw
                if (Player.died == false)
                {
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
                    theBatch.Draw(gameOver, new Vector2(350 - camera.ViewMatrix.Translation.X, 120 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                #endregion

                #region EnemyDraw
                // Enemy
                if (enemyArcher.died == false)
                {
                    if (ai1_Wave == true && arrowOn)
                    {
                        theBatch.Draw(eWaveAtk, Ai1WavePos, Color.White);
                    }
                    if (enemyArcher.atk == true)
                    {
                        if (gamePause) { enemyArcher.atkAni.Pause(); }
                        if (Player.died) { enemyArcher.atkAni.Pause(0, 0); }
                        enemyArcher.atkAni.DrawFrame(theBatch, enemyArcher.Position);
                    }
                    else if (enemyArcher.idle == true)
                    {
                        if (Player.died) { enemyArcher.idleAni.Pause(); }
                        if (gamePause) { enemyArcher.idleAni.Pause(0, 0); }
                        enemyArcher.idleAni.DrawFrame(theBatch, enemyArcher.Position);
                    }
                }
                if (enemyGold.died == false && enemyArcher.died == true)
                {
                    if (ai2_Wave == true)
                    {
                        theBatch.Draw(eWaveAtk2, Ai2WavePos, Color.White);
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
                if (!enemyGhost.died && enemyArcher.died && enemyGold.died)
                {
                    if (enemyGhost.atk)
                    {
                        enemyGold.walkAni.Play();
                        if (gamePause) { enemyGhost.walkAni.Pause(); }
                        if (Player.died) { enemyGhost.walkAni.Pause(0, 0); }
                        enemyGhost.walkAni.DrawFrame(theBatch, enemyGhost.Position);
                    }
                }
                if (!enemyGhost2.died && enemyArcher.died && enemyGold.died)
                {
                    if (enemyGhost2.atk)
                    {
                        enemyGold.walkAni.Play();
                        if (gamePause) { enemyGhost2.walkAni.Pause(); }
                        if (Player.died) { enemyGhost2.walkAni.Pause(0, 0); }
                        enemyGhost2.walkAni.DrawFrame(theBatch, enemyGhost2.Position);
                    }
                }
                if (enemyArcherR.died == false && enemyArcher.died && enemyGold.died)
                {
                    if (ai5_Wave == true && arrowOn5)
                    {
                        theBatch.Draw(eWaveAtk5, Ai5WavePos, Color.White);
                    }
                    if (enemyArcherR.atk == true)
                    {
                        if (gamePause) { enemyArcherR.atkAni.Pause(); }
                        if (Player.died) { enemyArcherR.atkAni.Pause(0, 0); }
                        enemyArcherR.atkAni.DrawFrame(theBatch, enemyArcherR.Position);
                    }
                    else if (enemyArcherR.idle == true)
                    {
                        if (Player.died) { enemyArcherR.idleAni.Pause(); }
                        if (gamePause) { enemyArcherR.idleAni.Pause(0, 0); }
                        enemyArcherR.idleAni.DrawFrame(theBatch, enemyArcherR.Position);
                    }
                }
                if (enemyBlack2.died == false && enemyArcherR.died && enemyArcher.died && enemyGold.died)
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
                #endregion

                // effect1
                effect1.DrawFrame(theBatch, effect1_Pos);

                string strDead = "Dead = " + dead_count;
                string strDevMode = "DevMode";
                if (Switch == "InGame2")
                {
                    theBatch.DrawString(ArialFont, strDead, new Vector2(470 - camera.ViewMatrix.Translation.X, 435 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                if (devMode)
                {
                    theBatch.DrawString(ArialFont, strDevMode, new Vector2(880 - camera.ViewMatrix.Translation.X, 435 - camera.ViewMatrix.Translation.Y), Color.Red);
                }

                if (gamePause && Switch == "InGame2")
                {
                    theBatch.Draw(pausePic, new Vector2(0 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                    theBatch.Draw(pausePic, new Vector2(0 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                    theBatch.Draw(buttonRetry, new Vector2(435 - camera.ViewMatrix.Translation.X, 180 - camera.ViewMatrix.Translation.Y), Color.White);
                    theBatch.Draw(buttonExit, new Vector2(435 - camera.ViewMatrix.Translation.X, 320 - camera.ViewMatrix.Translation.Y), Color.White);
                    theBatch.Draw(ButtonGuide, new Vector2(860 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                    buttonSelect.DrawFrame(theBatch, new Vector2(select_Pos.X - camera.ViewMatrix.Translation.X, select_Pos.Y - camera.ViewMatrix.Translation.Y));
                    if (Game1.soundOn)
                    {
                        theBatch.Draw(buttonSoundOn, new Vector2(435 - camera.ViewMatrix.Translation.X, 250 - camera.ViewMatrix.Translation.Y), Color.White);
                    }
                    else if (!Game1.soundOn)
                    {
                        theBatch.Draw(buttonSoundOff, new Vector2(435 - camera.ViewMatrix.Translation.X, 250 - camera.ViewMatrix.Translation.Y), Color.White);
                    }
                }
                else if (!gamePause)
                {
                    theBatch.Draw(ButtonMenu, new Vector2(860 - camera.ViewMatrix.Translation.X, 0 - camera.ViewMatrix.Translation.Y), Color.White);
                }
                theBatch.End();
            }
            if (menuLoading == true)
            {
                theBatch.Begin();
                loading.DrawFrame(theBatch, new Vector2(0, 0));
                Switch = "InGame2";
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
            enemyArcher.atkAni.UpdateFrame(Elapsed);
            enemyArcher.idleAni.UpdateFrame(Elapsed);
            enemyGold.walkAni.UpdateFrame(Elapsed);
            enemyGold.atkAni.UpdateFrame(Elapsed);
            enemyGhost.walkAni.UpdateFrame(Elapsed);
            enemyGhost2.walkAni.UpdateFrame(Elapsed);
            enemyArcherR.atkAni.UpdateFrame(Elapsed);
            enemyArcherR.idleAni.UpdateFrame(Elapsed);
            enemyBlack2.walkAni.UpdateFrame(Elapsed);
            enemyBlack2.idleAni.UpdateFrame(Elapsed);
            enemyBlack2.atkAni.UpdateFrame(Elapsed);

            // Player Update frame
            Player.diedAni.UpdateFrame(Elapsed);
            Player.walkAni.UpdateFrame(Elapsed);
            Player.idleAni.UpdateFrame(Elapsed);
            Player.atkAni.UpdateFrame(Elapsed);
            Player.defAni.UpdateFrame(Elapsed);
            Player.specialAni.UpdateFrame(Elapsed);
            Player.specialAtkAni.UpdateFrame(Elapsed);

            // ฉาก Update frame
            Touch.UpdateFrame(Elapsed);
            buttonSelect.UpdateFrame(Elapsed);
            effect1.UpdateFrame(Elapsed);
            Bat.UpdateFrame(Elapsed);

            loading.UpdateFrame(Elapsed);
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
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !Player.died)
                {
                    if (gamePause == false)
                    {
                        lastTimePauseOff = theTime.TotalGameTime;

                        Player.diedAni.Pause();
                        Player.walkAni.Pause();
                        Player.idleAni.Pause();
                        Player.atkAni.Pause();
                        Player.defAni.Pause();
                        Player.specialAni.Pause();
                        Player.specialAtkAni.Pause();
                        enemyArcher.atkAni.Pause();
                        enemyArcher.idleAni.Pause();
                        enemyGold.atkAni.Pause();
                        enemyGold.walkAni.Pause();
                        enemyGhost.walkAni.Pause();
                        enemyGhost2.walkAni.Pause();
                        enemyArcherR.atkAni.Pause();
                        enemyArcherR.idleAni.Pause();
                        enemyBlack2.atkAni.Pause();
                        enemyBlack2.walkAni.Pause();

                        select_Pos = new Vector2(340, 185);
                        if (walkSoundInstance.State != SoundState.Paused) { walkSoundInstance.Pause(); }
                        gamePause = true;
                    }
                    else
                    {
                        lastTimePauseOn = theTime.TotalGameTime;
                        PauseTime = lastTimePauseOn - lastTimePauseOff;

                        Player.diedAni.Play();
                        Player.walkAni.Play();
                        Player.idleAni.Play();
                        Player.atkAni.Play();
                        Player.defAni.Play();
                        Player.specialAni.Play();
                        Player.specialAtkAni.Play();
                        enemyArcher.atkAni.Play();
                        enemyArcher.idleAni.Play();
                        enemyGold.atkAni.Play();
                        enemyGold.walkAni.Play();
                        enemyGhost.walkAni.Play();
                        enemyGhost2.walkAni.Play();
                        enemyArcherR.atkAni.Play();
                        enemyArcherR.idleAni.Play();
                        enemyBlack2.atkAni.Play();
                        enemyBlack2.walkAni.Play();

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
            if (gamePause)
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
                        if (Game1.soundOn)
                        {
                            stopPress = false;
                            Game1.soundOn = false;
                            select = 0;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                        else if (!Game1.soundOn)
                        {
                            stopPress = false;
                            Game1.soundOn = true;
                            select = 0;

                            lastTimeSelect = theTime.TotalGameTime;
                        }
                    }
                    else if (select == 3)
                    {
                        string filepath = Path.Combine(@"Content\data.txt");
                        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);
                        if (Switch == "InGame2")
                        { sw.WriteLine("InGame2"); }
                        sw.Flush();
                        sw.Close();
                        
                        string filepathDead = Path.Combine(@"Content\Dead.txt");
                        FileStream fsDead = new FileStream(filepathDead, FileMode.Open, FileAccess.Write);
                        StreamWriter swDead = new StreamWriter(fsDead);
                        if (Switch == "InGame2")
                        { swDead.WriteLine(dead_count.ToString()); }
                        swDead.Flush();
                        swDead.Close();
                        game.Exit();
                    }
                }
            }
        }

    }
}
