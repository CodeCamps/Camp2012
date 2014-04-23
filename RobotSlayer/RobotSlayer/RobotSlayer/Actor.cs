using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotSlayer
{
    public abstract class Actor
    {
        public const int BASE_Y = 310;
        public const bool IS_DEBUG = false;

        public Texture2D Texture { get; set; }

        public Vector2 Location;
        public Vector2 Velocity;
        public static readonly Vector2 Acceleration = new Vector2(0, 1);

        public Color Tint { get; set; }
        public int Health { get; set; }

        public Rectangle CollisionRectangle { get; set; }
        public int CollisionRectangleInflate { get; set; }

        public Rectangle[] AnimationRectangles { get; set; }
        public int AnimationIndex { get; set; }
        public double AnimationElapsed { get; set; }
        public double AnimationDuration { get; set; }
        public int MissleCount { get; set; }

        public abstract void Update(GameTime gameTime);

        public static Color DebugTint = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        public static Color DebugTintNotTouching = new Color(0.0f, 1.0f, 0.0f, 0.25f);
        public static Color DebugTintTouching = new Color(1.0f, 0.0f, 0.0f, 0.25f);
        
        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (IS_DEBUG || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder))
            {
                Rectangle destRect = CollisionRectangle;
                destRect.X = (int)Location.X;
                destRect.Y = (int)Location.Y;
                destRect.Inflate(CollisionRectangleInflate, CollisionRectangleInflate);
                batch.Draw(Texture, destRect, new Rectangle(320, 64, 1, 1), DebugTint);
            }
        }

        public bool IsDead;
        public void Kill()
        {
            Health -= 1;
            if (Health <= 0)
            {
                IsDead = true;
            }
        }
    }
}
