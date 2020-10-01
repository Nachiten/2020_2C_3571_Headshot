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

        private bool moviendoseX = false;
        private bool moviendoseZ = false;

        private double tiempo = 0;

        public Enemigo(Vector3 posicion)
        {
            this.posicion = posicion;
            World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(10, 0, 10);
        }

        private double tiempoInicialMovimiento;
        private double tiempoTotalMovimientoX;
        private double tiempoTotalMovimientoZ;
        private double tiempoActualMovimiento;

        private float distanciaX;
        private float distanciaZ;

        private Vector3 posicionInicial;

        public void Update(GameTime gameTime, Vector3 posicionCamara)
        {
            // Tiempo total desde el comienzo del juego
            tiempo += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            //Debug.WriteLine("Tiempo actual tota:" + tiempo);

            // using System.Diagnostics;
            //Debug.WriteLine(tiempo);

            // Testing de animacion
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                // Posicion incial: 1,20,200
                moverHacia(posicionCamara.X, 20, posicionCamara.Z, 20, tiempo);

                // 20unidadesXSegundo -> 10 segundos
                // si lo muevo a 0,20,100 -> 5 segundos
            }

            tiempoActualMovimiento = tiempo - tiempoInicialMovimiento;

            if (moviendoseX) 
            {
                // Es una funcion lineal que determina la posicion en base al tiempo y la distancia.
                // (regla de 3) Si tiempoTotal = 2 y distanciaTotal = 20 => desplazamientoX = tiempoX * distanciaTotal / tiempoTotal 
                posicion.X = (float)(posicionInicial.X + tiempoActualMovimiento * distanciaX / tiempoTotalMovimientoX);
                 
                if (tiempoActualMovimiento >= tiempoTotalMovimientoX ) {                               
                    Debug.WriteLine("Finalizó movimiento de " + tiempoActualMovimiento + " segundos");
                    moviendoseX = false;
                }
            }

            if (moviendoseZ)
            {
                posicion.Z = (float)(posicionInicial.Z + tiempoActualMovimiento * distanciaZ / tiempoTotalMovimientoZ);

                if (tiempoActualMovimiento >= tiempoTotalMovimientoZ)
                {
                    moviendoseZ = false;
                }
            }


        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ModeloTgcitoClassic = Content.Load<Model>(ContentFolder3D + "tgcito-classic/tgcito-classic");

            var modelEffectArmor = (BasicEffect)ModeloTgcitoClassic.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = Color.White.ToVector3();
            modelEffectArmor.EnableDefaultLighting();
        }

        public void moverHacia(float posX, float posY, float posZ, float velocidadMovimiento, double gameTime) {
            if (!moviendoseX && !moviendoseZ)
            {
                distanciaX = posX - posicion.X;
                distanciaZ = posZ - posicion.Z;

                if (distanciaX == 0)
                {
                    Debug.WriteLine("No hay movimiento en X!!");
                }
                else {
                    moviendoseX = true;
                }

                if (distanciaZ == 0)
                {
                    Debug.WriteLine("No hay movimiento en Z!!");
                }
                else {
                    moviendoseZ = true;
                }
                
                Debug.WriteLine("distanciaX: " + distanciaX);
                Debug.WriteLine("distanciaZ: " + distanciaZ);

                // Fijo el tiempo inicial y el tiempo total, mas la posicion inicial del moviemiento
                tiempoInicialMovimiento = gameTime;

                tiempoTotalMovimientoX = Math.Abs(distanciaX) / velocidadMovimiento;
                tiempoTotalMovimientoZ = Math.Abs(distanciaZ) / velocidadMovimiento;

                Debug.WriteLine("TiempoTotalMovimientoX: " + tiempoTotalMovimientoX);
                Debug.WriteLine("TiempoTotalMovimientoZ: " + tiempoTotalMovimientoZ);

                posicionInicial = posicion;



                Debug.WriteLine("Inicio movimiento de velocidad [" + velocidadMovimiento + "] unidadesPorSegundo desde la posicion [" + posicion.X + ", " + posicion.Y + ", " + posicion.Z + "] hasta la posicion [" + posX + ", " + posY + ", " + posZ + "]");
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo en las coordenadas actuales
            ModeloTgcitoClassic.Draw(World * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(posicion.X, posicion.Y, posicion.Z), view, projection);

        }
    }
}
