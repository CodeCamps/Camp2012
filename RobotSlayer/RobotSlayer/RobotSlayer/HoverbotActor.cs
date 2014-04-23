using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotSlayer
{
    public class HoverbotActor : Actor
    {
        public static Random rand = new Random();
        public double randHoverStart = rand.NextDouble() * 20.0;

        public override void Update(GameTime gameTime)
        {
            Location = new Vector2(
                Location.X + Velocity.X,
                BASE_Y + (float)Math.Sin(randHoverStart + 3.0 * gameTime.TotalGameTime.TotalSeconds) * 20.0f);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(Texture, Location, AnimationRectangles[AnimationIndex], Tint);
            base.Draw(gameTime, batch);
        }
    }
}
