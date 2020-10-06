using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;

namespace TGC.MonoGame.TP.Utils{
    public class ModelCollidable {
        public Model Model { get; set; }
        public AABB Aabb { get; set; }
        public Matrix World { get; set; }
        public ModelCollidable(ContentManager Content, string Filepath, Matrix world){
            Model = Content.Load<Model>(Filepath);
            // TODO: infer the size of the model & translate it to a vector
            World = world;
            CreateAABB();
        }
        public void Transform(Matrix world, bool dynamic)
        {
            World = world;
            if(dynamic)
            {
                World.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);;
                Aabb.Translation(translation);
            }
        }
        private void CreateAABB() {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), World);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            Aabb = new AABB(min, max);
        }
        public void Draw(Matrix View, Matrix Projection)
        {
            Model.Draw(World, View, Projection);
        }
    }
}