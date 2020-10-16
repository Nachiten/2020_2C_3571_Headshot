using System;
using System.Diagnostics;
using System.Collections.Generic;
using jorge = System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP
{
    public class Enemigo
    {
        private const string ContentFolder3D = "Models/";

        private Vector3 posicion;

        private Matrix World { get; set; }

        private ModelCollidable ModeloTgcitoClassic { get; set; }

        private Matrix Rotacion;

        private double tiempo = 0;


        public Enemigo(Vector3 posicion)
        {
            this.posicion = posicion;
            World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(1f) * Matrix.CreateTranslation(posicion);
        }

        //private Vector3 posicionInicial;
        private Vector3 mirandoInicial = new Vector3(0,0,-1);
        float velocidadMovimiento = 2;
        Vector3 posicionObjetivo = Vector3.Zero;
        Vector3 vectorDireccion = Vector3.Zero;

        float anguloRotacionRadianes = 0;
        Vector3 OldPosition = Vector3.Zero;

        public void Update(GameTime gameTime, Vector3 posicionCamara)
        {
            if (!Config.enemigosFollowActivado) return;

            // Tiempo total desde el comienzo del juego
            //tiempo += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Calculo la posicion a la que me voy a mover
            posicionObjetivo = new Vector3(posicionCamara.X, 50, posicionCamara.Z);

            float distanciaAlObjetivo = Vector3.Distance(posicion, posicionObjetivo);
            // Si la distancia es menor a un margen comienzo a moverme
            if (distanciaAlObjetivo < 200 && distanciaAlObjetivo > 50) {
                //Guardo la posición anterior
                OldPosition = posicion;

                vectorDireccion = Vector3.Normalize(posicionObjetivo - posicion);

                // Establezco el giro al inicial
                World *= Matrix.CreateRotationY(-anguloRotacionRadianes);

                // Calculo angulo de rotacion entre el robot y el objetivo
                anguloRotacionRadianes = (float)Math.Acos(Vector3.Dot(vectorDireccion, mirandoInicial)
                    / (Vector3.Distance(vectorDireccion, Vector3.Zero) * Vector3.Distance(mirandoInicial, Vector3.Zero)));

                //Debug.WriteLine("Dot product: " + Vector3.Dot(vectorDireccion, mirandoInicial));
                //Debug.WriteLine("Angulo rotacion: " + anguloRotacionRadianes);

                // Si posX del objetivo es mayor a posX actual => * -1
                // Si el objetivo está en el tercer o cuarto cuadrante (angulo > 180) entonces debo invertir el angulo
                if (posicionObjetivo.X > posicion.X)
                {
                    anguloRotacionRadianes *= -1;
                }

                // Aplico la rotacion que corresponde
                World = Matrix.CreateScale(1f) * Matrix.CreateRotationY(MathHelper.Pi + anguloRotacionRadianes);
                posicion = posicion + (vectorDireccion * velocidadMovimiento);
                
                // Muevo el modelo y chequeo si colisiono con algo
                ModeloTgcitoClassic.Transform(World * Matrix.CreateTranslation(posicion),true);
                Collision.Instance.CheckStatic(ModeloTgcitoClassic.Aabb, CollisionCallback);

            }
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ModeloTgcitoClassic = new ModelCollidable(Content, ContentFolder3D + "tgcito-classic/tgcito-classic", World);
            Collision.Instance.AppendStatic(ModeloTgcitoClassic.Aabb);

            var modelEffectArmor = (BasicEffect)ModeloTgcitoClassic.Model.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = Color.White.ToVector3();
            modelEffectArmor.EnableDefaultLighting();
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo en las coordenadas actuales
            ModeloTgcitoClassic.Draw(view, projection);

        }
        private int CollisionCallback(AABB a, AABB b)
        {
            //TODO: Handle Collision
            posicion = OldPosition;
            ModeloTgcitoClassic.Transform(World * Matrix.CreateTranslation(posicion), true);
            return 0;
        }
    }
}
