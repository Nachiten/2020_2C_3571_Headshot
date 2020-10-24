using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP
{
    class Health : ARecolectable
    {
        private int vidaSumada = 20;
        public Health(Vector3 posicionModelo)
        {
            pathModelo = "healthAndArmor/corazon";
            tamanioModelo = 0.25f;
            modelColor = Color.Red.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;
        }

        public override void recolectar(AStage Stage)
        {

            bool sumeVida = Player.Instance.sumarVida(vidaSumada);
            
            if (sumeVida)
            eliminarRecolectableDeLista(Stage);

            //if (Player.Instance.Health > 100) { 
            //}

        }
    }
}
