using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.FPS.Interface;
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

        List<ModelCollidable> Tables = new List<ModelCollidable>();
        #endregion

        Color LightsColor = Color.White;
        RocketLauncher rl;

        public LibraryStage(Game game) : base(game)
        { 
            Effect = Content.Load<Effect>(FPSManager.ContentFolderEffect + "BlinnPhong");
            // Sala A
            Rooms.Add(new List<IElementEffect>());
            // Sala B
            Rooms.Add(new List<IElementEffect>());
            // Sala C
            Rooms.Add(new List<IElementEffect>());
            // Sala D
            Rooms.Add(new List<IElementEffect>());

            // Weapons
            ARecolectable m4 = new M4(new Vector3(-4 * xLenFloor / 10, 50, zLenFloor / 8));
            rl = new RocketLauncher(new Vector3(-xLenFloor / 10, 50, zLenFloor / 8));
            // Seteo las luces de sala A
            Rooms[0].Add(m4);
            Rooms[0].Add(rl);
            // Las agrego a los recolectables
            Recolectables.Add(m4);
            Recolectables.Add(rl);
        }

        public override void LoadContent()
        {
            Texture2D TextureBooks = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/books");
            Texture2D TextureWood = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/wooden-floor");
            Texture2D TextureCarpet = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/red-carpet");
            Texture2D TextureBlackWood = Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "library/black-wood");

            // Piso
            QuadPrimitiveCollidable Piso = new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TextureCarpet, new Vector2(10, 10));
            // Techo
            QuadPrimitiveCollidable Techo = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall, 0), Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TextureBlackWood, new Vector2(10, 10));
            // Paredes Principales
            QuadPrimitiveCollidable ParedPX = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, yLenWall / 2, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TextureBooks, new Vector2(16, 1));
            QuadPrimitiveCollidable ParedNX = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, yLenWall / 2, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TextureBooks, new Vector2(16, 1));
            QuadPrimitiveCollidable ParedPZ = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TextureBooks, new Vector2(10, 1));
            QuadPrimitiveCollidable ParedNZ = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, yLenWall / 2, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TextureBooks, new Vector2(10, 1));

            // Agrego Paredes
            Quads.Add(Piso);
            Quads.Add(Techo);
            Quads.Add(ParedPX);
            Quads.Add(ParedNX);
            Quads.Add(ParedPZ);
            Quads.Add(ParedNZ);
            Rooms[0].Add(Piso);
            Rooms[0].Add(Techo);
            Rooms[0].Add(ParedNX);
            Rooms[1].Add(ParedNZ);
            Rooms[1].Add(ParedPX);
            Rooms[2].Add(ParedPZ);

            // Y constants
            float Y = yLenWall / 2;
            float lightY = 7 * yLenWall / 8;

            // Salas
            #region Sala A
            // Lado +X de sala A
            WallCollidable LadoPXsalaA = new WallCollidable(GraphicsDevice, new Vector3(0, Y, -zLenFloor / 10), -Vector3.UnitX, 3 * zLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood);
            // Puerta -Z de sala A
            WallCollidable PuertaNZsalaA1 = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 10, Y, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, xLenFloor / 5, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood);
            WallCollidable PuertaNZsalaA2 = new WallCollidable(GraphicsDevice, new Vector3(-4 * xLenFloor / 10, Y, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, xLenFloor / 5, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood);
            // Puerta
            WallCollidable PuertaSalaA = new WallCollidable(GraphicsDevice, new Vector3(-5 * xLenFloor / 20, 5 * yLenWall / 6, -zLenFloor / 2 * 0.8f), -Vector3.UnitZ, xLenFloor / 10, yLenWall / 3, thickness, TextureWood, TextureBooks, new Vector2(1, 0.33f), TextureWood);
            // Lado +Z de sala A
            WallCollidable LadoPZsalaA = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, Y, zLenFloor / 5), Vector3.UnitZ, xLenFloor / 2, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(10, 1), TextureWood);
            
            // Agrego Paredes
            Walls.Add(LadoPXsalaA);
            Walls.Add(PuertaNZsalaA1);
            Walls.Add(PuertaNZsalaA2);
            Walls.Add(PuertaSalaA);
            Walls.Add(LadoPZsalaA);
            Rooms[0].Add(LadoPXsalaA);
            Rooms[0].Add(PuertaNZsalaA1);
            Rooms[0].Add(PuertaNZsalaA2);
            Rooms[0].Add(PuertaSalaA);
            Rooms[0].Add(LadoPZsalaA);

            // Mesas
            Rooms[0].Add(AddTable(new Vector2(-2 * xLenFloor / 16, 0)));
            Rooms[0].Add(AddTable(new Vector2(-2 * xLenFloor / 16, -1 * zLenFloor / 5)));
            Rooms[0].Add(AddTable(new Vector2(-6 * xLenFloor / 16, 0)));
            Rooms[0].Add(AddTable(new Vector2(-6 * xLenFloor / 16, -1 * zLenFloor / 5)));

            // Pos Recolectables
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-5 * xLenFloor / 20, 0, 0), Room = 0 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-5 * xLenFloor / 20, 0, -1 * zLenFloor / 5), Room = 0 });

            //Luz
            Lights.Add(new Light { Position = new Vector3(-5 * xLenFloor / 20, lightY, -zLenFloor / 10), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            #endregion

            #region Sala B
            // Lado +Z de sala B
            WallCollidable LadoPZsalaB = new WallCollidable(GraphicsDevice, new Vector3(5 * xLenFloor / 16, Y, -zLenFloor / 2 * 0.6f), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood);
            // Pared Separadora
            WallCollidable ParedSeparadoraSalaB = new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 16, Y, -zLenFloor / 2 * 0.2f), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood);
            
            // Agrego Paredes
            Walls.Add(LadoPZsalaB);
            Walls.Add(ParedSeparadoraSalaB);
            Rooms[1].Add(LadoPZsalaB);
            Rooms[1].Add(ParedSeparadoraSalaB);

            // Mesa
            Rooms[1].Add(AddTable(new Vector2(5 * xLenFloor / 16, -zLenFloor / 2 * 0.8f)));

            // Pos Recolectables
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(7 * xLenFloor / 16, 0, -zLenFloor / 2 * 0.9f), Room = 1 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(7 * xLenFloor / 16, 0, -zLenFloor / 2 * 0.7f), Room = 1 });

            //Luz
            Lights.Add(new Light { Position = new Vector3(5 * xLenFloor / 16, lightY, -zLenFloor / 2 * 0.8f), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            #endregion

            #region Sala C
            // Lado -X de sala C
            WallCollidable LadoNXsalaC = new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 16, Y, 3 * zLenFloor / 10), Vector3.UnitX, 2 * zLenFloor / 5, yLenWall, thickness, TextureBooks, TextureWood, new Vector2(10, 1), TextureWood);
            // Puerta -Z de sala C
            WallCollidable PuertaNZsalaC1 = new WallCollidable(GraphicsDevice, new Vector3(4 * xLenFloor / 16, Y, 3 * zLenFloor / 10 - zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 8, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood);
            WallCollidable PuertaNZsalaC2 = new WallCollidable(GraphicsDevice, new Vector3(7 * xLenFloor / 16, Y, 3 * zLenFloor / 10 - zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 8, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood);
            // Puerta
            WallCollidable PuertaSalaC = new WallCollidable(GraphicsDevice, new Vector3(11 * xLenFloor / 32, 5 * yLenWall / 6, 3 * zLenFloor / 10 - zLenFloor / 5), -Vector3.UnitZ, xLenFloor / 16, yLenWall / 3, thickness, TextureWood, TextureBooks, new Vector2(1, 0.33f), TextureWood);

            // Agrego Paredes
            Walls.Add(LadoNXsalaC);
            Walls.Add(PuertaNZsalaC1);
            Walls.Add(PuertaNZsalaC2);
            Walls.Add(PuertaSalaC);
            Rooms[2].Add(LadoNXsalaC);
            Rooms[2].Add(PuertaNZsalaC1);
            Rooms[2].Add(PuertaNZsalaC2);
            Rooms[2].Add(PuertaSalaC);

            // Mesa
            Rooms[2].Add(AddTable(new Vector2(11 * xLenFloor / 32, 3 * zLenFloor / 10)));

            // Pos Recolectables
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(4 * xLenFloor / 16, 0, 4 * zLenFloor / 10), Room = 2 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(7 * xLenFloor / 16, 0, 4 * zLenFloor / 10), Room = 2 });

            //Luz
            Lights.Add(new Light { Position = new Vector3(11 * xLenFloor / 32, lightY, 3 * zLenFloor / 10), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            #endregion

            #region Sala D
            // Puerta +X de sala D
            WallCollidable PuertaPXsalaD1 = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 8, Y, 5 * zLenFloor / 20), Vector3.UnitX, zLenFloor / 10, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood);
            WallCollidable PuertaPXsalaD2 = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 8, Y, 9 * zLenFloor / 20), Vector3.UnitX, zLenFloor / 10, yLenWall, thickness, TextureWood, TextureBooks, new Vector2(2, 1), TextureWood);
            // Puerta
            WallCollidable PuertaSalaD = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 8, 5 * yLenWall / 6, 7 * zLenFloor / 20), Vector3.UnitX, zLenFloor / 10, yLenWall / 3, thickness, TextureWood, TextureBooks, new Vector2(2, 0.33f), TextureWood);
            
            // Agrego Paredes
            Walls.Add(PuertaPXsalaD1);
            Walls.Add(PuertaPXsalaD2);
            Walls.Add(PuertaSalaD);
            Rooms[3].Add(PuertaPXsalaD1);
            Rooms[3].Add(PuertaPXsalaD2);
            Rooms[3].Add(PuertaSalaD);

            // Mesas
            Rooms[3].Add(AddTable(new Vector2(-5 * xLenFloor / 16, 5 * zLenFloor / 20)));
            Rooms[3].Add(AddTable(new Vector2(-5 * xLenFloor / 16, 9 * zLenFloor / 20)));

            // Pos Recolectables
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-7 * xLenFloor / 16, 0, 5 * zLenFloor / 20), Room = 3 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-7 * xLenFloor / 16, 0, 9 * zLenFloor / 20), Room = 3 });

            //Luz
            Lights.Add(new Light { Position = new Vector3(-5 * xLenFloor / 16, lightY, 7 * zLenFloor / 20), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            #endregion

            enemyPath.Add(new PathTrace { posicion = new Vector2(-5 * xLenFloor / 16, 7 * zLenFloor / 20), normal = Vector3.UnitX }); //1
            enemyPath.Add(new PathTrace { posicion = new Vector2(3* xLenFloor / 32 , 7 * zLenFloor / 20), normal = -Vector3.UnitZ });//2
            enemyPath.Add(new PathTrace { posicion = new Vector2(3 * xLenFloor / 32, 0), normal = Vector3.UnitX });//3
            enemyPath.Add(new PathTrace { posicion = new Vector2(7 * xLenFloor / 16, 0), normal = -Vector3.UnitZ });//4
            enemyPath.Add(new PathTrace { posicion = new Vector2(7 * xLenFloor / 16, -zLenFloor / 2 * 0.4f), normal = -Vector3.UnitX });//5
            enemyPath.Add(new PathTrace { posicion = new Vector2(1 * xLenFloor / 16, -zLenFloor / 2 * 0.4f), normal = -Vector3.UnitZ });//6
            enemyPath.Add(new PathTrace { posicion = new Vector2(1 * xLenFloor / 16, -zLenFloor / 2 * 0.9f), normal = -Vector3.UnitX});//7
            enemyPath.Add(new PathTrace { posicion = new Vector2(-5 * xLenFloor / 20, -zLenFloor / 2 * 0.9f), normal = Vector3.UnitZ });//8
            enemyPath.Add(new PathTrace { posicion = new Vector2(-5 * xLenFloor / 20, 0), normal = Vector3.UnitZ });//9

            Enemigo enemy = new Enemigo(enemyPath);
            Rooms[3].Add(enemy);
            Enemigos.Add(enemy);

            Player.Instance.Move(new Vector3(-5 * xLenFloor / 20, Player.Instance.Position.Y, zLenFloor / 8));
            //Player.Instance.Move(new Vector3(enemyPath[0].posicion.X-100, Player.Instance.Position.Y, enemyPath[0].posicion.Y));

            generarRecolectablesRandom();

            SoundManager.Instance.comenzarMusica(SoundManager.Musica.LibraryStage);

            base.LoadContent();

            rl.LoadContentRocket(Content, GraphicsDevice, Effect);

            Boxes.Add(rl.Rocket.Aabb);
        }
        public override void Update(GameTime gameTime)
        {
            foreach (ModelCollidable t in Tables)
            {
                t.SetCameraPos(Player.Instance.GetCameraPos());
            }
            base.Update(gameTime);
        }

        #region metodoDraw
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (ModelCollidable t in Tables)
            {
                if (Player.Camera.InView(t.Aabb))
                    t.Draw(View, Projection);
            }
            foreach (QuadPrimitiveCollidable q in Quads)
            {
                if (Player.Camera.InView(q.aabb))
                    q.Draw(View, Projection);
            }
            foreach (WallCollidable w in Walls)
            {
                if (w.InView(Player.Camera))
                    w.Draw(View, Projection);
            }
            rl.Rocket.Draw(View, Projection);
        }
        #endregion

        private ModelCollidable AddTable(Vector2 Position)
        {
            Matrix World = Matrix.CreateScale(1f) * Matrix.CreateTranslation(new Vector3(Position.X, 5, Position.Y));
            ModelCollidable table = new ModelCollidable(GraphicsDevice, Content, FPSManager.ContentFolder3D + "MobiusDesk/mobius_desk", World);

            // Carga de textura
            table.SetEffect(Effect);
            table.SetLightParameters(.3f,.3f,.4f,100f);
            table.SetTexture(Content.Load<Texture2D>(FPSManager.ContentFolder3D + "MobiusDesk/mobius_desk_tex"));

            // Ajuste de AABB
            float xOffset = 60;
            table.Aabb.SetManually(new Vector3(Position.X - xOffset, 0, Position.Y - 10), new Vector3(Position.X + xOffset, 60, Position.Y + 40));

            Boxes.Add(table.Aabb);

            Collision.Instance.AppendStatic(table.Aabb);

            Tables.Add(table);
            return table;
        }

        public void UbicarObjetos(IList<GameComponent> componentes)
        {
            throw new NotImplementedException();
        }
    }
}
