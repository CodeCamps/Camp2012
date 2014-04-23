using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RobotSlayer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static PlayerActor player = new PlayerActor();
        HoverbotActor hover = new HoverbotActor();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D texSprites;
        Texture2D texBackground;
        public static SpriteFont font;

        public static GameState GameState = GameState.SplashScreen;

        public double TankSpawnFrequency = 15;
        public double HoverSpawnFrequency = 5;
        public double SecondsSinceTankSpawn = 999;
        public double SecondsSinceHoverSpawn = 999;

        public static Song theSong;
        public static SoundEffect sfxHoverbotCreated;
        public static SoundEffect sfxTankbotCreated;

        public static SoundEffect sfxRocketLaunch;
        public static SoundEffect sfxRocketImpact;

        public static SoundEffect sfxRobotImpact; // (bot hits player)
        public static SoundEffect sfxPowerup;
        public static SoundEffect sfxStartButton;

        public static SoundEffect sfxPlayerJump;
        public static SoundEffect sfxPlayerDeath;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected void InitGameData()
        {
            player.Location = new Vector2(360, Actor.BASE_Y);
            player.Velocity = Vector2.Zero;
            player.AnimationRectangles = PlayerActor.rectWalkSprites;
            player.Tint = Color.Yellow;
            player.AnimationDuration = 0.05;
            player.CollisionRectangle = player.AnimationRectangles[0];
            player.CollisionRectangleInflate = -12;
            player.Health = 3;
            player.GearCount = 0;

            RobotActors.Clear();
            //CreateHoverbot(650);
            //CreateHoverbot(600);
            //CreateHoverbot(550);
            //CreateHoverbot(500);
            //CreateHoverbot(450);
            //CreateTankbot(700);

            //CreateHoverbot(-114);
            //CreateHoverbot(-214);
            //CreateHoverbot(-264);
            //CreateHoverbot(-164);
            //CreateHoverbot(-64);
            //CreateTankbot(-90);

            player.Texture = texSprites;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        public void CreateHoverbot(int x)
        {
            var hover = new HoverbotActor();
            hover.Location = new Vector2(x, Actor.BASE_Y);
            hover.Velocity = new Vector2(-1, 0);
            if (x < player.Location.X)
            {
                hover.Velocity.X = 1.0f;
            }
            hover.AnimationRectangles =
                new Rectangle[] { new Rectangle(192, 768, 64, 128) };
            hover.Tint = Color.CornflowerBlue;
            hover.AnimationDuration = 90.0;
            hover.CollisionRectangle = hover.AnimationRectangles[0];
            hover.CollisionRectangleInflate = -20;
            hover.Texture = texSprites;
            RobotActors.Add(hover);
            sfxHoverbotCreated.Play();
        }

        public void CreateTankbot(int x)
        {
            var tank = new TankbotActor();
            tank.Location = new Vector2(x, Actor.BASE_Y + 30);
            tank.Velocity = new Vector2(-0.75f, 0.0f);
            if (x < player.Location.X)
            {
                tank.Velocity.X = 0.75f;
            }
            tank.AnimationRectangles =
                new Rectangle[] { new Rectangle(192, 926, 64, 98) };
            tank.Tint = Color.Salmon;
            tank.AnimationDuration = 90.0;
            tank.CollisionRectangle = tank.AnimationRectangles[0];
            tank.CollisionRectangleInflate = -2;
            tank.Texture = texSprites;
            tank.Health = 2;
            RobotActors.Add(tank);

            sfxTankbotCreated.Play();
        }

        public static List<Actor> RobotActors = new List<Actor>();

        protected static void InitializePowerUp(PowerUpActor powerup, int x)
        {
            powerup.Location.X = x;
            powerup.Location.Y = Actor.BASE_Y + 30;
            powerup.AnimationDuration = 90.0;
            powerup.CollisionRectangle = powerup.AnimationRectangles[0];
            powerup.CollisionRectangleInflate = -2;
            powerup.Texture = texSprites;
            RobotActors.Add(powerup);
        }

        public static void CreateHeartPowerup(int x)
        {
            var powerup = new HeartActor();
            powerup.AnimationRectangles = 
                new Rectangle[] { new Rectangle(384, 640, 64, 64) };
            powerup.Tint = Color.Red;
            InitializePowerUp(powerup, x);
        }

        public static void CreateGearPowerup(int x)
        {
            var powerup = new GearActor();
            powerup.AnimationRectangles =
                new Rectangle[] { new Rectangle(448, 640, 64, 64) };
            powerup.Tint = Color.Gold;
            InitializePowerUp(powerup, x);
        }

        public static void CreateMisslePowerup(int x)
        {
            var powerup = new MissleActor();
            powerup.AnimationRectangles =
                new Rectangle[] { new Rectangle(256, 640, 64, 64) };
            powerup.Tint = Color.White;
            powerup.Velocity.X = -3.0f;
            if (x < player.Location.X)
            {
                powerup.Velocity.X = 3.0f;
            }
            InitializePowerUp(powerup, x);
            sfxRocketLaunch.Play();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            texSprites = Content.Load<Texture2D>("Sprites");
            texBackground = Content.Load<Texture2D>("Background");
            font = Content.Load<SpriteFont>("SpriteFont1");

            player.Texture = texSprites;

            foreach (Actor actor in RobotActors)
            {
                actor.Texture = texSprites;
            }

            theSong = Content.Load<Song>(@"sound/music/Title");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(theSong);

            sfxHoverbotCreated = Content.Load<SoundEffect>(@"sound/hoverbot-spawn");
            sfxTankbotCreated = Content.Load<SoundEffect>(@"sound/tankbot-spawn");

            sfxRocketLaunch = Content.Load<SoundEffect>(@"sound/rocket-launch");
            sfxRocketImpact = Content.Load<SoundEffect>(@"sound/rocket-impact");

            sfxRobotImpact = Content.Load<SoundEffect>(@"sound/robot-impact"); // (bot hits player)
            sfxPowerup = Content.Load<SoundEffect>(@"sound/powerup-pickup");
            sfxStartButton = Content.Load<SoundEffect>(@"sound/start-button");

            sfxPlayerJump = Content.Load<SoundEffect>(@"sound/player-jump");
            sfxPlayerDeath = Content.Load<SoundEffect>(@"sound/player-death");
        }

        Rectangle[] rectBackgrounds = 
        {
            new Rectangle(512,   0, 256, 512),
            new Rectangle(256, 512, 256, 512),
            new Rectangle(256,   0, 256, 512),
            new Rectangle(  0, 512, 256, 512),
            new Rectangle(  0,   0, 256, 512),
        };

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (GameState == RobotSlayer.GameState.Initializing)
            {
                InitGameData();
                GameState = RobotSlayer.GameState.Playing;
                MediaPlayer.Stop();
                theSong = Content.Load<Song>(@"sound/music/Playing");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(theSong);
            }
            else if (GameState == RobotSlayer.GameState.SplashScreen)
            {
                TitleScreen.Update(gameTime);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    GameState = RobotSlayer.GameState.Initializing;
                    sfxStartButton.Play();
                }
            }
            else if (GameState == RobotSlayer.GameState.GameOver)
            {
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
                GameOverScreen.Update(gameTime);
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                {
                    GameState = RobotSlayer.GameState.Initializing;
                    sfxStartButton.Play();
                }
                if (GameOverScreen.PlayMusic)
                {
                    GameOverScreen.PlayMusic = false;
                    MediaPlayer.Stop();
                    theSong = Content.Load<Song>(@"sound/music/GameOver");
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(theSong);
                }
            }
            
            if (GameState == RobotSlayer.GameState.Playing)
            {
                player.Update(gameTime);

                SecondsSinceTankSpawn += gameTime.ElapsedGameTime.TotalSeconds;
                SecondsSinceHoverSpawn += gameTime.ElapsedGameTime.TotalSeconds;

                if (SecondsSinceTankSpawn > TankSpawnFrequency)
                {
                    double deltaX = HoverbotActor.rand.NextDouble() * 200;
                    CreateTankbot(-64 - (int)deltaX);
                    deltaX = HoverbotActor.rand.NextDouble() * 200;
                    CreateTankbot(800 + (int)deltaX);
                    SecondsSinceTankSpawn = 0;
                }
                if (SecondsSinceHoverSpawn > HoverSpawnFrequency)
                {
                    double deltaX = HoverbotActor.rand.NextDouble() * 200;
                    CreateHoverbot(-64 - (int)deltaX);
                    deltaX = HoverbotActor.rand.NextDouble() * 200;
                    CreateHoverbot(800 + (int)deltaX);
                    SecondsSinceHoverSpawn = 0;
                }

                for (int i = 0; i < RobotActors.Count; i++)
                {
                    if (!RobotActors[i].IsDead)
                    {
                        RobotActors[i].Update(gameTime);
                    }
                }

                if (player.IsTouching)
                {
                    Actor.DebugTint = Actor.DebugTintTouching;
                    GamePad.SetVibration(player.PlayerIndex, 1.0f, 1.0f);
                }
                else
                {
                    Actor.DebugTint = Actor.DebugTintNotTouching;
                    GamePad.SetVibration(player.PlayerIndex, 0.0f, 0.0f);
                }
            }

            base.Update(gameTime);
        }

        float backgroundStart = 0.0f;
        int backgroundIndex = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (GameState == RobotSlayer.GameState.Initializing)
            {
            }
            else if (GameState == RobotSlayer.GameState.SplashScreen)
            {
                TitleScreen.Draw(gameTime, texSprites, spriteBatch);
            }
            else if (GameState == RobotSlayer.GameState.GameOver)
            {
                GameOverScreen.Draw(gameTime, texSprites, spriteBatch);
            }
            else if (GameState == RobotSlayer.GameState.Playing)
            {
                for (int i = 0; i < 4; i++)
                {
                    float x = backgroundStart + i * 256;
                    spriteBatch.Draw(
                        texBackground,
                        new Vector2(x, -32),
                        rectBackgrounds[(backgroundIndex + i) % rectBackgrounds.Length],
                        Color.White);
                }

                player.Draw(gameTime, spriteBatch);

                for (int i = 0; i < RobotActors.Count; i++)
                {
                    if (!RobotActors[i].IsDead)
                    {
                        RobotActors[i].Draw(gameTime, spriteBatch);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
