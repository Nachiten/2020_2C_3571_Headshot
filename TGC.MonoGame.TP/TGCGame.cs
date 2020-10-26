using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS.Scenarios;
using System.Diagnostics;
using System;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Interface;
using System.Threading;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffect = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            Graphics.IsFullScreen = Config.pantallaCompleta;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        public enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }


        private Texture2D startButton;

        private Texture2D loadingScreen;

        private Texture2D resumeButton;

        private Texture2D exitButton;

        private Vector2 startButtonPosition;

        private Vector2 exitButtonPosition;

        private Vector2 resumeButtonPosition;


        private Thread backgroundThreat;

        private bool isLoading = false;

        MouseState mouseState;

        MouseState previousMouseState;

        private GameState gameState;

        private SpriteBatch SpriteBatch { get; set; }

        private GraphicsDeviceManager Graphics { get; }

        private Matrix WorldM4 { get; set; }
        //private Model ModeloTgcitoClassic { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        public BasicEffect Effect { get; set; }
        public FreeCamera Camera { get; set; }
        AStage Stage { get; set; }
        //private Player jugador { get; set; }
        private PlayerGUI interfaz { get; set; }
        private Weapon arma { get; set; }

        // Esta lita de recolectables deberia estar en otra clase "recolectables"
        //private List<Recolectable> recolectables = new List<Recolectable>();

        //private Enemigo enemigo1;
        //private Enemigo enemigo2;

        // Array de recolectables
        // Cuando recolecta algo se quita de la lista

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            Collision.Init();
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 200);

            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 200);

            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 300);

            gameState = GameState.StartMenu;

            // NOTA: Cambiar esta linea por la de abajo para cargar el otro mapa
            Stage = new IceWorldStage(this);
            // StageBuilder = new Nivel2(this); | Mapa 2
            // StageBuilder = new IceWorldStage(this); | Mapa 1 con objetos

            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 100, 0), screenSize);

            Player.Init(this, Camera, Stage);
            interfaz = new PlayerGUI(this);

            KeyboardManager.Init(Camera);
            MouseManager.Init(Camera);

            interfaz.Initialize(Player.Instance);

            // Configuramos nuestras matrices de la escena.
            //World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(10, 0, 10);
            //View = Matrix.CreateLookAt(new Vector3(30, 20, 150), new Vector3(30, 0, 0), Vector3.Up);
            //Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            WorldM4 = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(50, 0, 110);
            Effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el
        ///     procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Menu

            startButton = Content.Load<Texture2D>(ContentFolderTextures + "otroStart");

            exitButton = Content.Load<Texture2D>(ContentFolderTextures + "exit");

            resumeButton = Content.Load<Texture2D>(ContentFolderTextures + "resume");

            loadingScreen = Content.Load<Texture2D>(ContentFolderTextures + "loading");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
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
                Player.Instance.Update(gameTime);

                Stage.Update(gameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.Paused;
                }
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

            if (gameState == GameState.StartMenu)
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
            //deberia 
            if (gameState == GameState.Paused) {

                var resumeButtonRect = new Rectangle((int)resumeButtonPosition.X + 50, (int)resumeButtonPosition.Y, 200, 100);

                var exitButtonRect = new Rectangle((int)exitButtonPosition.X + 50, (int)exitButtonPosition.Y, 200, 100);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }

                if (mouseClickRect.Intersects(exitButtonRect))
                {
                    Exit();
                }
            }
        }

        void LoadGame()
        {
            //cargo el mapa etc..
            Stage.LoadContent();
            gameState = GameState.Playing;
            isLoading = false;
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);

            //Handle game state
            if (gameState == GameState.StartMenu)
            {
                var startRectangule = new Rectangle((int)startButtonPosition.X + 50, (int)startButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(startButton, startRectangule, Color.White);

                //var exitRectangule = new Rectangle((int)exitButtonPosition.X + 50, (int)exitButtonPosition.Y, 200, 100);
                //spriteBatch.Draw(exitButton, exitRectangule, Color.White);
            }

            if (gameState == GameState.Paused) {
                var resumeRectangule = new Rectangle((int)resumeButtonPosition.X + 50, (int)startButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(resumeButton, resumeRectangule, Color.White);

                var exitRectangule = new Rectangle((int)exitButtonPosition.X + 50, (int)exitButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(exitButton, exitRectangule, Color.White);
            }

            if (gameState == GameState.Loading)
            {
                SpriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - loadingScreen.Width / 2, (GraphicsDevice.Viewport.Height / 2) - loadingScreen.Height / 2), Color.White);
            }

            if (gameState == GameState.Playing)
            {
                isLoading = false;
                Stage.Draw(gameTime);
                Player.Instance.Draw(gameTime);
                interfaz.Draw(gameTime);
            }

            SpriteBatch.End();
            

            base.Draw(gameTime);
        }

        //private void recolectarEnIndice(int index)
        //{
        //    recolectables.RemoveAt(index);
        //}
        private float VectorsAngle(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Acos(Vector3.Dot(v1, v2) / (Vector3.Distance(v1, Vector3.Zero) * Vector3.Distance(v2, Vector3.Zero)));
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }
    }
}