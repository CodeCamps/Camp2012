using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotSlayer
{
    public class PlayerActor : Actor
    {
        public PlayerIndex PlayerIndex { get; set; }

        public static readonly Rectangle[] rectWalkSprites = 
        {
            new Rectangle(192, 640, 64, 128),
            new Rectangle(448, 512, 64, 128),
            new Rectangle(384, 512, 64, 128),
            new Rectangle(320, 512, 64, 128),
            new Rectangle(256, 512, 64, 128),
            new Rectangle(192, 512, 64, 128),
            new Rectangle(128, 896, 64, 128),
            new Rectangle(128, 768, 64, 128),
        };

        public bool IsJumping { get; set; }
        public bool IsWalking { get; set; }
        public bool WasFacingBack { get; set; }
        public bool IsTouching { get; set; }
        public int GearCount { get; set; }
        public static int EnemyDropCount { get; set; }

        public double SecondsSincePlayerHit = 1.5;
        public double SecondsBetweenHits = 1.5;

        public override void Update(GameTime gameTime)
        {
            SecondsSincePlayerHit += gameTime.ElapsedGameTime.TotalSeconds;
            int deltaX = 0;
            bool isLeft =
                GamePad.GetState(PlayerIndex).DPad.Left == ButtonState.Pressed ||
                GamePad.GetState(PlayerIndex).ThumbSticks.Left.X < -0.5f;
            bool isRight =
                GamePad.GetState(PlayerIndex).DPad.Right == ButtonState.Pressed ||
                GamePad.GetState(PlayerIndex).ThumbSticks.Left.X > 0.5f;
            if (isLeft)
            {
                if (GamePad.GetState(PlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    deltaX = 5;
                }
                else
                {
                    deltaX = 2;
                }
                IsWalking = true;
                WasFacingBack = true;
            }
            else if (isRight)
            {
                if (GamePad.GetState(PlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    deltaX = -5;
                }
                else
                {
                    deltaX = -2;
                }
                IsWalking = true;
                WasFacingBack = false;
            }
            else
            {
                IsWalking = false;
            }

            //Location = new Vector2(Location.X - deltaX, Location.Y);
            Location.X = Location.X - deltaX;
            if (Location.X < 0)
            {
                Location.X = 0;
            }
            else if (Location.X > 750)
            {
                Location.X = 750;
            }

            if (IsWalking)
            {
                AnimationElapsed += gameTime.ElapsedGameTime.Milliseconds / 1000.0;
                double duration = AnimationDuration;
                
                if (GamePad.GetState(PlayerIndex).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    duration = duration / 2.0;
                }
                
                if (AnimationElapsed > duration)
                {
                    AnimationIndex = (AnimationIndex + 1) % AnimationRectangles.Length;
                    AnimationElapsed = 0.0;
                }
            }

            Velocity = new Vector2(Velocity.X, Velocity.Y + Acceleration.Y);
            if (Velocity.Y > 16)
            {
                Velocity = new Vector2(Velocity.X, 16);
            }
            Location = new Vector2(Location.X, Location.Y + Velocity.Y);

            if (Location.Y >= BASE_Y)
            {
                Location = new Vector2(Location.X, BASE_Y);
                Velocity = new Vector2(Velocity.X, 0);
                IsJumping = false;
            }

            if (GamePad.GetState(PlayerIndex).Buttons.A == ButtonState.Pressed)
            {
                if (!IsJumping)
                {
                    IsJumping = true;
                    Velocity = new Vector2(Velocity.X, -16);
                    Game1.sfxPlayerJump.Play();
                }
            }

            Actor enemy = CheckForCollisions();

            if (enemy != null)
            {
                float maxY = enemy.Location.Y + enemy.AnimationRectangles[0].Height / 3.0f;
                float playerY = Location.Y + AnimationRectangles[0].Height * 0.75f;

                if (enemy is HeartActor)
                {
                    Health += 1;
                    if (Health > 3)
                    {
                        Health = 3;
                    }
                    enemy.Kill();
                    Game1.sfxPowerup.Play();
                }
                else if (enemy is GearActor)
                {
                    GearCount += 1;
                    enemy.Kill();
                    Game1.sfxPowerup.Play();
                }
                else if (playerY < maxY)
                {
                    enemy.Kill();
                    ///////////////
                    if (enemy.Health > 0)
                    {
                        Velocity.Y = -16;
                        //Location.Y = maxY;
                    }
                    ///////////////
                    int x = (int)Location.X;
                    if (WasFacingBack)
                    {
                        x -= 80;
                    }
                    else
                    {
                        x += AnimationRectangles[0].Width + 20;
                    }
                    EnemyDropCount++;
                    if (EnemyDropCount > 4)
                    {
                        Game1.CreateHeartPowerup(x);
                        EnemyDropCount = 0;
                    }
                    else
                    {
                        Game1.CreateGearPowerup(x);
                    }
                }
                else
                {
                    if (SecondsSincePlayerHit > SecondsBetweenHits)
                    {
                        Health -= 1;
                        SecondsSincePlayerHit = 0.0;
                        if (Health < 0)
                        {
                            Game1.GameState = GameState.GameOver;
                            Game1.sfxPlayerDeath.Play();
                            GameOverScreen.PlayMusic = true;
                        }
                    }
                    if (enemy is MissleActor)
                    {
                        enemy.Kill();
                        Game1.sfxRocketImpact.Play();
                    }
                    else
                    {
                        Game1.sfxRobotImpact.Play(); // (bot hits player)
                    }
                    if (enemy.Location.X > Location.X)
                    {
                        Location.X -= 25;
                    }
                    else
                    {
                        Location.X += 25;
                    }
                }
            }
        
        }

        public Actor CheckForCollisions()
        {
            Actor result = null;
            IsTouching = false;

            Rectangle rectMe = CollisionRectangle;
            rectMe.X = (int)Location.X;
            rectMe.Y = (int)Location.Y;
            rectMe.Inflate(CollisionRectangleInflate, CollisionRectangleInflate);
            
            foreach (Actor enemy in Game1.RobotActors)
            {
                Rectangle rectEnemy = enemy.CollisionRectangle;
                rectEnemy.X = (int)enemy.Location.X;
                rectEnemy.Y = (int)enemy.Location.Y;
                rectEnemy.Inflate(enemy.CollisionRectangleInflate, enemy.CollisionRectangleInflate);

                if (rectEnemy.Intersects(rectMe) && !enemy.IsDead)
                {
                    IsTouching = true;
                    result = enemy;
                    break;
                }
            }

            return result;
        }

        public Rectangle rectAnimationStanding = new Rectangle(128, 512, 64, 128);
        public Rectangle rectAnimationJumping = new Rectangle(128, 640, 64, 128);

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            SpriteEffects effect = SpriteEffects.None;
            Rectangle animationFrame = new Rectangle();

            if (WasFacingBack)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            if (IsJumping)
            {
                animationFrame = rectAnimationJumping;
            }
            else if (IsWalking)
            {
                animationFrame = AnimationRectangles[AnimationIndex];
            }
            else
            {
                animationFrame = rectAnimationStanding;
            }

            Color colorFlash = Tint;
            if (SecondsSincePlayerHit < SecondsBetweenHits)
            {
                double flash = SecondsSincePlayerHit * 10.0;
                if ((int)flash % 2 == 0)
                {
                    colorFlash = Color.Red;
                }
            }
            batch.Draw(Texture, Location, animationFrame, colorFlash, 0, Vector2.Zero, 1, effect, 0);

            var rectHeart = new Rectangle(384, 640, 64, 64);
            var color = Color.Red;
            for (int i = 0; i < 3; i++)
            {
                if (Health > i)
                {
                    color = Color.Red;
                }
                else
                {
                    color = Color.White;
                }
                batch.Draw(Texture, new Vector2(i * 64, 0), rectHeart, color);
            }

            var rectGear = new Rectangle(448, 640, 64, 64);
            batch.Draw(Texture, new Vector2(0, 64), rectGear, Color.Goldenrod);
            var score = String.Format("x{0}", GearCount);
            batch.DrawString(Game1.font, score, new Vector2(64, 64), Color.Black);
            
            base.Draw(gameTime, batch);

        }
    }
}
