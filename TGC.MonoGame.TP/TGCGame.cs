using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Scenarios;
using TGC.MonoGame.TP.FPS.Interface;
using System;
using System.Diagnostics;
using System.Threading;

namespace TGC.MonoGame.TP
{
    public struct Button
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Rectangle Rectangle;
    }
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

            resolution = new Resolution { Width = 1280, Height = 720 };
        }

        public enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused,
            Finished
        }
        public struct Resolution
        {
            public int Width;
            public int Height;
        }

        private Thread backgroundThread;

        private bool isLoading = false;

        MouseState mouseState;

        MouseState previousMouseState;

        private GameState gameState;

        private SpriteBatch SpriteBatch { get; set; }

        private GraphicsDeviceManager Graphics { get; }
        public Effect Effect { get; set; }
        public FreeCamera Camera { get; set; }
        AStage Stage { get; set; }
        private PlayerGUI interfaz { get; set; }
        private SpriteFont font { get; set; }
        private BloomFilter _bloomFilter;
        private RenderTarget2D RenderTarget { get; set; }
        private RenderTarget2D RenderTargetBlurMenu;
        private ModelCollidable Modelo3dMenu1;
        private ModelCollidable Modelo3dMenu2;
        private QuadPrimitiveCollidable Plane3dMenu;
        private FullScreenQuad fsq;
        public Resolution resolution;
        private Button LibraryStageButt;
        private Button MazeStageButt;
        private Button ResumeButt;
        private Button ExitButt;
        private Button LoadingButt;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            Collision.Init();
            Graphics.PreferredBackBufferWidth = resolution.Width;
            Graphics.PreferredBackBufferHeight = resolution.Height;
            Graphics.ApplyChanges();

            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            font = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Arial");

            gameState = GameState.StartMenu;


            var screenSize = new Point(resolution.Width / 2, resolution.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 5, 20), screenSize);

            interfaz = new PlayerGUI(this);

            KeyboardManager.Init(Camera);
            MouseManager.Init(Camera);
            SoundManager.Init(Content);

            interfaz.Initialize();

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
            LibraryStageButt = CreateButton(ContentFolderTextures + "library", 0, -0.1f);
            MazeStageButt = CreateButton(ContentFolderTextures + "maze-level", 0, 0.1f);
            ResumeButt = CreateButton(ContentFolderTextures + "resume", 0, -0.1f);
            ExitButt = CreateButton(ContentFolderTextures + "exit", 0, 0.1f);
            LoadingButt = CreateButton(ContentFolderTextures + "loading", 0, 0);

            _bloomFilter = new BloomFilter();
            _bloomFilter.Load(GraphicsDevice, Content, resolution.Width, resolution.Height);

            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Cheap;
            RenderTarget = new RenderTarget2D(GraphicsDevice, resolution.Width, resolution.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            RenderTargetBlurMenu = new RenderTarget2D(GraphicsDevice, resolution.Width, resolution.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            Effect = Content.Load<Effect>(ContentFolderEffect + "BasicShader");
            Effect.Parameters["screenSize"].SetValue(new Vector2(resolution.Width, resolution.Height));

            Light light = new Light { Position = new Vector3(0, 30, 0), AmbientColor = Color.DarkRed, DiffuseColor = Color.White, SpecularColor = Color.White };

            Modelo3dMenu1 = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "Knight/Knight_01", Matrix.Identity);
            Modelo3dMenu1.SetEffect(Effect);
            Modelo3dMenu1.SetLightParameters(.3f, .6f, .1f, 10f);
            Modelo3dMenu1.SetLight(light);
            Modelo3dMenu1.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "Knight/Knight01_albedo"));
            Modelo3dMenu2 = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "Knight/Knight_01", Matrix.Identity);
            Modelo3dMenu2.SetEffect(Effect);
            Modelo3dMenu2.SetLightParameters(.3f, .6f, .1f, 10f);
            Modelo3dMenu2.SetLight(light);
            Modelo3dMenu2.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "Knight/Knight01_albedo"));

            Plane3dMenu = new QuadPrimitiveCollidable(GraphicsDevice,Vector3.Zero,Vector3.UnitY, Vector3.UnitZ,50f,50f,
                Content.Load<Texture2D>(ContentFolderTextures + "menu-floor"),Vector2.One*3);
            Plane3dMenu.SetEffect(Effect);
            Plane3dMenu.SetLightParameters(.3f, .6f, .1f, 10f);
            Plane3dMenu.SetLight(light);

            SoundManager.Instance.reproducirSonido(SoundManager.Sonido.Menu);
            fsq = new FullScreenQuad(GraphicsDevice);

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
                backgroundThread = new Thread(LoadGame);
                isLoading = true;
                backgroundThread.Start();

            }

            if (gameState == GameState.Playing)
            {
                Player.Instance.Update(gameTime, Effect);

                Stage.Update(gameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    gameState = GameState.Paused;
                }
                if (Player.Instance.Health <= 0) {
                    SoundManager.Instance.reproducirSonido(SoundManager.Sonido.MuerteJugador);
                    gameState = GameState.Finished;
                    // Reset Camera
                    Camera.ResetCamera(new Vector3(0, 5, 20));
                    Modelo3dMenu1.SetEffect(Effect);
                    Modelo3dMenu2.SetEffect(Effect);
                }
            }

            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            
            if (gameState == GameState.StartMenu)
            {
                var time = (float)gameTime.TotalGameTime.TotalSeconds;
                Matrix WorldModel = Matrix.CreateScale(0.05f) * Matrix.CreateRotationX(-MathHelper.PiOver2);
                Modelo3dMenu1.Transform(WorldModel * Matrix.CreateRotationY(time) * Matrix.CreateTranslation(Vector3.UnitX * 7), false);
                Modelo3dMenu2.Transform(WorldModel * Matrix.CreateRotationY(-time) * Matrix.CreateTranslation(-Vector3.UnitX * 7), false);
                Modelo3dMenu1.SetCameraPos(Camera.Position);
                Modelo3dMenu2.SetCameraPos(Camera.Position);
                Plane3dMenu.SetCameraPos(Camera.Position);
                SoundManager.Instance.reproducirSonido(SoundManager.Sonido.Menu);
            }

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            //Handle game state
            if (gameState == GameState.StartMenu)
            {
                // 1ra pasada (3d)
                GraphicsDevice.SetRenderTarget(RenderTarget);
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                Modelo3dMenu1.Draw(Camera.View, Camera.Projection);
                Modelo3dMenu2.Draw(Camera.View, Camera.Projection);
                Plane3dMenu.Draw(Camera.View, Camera.Projection);

                // 2da pasada (Blur)
                GraphicsDevice.SetRenderTarget(RenderTargetBlurMenu);
                Effect.CurrentTechnique = Effect.Techniques["PostProcessingBlur"];
                Effect.Parameters["RenderTargetTexture"].SetValue(RenderTarget);
                fsq.Draw(Effect);

                // 3ra pasada
                GraphicsDevice.SetRenderTarget(null);
                SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);
                SpriteBatch.Draw(RenderTargetBlurMenu, new Rectangle(0,0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                DrawCenteredString("HEADSHOT", 0, -0.4f);
                DrawCenteredString("Seleccionar Stage", 0, -0.3f);
                SpriteBatch.Draw(LibraryStageButt.Texture, LibraryStageButt.Rectangle, Color.White);
                SpriteBatch.Draw(MazeStageButt.Texture, MazeStageButt.Rectangle, Color.White);

                SpriteBatch.End();

            }

            if (gameState == GameState.Paused)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);

                SpriteBatch.Draw(ResumeButt.Texture, ResumeButt.Rectangle, Color.White);
                SpriteBatch.Draw(ExitButt.Texture, ExitButt.Rectangle, Color.White);

                SpriteBatch.End();
            }

            if (gameState == GameState.Loading)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);
                SpriteBatch.Draw(LoadingButt.Texture, LoadingButt.Position, Color.White);
                SpriteBatch.End();
            }

            if (gameState == GameState.Playing)
            {
                // 1ra pasada (3d)
                GraphicsDevice.SetRenderTarget(RenderTarget);
                GraphicsDevice.Clear(Color.Black);

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                isLoading = false;
                Stage.Draw(gameTime);
                Player.Instance.Draw(gameTime);

                // 2da pasada (Bloom)
                Texture2D bloom = _bloomFilter.Draw(RenderTarget, resolution.Width, resolution.Height);
                GraphicsDevice.SetRenderTarget(RenderTargetBlurMenu);

                Effect.CurrentTechnique = Effect.Techniques["PostProcessing"];
                Effect.Parameters["RenderTargetTexture"].SetValue(RenderTarget);
                Effect.Parameters["BloomTexture"].SetValue(bloom);
                fsq.Draw(Effect);

                // 3ra pasada
                GraphicsDevice.SetRenderTarget(null);
                interfaz.Draw(gameTime, RenderTargetBlurMenu);
            }

            if (gameState == GameState.Finished)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);

                DrawCenteredString("GAME OVER", 0, -0.3f);
                DrawCenteredString("Score " + Player.Instance.Score, 0, -0.2f);

                SpriteBatch.Draw(ExitButt.Texture, ExitButt.Rectangle, Color.White);
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
        void MouseClicked(int x, int y)
        {
            var mouseClickRect = new Rectangle(x, y, 10, 10);

            if (gameState == GameState.StartMenu)
            {
                if (mouseClickRect.Intersects(LibraryStageButt.Rectangle))
                {
                    Stage = new LibraryStage(this);
                    gameState = GameState.Loading;
                    isLoading = false;
                    Player.Init(this, Camera, Stage);
                }
                if (mouseClickRect.Intersects(MazeStageButt.Rectangle))
                {
                    Stage = new MazeStage(this);
                    gameState = GameState.Loading;
                    isLoading = false;
                    Player.Init(this, Camera, Stage);
                }
            }

            if (gameState == GameState.Paused)
            {
                if (mouseClickRect.Intersects(ResumeButt.Rectangle))
                {
                    gameState = GameState.Playing;
                }

                if (mouseClickRect.Intersects(ExitButt.Rectangle))
                {
                    Exit();
                }
            }
            if (gameState == GameState.Finished)
            {
                if (mouseClickRect.Intersects(ExitButt.Rectangle))
                {
                    SoundManager.Instance.detenerMusica();
                    gameState = GameState.StartMenu;
                    Player.Instance.Dispose();
                    Collision.Instance.Dispose();
                    Collision.Init();
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

        private float VectorsAngle(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Acos(Vector3.Dot(v1, v2) / (Vector3.Distance(v1, Vector3.Zero) * Vector3.Distance(v2, Vector3.Zero)));
        }
        public Vector2 centPos(Texture2D tex)
        {
            return new Vector2((resolution.Width / 2) - tex.Width / 2, (resolution.Height / 2) - tex.Height / 2);
        }
        public Button CreateButton(String texturePath, float offsetXPercent, float offsetYPercent)
        {
            Texture2D tex = Content.Load<Texture2D>(texturePath);
            Vector2 pos = centPos(tex);
            pos.X += resolution.Width * offsetXPercent;
            pos.Y += resolution.Height * offsetYPercent;
            Rectangle rec = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            return new Button { Texture = tex, Position = pos, Rectangle = rec };
        }
        public void DrawCenteredString(String str, float offsetXPercent, float offsetYPercent)
        {
            Vector2 strMes = font.MeasureString(str);
            Vector2 pos = new Vector2((resolution.Width / 2) - strMes.X / 2, (resolution.Height / 2) - strMes.Y / 2);
            pos.X += resolution.Width * offsetXPercent;
            pos.Y += resolution.Height * offsetYPercent;
            SpriteBatch.DrawString(font, str, pos, Color.White);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();
            _bloomFilter.Dispose();

            base.UnloadContent();
        }
    }
}