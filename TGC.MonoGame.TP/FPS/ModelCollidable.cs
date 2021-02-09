using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using TGC.MonoGame.TP.FPS.Scenarios;
using TGC.MonoGame.TP.FPS;

namespace TGC.MonoGame.TP.Utils{
    public class ModelCollidable : IElementEffect {
        public Model Model { get; set; }
        public AABB Aabb { get; set; }
        public Matrix World { get; set; }
        private Effect Effect;
        private float KAmbient;
        private float KDiffuse;
        private float KSpecular;
        private float Shininess;
        private Light Light;
        private Texture2D Texture;
        public ModelCollidable(GraphicsDevice GraphicsDevice, ContentManager Content, string Filepath, Matrix world){
            Model = Content.Load<Model>(Filepath);
            World = world;
            CreateAABB(GraphicsDevice);
        }
        public void Transform(Matrix world, bool dynamic)
        {
            World = world;
            if(dynamic)
            {
                World.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);
                Aabb.Translation(translation);
            }
        }
        private void CreateAABB(GraphicsDevice GraphicsDevice) {
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
            Aabb = new AABB(GraphicsDevice, min, max);
        }
        public void SetEffect(Effect Effect)
        {
            this.Effect = Effect;
            foreach (var mesh in Model.Meshes)
                foreach (var part in mesh.MeshParts)
                    part.Effect = Effect;
        }
        public void SetLightParameters(float KAmbient, float KDiffuse, float KSpecular, float Shininess)
        {
            if (KAmbient + KDiffuse + KSpecular > 1)
            {
                throw new System.Exception("SetLightParameters expects parameters 'K' to sum 1");
            }
            this.KAmbient = KAmbient;
            this.KDiffuse = KDiffuse;
            this.KSpecular = KSpecular;
            this.Shininess = Shininess;
        }
        public void SetLight(Light Light)
        {
            this.Light = Light;
        }
        private Light LightMuzzle;
        private Light nolight = new Light { Position = Vector3.Zero, AmbientColor = Color.Black, DiffuseColor = Color.Black, SpecularColor = Color.Black };
        public void SetLightMuzzle(Light Light)
        {
            LightMuzzle = Light;
        }
        public void UnsetLightMuzzle()
        {
            LightMuzzle = nolight;
        }
        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }
        public void SetCameraPos(Vector3 pos)
        {
            Effect.Parameters["EyePosition"]?.SetValue(pos);
        }
        public void SetTime(float time)
        {
            Effect.Parameters["Time"]?.SetValue(time);
        }
        public void SetRecolectable(float val)
        {
            Effect.Parameters["Recolectable"]?.SetValue(val);
        }
        public void Draw(Matrix View, Matrix Projection)
        {   
            if(Effect == null)
            {
                Model.Draw(World, View, Projection);
                return;
            }
            Effect.Parameters["ModelTexture"].SetValue(Texture);
            Effect.Parameters["View"]?.SetValue(View);
            Effect.Parameters["Projection"]?.SetValue(Projection);
            // Update Light Parameters
            Effect.Parameters["KAmbient"]?.SetValue(KAmbient);
            Effect.Parameters["KDiffuse"]?.SetValue(KDiffuse);
            Effect.Parameters["KSpecular"]?.SetValue(KSpecular);
            Effect.Parameters["Shininess"]?.SetValue(Shininess);
            // Main Light
            Effect.Parameters["AmbientColor"]?.SetValue(Light.AmbientColor.ToVector3());
            Effect.Parameters["DiffuseColor"]?.SetValue(Light.DiffuseColor.ToVector3());
            Effect.Parameters["SpecularColor"]?.SetValue(Light.SpecularColor.ToVector3());
            Effect.Parameters["LightPosition"]?.SetValue(Light.Position);
            // Muzzle Light
            Effect.Parameters["AmbientColorMuzzle"]?.SetValue(LightMuzzle.AmbientColor.ToVector3());
            Effect.Parameters["DiffuseColorMuzzle"]?.SetValue(LightMuzzle.DiffuseColor.ToVector3());
            Effect.Parameters["SpecularColorMuzzle"]?.SetValue(LightMuzzle.SpecularColor.ToVector3());
            Effect.Parameters["LightPositionMuzzle"]?.SetValue(LightMuzzle.Position);
            Effect.CurrentTechnique = Effect.Techniques["BasicColorDrawing"];

            foreach (var mesh in Model.Meshes)
            {
                var world = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"]?.SetValue(world);
                Effect.Parameters["WorldViewProjection"]?.SetValue(world * View * Projection);
                Effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Invert(Matrix.Transpose(world)));
                mesh.Draw();
            }
        }
        public void Draw(Matrix World, Matrix View, Matrix Projection)
        {
            if (Effect == null)
            {
                Model.Draw(World, View, Projection);
                return;
            }
            Effect.Parameters["ModelTexture"].SetValue(Texture);
            Effect.Parameters["View"]?.SetValue(View);
            Effect.Parameters["Projection"]?.SetValue(Projection);
            // Update Light Parameters
            Effect.Parameters["KAmbient"]?.SetValue(KAmbient);
            Effect.Parameters["KDiffuse"]?.SetValue(KDiffuse);
            Effect.Parameters["KSpecular"]?.SetValue(KSpecular);
            Effect.Parameters["Shininess"]?.SetValue(Shininess);
            Effect.Parameters["AmbientColor"]?.SetValue(Light.AmbientColor.ToVector3());
            Effect.Parameters["DiffuseColor"]?.SetValue(Light.DiffuseColor.ToVector3());
            Effect.Parameters["SpecularColor"]?.SetValue(Light.SpecularColor.ToVector3());
            Effect.Parameters["LightPosition"]?.SetValue(Light.Position);

            foreach (var mesh in Model.Meshes)
            {
                var world = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"]?.SetValue(world);
                Effect.Parameters["WorldViewProjection"]?.SetValue(world * View * Projection);
                Effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Invert(Matrix.Transpose(world)));
                mesh.Draw();
            }
        }
    }
}