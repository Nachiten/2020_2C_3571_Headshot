using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    enum TipoRecolectable
    {
        vida,
        armor,
    }

    class Recolectable
    {
        public const string ContentFolder3D = "Models/";

        private Model ModeloVida { get; set; }
        private Model ModeloArmor { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }

        // armorOVida
        private TipoRecolectable tipoRecolectable = TipoRecolectable.vida;

        // Coords
        private Vector3 posicion;

        // Recolectado
        private bool recolectado = false;

        public Recolectable(Vector3 posicion, TipoRecolectable tipoRecolectable) {
            this.posicion = posicion;
            this.tipoRecolectable = tipoRecolectable;
            World = Matrix.CreateRotationY(MathHelper.Pi);
        }

        public void Update(GameTime gameTime) {
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 0.7f;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            ModeloVida = Content.Load<Model>(ContentFolder3D + "healthAndArmor/corazon");
            ModeloArmor = Content.Load<Model>(ContentFolder3D + "healthAndArmor/cascoConvertido");

            var modelEffectArmor = (BasicEffect)ModeloArmor.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = Color.DarkGreen.ToVector3();
            modelEffectArmor.EnableDefaultLighting();

            var modelEffectVida = (BasicEffect)ModeloVida.Meshes[0].Effects[0];
            modelEffectVida.DiffuseColor = Color.Red.ToVector3();
            modelEffectVida.EnableDefaultLighting();
        }

        public void Draw(Matrix view, Matrix projection) {
            // dibujo dependiendo de que es en las coords que le pase

            switch (tipoRecolectable) {
                case TipoRecolectable.armor:
                    dibujarArmorEn(posicion.X, posicion.Y, posicion.Z, view, projection);
                    break;
                case TipoRecolectable.vida:
                    dibujarVidaEn(posicion.X, posicion.Y, posicion.Z, view, projection);
                    break;
            }

            // Si ya esta recolectado esto no se llama (pq el objeto se eliminó de la lista)
        }

        private void dibujarVidaEn(float posX, float posY, float posZ, Matrix view, Matrix projection) {
            ModeloVida.Draw(World * Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posX, posY, posZ), view, projection);
        }

        private void dibujarArmorEn(float posX, float posY, float posZ, Matrix view, Matrix projection) {
            ModeloArmor.Draw(World * Matrix.CreateScale(0.7f) * Matrix.CreateTranslation(posX, posY, posZ), view, projection);
        }
    }
}
