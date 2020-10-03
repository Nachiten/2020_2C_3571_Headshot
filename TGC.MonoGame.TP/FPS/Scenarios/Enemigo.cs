using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP
{
    class Enemigo
    {
        private const string ContentFolder3D = "Models/";

        private Vector3 posicion;

        private Matrix World { get; set; }

        private Model ModeloTgcitoClassic { get; set; }

        private Matrix Rotacion;

        private double tiempo = 0;

        public Enemigo(Vector3 posicion)
        {
            this.posicion = posicion;
            World = Matrix.CreateRotationY(MathHelper.Pi);
        }

        //private double tiempoInicialMovimiento;
        //private double tiempoActualMovimiento;
        private Vector3 posicionInicial;

        private bool moverse = false;
        float velocidadMovimiento = 5;
        Vector3 posicionObjetivo = Vector3.Zero;
        Vector3 vectorDireccion = Vector3.Zero;

        public void Update(GameTime gameTime, Vector3 posicionCamara)
        {
            // Tiempo total desde el comienzo del juego
            tiempo += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            Debug.WriteLine("Tiempo actual tota:" + tiempo);

            // Testing de animacion
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                if (!moverse) { 
                    moverse = true;
                    // Fijo el tiempo inicial y el tiempo total, mas la posicion inicial del moviemiento
                    //tiempoInicialMovimiento = tiempo;
                    posicionInicial = posicion;

                    posicionObjetivo = new Vector3(posicionCamara.X, 50, posicionCamara.Z);
                    vectorDireccion = Vector3.Normalize(posicionObjetivo - posicion);
                    
                    //World *= Matrix.CreateRotationY();
                }
            }

            if (moverse) { 

                posicion = posicion + (vectorDireccion * velocidadMovimiento);

                if (Vector3.Distance(posicion, posicionObjetivo) < 50)
                {
                    moverse = false;
                    Debug.WriteLine("Termino de moverme");
                }

                Debug.WriteLine("Inicio movimiento de velocidad [" + velocidadMovimiento + "] unidadesPorSegundo desde la posicion [" + posicion.X + ", " + posicion.Y + ", " + posicion.Z + "] hasta la posicion [" + posicionObjetivo.X + ", " + posicionObjetivo.Y + ", " + posicionObjetivo.Z + "]");

            }
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ModeloTgcitoClassic = Content.Load<Model>(ContentFolder3D + "tgcito-classic/tgcito-classic");

            var modelEffectArmor = (BasicEffect)ModeloTgcitoClassic.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = Color.White.ToVector3();
            modelEffectArmor.EnableDefaultLighting();
        }

        public void moverHacia(Vector3 posicionObjetivo, float velocidadMovimiento) {
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo en las coordenadas actuales
            ModeloTgcitoClassic.Draw(World * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(posicion.X, posicion.Y, posicion.Z), view, projection);

        }
    }
}
