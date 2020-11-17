using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP
{
    class MP44 : AWeaponRecolectable
    {
        public MP44(Vector3 posicionModelo)
        {
            pathModelo = "MP44/MP44";
            tamanioModelo = 2;
            //modelColor = Color.Gray.ToVector3();

            Damage = 40;

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;
            //matrizOffsetPosicion = Matrix.CreateTranslation(new Vector3(0,0,20));
            Index = 2;
        }

        // Hago override ya que debo agregar logica extra
        public override void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            base.LoadContent(Content, GraphicsDevice);
            Modelo.SetLightParameters(.45f, .5f, .05f, 100f);
            Modelo.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "MP44/textures/MP44_albedo"));
        }
    }
}
