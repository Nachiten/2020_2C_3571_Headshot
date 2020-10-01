﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.FPS;
using BepuPhysics;
using BepuUtilities.Memory;
using TGC.MonoGame.TP.FPS.Interface;

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
        public GraphicsDeviceManager Graphics { get; }
        private Player Player { get; set; }

        private PlayerGUI  PlayerGUI {get;set;}

        //private FreeCamera Camera { get; set; }

        
        private BasicEffect Effect { get; set; }

        private VertexPositionTexture[] floorVerts { get; set; }

        private KeyboardManager PlayerControl { get; set; }


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
            //Camera = new FreeCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-350, 50, 400), screenSize);

            Player = new Player(this);
            Player.Initialize();

            PlayerGUI = new PlayerGUI(this);
            PlayerGUI.Initialize(Player);

            PlayerControl = new KeyboardManager(Player);


            //StageBuilder = new IceWorldStage(this);
            //StageBuilder.CrearPiso(800, 1000);


            Effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //Camera.Update(gameTime);
            PlayerControl.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Vector3 piso4 = new Vector3(20, 20, 0);
            Vector3 piso1 = new Vector3(-20, -20, 0);
            Vector3 piso2 = new Vector3(-20, 20, 0);
            Vector3 piso3 = new Vector3(20, -20, 0);


            var textura = new Vector2(5, 10);
            floorVerts = TP.Utils.ShapeCreatorHelper.CreatePlane(piso1, piso2, piso3, piso4, textura);
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3(0, 40, 20);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            Effect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);

            float aspectRatio =
                Graphics.PreferredBackBufferWidth / (float)Graphics.PreferredBackBufferHeight;
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            Effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            Effect.TextureEnabled = true;
            Player.Draw(gameTime);
            PlayerGUI.Draw(gameTime);
            Effect.Texture = Content.Load<Texture2D>(ContentFolderTextures + "ice_rink");
            

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    floorVerts,
                    // The offset, which is 0 since we want to start
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }


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
