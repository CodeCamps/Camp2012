using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotSlayer
{
    public class GameOverScreen
    {
        public static bool PlayMusic = false;

        public static void Update(GameTime gameTime)
        {
        }

        public static void Draw(GameTime gameTime, Texture2D texture, SpriteBatch batch)
        {
            // DRAW SCORE
            var rectGear = new Rectangle(448, 640, 64, 64);
            batch.Draw(texture, new Vector2(0, 0), rectGear, Color.Goldenrod);
            var score = String.Format("x{0}", Game1.player.GearCount);
            batch.DrawString(Game1.font, score, new Vector2(64, 0), Color.Black);

            int i = 0;

            batch.DrawString(Game1.font, "               GAME OVER  ",   new Vector2(100, 50 * i++), Color.Black);
            batch.DrawString(Game1.font, "Students:       Ben Branch", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "            Addison Denchik", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "             Joshua Gold", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "              Aaron Kimbrell", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "              Micki Salkin", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "Counselors:  Laynie Boland", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "              Derek Garde", new Vector2(100, 50 * i++), Color.White);
            batch.DrawString(Game1.font, "Instructor:  Joseph Hall", new Vector2(100, 50 * i++), Color.White);

            Vector2 origin = new Vector2(64, 0);
            float rotate = MathHelper.ToRadians(-90);
            Vector2 locDeadGuy = new Vector2(250,0);
            Rectangle rectStanding = new Rectangle(128, 512, 64, 128);
            batch.Draw(texture, locDeadGuy, rectStanding, Color.White, rotate, origin, 1, SpriteEffects.None, 0); 
        }
    }
}
