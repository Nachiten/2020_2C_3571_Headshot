using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    class Health : ARecolectable
    {
        public Health(Vector3 posicionModelo)
        {
            pathModelo = "healthAndArmor/corazon";
            tamanioModelo = 0.25f;
            modelColor = Color.Red.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;
        }

        public override void recolectar()
        {
            // TODO | Sumar la vida que corresponde
        }
    }
}
