using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using System.Diagnostics;

namespace TGC.MonoGame.TP
{
    public enum TipoRecolectable
    {
        vida,
        armor,
    }

    public class Recolectable
    {
        public const string ContentFolder3D = "Models/";
        public ModelCollidable Modelo { get; set; }
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
            Modelo.Turn(World * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posicion));
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            Vector3 modelColor = Vector3.Zero;
            switch (tipoRecolectable)
            {
                case TipoRecolectable.armor:
                    World *= Matrix.CreateScale(0.7f) * Matrix.CreateTranslation(-37, 0, 2);
                    Modelo = new ModelCollidable(Content, ContentFolder3D + "healthAndArmor/armadura", World);
                    modelColor = Color.Gray.ToVector3();
                    break;
                case TipoRecolectable.vida:
                    World *= Matrix.CreateScale(0.15f);
                    Modelo = new ModelCollidable(Content, ContentFolder3D + "healthAndArmor/corazon", World);
                    Modelo.Aabb.Translation(Vector3.UnitY*30);
                    modelColor = Color.Red.ToVector3();
                    break;
                default:
                    throw new Exception("Unknown Recolectable type");
            }

            Collision.Instance.AppendCollectable(this);
            //Debug.WriteLine("Recolectable Bounding Box: " + Modelo.Aabb.minExtents + " - " + Modelo.Aabb.maxExtents);
            
            var modelEffectArmor = (BasicEffect)Modelo.Model.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = modelColor;
            modelEffectArmor.EnableDefaultLighting();
        }

        public void Draw(Matrix view, Matrix projection) {
            // dibujo dependiendo de que es en las coords que le pase

            Modelo.Draw(view, projection);

            /*switch (tipoRecolectable) {
                case TipoRecolectable.armor:
                    dibujarArmorEn(posicion.X, posicion.Y, posicion.Z, view, projection);
                    break;
                case TipoRecolectable.vida:
                    dibujarVidaEn(posicion.X, posicion.Y, posicion.Z, view, projection);
                    break;
            }*/

            // Si ya esta recolectado esto no se llama (pq el objeto se eliminó de la lista)
        }

        /*private void dibujarVidaEn(float posX, float posY, float posZ, Matrix view, Matrix projection) {
            ModeloVida.Draw(World * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posX, posY, posZ), view, projection);
        }

        private void dibujarArmorEn(float posX, float posY, float posZ, Matrix view, Matrix projection) {
            // Corrijo offset del modelo (-37 , 0, 2)
            ModeloArmor.Draw(World * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posX, posY, posZ), view, projection);
        }*/
    }
}
