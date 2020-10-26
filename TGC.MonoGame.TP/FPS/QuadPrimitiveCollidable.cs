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
                size = new Vector3(1,height/2,width/2);
            } else if(normal == Vector3.UnitY || normal == -Vector3.UnitY){
                size = new Vector3(height/2,1,width/2);
            } else if(normal == Vector3.UnitZ || normal == -Vector3.UnitZ) {
                size = new Vector3(width/2,height/2,1);
            } else {
                throw new System.Exception("QuadPrimitiveCollidable expects parameter 'normal' to be Axis Aligned");
            }
            aabb = new AABB(graphicsDevice,size);
            aabb.Translation(origin);
            Collision.Instance.AppendStatic(aabb);
        }
        public void Draw(Matrix view, Matrix projection)
        {
            Matrix world = Matrix.CreateTranslation(Vector3.Zero);
            base.Draw(world, view, projection);
        }
    }
}