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
    public abstract class AStage
    {
        public Game Game;
        public GraphicsDevice GraphicsDevice;
        public ContentManager Content;
        public LinePrimitive XAxis;
        public LinePrimitive YAxis;
        public LinePrimitive ZAxis;
        public List<ARecolectable> Recolectables = new List<ARecolectable>();

        protected int cantidadCorazonesRandom = 4;
        protected int cantidadArmorRandom = 4;

        protected List<Vector3> posicionesPosiblesRecolectables = new List<Vector3>();
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
                R.LoadContent(Content, GraphicsDevice);
            }
            foreach (Enemigo unEnemigo in Enemigos)
            {
                unEnemigo.LoadContent(Content, GraphicsDevice);
                Collision.Instance.AppendShootable(unEnemigo);
            }
        }

        //public abstract void UbicarObjetos(IList<GameComponent> componentes); 
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
                    Collision.Instance.RemoveShootable(unEnemigo);
                    Collision.Instance.RemoveStatic(unEnemigo.Aabb);
                }
            }

            Enemigos = Enemigos.Where(x => !x.IsDead()).ToList();

            foreach (Enemigo unEnemigo in Enemigos)
            {
                var position = ((TGCGame)Game).Camera.Position;
                unEnemigo.Update(gameTime, position);
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

            if (Config.drawAxis)
            {
                XAxis.Draw(View, Projection);
                YAxis.Draw(View, Projection);
                ZAxis.Draw(View, Projection);
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
            List<int> valoresRandom = generarValoresRandom(cantidadArmorRandom + cantidadCorazonesRandom);

            int i;

            // Itero para generar los Armor en posiciones random ya definidas
            for (i = 0; i < cantidadArmorRandom; i++)
            {
                int index = valoresRandom[i];
                Vector3 vectorActual = posicionesPosiblesRecolectables[index];

                Recolectables.Add(new Armor(new Vector3(vectorActual.X, -45, vectorActual.Z)));
            }

            // Itero para generar los Health en posiciones random ya definidas
            for (int j = i; j < cantidadCorazonesRandom + cantidadArmorRandom; j++)
            {
                int index = valoresRandom[j];
                Vector3 vectorActual = posicionesPosiblesRecolectables[index];

                Recolectables.Add(new Health(new Vector3(vectorActual.X, 55, vectorActual.Z)));
            }
        }
        private List<int> generarValoresRandom(int cantidad)
        {

            List<int> numerosRandom = new List<int>();

            Random rnd = new Random();

            // Itero para agregar elementos (no repetidos) a la lista de numeros random
            while (numerosRandom.Count < cantidad)
            {
                int numRandom = rnd.Next(1, posicionesPosiblesRecolectables.Count);
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
