using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Collisions{
    public class ModelCollidableStatic {
        public Model model { get; set; }
        public AABB aabb { get; set; }
        public ModelCollidableStatic(ContentManager Content, string Filepath, Collision Collision){
            model = Content.Load<Model>(Filepath);
            // TODO: infer the size of the model & translate it to a vector
            
            do {
                ModelMesh mesh = model.Meshes.GetEnumerator().Current;
                //mesh.BoundingSphere;
            } while(model.Meshes.GetEnumerator().MoveNext());

            aabb = new AABB(Vector3.One);
            Collision.appendStatic(aabb);
        }
        public void Draw(Matrix World, Matrix View, Matrix Projection){
            // TODO: infer the Axis Aligned position from World Matrix & translate it to a matrix/vector
            aabb.Translation(World);
            model.Draw(World, View, Projection);
        }
    }
}