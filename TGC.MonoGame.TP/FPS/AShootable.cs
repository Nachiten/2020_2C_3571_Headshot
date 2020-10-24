using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.FPS
{
    public abstract class Ashootable
    {
        public AABB Aabb;
        public abstract void GetDamaged(int damage);
    }
}
