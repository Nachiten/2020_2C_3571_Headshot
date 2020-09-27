using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.FPS
{
    class Recolectables
    {
        public const string ContentFolder3D = "Models/";

        private Model ModeloVida { get; set; }
        private Model ModeloArmor { get; set; }
        private float Rotation { get; set; }

        private Matrix World { get; set; }

        public Recolectables() {
            World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(10, 0, 10);
        }

        public void Update(GameTime gameTime) {
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            Console.Out.Write("Hola");
            ModeloVida = Content.Load<Model>(ContentFolder3D + "healthAndArmor/corazon");
            //ModeloArmor = Content.Load<Model>(ContentFolder3D + "healthAndArmor/escudo");
        }

        public void dibujarVidaEn(float posX, float posY, float posZ, Matrix view, Matrix projection) {
            ModeloVida.Draw(World * Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posX, posY, posZ), view, projection);
        }

        public void dibujarArmorEn(float posX, float posY, float posZ, Matrix view, Matrix projection) {
            ModeloArmor.Draw(World * Matrix.CreateScale(1) * Matrix.CreateTranslation(posX, posY, posZ), view, projection);
        }
    }
}
