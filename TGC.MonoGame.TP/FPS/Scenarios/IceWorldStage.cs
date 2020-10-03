using Microsoft.Xna.Framework;
using GraficsModel = Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.Samples.Cameras;

namespace TGC.MonoGame.TP.FPS.Scenarios
{
    public class IceWorldStage : DrawableGameComponent, IStageBuilder
    {
        public GraficsModel.VertexPositionTexture[] floor { get; set; }
        public IceWorldStage(Game game) : base(game)
        {
        }

        public void CrearParedes(int alto)
        {
            throw new NotImplementedException();
        }

        public void CrearPiso(int largo, int ancho)
        {
            Vector3 piso1 = new Vector3(-largo / 2, 0, -ancho / 2);
            Vector3 piso2 = new Vector3(-largo / 2, 0, ancho / 2);
            Vector3 piso3 = new Vector3(largo / 2, 0, -ancho / 2);
            Vector3 piso4 = new Vector3(largo / 2, 0, ancho / 2);


            var textura = new Vector2(5, 10);
            floor = ShapeCreatorHelper.CreatePlane(piso1, piso2, piso3, piso4, textura);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void UbicarObjetos(IList<GameComponent> componentes)
        {
            throw new NotImplementedException();
        }
    }
}
