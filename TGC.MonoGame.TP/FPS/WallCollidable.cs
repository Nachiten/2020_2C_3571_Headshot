using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP.FPS
{
    public class WallCollidable : IElementEffect
    {
        QuadPrimitiveCollidable Front;
        QuadPrimitiveCollidable Back;
        QuadPrimitiveCollidable Left;
        QuadPrimitiveCollidable Right;
        QuadPrimitiveCollidable Bottom;
        public WallCollidable(GraphicsDevice gd, Vector3 Origin, Vector3 Normal, float Width, float Height, float Thickness, Texture2D TextureFront, Texture2D TextureBack, Vector2 TextureRepeats, Texture2D TextureSide)
        {
            Front = new QuadPrimitiveCollidable(gd, Origin, Normal, Vector3.UnitY, Width, Height, TextureFront, TextureRepeats);
            Back = new QuadPrimitiveCollidable(gd, Origin - Thickness * Normal, -Normal, Vector3.UnitY, Width, Height, TextureBack, TextureRepeats);

            Vector3 SideOrigin = (Origin - Thickness * Normal / 2);
            Vector3 SideNormal = Vector3.Cross(Normal,-Vector3.UnitY);
            Left = new QuadPrimitiveCollidable(gd, SideOrigin + Width/2 * SideNormal, SideNormal, Vector3.UnitY, Thickness, Height, TextureSide, new Vector2(1, Height / 100));
            Right = new QuadPrimitiveCollidable(gd, SideOrigin - Width/2 * SideNormal, -SideNormal, Vector3.UnitY, Thickness, Height, TextureSide, new Vector2(1, Height / 100));
            Bottom = new QuadPrimitiveCollidable(gd, SideOrigin - Height/2 * Vector3.UnitY, -Vector3.UnitY, Normal, Width, Thickness, TextureSide, new Vector2(1, 1 / Thickness));
        }
        public void Draw(Matrix view, Matrix projection)
        {
            Front.Draw(view, projection);
            Back.Draw(view, projection);
            Left.Draw(view, projection);
            Right.Draw(view, projection);
            Bottom.Draw(view, projection);
        }
        public void SetEffect(Effect Effect)
        {
            Front.SetEffect(Effect);
            Back.SetEffect(Effect);
            Left.SetEffect(Effect);
            Right.SetEffect(Effect);
            Bottom.SetEffect(Effect);
        }
        public void SetCameraPos(Vector3 pos)
        {
            Front.SetCameraPos(pos);
            Back.SetCameraPos(pos);
            Left.SetCameraPos(pos);
            Right.SetCameraPos(pos);
            Bottom.SetCameraPos(pos);
        }
        public void SetLightParameters(float KAmbient, float KDiffuse, float KSpecular, float Shininess)
        {
            Front.SetLightParameters(KAmbient, KDiffuse, KSpecular, Shininess);
            Back.SetLightParameters(KAmbient, KDiffuse, KSpecular, Shininess);
            Left.SetLightParameters(KAmbient, KDiffuse, KSpecular, Shininess);
            Right.SetLightParameters(KAmbient, KDiffuse, KSpecular, Shininess);
            Bottom.SetLightParameters(KAmbient, KDiffuse, KSpecular, Shininess); 
        }
        public void SetLight(Light Light)
        {
            Front.SetLight(Light);
            Back.SetLight(Light);
            Left.SetLight(Light);
            Right.SetLight(Light);
            Bottom.SetLight(Light);
        }
    }
}
