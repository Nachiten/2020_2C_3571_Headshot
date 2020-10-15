using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP
{
    class Armor : ARecolectable
    {
        public Armor(Vector3 posicionModelo)
        {
            pathModelo = "healthAndArmor/armadura";
            tamanioModelo = 1;
            modelColor = Color.Gray.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;

            offsetPosicion = new Vector3(66, 110, -8);
            matrizOffsetPosicion = Matrix.CreateTranslation(-52, 0, 2);
        }

        public override void recolectar()
        {
            // TODO | Sumar el amor que corresponde
        }
    }
}
