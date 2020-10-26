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
        List<ModelCollidable> Columnas = new List<ModelCollidable>();

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
            posicionesPosiblesRecolectables.Add(new Vector3(-distanciaCentroRecolectable, 0, -distanciaCentroRecolectable));

            Recolectables.Add(new M4(new Vector3(0, 50, zLenFloor / 4)));
            Recolectables.Add(new Pistol(new Vector3(0, 50, -zLenFloor / 4)));

            this.generarRecolectablesRandom();

        }
        

        public override void LoadContent()
        {
            Texture2D TexCemento = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento");
            Texture2D TexCemento2 = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento2");
            Texture2D TexGrayWall = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "grayWall");
            Texture2D TexWhiteBricks = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "whiteBricks");

            Floor = new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor,TexCemento, new Vector2(1, 1));
            Roof = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TexCemento2, new Vector2(1, 1));
            WallXp = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, yLenWall / 2, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexGrayWall, new Vector2(5, 1));
            WallXn = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, yLenWall / 2, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexGrayWall, new Vector2(5, 1));
            WallZp = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexWhiteBricks, new Vector2(5, 1));
            WallZn = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexWhiteBricks, new Vector2(5, 1));

            AddColumn(new Vector2(distanciaCentroColumna, distanciaCentroColumna));
            AddColumn(new Vector2(-distanciaCentroColumna, distanciaCentroColumna));
            AddColumn(new Vector2(distanciaCentroColumna, -distanciaCentroColumna));
            AddColumn(new Vector2(-distanciaCentroColumna, -distanciaCentroColumna));

            base.LoadContent();
        }
        private void AddColumn(Vector2 Position)
        {
            Matrix world = Matrix.CreateTranslation(new Vector3(Position.X, offsetYCilindro, Position.Y));
            ModelCollidable column = new ModelCollidable(GraphicsDevice, Game.Content, FPSManager.ContentFolder3D + "columnas/columnaCilindro", world);
            // Ajuste de AABB
            float xOffset = 50;
            float zOffset = 50;
            column.Aabb.SetManually(new Vector3(Position.X - xOffset, 0, Position.Y - zOffset), new Vector3(Position.X + xOffset, yLenWall, Position.Y + zOffset));

            Boxes.Add(column.Aabb);
            Collision.Instance.AppendStatic(column.Aabb);

            var modelEffectCilindro = (BasicEffect)column.Model.Meshes[0].Effects[0];
            modelEffectCilindro.DiffuseColor = Color.White.ToVector3();
            modelEffectCilindro.Texture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cuarzo");
            modelEffectCilindro.TextureEnabled = true;
            modelEffectCilindro.EnableDefaultLighting();

            Columnas.Add(column);
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

            foreach (ModelCollidable unModelo in Columnas)
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
