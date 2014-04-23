using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotSlayer
{
    public class PowerUpActor : Actor
    {
        public override void Update(GameTime gameTime)
        {
            Location.X += Velocity.X;
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            SpriteEffects effect = SpriteEffects.None;
            if (Velocity.X > 0)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            batch.Draw(Texture, Location, AnimationRectangles[AnimationIndex], Tint, 0, Vector2.Zero, 1, effect, 0);
            base.Draw(gameTime, batch);
        }
    }
}
