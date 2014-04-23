using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotSlayer
{
    public class TankbotActor : Actor
    {
        public double SecondsSinceLastMissle = 4.0;
        public const double SecondsBetweenMissles = 5.0;

        public override void Update(GameTime gameTime)
        {
            Location.X += Velocity.X;
            SecondsSinceLastMissle += gameTime.ElapsedGameTime.TotalSeconds;

            if (SecondsSinceLastMissle >= SecondsBetweenMissles)
            {
                SecondsSinceLastMissle = 0.0;
                Game1.CreateMisslePowerup((int)Location.X);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(Texture, Location, AnimationRectangles[AnimationIndex], Tint);
            base.Draw(gameTime, batch);
        }
    }
}
