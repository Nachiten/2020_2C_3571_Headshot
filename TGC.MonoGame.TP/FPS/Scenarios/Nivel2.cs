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
        private QuadPrimitive Floor { get; set; }
        private QuadPrimitive Roof { get; set; }
        private QuadPrimitive WallXp { get; set; }
        private QuadPrimitive WallZn { get; set; }
        private QuadPrimitive WallXn { get; set; }
        private QuadPrimitive WallZp { get; set; }
        float xLenFloor = 1500;
        float zLenFloor = 2500;
        int yLenWall = 450;
        float thickness = 20;
        
        List<QuadPrimitiveCollidable> Quads = new List<QuadPrimitiveCollidable>();
        List<WallCollidable> Walls = new List<WallCollidable>();
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

            //enemyPath.Add(new PathTrace { posicion = new Vector2(-5 * xLenFloor / 16, 7 * zLenFloor / 20), normal = Vector3.UnitX }); //1

            generarRecolectablesRandom();
        }

        public override void LoadContent()
        {
            Texture2D TexCemento = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento");
            Texture2D TexCemento2 = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento2");
            Texture2D TexGrayWall = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "grayWall");
            Texture2D TexWhiteBricks = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "whiteBricks");

            float Y = yLenWall / 2;

            // Piso
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TexCemento, new Vector2(1, 1)));

            // Techo
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TexCemento2, new Vector2(1, 1)));

            // Paredes Principales
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, Y, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexGrayWall, new Vector2(5, 1))); // +X
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, Y, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexGrayWall, new Vector2(5, 1))); // -X
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, Y, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexWhiteBricks, new Vector2(5, 1))); // +Z
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, Y, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexWhiteBricks, new Vector2(5, 1))); // -Z

            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(xLenFloor / 6, Y, -zLenFloor / 8), -Vector3.UnitZ, 2 * xLenFloor / 3, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));

            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-3 * xLenFloor / 12, Y, zLenFloor / 8), -Vector3.UnitZ, xLenFloor / 2, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));

            AddColumn(new Vector2(2 * xLenFloor / 6, -3 * zLenFloor / 8));
            AddColumn(new Vector2(-2 * xLenFloor / 6, -3 * zLenFloor / 8));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (QuadPrimitiveCollidable q in Quads)
            {
                q.Draw(View, Projection);
            }
            foreach (WallCollidable w in Walls)
            {
                w.Draw(View, Projection);
            }
            foreach (ModelCollidable unModelo in Columnas)
            {
                unModelo.Draw(View, Projection);
            }
        }

        private void AddColumn(Vector2 Position)
        {
            float offsetYCilindro = 20;
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
    }



}
