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
        m4,
        cuchillo
    }

    public class RecolectableOLD
    {
        public const string ContentFolder3D = "Models/";
        public ModelCollidable Modelo { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }

        // armorOVida
        public TipoRecolectable tipoRecolectable { get; set; }

        // Coords
        private Vector3 posicion;

        public RecolectableOLD(Vector3 posicion, TipoRecolectable tipoRecolectable) {
            this.posicion = posicion;
            this.tipoRecolectable = tipoRecolectable;
            World = Matrix.CreateRotationY(MathHelper.Pi);
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {

            /*
             * Tamanios:
             * armor: 1
             * vida: 0.25
             * m4: 2
             */

            /*
             * Paths:
             * armor: "healthAndArmor/armadura"
             * vida: "healthAndArmor/corazon"
             * m4: "weapons/fbx/m4a1_s"
             */

            Vector3 modelColor = Vector3.Zero;
            switch (tipoRecolectable)
            {
                case TipoRecolectable.armor:
                    World *= Matrix.CreateScale(1f) * Matrix.CreateTranslation(-52, 0, 2);
                    Modelo = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "healthAndArmor/armadura", World);
                    Modelo.Aabb.Translation(World * Matrix.CreateTranslation(posicion + new Vector3(66,110,-8)));
                    modelColor = Color.Gray.ToVector3();
                    break;
                case TipoRecolectable.vida:
                    World *= Matrix.CreateScale(0.25f);
                    Modelo = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "healthAndArmor/corazon", World);
                    Modelo.Aabb.Translation(World * Matrix.CreateTranslation(posicion));
 
                    modelColor = Color.Red.ToVector3();
                    break;
                case TipoRecolectable.m4:
                    World *= Matrix.CreateScale(2f);
                    Modelo = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "weapons/fbx/m4a1_s", World);
                    Modelo.Aabb.Translation(World * Matrix.CreateTranslation(posicion));

                    var modelEffect = (BasicEffect)Modelo.Model.Meshes[0].Effects[0];
                    modelEffect.TextureEnabled = true;
                    modelEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
                    //modelEffect.EnableDefaultLighting();

                    //Mesh Arma
                    var modelEffect2 = (BasicEffect)Modelo.Model.Meshes[1].Effects[0];
                    modelEffect2.EnableDefaultLighting();
                    modelEffect2.TextureEnabled = true;
                    modelEffect2.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
                    break;

                case TipoRecolectable.cuchillo:
                    World = World * Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateScale(3f);
                    Modelo = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "weapons/knife/Karambit", World);
                    Modelo.Aabb.Translation(World * Matrix.CreateTranslation(posicion));
                    //modelColor = Color.Red.ToVector3();
                    break;
                default:
                    throw new Exception("Unknown Recolectable type");
            }

            //Collision.Instance.AppendCollectable(this);
            //Debug.WriteLine("Recolectable Bounding Box: " + Modelo.Aabb.minExtents + " - " + Modelo.Aabb.maxExtents);
            
            var modelEffectArmor = (BasicEffect)Modelo.Model.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = modelColor;
            modelEffectArmor.EnableDefaultLighting();
        }
        public void Update(GameTime gameTime)
        {
            // Basado en el tiempo que pasa se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 0.7f;
            Modelo.Transform(World * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posicion), false);
        }

        public void Draw(Matrix view, Matrix projection) {
            Modelo.Draw(view, projection);
        }
    }
}
