using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotSlayer
{
    public class TitleScreen
    {
        public static readonly Rectangle rectPressStart = new Rectangle(96, 144, 82, 12);
        public static readonly Rectangle rectTitleBot = new Rectangle(15, 52, 32, 117);
        public static readonly Rectangle rectTitleText = new Rectangle(53, 53, 183, 84);
        public static readonly Rectangle[] rectTitlePlayerWalkFrames = new Rectangle[] {
            new Rectangle(384, 256, 128, 256),
            new Rectangle(256, 256, 128, 256),
            new Rectangle(128, 256, 128, 256),
            new Rectangle(  0, 768, 128, 256),
            new Rectangle(  0, 512, 128, 256),
            new Rectangle(  0, 256, 128, 256),
            new Rectangle(384,   0, 128, 256),
            new Rectangle(256,   0, 128, 256),
        };

        public static double WalkFrameDelay = 0.1;
        public static double CurrentWalkElapsed = 0.0;
        public static int CurrentWalkFrame = 0;

        public static Vector2 locTitlePlayer = new Vector2(560, 175);
        public static Vector2 locTitleText = new Vector2(300, 200);
        public static Vector2 locTitleBot = new Vector2(150, 100);
        public static Vector2 locPressStart = new Vector2(350, 300);

        public static void Update(GameTime gameTime)
        {
            CurrentWalkElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (CurrentWalkElapsed > WalkFrameDelay)
            {
                CurrentWalkElapsed = 0.0;
                CurrentWalkFrame = (CurrentWalkFrame + 1) % rectTitlePlayerWalkFrames.Length;
            }

            locTitleBot.Y = 100.0f + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 20.0f;
        }

        public static void Draw(GameTime gameTime, Texture2D texture, SpriteBatch batch)
        {
            // draw hoverbot
            batch.Draw(texture, locTitleBot, rectTitleBot, Color.White);
            
            // draw player
            batch.Draw(texture, locTitlePlayer, rectTitlePlayerWalkFrames[CurrentWalkFrame], Color.White);

            // draw title
            batch.Draw(texture, locTitleText, rectTitleText, Color.White);
            
            // draw "Press Start"
            long totalSeconds = (long)Math.Round(gameTime.TotalGameTime.TotalSeconds);
            if (totalSeconds % 2 == 0)
            {
                batch.Draw(texture, locPressStart, rectPressStart, Color.White);
            }
        }
    }
}
