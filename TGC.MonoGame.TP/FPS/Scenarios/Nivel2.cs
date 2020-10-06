using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.Samples.Cameras;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.FPS.Scenarios
{
    public class Nivel2 : DrawableGameComponent, IStageBuilder
    {

        #region Propiedades de Elementos
        private List<Recolectable> Recolectables = new List<Recolectable>();
        #endregion

        public VertexPositionTexture[] floor { get; set; }
        private QuadPrimitive Floor { get; set; }
        private QuadPrimitive Roof { get; set; }
        private QuadPrimitive WallXp { get; set; }
        private QuadPrimitive WallZn { get; set; }
        private QuadPrimitive WallXn { get; set; }
        private QuadPrimitive WallZp { get; set; }
        //private QuadPrimitive Rampa1 { get; set; }
        private int SteelBoxSize = 80;
        private int WoodenBoxSize = 40;
        float xLenFloor = 800;
        float zLenFloor = 1000;
        int yLenWall = 200;
        int gapMiddleBoxes = 100;
        List<Model> ColumnasCilindros { get; set; }
        List<Model> ColumnasCuadradas { get; set; }
        //private Matrix WoodenBoxWorld { get; set; }
        //private Matrix SteelBoxWorld { get; set; }
        public Nivel2(Game game) : base(game)
        {
            ColumnasCilindros = new List<Model>();
            ColumnasCuadradas = new List<Model>();
            //WoodenBoxWorld = Matrix.CreateTranslation(Vector3.UnitY * WoodenBoxSize / 2 - Vector3.UnitX * WoodenBoxSize / 2 - Vector3.UnitZ * WoodenBoxSize / 2);
            //SteelBoxWorld = Matrix.CreateTranslation(Vector3.UnitY * SteelBoxSize / 2);
        }

        public void CrearEstructura()
        {
            Floor = new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor,
                Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento"), new Vector2(1, 1));

            Roof = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor,
                Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento2"), new Vector2(1, 1));

            WallXp = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, yLenWall / 2, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall,
                Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "grayWall"), new Vector2(5, 1));
            WallXn = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, yLenWall / 2, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall,
                Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "grayWall"), new Vector2(5, 1));
            WallZp = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall,
                Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "whiteBricks"), new Vector2(5, 1));
            WallZn = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall,
                Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "whiteBricks"), new Vector2(5, 1));

            //Rampa1 = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0,0,0), new Vector3(1,0,1), Vector3.UnitX, 300, 300,
            //    Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento"), new Vector2(1, 1));

            for (int i = 0; i < 4; i++)
            {
                ColumnasCilindros.Add(Game.Content.Load<Model>(FPSManager.ContentFolder3D + "columnas/columnaCilindro"));
                ColumnasCuadradas.Add(Game.Content.Load<Model>(FPSManager.ContentFolder3D + "columnas/columnaCuadrada"));
            }

            foreach (Model unModelo in ColumnasCilindros)
            {
                var modelEffectCilindro = (BasicEffect)unModelo.Meshes[0].Effects[0];
                modelEffectCilindro.DiffuseColor = Color.White.ToVector3();
                modelEffectCilindro.Texture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cuarzo");
                modelEffectCilindro.TextureEnabled = true;
                modelEffectCilindro.EnableDefaultLighting();
            }

            foreach (Model unModelo in ColumnasCuadradas)
            {
                var modelEffectCuadrado = (BasicEffect)unModelo.Meshes[0].Effects[0];
                modelEffectCuadrado.DiffuseColor = Color.White.ToVector3();
                modelEffectCuadrado.EnableDefaultLighting();
                // No funciona bien (si se carga pero no se mapea bien al mesh y queda como el orto)
                //modelEffectCuadrado.Texture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cuarzo");
                //modelEffectCuadrado.TextureEnabled = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var View = ((TGCGame)Game).Camera.View;
            var Projection = ((TGCGame)Game).Camera.Projection;
            Floor.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            Roof.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallXp.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallXn.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallZp.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallZn.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);

            Matrix escalaCilindro = Matrix.CreateScale(0.43f);
            Matrix escalaCuadrado = Matrix.CreateScale(0.53f);
            float alturaCilindro = 20;
            float alturaCuadrado = 0;

            ColumnasCilindros[0].Draw(escalaCilindro * Matrix.CreateTranslation( new Vector3(100, alturaCilindro, 100) ), View, Projection);
            ColumnasCilindros[1].Draw(escalaCilindro * Matrix.CreateTranslation( new Vector3(-100, alturaCilindro, 100) ), View, Projection);
            ColumnasCilindros[2].Draw(escalaCilindro * Matrix.CreateTranslation( new Vector3(100, alturaCilindro, -100) ), View, Projection);
            ColumnasCilindros[3].Draw(escalaCilindro * Matrix.CreateTranslation( new Vector3(-100, alturaCilindro, -100) ), View, Projection);

            ColumnasCuadradas[0].Draw(escalaCuadrado * Matrix.CreateTranslation(new Vector3(200, alturaCuadrado, 200)), View, Projection);
            ColumnasCuadradas[1].Draw(escalaCuadrado * Matrix.CreateTranslation(new Vector3(-200, alturaCuadrado, 200)), View, Projection);
            ColumnasCuadradas[2].Draw(escalaCuadrado * Matrix.CreateTranslation(new Vector3(200, alturaCuadrado, -200)), View, Projection);
            ColumnasCuadradas[3].Draw(escalaCuadrado * Matrix.CreateTranslation(new Vector3(-200, alturaCuadrado, -200)), View, Projection);

            //base.Draw(gameTime);
        }

        public void UbicarObjetos(IList<GameComponent> componentes)
        {
            throw new NotImplementedException();
        }

        public void RemoveRecolectable(Recolectable R)
        {
            Collision.Instance.RemoveCollectable(R);
            Recolectables.Remove(R);
        }
    }
}
