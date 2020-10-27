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
        float xLenFloor = 2000;
        float zLenFloor = 2000;
        int yLenWall = 200;
        float thickness = 20;
        
        List<QuadPrimitiveCollidable> Quads = new List<QuadPrimitiveCollidable>();
        List<WallCollidable> Walls = new List<WallCollidable>();
        List<ModelCollidable> Columnas = new List<ModelCollidable>();

        public Nivel2(Game game) : base(game)
        {
            #region Recolectables
            posicionesPosiblesRecolectables.Add(new Vector3(-5 * xLenFloor / 32, 0, -1 * zLenFloor / 16));
            posicionesPosiblesRecolectables.Add(new Vector3(-11 * xLenFloor / 32, 0, -1 * zLenFloor / 16));
            posicionesPosiblesRecolectables.Add(new Vector3(-1 * xLenFloor / 16, 0, -3 * zLenFloor / 16));
            posicionesPosiblesRecolectables.Add(new Vector3(-7 * xLenFloor / 16, 0, -3 * zLenFloor / 16));

            posicionesPosiblesRecolectables.Add(new Vector3(5 * xLenFloor / 16, 0, -3 * zLenFloor / 16));
            posicionesPosiblesRecolectables.Add(new Vector3(5 * xLenFloor / 16, 0, -5 * zLenFloor / 16));

            posicionesPosiblesRecolectables.Add(new Vector3(3 * xLenFloor / 16, 0, 3 * zLenFloor / 16));
            posicionesPosiblesRecolectables.Add(new Vector3(3 * xLenFloor / 16, 0, 6 * zLenFloor / 16));
            posicionesPosiblesRecolectables.Add(new Vector3(-3 * xLenFloor / 16, 0, 6 * zLenFloor / 16));
            #endregion

            generarRecolectablesRandom();

            #region Enemy Path
            enemyPath.Add(new PathTrace { posicion = new Vector2(-7 * xLenFloor / 16, 4 * zLenFloor / 16), normal = Vector3.UnitZ }); //1
            enemyPath.Add(new PathTrace { posicion = new Vector2(-7 * xLenFloor / 16, 6 * zLenFloor / 16), normal = Vector3.UnitX }); //2
            enemyPath.Add(new PathTrace { posicion = new Vector2(7 * xLenFloor / 16, 6 * zLenFloor / 16), normal = -Vector3.UnitZ }); //3
            enemyPath.Add(new PathTrace { posicion = new Vector2(7 * xLenFloor / 16, -1 * zLenFloor / 16), normal = -Vector3.UnitX }); //4
            enemyPath.Add(new PathTrace { posicion = new Vector2(1 * xLenFloor / 16, -1 * zLenFloor / 16), normal = Vector3.UnitZ }); //5
            enemyPath.Add(new PathTrace { posicion = new Vector2(1 * xLenFloor / 16, 2 * zLenFloor / 16), normal = -Vector3.UnitX }); //6
            enemyPath.Add(new PathTrace { posicion = new Vector2(-7 * xLenFloor / 16, 2 * zLenFloor / 16), normal = Vector3.UnitZ }); //7
            enemyPath.Add(new PathTrace { posicion = new Vector2(-7 * xLenFloor / 16, 6 * zLenFloor / 16), normal = Vector3.UnitX }); //8
            enemyPath.Add(new PathTrace { posicion = new Vector2(7 * xLenFloor / 16, 6 * zLenFloor / 16), normal = -Vector3.UnitZ }); //9
            enemyPath.Add(new PathTrace { posicion = new Vector2(7 * xLenFloor / 16, -7 * zLenFloor / 16), normal = -Vector3.UnitX }); //10
            enemyPath.Add(new PathTrace { posicion = new Vector2(-4 * xLenFloor / 16, -7 * zLenFloor / 16), normal = Vector3.UnitZ }); //11
            enemyPath.Add(new PathTrace { posicion = new Vector2(-4 * xLenFloor / 16, -1 * zLenFloor / 16), normal = Vector3.UnitZ }); //12
            #endregion
            
        }

        public override void LoadContent()
        {
            Texture2D TexCemento = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento");
            Texture2D TexCemento2 = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento2");
            Texture2D TexGrayWall = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "grayWall");
            Texture2D TexWhiteBricks = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "whiteBricks");

            Player.Instance.Move(new Vector3(-7 * xLenFloor / 16, Player.Instance.Position.Y, 2 * zLenFloor / 16));

            float Y = yLenWall / 2;

            // Piso
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TexCemento, new Vector2(1, 1)));

            // Techo
            //Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TexCemento2, new Vector2(1, 1)));

            // Paredes Principales
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, Y, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexGrayWall, new Vector2(5, 1))); // +X
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, Y, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexGrayWall, new Vector2(5, 1))); // -X
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, Y, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexWhiteBricks, new Vector2(5, 1))); // +Z
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, Y, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexWhiteBricks, new Vector2(5, 1))); // -Z

            #region Paredes
            // 1
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, Y, 0), -Vector3.UnitZ, xLenFloor / 2, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            // 2
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(0, Y, -3 * zLenFloor / 16), -Vector3.UnitX, 3 * zLenFloor / 8, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            // 3
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 16, Y, -3 * zLenFloor / 8), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            // 4
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 8, Y, -2 * zLenFloor / 8), Vector3.UnitX, 2 * zLenFloor / 8, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            // 5
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(2 * xLenFloor / 8, Y, - zLenFloor / 8), Vector3.UnitZ, 2 * zLenFloor / 8, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            // 6
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(0, Y, 2 * zLenFloor / 8), Vector3.UnitZ, 3 * xLenFloor / 4, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            // 7
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(2 * xLenFloor / 8, Y, zLenFloor / 8), Vector3.UnitX, zLenFloor / 4, yLenWall, thickness, TexGrayWall, TexWhiteBricks, new Vector2(10, 1), TexWhiteBricks));
            #endregion

            #region Columnas
            // Columnas
            AddColumn(new Vector2(-7 * xLenFloor / 16, zLenFloor / 16));
            AddColumn(new Vector2(-7 * xLenFloor / 16, 7 * zLenFloor / 16));
            AddColumn(new Vector2(7 * xLenFloor / 16, 7 * zLenFloor / 16));

            AddColumn(new Vector2(-xLenFloor / 16, -zLenFloor / 16));
            AddColumn(new Vector2(-7 * xLenFloor / 16, -zLenFloor / 16));
            AddColumn(new Vector2(-xLenFloor / 16, -5 * zLenFloor / 16));
            AddColumn(new Vector2(-7 * xLenFloor / 16, -5 * zLenFloor / 16));

            AddColumn(new Vector2(5 * xLenFloor / 16, -4 * zLenFloor / 16));
            #endregion

            // Armas
            Recolectables.Add(new M4(new Vector3(-5 * xLenFloor / 32, 50, -3 * zLenFloor / 16)));
            Recolectables.Add(new Pistol(new Vector3(-11 * xLenFloor / 32, 50, -3 * zLenFloor / 16)));

            Enemigos.Add(new Enemigo(enemyPath));

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
