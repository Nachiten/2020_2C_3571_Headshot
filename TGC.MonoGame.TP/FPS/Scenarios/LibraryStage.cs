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

        public LibraryStage(Game game) : base(game)
        {

            //Recolectables
            Recolectables.Add(new Health(new Vector3(xLenFloor / 2 - 100, 55, zLenFloor / 2 - 100)));
            Recolectables.Add(new Health(new Vector3(-xLenFloor / 2 + 100, 55, zLenFloor / 2 - 100)));
            Recolectables.Add(new Health(new Vector3(xLenFloor / 2 - 100, 55, -zLenFloor / 2 + 100)));
            Recolectables.Add(new Health(new Vector3(-xLenFloor / 2 + 100, 55, -zLenFloor / 2 + 100)));

            Recolectables.Add(new Armor(new Vector3(xLenFloor / 4, -45, 0)));
            Recolectables.Add(new Armor(new Vector3(-xLenFloor / 4, -45, 0)));

            Recolectables.Add(new M4(new Vector3(0, 50, zLenFloor / 4)));
            Recolectables.Add(new Pistol(new Vector3(0, 50, -zLenFloor / 4)));

        }

        public override void LoadContent()
        {
            Texture2D TextureBooks = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books");
            Texture2D TextureCemento = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cemento2");
            Texture2D TextureWood = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/wooden-floor");
            Texture2D TextureCarpet = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/red-carpet");
            // Piso
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TextureCarpet, new Vector2(10, 10)));

            // Techo
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TextureWood, new Vector2(10, 10)));

            // Paredes Principales
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, yLenWall / 2, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TextureBooks, new Vector2(8, 1)));
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, yLenWall / 2, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TextureBooks, new Vector2(8, 1)));
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TextureBooks, new Vector2(10, 1)));
            Quads.Add(new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TextureBooks, new Vector2(10, 1)));

            // Paredes
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, -zLenFloor / 10), -Vector3.UnitX, 3 * zLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));

            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 10, yLenWall / 2, -zLenFloor / 2 * 0.8f), Vector3.UnitZ, xLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-4 * xLenFloor / 10, yLenWall / 2, -zLenFloor / 2 * 0.8f), Vector3.UnitZ, xLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));
            
            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, yLenWall / 2, zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 2, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));

            Walls.Add(new WallCollidable(GraphicsDevice, new Vector3(5 * xLenFloor / 16, yLenWall / 2, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood));


            /*Walls.Add(
                new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, - zLenFloor / 10), Vector3.UnitX, Vector3.UnitY, 3 * zLenFloor / 5, yLenWall,
                Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books"), new Vector2(10, 1))
                );
            Walls.Add(
                new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 10, yLenWall / 2, -zLenFloor /2 * 0.8f), -Vector3.UnitZ, Vector3.UnitY, xLenFloor / 5, yLenWall,
                Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books"), new Vector2(10, 1))
                );
            Walls.Add(
                new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-4 * xLenFloor / 10, yLenWall / 2, -zLenFloor /2 * 0.8f), -Vector3.UnitZ, Vector3.UnitY, xLenFloor / 5, yLenWall,
                Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books"), new Vector2(10, 1))
                );
            Walls.Add(
                new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, yLenWall / 2, zLenFloor / 5), -Vector3.UnitZ, Vector3.UnitY, xLenFloor / 2, yLenWall,
                Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/wooden-floor"), new Vector2(10, 1))
                );

            Walls.Add(
                new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(5 * xLenFloor / 16, yLenWall / 2, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, Vector3.UnitY, 3 * xLenFloor / 8, yLenWall,
                Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books"), new Vector2(10, 1))
                );*/






            /*Vector3 Enemy1Pos = new Vector3(200, 50, zLenFloor / 2 - 100);
            Vector3 Enemy2Pos = new Vector3(-200, 50, -zLenFloor / 2 + 100);

            // Inicializacion enemigo
            Enemigos.Add(new Enemigo(Enemy1Pos, new M4(Enemy1Pos), MathHelper.Pi));
            Enemigos.Add(new Enemigo(Enemy2Pos, new M4(Enemy2Pos), 0));*/

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

        public void UbicarObjetos(IList<GameComponent> componentes)
        {
            throw new NotImplementedException();
        }
    }
}
