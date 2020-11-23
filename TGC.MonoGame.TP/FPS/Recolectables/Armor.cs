using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.FPS.Scenarios;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Interface;

namespace TGC.MonoGame.TP
{
    class Armor : ARecolectable
    {
        private int armorSumada = 60;
        public Armor(Vector3 posicionModelo)
        {
            pathModelo = "Armor/armor";
            tamanioModelo = .25f;
            modelColor = Color.Blue.ToVector3();

            posicion = posicionModelo;
            World = Matrix.CreateRotationY(MathHelper.Pi);
            Rotation = 0;

            //offsetPosicion = new Vector3(66, 110, -8);
            matrizOffsetPosicion = Matrix.CreateTranslation(0, 90, 0);
        }

        public override void recolectar(AStage Stage)
        {
            bool sumeArmor = Player.Instance.sumarArmor(armorSumada);

            if (sumeArmor) 
            {
                SoundManager.Instance.reproducirSonido(3);
                eliminarRecolectableDeLista(Stage);
            }
                
        }
        public override void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Ejecuto la logica compartida (sigue siendo necesaria)
            base.LoadContent(Content, GraphicsDevice);

            // -- Agrego logica extra --
            Modelo.SetLightParameters(.45f, .5f, .05f, 100f);
            Modelo.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "Armor/armor_tex"));
        }
    }
}
