using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.FPS;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP
{
    public abstract class AWeaponRecolectable : ARecolectable
    {
        public bool Scalable = false;
        public int Damage;
        public override void recolectar(AStage Stage)
        {
            // Todas las armas recolectables tienen la misma logica
            Player.Instance.AgarrarArma(new Weapon(this));

            eliminarRecolectableDeLista(Stage);
        }
    }
}
