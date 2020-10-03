using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.FPS.GameModel
{
    public class SpeedBoost : IBoost
    {
        public void BoostPlayer(Player player)
        {
            //es complicado lo de la velocidad porque deberia reducirle la velocidad en un tiempo x.
            player.Speed += 10;
        }
    }
}
