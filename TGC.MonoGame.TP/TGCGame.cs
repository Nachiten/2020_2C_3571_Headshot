﻿using System.Collections.Generic;
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
            Paused,
            Finished
        }


        private Texture2D startButton;

        private Texture2D loadingScreen;

        private Texture2D resumeButton;

        private Texture2D returnButton;

        private Texture2D exitButton;

        private Texture2D otroMapa;

        private Vector2 startButtonPosition;

        private Vector2 otroMapaPosition2;

        private Vector2 exitButtonPosition;

        private Vector2 resumeButtonPosition;


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

            font = Content.Load<SpriteFont>(ContentFolderSpriteFonts + "Arial");

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 150);

            otroMapaPosition2 = new Vector2((GraphicsDevice.Viewport.Height / 2), 300);

            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 200);

            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Height / 2), 300);

            gameState = GameState.StartMenu;


            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
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

            startButton = Content.Load<Texture2D>(ContentFolderTextures + "library");

            otroMapa = Content.Load<Texture2D>(ContentFolderTextures + "maze-level");

            exitButton = Content.Load<Texture2D>(ContentFolderTextures + "exit");

            resumeButton = Content.Load<Texture2D>(ContentFolderTextures + "resume");

            returnButton = Content.Load<Texture2D>(ContentFolderTextures + "exit");

            loadingScreen = Content.Load<Texture2D>(ContentFolderTextures + "loading");

            _bloomFilter = new BloomFilter();
            _bloomFilter.Load(GraphicsDevice, Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            _bloomFilter.BloomPreset = BloomFilter.BloomPresets.Cheap;
            RenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            RenderTargetBlurMenu = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            Effect = Content.Load<Effect>(ContentFolderEffect + "BasicShader");
            Effect.Parameters["screenSize"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

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
                Player.Instance.Update(gameTime);

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
                SpriteBatch.DrawString(font, "Seleccione un mapa", new Vector2((GraphicsDevice.Viewport.Width / 2) - 150, 75), Color.White);

                var startRectangule = new Rectangle((int)startButtonPosition.X + 50, (int)startButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(startButton, startRectangule, Color.White);
                var otroMapaRect = new Rectangle((int)otroMapaPosition2.X + 50, (int)otroMapaPosition2.Y, 200, 100);
                SpriteBatch.Draw(otroMapa, otroMapaRect, Color.White);

                SpriteBatch.End();

            }

            if (gameState == GameState.Paused)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);
                var resumeRectangule = new Rectangle((int)resumeButtonPosition.X + 50, (int)resumeButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(resumeButton, resumeRectangule, Color.White);

                var exitRectangule = new Rectangle((int)exitButtonPosition.X + 50, (int)exitButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(exitButton, exitRectangule, Color.White);
                SpriteBatch.End();
            }

            if (gameState == GameState.Loading)
            {
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;

                SpriteBatch.Begin(samplerState: GraphicsDevice.SamplerStates[0], rasterizerState: GraphicsDevice.RasterizerState);
                SpriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - loadingScreen.Width / 2, (GraphicsDevice.Viewport.Height / 2) - loadingScreen.Height / 2), Color.White);
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
                Texture2D bloom = _bloomFilter.Draw(RenderTarget, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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
                SpriteBatch.DrawString(font, "GAME OVER", new Vector2((GraphicsDevice.Viewport.Width / 2) - 100, GraphicsDevice.Viewport.Height / 2 - 150), Color.White);
                SpriteBatch.DrawString(font, "Score: " + Player.Instance.Score, new Vector2((GraphicsDevice.Viewport.Width / 2) - 80, GraphicsDevice.Viewport.Height / 2 - 100), Color.White);

                var returnRectangle = new Rectangle((int)resumeButtonPosition.X + 50, (int)resumeButtonPosition.Y, 200, 100);
                SpriteBatch.Draw(returnButton, returnRectangle, Color.White);
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
        void MouseClicked(int x, int y)
        {
            var mouseClickRect = new Rectangle(x, y, 10, 10);

            if (gameState == GameState.StartMenu)
            {
                var startButtonRect = new Rectangle((int)startButtonPosition.X + 50, (int)startButtonPosition.Y, 200, 100);

                var otroMapaRect = new Rectangle((int)otroMapaPosition2.X + 50, (int)otroMapaPosition2.Y, 200, 100);

                if (mouseClickRect.Intersects(startButtonRect))
                {
                    Stage = new LibraryStage(this);
                    gameState = GameState.Loading;
                    isLoading = false;
                    Player.Init(this, Camera, Stage);
                }
                if (mouseClickRect.Intersects(otroMapaRect))
                {
                    Stage = new MazeStage(this);
                    gameState = GameState.Loading;
                    isLoading = false;
                    Player.Init(this, Camera, Stage);
                }
            }

            if (gameState == GameState.Paused)
            {

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
            if (gameState == GameState.Finished)
            {
                var resumeButtonRect = new Rectangle((int)resumeButtonPosition.X + 50, (int)resumeButtonPosition.Y, 200, 100);

                if (mouseClickRect.Intersects(resumeButtonRect))
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