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
        private string filepath { get; set; }
        public ModelCollidable(ContentManager Content, string Filepath, Matrix world){
            filepath = Filepath;
            Model = Content.Load<Model>(Filepath);
            // TODO: infer the size of the model & translate it to a vector
            World = world;
            CreateAABB();
            //Collision.appendStatic(Aabb);
        }
        public void Transform(Matrix world)
        {
            World = world;

            float angleX = VectorsAngle(Vector3.Transform(Vector3.Zero, world),Vector3.UnitX);
            float angleY = VectorsAngle(Vector3.Transform(Vector3.Zero, world), Vector3.UnitY);
            float angleZ = VectorsAngle(Vector3.Transform(Vector3.Zero, world), Vector3.UnitZ);

            //Matrix AAWorld = World * Matrix.CreateRotationX(-angleX) * Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);
            Aabb.Translation(World);
        }
        public void Turn(Matrix world)
        {
            World = world;

        }
        /*public void Translation(Vector3 position)
        {
            World *= Matrix.CreateTranslation(position);
            float angleX = VectorsAngle(Vector3.Transform(Vector3.Zero, World), Vector3.UnitX);
            float angleY = VectorsAngle(Vector3.Transform(Vector3.Zero, World), Vector3.UnitY);
            float angleZ = VectorsAngle(Vector3.Transform(Vector3.Zero, World), Vector3.UnitZ);
            Matrix AAWorld = World * Matrix.CreateRotationX(-angleX) * Matrix.CreateRotationY(-angleY) * Matrix.CreateRotationZ(-angleZ);
            Aabb.Translation(AAWorld);
        }*/
        private float VectorsAngle(Vector3 v1, Vector3 v2)
        {
            return (float)Math.Acos(Vector3.Dot(v1, v2) / (Vector3.Distance(v1, Vector3.Zero) * Vector3.Distance(v2, Vector3.Zero)));
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
            Debug.WriteLine("AABB Coords " + filepath + ": min: " + min + " - max: " + max);
            Aabb = new AABB(min, max);
        }
        public void Draw(Matrix View, Matrix Projection)
        {
            // TODO: infer the Axis Aligned position from World Matrix & translate it to a matrix/vector
            Model.Draw(World, View, Projection);
        }
    }
}