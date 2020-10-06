using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Utils;
namespace TGC.MonoGame.TP.Utils
{
    public class QuadPrimitiveCollidable : QuadPrimitive {
        public AABB aabb { get; set; }
        public QuadPrimitiveCollidable(GraphicsDevice graphicsDevice, Vector3 origin, Vector3 normal, Vector3 up, float width, float height, Texture2D texture, Vector2 textureRepeats)
            : base(graphicsDevice,origin,normal,up,width,height,texture,textureRepeats)
        { 
            Vector3 size;
            if(normal == Vector3.UnitX || normal == -Vector3.UnitX) {
                size = new Vector3(1,height,width);
            } else if(normal == Vector3.UnitY || normal == -Vector3.UnitY){
                size = new Vector3(height,1,width);
            } else if(normal == Vector3.UnitZ || normal == -Vector3.UnitZ) {
                size = new Vector3(width,height,1);
            } else {
                throw new System.Exception("QuadPrimitiveCollidable expects parameter 'normal' to be Axis Aligned");
            }
            aabb = new AABB(size);
            aabb.Translation(origin);
            Collision.Instance.appendStatic(aabb);
        }
    }
}