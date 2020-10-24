using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.Utils;

namespace TGC.MonoGame.TP.FPS
{
    public class WallCollidable
    {
        QuadPrimitiveCollidable Front;
        QuadPrimitiveCollidable Back;
        QuadPrimitiveCollidable Left;
        QuadPrimitiveCollidable Right;
        public WallCollidable(GraphicsDevice gd, Vector3 Origin, Vector3 Normal, float Width, float Height, float Thickness, Texture2D TextureFront, Texture2D TextureBack, Vector2 TextureRepeats, Texture2D TextureSide)
        {
            Front = new QuadPrimitiveCollidable(gd, Origin, Normal, Vector3.UnitY, Width, Height, TextureFront, TextureRepeats);
            Back = new QuadPrimitiveCollidable(gd, Origin - Thickness * Normal, -Normal, Vector3.UnitY, Width, Height, TextureBack, TextureRepeats);

            Vector3 SideOrigin = (Origin - Thickness * Normal / 2);
            Vector3 SideNormal = Vector3.Cross(Normal,-Vector3.UnitY);
            Left = new QuadPrimitiveCollidable(gd, SideOrigin + Width/2 * SideNormal, SideNormal, Vector3.UnitY, Thickness, Height, TextureSide, new Vector2(1, Height/50));
            Right = new QuadPrimitiveCollidable(gd, SideOrigin - Width/2 * SideNormal, -SideNormal, Vector3.UnitY, Thickness, Height, TextureSide, new Vector2(1, Height/50));
        }
        public void Draw(Matrix view, Matrix projection)
        {
            Front.Draw(view, projection);
            Back.Draw(view, projection);
            Left.Draw(view, projection);
            Right.Draw(view, projection);
        }
    }
}
