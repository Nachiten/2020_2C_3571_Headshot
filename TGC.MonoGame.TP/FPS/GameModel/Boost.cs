using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.FPS
{
    //interface para agregar un boost a un player
    public interface Boost
    {
        public void BoostPlayer(Player player);
    }
}
