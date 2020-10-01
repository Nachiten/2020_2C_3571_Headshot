using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    class Enemigo
    {
        private const string ContentFolder3D = "Models/";

        private Vector3 posicion;

        private Matrix World { get; set; }

        private Model ModeloTgcitoClassic { get; set; }

        private bool moviendose = false;

        private float tiempo = 0;

        public Enemigo(Vector3 posicion)
        {
            this.posicion = posicion;
            World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(10, 0, 10);
        }

        public void Update(GameTime gameTime)
        {
            // No es necesario (por ahora)

            tiempo += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ModeloTgcitoClassic = Content.Load<Model>(ContentFolder3D + "tgcito-classic/tgcito-classic");

            var modelEffectArmor = (BasicEffect)ModeloTgcitoClassic.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = Color.White.ToVector3();
            modelEffectArmor.EnableDefaultLighting();
        }

        public void moverHacia(float posX, float posY, float posZ, float segundos) { 

        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo en las coordenadas actuales
            ModeloTgcitoClassic.Draw(World * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(posicion.X, posicion.Y, posicion.Z), view, projection);

        }
    }
}
