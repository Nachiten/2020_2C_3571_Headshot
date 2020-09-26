using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.FPS;

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

        #region Propiedades
        private GraphicsDeviceManager Graphics { get; }
        private Player Player { get; set; }

        private PlayerGUI  PlayerGUI {get;set;}

        private Weapon Weapon { get; set; }
        private Weapon WeaponKnife { get; set; }

        private FreeCamera Camera { get; set; }

        private Stage Stage { get; set; }
        private BasicEffect Effect { get; set; }
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

            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-350, 50, 400), screenSize);

            Player = new Player(this);
            Player.Initialize();

            PlayerGUI = new PlayerGUI(this);
            PlayerGUI.Initialize(Player);


            //Agrego arma
            Weapon = new Weapon(Content.Load<Model>(ContentFolder3D + "weapons/fbx/m4a1_s"));
            WeaponKnife = new Weapon(Content.Load<Model>(ContentFolder3D + "weapons/knife/Karambit"));

            var modelEffect = (BasicEffect)Weapon.WeaponModel.Meshes[0].Effects[0];
            modelEffect.TextureEnabled = true;
            modelEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
            modelEffect.EnableDefaultLighting();

            var modelEffect2 = (BasicEffect)Weapon.WeaponModel.Meshes[1].Effects[0];
            modelEffect2.EnableDefaultLighting();
            modelEffect2.TextureEnabled = true;
            modelEffect2.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");

            Player.AgarrarArma(Weapon);
            Stage = new Stage();

            Effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            Stage.LoadContent(Content, GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                Player.AgarrarArma(WeaponKnife);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Player.AgarrarArma(Weapon);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            Player.Draw(gameTime);
            PlayerGUI.Draw(gameTime);
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
