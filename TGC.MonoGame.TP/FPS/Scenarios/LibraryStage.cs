using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.FPS.Scenarios
{
    public class LibraryStage : AStage
    {

        #region Propiedades de Estructura
        float xLenFloor = 1500;
        float zLenFloor = 2500;
        int yLenWall = 300;
        float thickness = 20;

        List<QuadPrimitiveCollidable> Quads = new List<QuadPrimitiveCollidable>();
        List<WallCollidable> Walls = new List<WallCollidable>();
        #endregion
        List<ModelCollidable> Tables = new List<ModelCollidable>();

        public LibraryStage(Game game) : base(game)
        {

            //Recolectables
            /*Recolectables.Add(new Health(new Vector3(xLenFloor / 2 - 100, 55, zLenFloor / 2 - 100)));
            Recolectables.Add(new Health(new Vector3(-xLenFloor / 2 + 100, 55, zLenFloor / 2 - 100)));
            Recolectables.Add(new Health(new Vector3(xLenFloor / 2 - 100, 55, -zLenFloor / 2 + 100)));
            Recolectables.Add(new Health(new Vector3(-xLenFloor / 2 + 100, 55, -zLenFloor / 2 + 100)));

            Recolectables.Add(new Armor(new Vector3(xLenFloor / 4, -45, 0)));
            Recolectables.Add(new Armor(new Vector3(-xLenFloor / 4, -45, 0)));*/

            Recolectables.Add(new M4(new Vector3(0, 50, zLenFloor / 4)));
            Recolectables.Add(new Pistol(new Vector3(0, 50, -zLenFloor / 4)));

        }

        public override void LoadContent()
        {
            Texture2D TextureBooks = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books");
            Texture2D TextureCemento = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento2");
            Texture2D TextureWood = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/wooden-floor");
            Texture2D TextureCarpet = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/red-carpet");
            Texture2D TextureBlackWood = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/black-wood");

            // Piso
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TextureCarpet, new Vector2(10, 10)));

            // Techo
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TextureBlackWood, new Vector2(10, 10)));

            // Paredes Principales
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, yLenWall / 2, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TextureBooks, new Vector2(16, 1))); // +X
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, yLenWall / 2, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TextureBooks, new Vector2(16, 1))); // -X
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TextureBooks, new Vector2(10, 1))); // +Z
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TextureBooks, new Vector2(10, 1))); // -Z

            float Y = yLenWall / 2;
            // Salas
            #region Sala A
            // Lado +X de sala A
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(0, Y, -zLenFloor / 10), -Vector3.UnitX, 3 * zLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));

            // Puerta -Z de sala A
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 10, Y, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, xLenFloor / 5, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood));
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-4 * xLenFloor / 10, Y, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, xLenFloor / 5, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood));
            // Puerta
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-5 * xLenFloor / 20, 5 * yLenWall / 6, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, xLenFloor / 10, yLenWall/3, thickness, TextureWood, TextureBooks, new Vector2(1, 0.33f), TextureWood));

            // Lado +Z de sala A
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, Y, zLenFloor / 5), Vector3.UnitZ, xLenFloor / 2, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(10, 1), TextureWood));

            // Mesas
            AddTable(new Vector2(-2 * xLenFloor / 16, 0));
            AddTable(new Vector2(-2 * xLenFloor / 16, -1 * zLenFloor / 5));
            AddTable(new Vector2(-6 * xLenFloor / 16, 0));
            AddTable(new Vector2(-6 * xLenFloor / 16, -1 * zLenFloor / 5));
            posicionesPosiblesRecolectables.Add(new Vector3(-5 * xLenFloor / 20, 0, 0));
            posicionesPosiblesRecolectables.Add(new Vector3(-5 * xLenFloor / 20, 0, -1 * zLenFloor / 5));
            #endregion

            #region Sala B
            // Lado +Z de sala B
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(5 * xLenFloor / 16, Y, -zLenFloor / 2 * 0.6f), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));
            // Mesa
            AddTable(new Vector2(5 * xLenFloor / 16, -zLenFloor / 2 * 0.8f));
            posicionesPosiblesRecolectables.Add(new Vector3(7 * xLenFloor / 16, 0, -zLenFloor / 2 * 0.9f));
            posicionesPosiblesRecolectables.Add(new Vector3(7 * xLenFloor / 16, 0, -zLenFloor / 2 * 0.7f));
            #endregion

            // Pared Separadora
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 16, Y, -zLenFloor / 2 * 0.2f), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));

            #region Sala C
            // Lado -X de sala C
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 16, Y, 3 * zLenFloor / 10), Vector3.UnitX, 2 * zLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));

            // Puerta -Z de sala C
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(4 * xLenFloor / 16, Y, 3 * zLenFloor / 10 - zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 8, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood));
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(7 * xLenFloor / 16, Y, 3 * zLenFloor / 10 - zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 8, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood));
            // Puerta
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(11 * xLenFloor / 32, 5 * yLenWall / 6, 3 * zLenFloor / 10 - zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 16, yLenWall/3, thickness, TextureWood, TextureBooks, new Vector2(1, 0.33f), TextureWood));
            // Mesa
            AddTable(new Vector2(11 * xLenFloor / 32, 3 * zLenFloor / 10));
            posicionesPosiblesRecolectables.Add(new Vector3(4 * xLenFloor / 16, 0, 4 * zLenFloor / 10));
            posicionesPosiblesRecolectables.Add(new Vector3(7 * xLenFloor / 16, 0, 4 * zLenFloor / 10));
            #endregion

            #region Sala D
            // Puerta +X de sala D
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 8, Y, 5 * zLenFloor / 20), Vector3.UnitX, zLenFloor / 10, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood));
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 8, Y, 9 * zLenFloor / 20), Vector3.UnitX, zLenFloor / 10, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood));
            // Puerta
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 8, 5 * yLenWall / 6, 7 * zLenFloor / 20), Vector3.UnitX, zLenFloor / 10, yLenWall/3, thickness, TextureWood, TextureBooks, new Vector2(2, 0.33f), TextureWood));
            // Mesas
            AddTable(new Vector2(-5 * xLenFloor / 16, 5 * zLenFloor / 20));
            AddTable(new Vector2(-5 * xLenFloor / 16, 9 * zLenFloor / 20));
            posicionesPosiblesRecolectables.Add(new Vector3(-7 * xLenFloor / 16, 0, 5 * zLenFloor / 20));
            posicionesPosiblesRecolectables.Add(new Vector3(-7 * xLenFloor / 16, 0, 9 * zLenFloor / 20));
            #endregion

            Vector3 Enemy1Pos = new Vector3(50, 100, zLenFloor / 2 - 100);
            Vector3 Enemy2Pos = new Vector3(-200, 100, -zLenFloor / 2 + 100);

            // Inicializacion enemigo
            Enemigos.Add(new Enemigo(Enemy1Pos, new M4(Enemy1Pos), MathHelper.Pi));
            //Enemigos.Add(new Enemigo(Enemy2Pos, new M4(Enemy2Pos), 0));

            //generarRecolectablesRandom();

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #region metodoDraw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (ModelCollidable t in Tables)
            {
                t.Draw(View, Projection);
            }

            foreach (QuadPrimitiveCollidable q in Quads)
            {
                q.Draw(View, Projection);
            }
            foreach (WallCollidable w in Walls)
            {
                w.Draw(View, Projection);
            }
        }
        #endregion

        private void AddTable(Vector2 Position)
        {
            Matrix World = Matrix.CreateScale(0.4f) * Matrix.CreateTranslation(new Vector3(Position.X, 5, Position.Y));
            ModelCollidable table = new ModelCollidable(GraphicsDevice, Content, FPSManager.ContentFolder3D + "tables/round-table", World);

            // Carga de textura
            var tableEffect = (BasicEffect)table.Model.Meshes[0].Effects[0];
            tableEffect.TextureEnabled = true;
            tableEffect.Texture = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "round-table/texture");

            // Ajuste de AABB
            float xOffset = 50;
            float zOffset = 50;
            table.Aabb.SetManually(new Vector3(Position.X - xOffset, 0, Position.Y - zOffset), new Vector3(Position.X + xOffset, 60, Position.Y + zOffset));

            Collision.Instance.AppendStatic(table.Aabb);

            Tables.Add(table);
        }

        public void UbicarObjetos(IList<GameComponent> componentes)
        {
            throw new NotImplementedException();
        }
    }
}
