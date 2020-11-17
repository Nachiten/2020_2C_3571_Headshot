using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Scenarios;


namespace TGC.MonoGame.TP
{
    class Health : ARecolectable
    {
        private int vidaSumada = 20;
        public Health(Vector3 posicionModelo)
        {
            pathModelo = "FirstAid/FirstAidBox";
            tamanioModelo = 1f;
            //modelColor = Color.Green.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(Vector3.UnitZ*20);
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
        public override void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Ejecuto la logica compartida (sigue siendo necesaria)
            base.LoadContent(Content, GraphicsDevice);

            // -- Agrego logica extra --
            Modelo.SetLightParameters(.2f, .4f, .4f, 100f);
            Modelo.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "FirstAid/FirstAidBox_tex"));
        }
    }
}
