using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;
namespace TGC.MonoGame.TP.Utils
{
    public class BoxPrimitiveCollidable : BoxPrimitive {
        public AABB aabb { get; set; }
        public BoxPrimitiveCollidable(GraphicsDevice graphicsDevice, Vector3 size, Texture2D texture) : base(graphicsDevice, size,texture) {
            aabb = new AABB(new Vector3(size.X/2,size.Y/2,size.Z/2));
            Collision.Instance.AppendStatic(aabb);
        }
        public override void Draw(Matrix world, Matrix view, Matrix projection) {
            aabb.Translation(world);
            base.Draw(world,view,projection);
        }
    }
}