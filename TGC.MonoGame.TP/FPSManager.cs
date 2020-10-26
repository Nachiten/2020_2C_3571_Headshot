using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Interface;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS.Scenarios;
using TGC.MonoGame.Samples.Cameras;
using System.Threading;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    public class FPSManager : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffect = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        #region Propiedades
        public GraphicsDeviceManager Graphics { get; }

        private BasicEffect Effect { get; set; }

        private Texture2D startButton;

        private Texture2D loadingScreen;

        private Texture2D exitButton;

        private Vector2 startButtonPosition;

        private Vector2 exitButtonPosition;


        private Thread backgroundThreat;

        private bool isLoading = false;

        MouseState mouseState;

        MouseState previousMouseState;

        private SpriteBatch spriteBatch;

        private GameState gameState;
        private KeyboardManager PlayerControl { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        AStage StageBuilder { get; set; }
        public Camera Camera { get; set; }

        private SpriteFont font { get; set; }

        #endregion

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public FPSManager()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);

            // Descomentar para que el juego sea pantalla completa.
            Graphics.IsFullScreen = false;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 200);

            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 250);

            gameState = GameState.StartMenu;

            Effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }
        protected override void LoadContent()
        {

            font = Content.Load<SpriteFont>("Arial");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            startButton = Content.Load<Texture2D>(ContentFolderTextures + "otroStart");
            
            exitButton = Content.Load<Texture2D>(ContentFolderTextures + "startButton");

            loadingScreen = Content.Load<Texture2D>(ContentFolderTextures + "loading");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            mouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;


            if (gameState == GameState.Loading && !isLoading) 
            {
                backgroundThreat = new Thread(LoadGame);
                isLoading = true;
                backgroundThreat.Start();
            
            }

            if (gameState == GameState.Playing) 
            { 
                //hacer algo con el juego
            }

            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            base.Update(gameTime);
        }

        void MouseClicked(int x, int y) 
        {
            var mouseClickRect = new Rectangle(x, y, 10, 10);

            if(gameState == GameState.StartMenu)
            {
                var startButtonRect = new Rectangle((int)startButtonPosition.X + 50, (int)startButtonPosition.Y, 200, 100);

                //var exitButtonRect = new Rectangle((int)exitButtonPosition.X + 50, (int)exitButtonPosition.Y, 200, 100);

                if (mouseClickRect.Intersects(startButtonRect))
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }

                //if (mouseClickRect.Intersects(exitButtonRect)) 
                //{
                //    Exit();
                //}
            }
        }

        void LoadGame() 
        {
            StageBuilder.LoadContent();
            Thread.Sleep(3000);
            //cargo el mapa etc..
            gameState = GameState.Playing;
            isLoading = false;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) { }

        

            spriteBatch.Begin();

            //Handle game state
            if (gameState == GameState.StartMenu) 
            {
                var startRectangule = new Rectangle((int)startButtonPosition.X + 50, (int)startButtonPosition.Y, 200, 100);
                spriteBatch.Draw(startButton, startRectangule, Color.White);

                //var exitRectangule = new Rectangle((int)exitButtonPosition.X + 50, (int)exitButtonPosition.Y, 200, 100);
                //spriteBatch.Draw(exitButton, exitRectangule, Color.White);
            }
            

            if (gameState == GameState.Loading) {
                spriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - loadingScreen.Width / 2, (GraphicsDevice.Viewport.Height / 2) - loadingScreen.Height / 2), Color.White);
            }

            if (gameState == GameState.Playing && isLoading)
            {
                isLoading = false;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();
            base.UnloadContent();
        }
    }
}
