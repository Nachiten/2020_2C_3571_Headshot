using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.FPS.Scenarios
{
    public interface IStage
    {
        public void RemoveRecolectable(ARecolectable R);
        public void CrearEstructura();

        public  void UbicarObjetos(IList<GameComponent> componentes);

        public void Draw(GameTime gameTime);
        public void Update(GameTime gameTime);
    }
}
