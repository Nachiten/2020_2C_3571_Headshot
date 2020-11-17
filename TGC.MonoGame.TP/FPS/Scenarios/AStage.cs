using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TGC.MonoGame.TP.FPS.Scenarios
{
    public struct PathTrace {
        public Vector2 posicion;
        public Vector3 normal;
    }
    public struct Light
    {
        public Vector3 Position;
        public Color AmbientColor;
        public Color DiffuseColor;
        public Color SpecularColor;

    }
    public struct RecolectablePosition
    {
        public Vector3 Position;
        public int Room;
    }
    public abstract class AStage
    {
        public Game Game;
        public GraphicsDevice GraphicsDevice;
        public ContentManager Content;
        public LinePrimitive XAxis;
        public LinePrimitive YAxis;
        public LinePrimitive ZAxis;
        public List<ARecolectable> Recolectables = new List<ARecolectable>();
        protected List<List<IElementEffect>> Rooms = new List<List<IElementEffect>>();
        public List<AABB> Boxes = new List<AABB>();
        public List<Light> Lights = new List<Light>();
        public List<ModelCollidable> Lamps = new List<ModelCollidable>();
        protected List<QuadPrimitiveCollidable> Quads = new List<QuadPrimitiveCollidable>();
        protected List<WallCollidable> Walls = new List<WallCollidable>();
        protected Effect Effect;

        protected int cantidadCorazonesRandom = 4;
        protected int cantidadArmorRandom = 2;
        protected int cantidadEnemigos = 2;

        protected int EnemyDeadScore = 100;

        protected List<PathTrace> enemyPath = new List<PathTrace>();
        protected List<RecolectablePosition> posicionesPosiblesRecolectables = new List<RecolectablePosition>();
        public List<Enemigo> Enemigos = new List<Enemigo>();
        public Matrix View;
        public Matrix Projection;
        public AStage(Game game)
        {
            Game = game;
            GraphicsDevice = Game.GraphicsDevice;
            Content = Game.Content;

            float AxisLenght = 50;

            XAxis = new LinePrimitive(GraphicsDevice, new Vector3(0, 2, 0), new Vector3(AxisLenght, 2, 0));
            YAxis = new LinePrimitive(GraphicsDevice, new Vector3(0, 2, 0), new Vector3(0, AxisLenght + 2, 0));
            ZAxis = new LinePrimitive(GraphicsDevice, new Vector3(0, 2, 0), new Vector3(0, 2, AxisLenght * 2));
        }
        public void RemoveRecolectable(ARecolectable R)
        {
            Collision.Instance.RemoveCollectable(R);
            Recolectables.Remove(R);
        }
        public virtual void LoadContent()
        {
            foreach (ARecolectable R in Recolectables)
            {
                R.SetEffect(Effect);
                R.LoadContent(Content, GraphicsDevice);
            }
            foreach (Enemigo unEnemigo in Enemigos)
            {
                unEnemigo.SetEffect(Effect);
                unEnemigo.LoadContent(Content, GraphicsDevice);
            }

            foreach (WallCollidable w in Walls)
            {
                w.SetEffect(Effect);
                w.SetLightParameters(.2f, .8f, 0f, 1f);
            }
            foreach (QuadPrimitiveCollidable q in Quads)
            {
                q.SetEffect(Effect);
                q.SetLightParameters(.2f, .8f, 0f, 1f);
            }

            AddLamps();

            // Agrego Luces
            for (int i = 0; i < Rooms.Count; i++)
                foreach (var e in Rooms[i])
                    e.SetLight(Lights[i]);
        }
        private void AddLamps()
        {
            foreach(var l in Lights)
            {
                Matrix World = Matrix.CreateScale(.1f) * Matrix.CreateTranslation(l.Position);
                ModelCollidable lamp = new ModelCollidable(GraphicsDevice, Content, FPSManager.ContentFolder3D + "Light/lamp", World);
                Lamps.Add(lamp);
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (ARecolectable R in Recolectables)
            {
                R.Update(gameTime);
            }

            foreach (Enemigo unEnemigo in Enemigos)
            {
                if (unEnemigo.IsDead())
                {
                    Player.Instance.AddScore(EnemyDeadScore);
                    unEnemigo.TriggerDead = true;
                    unEnemigo.Revivir();
                }
            }

            foreach (Enemigo unEnemigo in Enemigos)
            {
                var position = ((TGCGame)Game).Camera.Position;
                unEnemigo.Update(gameTime, position);
            }

            foreach (WallCollidable w in Walls)
            {
                w.SetCameraPos(Player.Instance.GetCameraPos());
            }
            foreach (QuadPrimitiveCollidable q in Quads)
            {
                q.SetCameraPos(Player.Instance.GetCameraPos());
            }
        }
        public virtual void Draw(GameTime gameTime)
        {
            View = ((TGCGame)Game).Camera.View;
            Projection = ((TGCGame)Game).Camera.Projection;
            foreach (ARecolectable R in Recolectables)
            {
                R.Draw(View, Projection);
            }

            foreach (Enemigo unEnemigo in Enemigos)
            {
                unEnemigo.Draw(View, Projection);
            }
            foreach (var l in Lamps)
            {
                l.Draw(View, Projection);
            }

            if (Config.drawAxis)
            {
                XAxis.Draw(View, Projection);
                YAxis.Draw(View, Projection);
                ZAxis.Draw(View, Projection);
            }

            DrawAABBs();
        }
        public void DrawAABBs()
        {
            if (Config.drawAABB)
            {
                RasterizerState rasterizerStateLines = new RasterizerState();
                rasterizerStateLines.FillMode = FillMode.WireFrame;
                rasterizerStateLines.CullMode = CullMode.None;
                GraphicsDevice.RasterizerState = rasterizerStateLines;

                BoxPrimitive AABBDraw = new BoxPrimitive(GraphicsDevice, Vector3.One);
                var scaleFactor = 3;

                foreach (ARecolectable R in Recolectables)
                {
                    R.Modelo.World.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);
                    AABBDraw.Draw(Matrix.CreateScale(R.Modelo.Aabb.size.X * scaleFactor, R.Modelo.Aabb.size.Y * scaleFactor, R.Modelo.Aabb.size.Z * scaleFactor) * Matrix.CreateTranslation(translation), View, Projection);
                }
                foreach (Enemigo unEnemigo in Enemigos)
                {
                    AABBDraw.Draw(Matrix.CreateScale(unEnemigo.Model.Aabb.size.X * scaleFactor, unEnemigo.Model.Aabb.size.Y * scaleFactor, unEnemigo.Model.Aabb.size.Z * scaleFactor) * Matrix.CreateTranslation(unEnemigo.Model.Aabb.Position), View, Projection);
                }
                foreach (AABB aabb in Boxes)
                {
                    AABBDraw.Draw(Matrix.CreateScale(aabb.size.X * scaleFactor, aabb.size.Y * scaleFactor, aabb.size.Z * scaleFactor) * Matrix.CreateTranslation(aabb.Position), View, Projection);
                }

                var rasterizerStateSolid = new RasterizerState();
                rasterizerStateSolid.CullMode = CullMode.None;
                GraphicsDevice.RasterizerState = rasterizerStateSolid;
            }
        }
        protected void generarRecolectablesRandom()
        {
            // Test: Genero corazones en TODAS las posiciones para probar que esten bien
            // Si se descomenta esto comentar todo el resto del metodo
            //for (int i = 0; i < posicionesPosiblesRecolectables.Count; i++)
            //{
            //    Vector3 vectorActual = posicionesPosiblesRecolectables[i];

            //    Recolectables.Add(new Health(new Vector3(vectorActual.X, 55, vectorActual.Z)));
            //}

            // Genero valores random que necesito
            List<int> valoresRandom = generarValoresRandom(cantidadArmorRandom + cantidadCorazonesRandom, posicionesPosiblesRecolectables.Count);

            int i;

            // Itero para generar los Armor en posiciones random ya definidas
            for (i = 0; i < cantidadArmorRandom; i++)
            {
                int index = valoresRandom[i];
                Vector3 vectorActual = posicionesPosiblesRecolectables[index].Position;
                ARecolectable armor = new Armor(new Vector3(vectorActual.X, -45, vectorActual.Z));
                Rooms[posicionesPosiblesRecolectables[index].Room].Add(armor);
                Recolectables.Add(armor);
            }

            // Itero para generar los Health en posiciones random ya definidas
            for (int j = i; j < cantidadCorazonesRandom + cantidadArmorRandom; j++)
            {
                int index = valoresRandom[j];
                Vector3 vectorActual = posicionesPosiblesRecolectables[index].Position;
                ARecolectable health = new Health(new Vector3(vectorActual.X, 55, vectorActual.Z));
                Rooms[posicionesPosiblesRecolectables[index].Room].Add(health);
                Recolectables.Add(health);
            }
        }
        private List<int> generarValoresRandom(int cantidad, int hasta)
        {

            List<int> numerosRandom = new List<int>();

            Random rnd = new Random();

            // Itero para agregar elementos (no repetidos) a la lista de numeros random
            while (numerosRandom.Count < cantidad)
            {
                int numRandom = rnd.Next(1, hasta);
                // Si el numero no existia en la lista lo agrego
                if (!numerosRandom.Exists(unNum => unNum == numRandom))
                {
                    numerosRandom.Add(numRandom);
                }
            }
            return numerosRandom;
        }
    }
}
