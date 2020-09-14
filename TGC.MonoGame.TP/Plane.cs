using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace TGC.MonoGame.TP{
    public class Plane {
        public VertexPositionTexture[] vertices { get; set; }
        public Texture2D texture { get; set; }
        private Vector2 textureRepetitions { get; set; }
        public Plane(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, Vector2 textureRepetitions) {
            vertices = new VertexPositionTexture[6];
            this.textureRepetitions = textureRepetitions;

            vertices[0].Position = point1;
            vertices[1].Position = point2;
            vertices[2].Position = point3;
            vertices[3].Position = vertices[1].Position;
            vertices[4].Position = point4;
            vertices[5].Position = vertices[2].Position;

            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(0, textureRepetitions.Y);
            vertices[2].TextureCoordinate = new Vector2(textureRepetitions.X, 0);
            
            vertices[3].TextureCoordinate = vertices[1].TextureCoordinate;
            vertices[4].TextureCoordinate = new Vector2(textureRepetitions.X, textureRepetitions.Y);
            vertices[5].TextureCoordinate = vertices[2].TextureCoordinate;
        }
        public void LoadTexture(string path,ContentManager Content) {
            texture = Content.Load<Texture2D>(path);
        }
        public void Draw(GraphicsDeviceManager Graphics, BasicEffect Effect, Matrix View, Matrix Projection){
            Effect.View = View;
            Effect.Projection = Projection;
            Effect.TextureEnabled = true;
            Effect.Texture = texture;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    vertices,
                    // The offset, which is 0 since we want to start
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }
    }
}