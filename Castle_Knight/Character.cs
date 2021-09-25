using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Castle_Knight
{
    public class Character
    {
        public int hp;
        public int speed;
        public bool idle = false;
        public bool died = false;
        public bool atk = false;
        public bool knockBack = false;
        public bool stop_move = false;
        public bool getHit = false;
        public bool playerInvi = false;
        public int SpeCount = 0;
        public Vector2 Position;
        public Vector2[] Heart_Pos = new Vector2[5];
        public AnimatedTexture idleAni;
        public AnimatedTexture walkAni;
        public AnimatedTexture atkAni;
        public AnimatedTexture defAni;
        public AnimatedTexture diedAni;
        public AnimatedTexture specialAni;
        public AnimatedTexture specialAtkAni;

        public TimeSpan DelayAttack = TimeSpan.FromMilliseconds(800);
        public TimeSpan CDAttack = TimeSpan.FromMilliseconds(240);
        public TimeSpan DelayBlock = TimeSpan.FromMilliseconds(1280);
        public TimeSpan CDBlock = TimeSpan.FromMilliseconds(410);
        public TimeSpan DelaySCount = TimeSpan.FromMilliseconds(800);
        public TimeSpan CDSAttack = TimeSpan.FromMilliseconds(300);
        public TimeSpan DelaySAttack = TimeSpan.FromMilliseconds(1200);
        public TimeSpan TimeDuringSAttack = TimeSpan.FromMilliseconds(500);
        public TimeSpan lastTimeAttack;
        public TimeSpan lastTimeBlock;
        public TimeSpan lastTimeSpecial;
        public TimeSpan lastTimeSpecialDuring;
        public TimeSpan lastTimeSpecialCount;

        public TimeSpan DelayHp = TimeSpan.FromMilliseconds(300);
        public TimeSpan lastTimeHp;

        public TimeSpan DelayKnockBack = TimeSpan.FromMilliseconds(100);
        public TimeSpan lastTimeKnockBack;

        public static readonly TimeSpan GetHitDelay = TimeSpan.FromMilliseconds(80);
        public static readonly TimeSpan _GetHitDelay = TimeSpan.FromMilliseconds(160);
        public static readonly TimeSpan _GetHitDelay2 = TimeSpan.FromMilliseconds(240);
        public TimeSpan GetHitTime;

        public Rectangle charBlock;
        public Rectangle charSpecialAtk;
    }

    public class Player : Character
    {

        public void PlayerKnockBack(GameTime gameTime)
        {
            if (knockBack == true)
            {
                Position.X -= 9;
                Position.Y -= 3;
            }
            else
            {
                if (Position.Y < 255)
                {
                    Position.Y += 4;
                }
                if (Position.Y > 255)
                {
                    Position.Y = 255;
                }
            }
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime)
            {
                knockBack = false;
            }
        }

        public void PlayerKnockBack2(GameTime gameTime)
        {
            if (knockBack == true)
            {
                Position.X -= 20;
                Position.Y -= 8;
            }
            else
            {
                if (Position.Y < 255)
                {
                    Position.Y += 4;
                }
                if (Position.Y > 255)
                {
                    Position.Y = 255;
                }
            }
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime)
            {
                knockBack = false;
            }
        }

        public void WhenDefTrue(GameTime gameTime, List<SoundEffect> sounds)
        {
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime && lastTimeHp + DelayHp < gameTime.TotalGameTime)
            {
                sounds[4].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                knockBack = true;
                stop_move = true;
                if (SpeCount < 4 && lastTimeSpecialCount + DelaySCount < gameTime.TotalGameTime)
                {
                    SpeCount += 1;

                    lastTimeSpecialCount = gameTime.TotalGameTime;
                }
                lastTimeKnockBack = gameTime.TotalGameTime;
            }
        }

        public void WhenDefFalse(GameTime gameTime, List<SoundEffect> sounds)
        {
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime)
            {
                sounds[2].Play(volume: 0.4f, pitch: 0.0f, pan: 0.0f);
                knockBack = true;
                stop_move = true;

                lastTimeKnockBack = gameTime.TotalGameTime;
            }
            if (lastTimeHp + DelayHp < gameTime.TotalGameTime)
            {
                hp -= 1;
                getHit = true;
                playerInvi = true;

                GetHitTime = gameTime.TotalGameTime;
                lastTimeHp = gameTime.TotalGameTime;
            }
        }
    }

    public class Enemy : Character
    {

        public void EnemyKnockBack(GameTime gameTime)
        {
            if (knockBack == true)
            {
                Position.X += 6;
                Position.Y -= 2;
            }
            else
            {
                if (Position.Y < 255)
                {
                    Position.Y += 4;
                }
                if (Position.Y > 255)
                {
                    Position.Y = 255;
                }
            }
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime)
            {
                knockBack = false;
            }
        }

        public void PlayerSpecialAtk(GameTime gameTime)
        {
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime)
            {
                knockBack = true;

                lastTimeKnockBack = gameTime.TotalGameTime;
            }
            if (lastTimeHp + DelayHp < gameTime.TotalGameTime)
            {
                hp -= 3;

                lastTimeHp = gameTime.TotalGameTime;
            }
        }

        public void PlayerAtk(GameTime gameTime, SoundEffectInstance sound)
        {
            if (lastTimeKnockBack + DelayKnockBack < gameTime.TotalGameTime)
            {
                knockBack = true;
                if (sound.State != SoundState.Stopped) { sound.Stop(); }
                if (sound.State != SoundState.Playing) { sound.Play(); }

                lastTimeKnockBack = gameTime.TotalGameTime;
            }
            if (lastTimeHp + DelayHp < gameTime.TotalGameTime)
            {
                hp -= 1;

                lastTimeHp = gameTime.TotalGameTime;
            }
        }

    }
}
