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
    public class MazeStage : AStage
    {
        float xLenFloor = 2000;
        float zLenFloor = 2000;
        int yLenWall = 150;
        float thickness = 20;
        
        List<ModelCollidable> Columnas = new List<ModelCollidable>();

        Color LightsColor = Color.White;

        public SkyBox SkyBox { get; set; }

        public MazeStage(Game game) : base(game)
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
            #region Recolectables
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-5 * xLenFloor / 32, 0, -1 * zLenFloor / 16), Room = 0 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-11 * xLenFloor / 32, 0, -1 * zLenFloor / 16), Room = 0 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-1 * xLenFloor / 16, 0, -3 * zLenFloor / 16), Room = 0 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-7 * xLenFloor / 16, 0, -3 * zLenFloor / 16), Room = 0 });

            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(5 * xLenFloor / 16, 0, -3 * zLenFloor / 16), Room = 1 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(5 * xLenFloor / 16, 0, -5 * zLenFloor / 16), Room = 1 });

            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(3 * xLenFloor / 16, 0, 3 * zLenFloor / 16), Room = 2 });

            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(3 * xLenFloor / 16, 0, 6 * zLenFloor / 16), Room = 3 });
            posicionesPosiblesRecolectables.Add(new RecolectablePosition { Position = new Vector3(-3 * xLenFloor / 16, 0, 6 * zLenFloor / 16), Room = 3 });
            #endregion

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

            #region Light Positions
            float lightY = 7 * yLenWall / 8;
            Lights.Add(new Light { Position = new Vector3(-4 * xLenFloor / 16, lightY, -15 * zLenFloor / 32), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            Lights.Add(new Light { Position = new Vector3(3 * xLenFloor / 16, lightY, -11 * zLenFloor / 32), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            Lights.Add(new Light { Position = new Vector3(0, lightY, 2 * zLenFloor / 8 - 2 * thickness), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            Lights.Add(new Light { Position = new Vector3(0, lightY, 15 * zLenFloor / 32), AmbientColor = LightsColor, DiffuseColor = LightsColor, SpecularColor = LightsColor });
            #endregion

            generarRecolectablesRandom();

        }

        public override void LoadContent()
        {
            Texture2D TexBush = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "Maze/bush");
            Texture2D TexGrass = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "Maze/grass");

            Player.Instance.Move(new Vector3(-4 * xLenFloor / 16, Player.Instance.Position.Y, -1 * zLenFloor / 16));

            float Y = yLenWall / 2;

            #region Paredes Principales
            QuadPrimitiveCollidable Piso = new QuadPrimitiveCollidable(GraphicsDevice, Vector3.Zero, Vector3.UnitY, Vector3.UnitX, zLenFloor, xLenFloor, TexGrass, new Vector2(4, 4));
            QuadPrimitiveCollidable ParedPX = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(xLenFloor / 2, Y, 0), -Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexBush, new Vector2(10, 1));
            QuadPrimitiveCollidable ParedNX = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(-xLenFloor / 2, Y, 0), Vector3.UnitX, Vector3.UnitY, zLenFloor, yLenWall, TexBush, new Vector2(10, 1));
            QuadPrimitiveCollidable ParedPZ = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, Y, zLenFloor / 2), -Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexBush, new Vector2(10, 1));
            QuadPrimitiveCollidable ParedNZ = new QuadPrimitiveCollidable(GraphicsDevice, new Vector3(0, Y, -zLenFloor / 2), Vector3.UnitZ, Vector3.UnitY, xLenFloor, yLenWall, TexBush, new Vector2(10, 1));
            Quads.Add(Piso);
            Quads.Add(ParedPX);
            Quads.Add(ParedNX);
            Quads.Add(ParedPZ);
            Quads.Add(ParedNZ);
            Rooms[0].Add(Piso);
            Rooms[3].Add(ParedPX);
            Rooms[0].Add(ParedNX);
            Rooms[3].Add(ParedPZ);
            Rooms[0].Add(ParedNZ);
            #endregion

            #region Paredes
            WallCollidable w1 = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, Y, 0), Vector3.UnitZ, xLenFloor / 2, yLenWall, thickness, TexBush, TexBush, new Vector2(10, 1), TexBush);
            WallCollidable w2 = new WallCollidable(GraphicsDevice, new Vector3(0, Y, -3 * zLenFloor / 16), -Vector3.UnitX, 3 * zLenFloor / 8, yLenWall, thickness, TexBush, TexBush, new Vector2(6, 1), TexBush);
            WallCollidable w3 = new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 16, Y, -3 * zLenFloor / 8 - thickness), -Vector3.UnitZ, 3 * xLenFloor / 8, yLenWall, thickness, TexBush, TexBush, new Vector2(6, 1), TexBush);
            WallCollidable w4 = new WallCollidable(GraphicsDevice, new Vector3(3 * xLenFloor / 8, Y, -2 * zLenFloor / 8), Vector3.UnitX, 2 * zLenFloor / 8, yLenWall, thickness, TexBush, TexBush, new Vector2(2, 1), TexBush);
            WallCollidable w5 = new WallCollidable(GraphicsDevice, new Vector3(2 * xLenFloor / 8, Y, -zLenFloor / 8 + thickness), Vector3.UnitZ, 2 * zLenFloor / 8, yLenWall, thickness, TexBush, TexBush, new Vector2(2, 1), TexBush);
            WallCollidable w6 = new WallCollidable(GraphicsDevice, new Vector3(0, Y, 2 * zLenFloor / 8 + thickness), Vector3.UnitZ, 3 * xLenFloor / 4, yLenWall, thickness, TexBush, TexBush, new Vector2(10, 1), TexBush);
            WallCollidable w7 = new WallCollidable(GraphicsDevice, new Vector3(2 * xLenFloor / 8, Y, zLenFloor / 8), Vector3.UnitX, zLenFloor / 4, yLenWall, thickness, TexBush, TexBush, new Vector2(4, 1), TexBush);
            WallCollidable w8 = new WallCollidable(GraphicsDevice, new Vector3(0, Y, 2 * zLenFloor / 8 + 2 * thickness), Vector3.UnitZ, 3 * xLenFloor / 4, yLenWall, thickness, TexBush, TexBush, new Vector2(10, 1), TexBush);
            WallCollidable w9 = new WallCollidable(GraphicsDevice, new Vector3(-xLenFloor / 4, Y, thickness), Vector3.UnitZ, xLenFloor / 2 + 4 * thickness, yLenWall, thickness, TexBush, TexBush, new Vector2(10, 1), TexBush);
            WallCollidable w10 = new WallCollidable(GraphicsDevice, new Vector3(thickness, Y, -3 * zLenFloor / 16), -Vector3.UnitX, 3 * zLenFloor / 8, yLenWall, thickness, TexBush, TexBush, new Vector2(6, 1), TexBush);
            WallCollidable w11 = new WallCollidable(GraphicsDevice, new Vector3(2 * xLenFloor / 8, Y, -zLenFloor / 8 + 2 * thickness), Vector3.UnitZ, 2 * zLenFloor / 8, yLenWall, thickness, TexBush, TexBush, new Vector2(2, 1), TexBush);
            Walls.Add(w1);
            Walls.Add(w2);
            Walls.Add(w3);
            Walls.Add(w4);
            Walls.Add(w5);
            Walls.Add(w6);
            Walls.Add(w7);
            Walls.Add(w8);
            Walls.Add(w9);
            Walls.Add(w10);
            Walls.Add(w11);
            Rooms[0].Add(w1);
            Rooms[0].Add(w2);
            Rooms[1].Add(w3);
            Rooms[1].Add(w4);
            Rooms[1].Add(w5);
            Rooms[2].Add(w6);
            Rooms[2].Add(w7);
            Rooms[3].Add(w8);
            Rooms[2].Add(w9);
            Rooms[1].Add(w10);
            Rooms[2].Add(w11);
            #endregion

            #region Columnas
            // Columnas
            Rooms[2].Add(AddColumn(new Vector2(-7 * xLenFloor / 16, zLenFloor / 16)));
            Rooms[3].Add(AddColumn(new Vector2(-7 * xLenFloor / 16, 7 * zLenFloor / 16)));
            Rooms[3].Add(AddColumn(new Vector2(7 * xLenFloor / 16, 7 * zLenFloor / 16)));

            Rooms[0].Add(AddColumn(new Vector2(-xLenFloor / 16, -zLenFloor / 16)));
            Rooms[0].Add(AddColumn(new Vector2(-7 * xLenFloor / 16, -zLenFloor / 16)));
            Rooms[0].Add(AddColumn(new Vector2(-xLenFloor / 16, -5 * zLenFloor / 16)));
            Rooms[0].Add(AddColumn(new Vector2(-7 * xLenFloor / 16, -5 * zLenFloor / 16)));

            Rooms[1].Add(AddColumn(new Vector2(5 * xLenFloor / 16, -4 * zLenFloor / 16)));
            #endregion

            // Weapons
            ARecolectable m4 = new M4(new Vector3(-5 * xLenFloor / 32, 50, -3 * zLenFloor / 16));
            ARecolectable mp44 = new MP44(new Vector3(-11 * xLenFloor / 32, 50, -3 * zLenFloor / 16));
            // Seteo las luces de sala A
            Rooms[0].Add(m4);
            Rooms[0].Add(mp44);
            // Las agrego a los recolectables
            Recolectables.Add(m4);
            Recolectables.Add(mp44);

            Enemigo enemy = new Enemigo(enemyPath);
            Rooms[3].Add(enemy);
            Enemigos.Add(enemy);

            // Skybox
            var skyBox = Game.Content.Load<Model>("skybox/cube");
            //var skyBoxTexture = Game.Content.Load<TextureCube>(FPSManager.ContentFolderTextures + "/skyboxes/sunset/sunset");
            //var skyBoxTexture = Game.Content.Load<TextureCube>(FPSManager.ContentFolderTextures + "/skyboxes/islands/islands");
            var skyBoxTexture = Game.Content.Load<TextureCube>(FPSManager.ContentFolderTextures + "/skyboxes/skybox/skybox");
            //var skyBoxTexture = Game.Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "/library");
            var skyBoxEffect = Game.Content.Load<Effect>(FPSManager.ContentFolderEffect + "SkyBox");
            SkyBox = new SkyBox(skyBox, skyBoxTexture, skyBoxEffect);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ModelCollidable c in Columnas)
            {
                c.SetCameraPos(Player.Instance.GetCameraPos());
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var samplerstate = GraphicsDevice.SamplerStates[0];
            var depthstencilstate = GraphicsDevice.DepthStencilState;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            SkyBox.Draw(View, Projection, Player.Instance.GetCameraPos());
            GraphicsDevice.SamplerStates[0] = samplerstate;
            GraphicsDevice.DepthStencilState = depthstencilstate;

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

        private ModelCollidable AddColumn(Vector2 Position)
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

            // Carga de textura
            column.SetEffect(Effect);
            column.SetLightParameters(.3f, .3f, .4f, 100f);
            column.SetTexture(Content.Load<Texture2D>(FPSManager.ContentFolderTextures + "cuarzo"));

            Columnas.Add(column);
            return column;
        }
    }



}
