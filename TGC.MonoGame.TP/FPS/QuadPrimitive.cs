using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP.Utils
{
    /// <summary>
    ///     The quad is like a plane but its made by two triangle and the surface is oriented in the XY plane of the local
    ///     coordinate space.
    /// </summary>
    public class QuadPrimitive
    {
        private const int NumberOfVertices = 4;
        private const int NumberOfIndices = 6;

        /// <summary>
        ///     Create a textured quad.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        /// <param name="origin">The center.</param>
        /// <param name="normal">Normal vector.</param>
        /// <param name="up">Up vector.</param>
        /// <param name="width">The Width.</param>
        /// <param name="height">The High.</param>
        /// <param name="texture">The texture to use.</param>
        /// <param name="textureRepeats">Times to repeat the given texture.</param>
        public QuadPrimitive(GraphicsDevice graphicsDevice, Vector3 origin, Vector3 normal, Vector3 up, float width,
            float height, Texture2D texture, Vector2 textureRepeats)
        {
            /*Effect = new BasicEffect(graphicsDevice);
            Effect.TextureEnabled = true;
            Effect.Texture = texture;*/
            //Effect.EnableDefaultLighting();
            Texture = texture;

            Origin = origin;
            Normal = normal;
            Up = up;

            CreateVertexBuffer(graphicsDevice, width, height, textureRepeats);
            CreateIndexBuffer(graphicsDevice);
        }

        /// <summary>
        ///     Represents a list of 3D vertices to be streamed to the graphics device.
        /// </summary>
        private VertexBuffer Vertices { get; set; }
        private Texture2D Texture;

        /// <summary>
        ///     Describes the rendering order of the vertices in a vertex buffer, using clockwise winding.
        /// </summary>
        private IndexBuffer Indices { get; set; }

        /// <summary>
        ///     The left direction.
        /// </summary>
        private Vector3 Left { get; set; }

        /// <summary>
        ///     The lower left corner.
        /// </summary>
        private Vector3 LowerLeft { get; set; }

        /// <summary>
        ///     The lower right corner.
        /// </summary>
        private Vector3 LowerRight { get; set; }

        /// <summary>
        ///     The normal direction.
        /// </summary>
        private Vector3 Normal { get; }

        /// <summary>
        ///     The center.
        /// </summary>
        private Vector3 Origin { get; }

        /// <summary>
        ///     The up direction.
        /// </summary>
        private Vector3 Up { get; }

        /// <summary>
        ///     The up left corner.
        /// </summary>
        private Vector3 UpperLeft { get; set; }

        /// <summary>
        ///     The up right corner.
        /// </summary>
        private Vector3 UpperRight { get; set; }

        /// <summary>
        ///     Used to set and query effects and choose techniques.
        /// </summary>
        public Effect Effect { get; set; }
        private float KAmbient;
        private float KDiffuse;
        private float KSpecular;
        private float Shininess;
        private Light Light;


        /// <summary>
        ///     Create a vertex buffer for the figure with the given information.
        /// </summary>
        /// <param name="graphicsDevice">Used to initialize and control the presentation of the graphics device.</param>
        /// <param name="width">The Width.</param>
        /// <param name="height">The High.</param>
        /// <param name="textureRepeats">Times to repeat the given texture.</param>
        private void CreateVertexBuffer(GraphicsDevice graphicsDevice, float width, float height, Vector2 textureRepeats)
        {
            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);
            var upperCenter = Up * height / 2 + Origin;
            UpperLeft = upperCenter + Left * width / 2;
            UpperRight = upperCenter - Left * width / 2;
            LowerLeft = UpperLeft - Up * height;
            LowerRight = UpperRight - Up * height;

            // Fill in texture coordinates to display full texture on quad
            var textureUpperLeft = Vector2.Zero;
            var textureUpperRight = Vector2.UnitX;
            var textureLowerLeft = Vector2.UnitY;
            var textureLowerRight = Vector2.One;

            // Set the position and texture coordinate for each vertex
            var vertices = new VertexPositionNormalTexture[NumberOfVertices];
            vertices[0].Position = LowerLeft;
            vertices[0].TextureCoordinate = textureLowerLeft * textureRepeats;
            vertices[1].Position = UpperLeft;
            vertices[1].TextureCoordinate = textureUpperLeft * textureRepeats;
            vertices[2].Position = LowerRight;
            vertices[2].TextureCoordinate = textureLowerRight * textureRepeats;
            vertices[3].Position = UpperRight;
            vertices[3].TextureCoordinate = textureUpperRight * textureRepeats;

            // Provide a normal for each vertex
            for (var i = 0; i < vertices.Length; i++) vertices[i].Normal = Normal;

            Vertices = new VertexBuffer(graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            Vertices.SetData(vertices);
        }

        private void CreateIndexBuffer(GraphicsDevice graphicsDevice)
        {
            // Set the index buffer for each vertex, using clockwise winding
            var indices = new ushort[NumberOfIndices];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 1;
            indices[5] = 3;

            Indices = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length,
                BufferUsage.WriteOnly);
            Indices.SetData(indices);
        }
        public void SetEffect(Effect Effect)
        {
            this.Effect = Effect;
        }
        public void SetCameraPos(Vector3 pos)
        {
            Effect.Parameters["EyePosition"]?.SetValue(pos);
        }
        public void SetLightParameters(float KAmbient, float KDiffuse, float KSpecular, float Shininess)
        {
            if(KAmbient + KDiffuse + KSpecular > 1)
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
        private float MuzzleStatus = 0;
        public void SetLightMuzzle(Light Light)
        {
            MuzzleStatus = 1;
            LightMuzzle = Light;
        }
        public void UnsetLightMuzzle()
        {
            MuzzleStatus = 0;
            LightMuzzle = nolight;
        }

        /// <summary>
        ///     Draw the Quad.
        /// </summary>
        /// <param name="world">The world matrix for this box.</param>
        /// <param name="view">The view matrix, normally from the camera.</param>
        /// <param name="projection">The projection matrix, normally from the application.</param>
        public virtual void Draw(Matrix world, Matrix view, Matrix projection)
        {
            Effect.Parameters["World"]?.SetValue(world);
            Effect.Parameters["View"]?.SetValue(view);
            Effect.Parameters["Projection"]?.SetValue(projection);
            Effect.Parameters["WorldViewProjection"]?.SetValue(world * view * projection);
            Effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Invert(Matrix.Transpose(world)));
            Effect.Parameters["ModelTexture"].SetValue(Texture);
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
            Effect.Parameters["Muzzle"]?.SetValue(MuzzleStatus);
            Effect.CurrentTechnique = Effect.Techniques["BasicColorDrawing"];

            // Draw the model, using BasicEffect.
            Draw(Effect);
        }

        /// <summary>
        ///     Draws the primitive model, using the specified effect. Unlike the other Draw overload where you just specify the
        ///     world/view/projection matrices and color, this method does not set any render states, so you must make sure all
        ///     states are set to sensible values before you call it.
        /// </summary>
        /// <param name="effect">Used to set and query effects, and to choose techniques.</param>
        public void Draw(Effect effect)
        {
            var graphicsDevice = effect.GraphicsDevice;

            // Set our vertex declaration, vertex buffer, and index buffer.
            graphicsDevice.SetVertexBuffer(Vertices);
            graphicsDevice.Indices = Indices;

            foreach (var effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumberOfIndices / 3);
            }
        }
    }
}