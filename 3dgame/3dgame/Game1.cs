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

namespace _3dgame
{
    /// <summary>
    /// Это главный тип игры
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Camera camera { get; protected set; }

        public Random rnd { get; protected set; }

        ModelManager modelManager;

        float shotSpeed = 10;
        int shotDelay = 300;
        int shotCountdown = 0;
        Texture2D crosshairTexture;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;

      

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rnd = new Random();

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;
#if !DEBUG
                graphics.IsFullScreen = true;
#endif
        }

        /// <summary>
        /// Позволяет игре выполнить инициализацию, необходимую перед запуском.
        /// Здесь можно запросить нужные службы и загрузить неграфический
        /// контент.  Вызов base.Initialize приведет к перебору всех компонентов и
        /// их инициализации.
        /// </summary>
        protected override void Initialize()
        {
            // ЗАДАЧА: добавьте здесь логику инициализации
            camera = new Camera(this, new Vector3(0, 0, 50),
            Vector3.Zero, Vector3.Up);
            Components.Add(camera);

            modelManager = new ModelManager(this);
            Components.Add(modelManager);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent будет вызываться в игре один раз; здесь загружается
        /// весь контент.
        /// </summary>
        protected override void LoadContent()
        {
            // Создайте новый SpriteBatch, который можно использовать для отрисовки текстур.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            crosshairTexture = Content.Load<Texture2D>(@"textures\crosshair");

            // Load sounds and play initial sounds
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            trackCue = soundBank.GetCue("Tracks");
            trackCue.Play();

            // ЗАДАЧА: используйте здесь this.Content для загрузки контента игры
        }

        /// <summary>
        /// UnloadContent будет вызываться в игре один раз; здесь выгружается
        /// весь контент.
        /// </summary>
        protected override void UnloadContent()
        {
            // ЗАДАЧА: выгрузите здесь весь контент, не относящийся к ContentManager
        }

        /// <summary>
        /// Позволяет игре запускать логику обновления мира,
        /// проверки столкновений, получения ввода и воспроизведения звуков.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        protected override void Update(GameTime gameTime)
        {
            // Позволяет выйти из игры
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // ЗАДАЧА: добавьте здесь логику обновления
            // See if the player has fired a shot
            FireShots(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Вызывается, когда игра отрисовывается.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // ЗАДАЧА: добавьте здесь код отрисовки

            base.Draw(gameTime);
            spriteBatch.Begin();

            spriteBatch.Draw(crosshairTexture,
                new Vector2((Window.ClientBounds.Width / 2)
                    - (crosshairTexture.Width / 2),
                    (Window.ClientBounds.Height / 2)
                    - (crosshairTexture.Height / 2)),
                    Color.White);

            spriteBatch.End();
        }

        protected void FireShots(GameTime gameTime)
        {
            if (shotCountdown <= 0)
            {
                // Did player press space bar or left mouse button?
                if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                    Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    // Add a shot to the model manager
                    modelManager.AddShot(
                        camera.cameraPosition + new Vector3(0, -5, 0),
                        camera.GetCameraDirection * shotSpeed);

                    // Play shot audio
                    PlayCue("Shot");

                    // Reset the shot countdown
                    shotCountdown = shotDelay;
                }
            }
            else
                shotCountdown -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public void PlayCue(string cue)
        {
            soundBank.PlayCue(cue);
        }
    }
}
