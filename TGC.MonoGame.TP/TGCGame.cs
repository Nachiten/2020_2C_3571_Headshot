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

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model ModeloM4 { get; set; }
        private Model Knife { get; set; }
        private Matrix WorldM4 { get; set; }
        //private Model ModeloTgcitoClassic { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        public BasicEffect Effect { get; set; }
        public FreeCamera Camera { get; set; }
        IStageBuilder StageBuilder { get; set; }
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

            // NOTA: Cambiar esta linea por la de abajo para cargar el otro mapa
            StageBuilder = new Nivel2(this);
            // StageBuilder = new Nivel2(this); | Mapa 2
            // StageBuilder = new IceWorldStage(this); | Mapa 1 con objetos

            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, 100, 0), screenSize, StageBuilder);

            Player.Init(this);
            interfaz = new PlayerGUI(this);

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

            StageBuilder.CrearEstructura();

            //ModelCollidableStatic m4collidable = new ModelCollidableStatic(Content, ContentFolder3D + "weapons/fbx/m4a1_s",Collision);
            //ModelCollidableStatic knifecollidable = new ModelCollidableStatic(Content, ContentFolder3D + "weapons/knife/Karambit", Collision);

            // Armas
            ModeloM4 = Content.Load<Model>(ContentFolder3D + "weapons/fbx/m4a1_s");
            Knife = Content.Load<Model>(ContentFolder3D + "weapons/knife/Karambit");

            // Obtengo su efecto para cambiarle el color y activar la luz predeterminada que tiene MonoGame.
            //Mesh Silenciador
            var modelEffect = (BasicEffect)ModeloM4.Meshes[0].Effects[0];
            modelEffect.TextureEnabled = true;
            modelEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
            modelEffect.EnableDefaultLighting();

            //Mesh Arma
            var modelEffect2 = (BasicEffect)ModeloM4.Meshes[1].Effects[0];
            modelEffect2.EnableDefaultLighting();
            modelEffect2.TextureEnabled = true;
            modelEffect2.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        bool presionadoTeclaQ = false;
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            // Desbloquear el mouse tocando la P
            if (Keyboard.GetState().IsKeyDown(Keys.P) && !presionadoTeclaQ) { 
                Config.bloquearMouse = !Config.bloquearMouse;
                presionadoTeclaQ = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.P))
                presionadoTeclaQ = false;

            Camera.Update(gameTime);
            StageBuilder.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            StageBuilder.Draw(gameTime);

            // Foreach de la lista de recolectables y dibujarlos
            

            // Testing de agarrar un recolectable
            //if (Keyboard.GetState().IsKeyDown(Keys.R))
            //{
            //    if (!agarrado)
            //    {
            //        recolectarEnIndice(0);
            //        agarrado = true;
            //    }
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.Q))
            //{
            //    Knife.Draw(WorldM4, View, Projection);
            //}
            //else
            //{
            //    ModeloM4.Draw(WorldM4, View, Projection);
            //}

            Player.Instance.Draw(gameTime);
            interfaz.Draw(gameTime);

            //Finalmente invocamos al draw del modelo.
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