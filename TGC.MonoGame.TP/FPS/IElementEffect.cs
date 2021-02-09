using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP.FPS
{
    public interface IElementEffect
    {
        public void SetLight(Light Light);
        public void SetLightMuzzle(Light Light);
        public void UnsetLightMuzzle();
    }
}
