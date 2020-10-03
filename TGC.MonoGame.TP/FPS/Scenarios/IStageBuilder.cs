using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.FPS.Scenarios
{
    public interface IStageBuilder
    {
        public  void CrearPiso(int largo, int ancho);

        public  void CrearParedes(int alto);

        public  void UbicarObjetos(IList<GameComponent> componentes);

        public void Draw(GameTime gameTime);
    }
}
