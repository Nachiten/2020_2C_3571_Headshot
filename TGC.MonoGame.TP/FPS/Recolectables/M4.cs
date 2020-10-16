using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    class M4 : AWeaponRecolectable
    {
        public M4(Vector3 posicionModelo)
        {
            pathModelo = "weapons/fbx/m4a1_s";
            tamanioModelo = 2;
            modelColor = Color.Gray.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;
            matrizOffsetPosicion = Matrix.CreateTranslation(new Vector3(0,0,20));
        }

        // Hago override ya que debo agregar logica extra
        public override void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Ejecuto la logica compartida (sigue siendo necesaria)
            base.LoadContent(Content, GraphicsDevice);

            // -- Agrego logica extra --

            // Mesh silenciador
            var modelEffect = (BasicEffect)Modelo.Model.Meshes[0].Effects[0];
            modelEffect.TextureEnabled = true;
            modelEffect.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
            modelEffect.EnableDefaultLighting();

            // Mesh Arma
            var modelEffect2 = (BasicEffect)Modelo.Model.Meshes[1].Effects[0];
            modelEffect2.EnableDefaultLighting();
            modelEffect2.TextureEnabled = true;
            modelEffect2.Texture = Content.Load<Texture2D>(ContentFolder3D + "weapons/fbx/noodas");
        }
    }
}
