using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    class Pistol : AWeaponRecolectable
    {
        public Pistol(Vector3 posicionModelo)
        {
            pathModelo = "weapons/Pistol_Beretta/Beretta Pistol";
            tamanioModelo = 0.05f;

            //tamanioModelo = 0.03f;
            //pathModelo = "weapons/Pistol/Pistol";

            //modelColor = Color.Gray.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;
            //matrizOffsetPosicion = Matrix.CreateTranslation(new Vector3(0, 0, 20));
            Scalable = true;
            Damage = 20;
            Index = 3;
        }

        // Hago override ya que debo agregar logica extra
        public override void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Ejecuto la logica compartida (sigue siendo necesaria)
            base.LoadContent(Content, GraphicsDevice);

            // -- Agrego logica extra --

            /*// Mesh silenciador
            var modelEffect = (BasicEffect)Modelo.Model.Meshes[0].Effects[0];
            modelEffect.TextureEnabled = true;
            modelEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
            modelEffect.EnableDefaultLighting();

            // Mesh Arma
            var modelEffect2 = (BasicEffect)Modelo.Model.Meshes[1].Effects[0];
            modelEffect2.EnableDefaultLighting();
            modelEffect2.TextureEnabled = true;
            modelEffect2.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");*/
        }
    }
}
