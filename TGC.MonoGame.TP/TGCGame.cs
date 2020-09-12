using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.Samples.Viewer.GUI;


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
            //Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model ModeloTanque { get; set; }

        private Model ModeloM4 { get; set; }
        private Model ModeloCiudad { get; set; }
        private Model ModeloTgcitoClassic { get; set; }
        private Model ModeloRobotTGC { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

        private VertexPositionTexture[] piso { get; set; }
        private VertexPositionTexture[] pared { get; set; }

        private AxisLines ejes { get; set; }
        private BasicEffect Effect { get; set; }

        private FreeCamera Camera { get; set; }

        private Texture2D TexturaPlano { get; set; }
        private Texture2D TexturaPared { get; set; }
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: todo procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.
            ejes = new AxisLines(GraphicsDevice);

            piso = new VertexPositionTexture[6];
            
            piso[0].Position = new Vector3(-20, 0, -20);
            piso[1].Position = new Vector3(-20, 0, 20);
            piso[2].Position = new Vector3(20, 0, -20);
            piso[3].Position = piso[1].Position;
            piso[4].Position = new Vector3(20, 0, 20);
            piso[5].Position = piso[2].Position;

            piso[0].TextureCoordinate = new Vector2(0, 0);
            piso[1].TextureCoordinate = new Vector2(0, 1);
            piso[2].TextureCoordinate = new Vector2(1, 0);
            
            piso[3].TextureCoordinate = piso[1].TextureCoordinate;
            piso[4].TextureCoordinate = new Vector2(1, 1);
            piso[5].TextureCoordinate = piso[2].TextureCoordinate;

            pared = new VertexPositionTexture[6];

            pared[0].Position = new Vector3(-20, -20, 0);
            pared[1].Position = new Vector3(-20, 20, 0);
            pared[2].Position = new Vector3(20, -20, 0);
            pared[3].Position = pared[1].Position;
            pared[4].Position = new Vector3(20, 20, 0);
            pared[5].Position = pared[2].Position;

            pared[0].TextureCoordinate = new Vector2(0, 0);
            pared[1].TextureCoordinate = new Vector2(0, 1);
            pared[2].TextureCoordinate = new Vector2(1, 0);
            
            pared[3].TextureCoordinate = piso[1].TextureCoordinate;
            pared[4].TextureCoordinate = new Vector2(1, 1);
            pared[5].TextureCoordinate = piso[2].TextureCoordinate;

            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-350, 50, 400), screenSize);

            // Configuramos nuestras matrices de la escena.
            World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(10,0,10);
            View = Matrix.CreateLookAt(new Vector3(30,20,150), new Vector3(30,0,0) , Vector3.Up) ;

            Effect = new BasicEffect(GraphicsDevice);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            base.Initialize();
        }

        void DrawWall()
        {

            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3(0, 40, 20);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            Effect.View = Camera.View;
            Effect.Projection = Camera.Projection;

            Effect.Texture = TexturaPared;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    pared,
                    // The offset, which is 0 since we want to start
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }

        void DrawFloor()
        {

            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3(0, 40, 20);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            Effect.View = Camera.View;
            Effect.Projection = Camera.Projection;

            // new code:
            Effect.TextureEnabled = true;
            Effect.Texture = TexturaPlano;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    piso,
                    // The offset, which is 0 since we want to start
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el
        ///     procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            //SpriteBatch = new SpriteBatch(GraphicsDevice);



            // Cargo el modelo del logo.
           // Model = Content.Load<Model>(ContentFolder3D + "tgc-logo/tgc-logo");
            ModeloTanque = Content.Load<Model>(ContentFolder3D + "tank/tank");

            ModeloM4 = Content.Load<Model>(ContentFolder3D + "weapons/fbx/m4a1_s");

            ModeloCiudad = Content.Load<Model>(ContentFolder3D + "scene/city");

            ModeloTgcitoClassic = Content.Load<Model>(ContentFolder3D + "tgcito-classic/tgcito-classic");

            ModeloRobotTGC = Content.Load<Model>(ContentFolder3D + "tgcito-mega/tgcito-mega");


            // We aren't using the content pipeline, so we need
            // to access the stream directly:
            using (var stream = TitleContainer.OpenStream("Content/pasto.jpg"))
            {
                TexturaPlano = Texture2D.FromStream(this.GraphicsDevice, stream);
            }

            using (var stream = TitleContainer.OpenStream("Content/ladrillo.png"))
            {
                TexturaPared = Texture2D.FromStream(this.GraphicsDevice, stream);
            }

            // Obtengo su efecto para cambiarle el color y activar la luz predeterminada que tiene MonoGame.
            var modelEffect = (BasicEffect) ModeloM4.Meshes[0].Effects[0];
            modelEffect.DiffuseColor = Color.YellowGreen.ToVector3();
            modelEffect.EnableDefaultLighting();

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            Camera.Update(gameTime);

            //// Basado en el tiempo que paso se va generando una rotacion.
            //Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.White);


            DrawFloor();
            DrawWall();
            // Rotacion en y
            //Matrix.CreateRotationY(Rotation)

            //Finalmente invocamos al draw del modelo.

            ModeloM4.Draw(World * Matrix.CreateScale(5) * Matrix.CreateTranslation(0,-10,0), View, Projection);

            ModeloCiudad.Draw(World * Matrix.CreateScale(0.1f), Camera.View, Camera.Projection);

            ModeloTanque.Draw(World * Matrix.CreateScale(3) * Matrix.CreateTranslation(20, -10, 30), Camera.View, Camera.Projection);

            ModeloTgcitoClassic.Draw(World * Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(35, 1, 90) , Camera.View, Camera.Projection);

            ModeloRobotTGC.Draw(World * Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(55, 1, 90), Camera.View, Camera.Projection);

            
            base.Draw(gameTime);
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