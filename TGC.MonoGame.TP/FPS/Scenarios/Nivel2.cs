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
    public class Nivel2 : AStage
    {

        public VertexPositionTexture[] floor { get; set; }
        private QuadPrimitive Floor { get; set; }
        private QuadPrimitive Roof { get; set; }
        private QuadPrimitive WallXp { get; set; }
        private QuadPrimitive WallZn { get; set; }
        private QuadPrimitive WallXn { get; set; }
        private QuadPrimitive WallZp { get; set; }
        float xLenFloor = 1200;
        float zLenFloor = 1500;
        int yLenWall = 450;
        float offsetYCilindro = 20;
        float distanciaCentroColumna = 300;
        List<ModelCollidable> ColumnasCilindros = new List<ModelCollidable>();

        public Nivel2(Game game) : base(game)
        {
            posicionesPosiblesRecolectables.Add(new Vector3(xLenFloor / 2 - 100, 0, zLenFloor / 2 - 100));
            posicionesPosiblesRecolectables.Add(new Vector3(-xLenFloor / 2 + 100, 0, zLenFloor / 2 - 100));
            posicionesPosiblesRecolectables.Add(new Vector3(xLenFloor / 2 - 100, 0, -zLenFloor / 2 + 100));
            posicionesPosiblesRecolectables.Add(new Vector3(-xLenFloor / 2 + 100, 0, -zLenFloor / 2 + 100));
            posicionesPosiblesRecolectables.Add(new Vector3(xLenFloor / 2 - 100, 0, 0));
            posicionesPosiblesRecolectables.Add(new Vector3(-xLenFloor / 2 + 100, 0, 0));
            posicionesPosiblesRecolectables.Add(new Vector3(0, 0, zLenFloor / 2 - 100));
            posicionesPosiblesRecolectables.Add(new Vector3(0, 0, -zLenFloor / 2 + 100));
            posicionesPosiblesRecolectables.Add(new Vector3(xLenFloor / 2 - 100, 0, zLenFloor / 4));
            posicionesPosiblesRecolectables.Add(new Vector3(xLenFloor / 2 - 100, 0, -zLenFloor / 4));
            posicionesPosiblesRecolectables.Add(new Vector3(-xLenFloor / 2 + 100, 0, zLenFloor / 4));
            posicionesPosiblesRecolectables.Add(new Vector3(-xLenFloor / 2 + 100, 0, -zLenFloor / 4));
            int distanciaCentroRecolectable = 230;
            posicionesPosiblesRecolectables.Add(new Vector3(distanciaCentroRecolectable, 0, distanciaCentroRecolectable));
            posicionesPosiblesRecolectables.Add(new Vector3(distanciaCentroRecolectable, 0, -distanciaCentroRecolectable));
            posicionesPosiblesRecolectables.Add(new Vector3(-distanciaCentroRecolectable, 0, distanciaCentroRecolectable));
            posicionesPosiblesRecolectables.Add(new Vector3(-distanciaCentroRecolectable, 0, -distanciaCentroRecolectable);

            Recolectables.Add(new M4(new Vector3(0, 50, zLenFloor / 4)));

            this.generarRecolectablesRandom();

        }
        

        public override void LoadContent()
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
            
            var modeloCollidable1 = new ModelCollidable(GraphicsDevice, Game.Content, FPSManager.ContentFolder3D + "columnas/columnaCilindro", Matrix.CreateTranslation(new Vector3(distanciaCentroColumna, offsetYCilindro, distanciaCentroColumna)) );
            var modeloCollidable2 = new ModelCollidable(GraphicsDevice, Game.Content, FPSManager.ContentFolder3D + "columnas/columnaCilindro", Matrix.CreateTranslation(new Vector3(-distanciaCentroColumna, offsetYCilindro, distanciaCentroColumna)));
            var modeloCollidable3 = new ModelCollidable(GraphicsDevice, Game.Content, FPSManager.ContentFolder3D + "columnas/columnaCilindro", Matrix.CreateTranslation(new Vector3(distanciaCentroColumna, offsetYCilindro, -distanciaCentroColumna)));
            var modeloCollidable4 = new ModelCollidable(GraphicsDevice, Game.Content, FPSManager.ContentFolder3D + "columnas/columnaCilindro", Matrix.CreateTranslation(new Vector3(-distanciaCentroColumna, offsetYCilindro, -distanciaCentroColumna)));

            ColumnasCilindros.Add(modeloCollidable1);
            ColumnasCilindros.Add(modeloCollidable2);
            ColumnasCilindros.Add(modeloCollidable3);
            ColumnasCilindros.Add(modeloCollidable4);

            modeloCollidable1.Aabb.Rotate(Matrix.CreateRotationX(MathHelper.PiOver2));

            Vector3 posicionModelo = Vector3.Transform(Vector3.Zero, modeloCollidable1.World);
            modeloCollidable1.Aabb.Translation(Matrix.CreateTranslation(posicionModelo + new Vector3(-20, 50,0)));

            Collision.Instance.AppendStatic(modeloCollidable1.Aabb);
            Collision.Instance.AppendStatic(modeloCollidable2.Aabb);
            Collision.Instance.AppendStatic(modeloCollidable3.Aabb);
            Collision.Instance.AppendStatic(modeloCollidable4.Aabb);

            foreach (ModelCollidable unModelo in ColumnasCilindros)
            {
                var modelEffectCilindro = (BasicEffect)unModelo.Model.Meshes[0].Effects[0];
                modelEffectCilindro.DiffuseColor = Color.White.ToVector3();
                modelEffectCilindro.Texture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cuarzo");
                modelEffectCilindro.TextureEnabled = true;
                modelEffectCilindro.EnableDefaultLighting();
            }

            //foreach (Model unModelo in ColumnasCuadradas)
            //{
            //    var modelEffectCuadrado = (BasicEffect)unModelo.Meshes[0].Effects[0];
            //    modelEffectCuadrado.DiffuseColor = Color.White.ToVector3();
            //    modelEffectCuadrado.EnableDefaultLighting();
            //    // No funciona bien (si se carga pero no se mapea bien al mesh y queda mal)
            //    //modelEffectCuadrado.Texture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cuarzo");
            //    //modelEffectCuadrado.TextureEnabled = true;
            //}
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Floor.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            Roof.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallXp.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallXn.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallZp.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);
            WallZn.Draw(Matrix.CreateTranslation(Vector3.Zero), View, Projection);

            foreach (ModelCollidable unModelo in ColumnasCilindros)
            {
                unModelo.Draw(View, Projection);
            }
        }


        public void UbicarObjetos(IList<GameComponent> componentes)
        {
            throw new NotImplementedException();
        }
    }



}
