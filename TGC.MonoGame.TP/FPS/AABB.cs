using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace TGC.MonoGame.TP.Utils{
    public class AABB{
        public Vector3 maxExtents { get; set; }
        public Vector3 minExtents { get; set; }
        private VertexBuffer Vertices { get; set; }
        private IndexBuffer Indices { get; set; }
        //public Matrix world;
        public Vector3 size;
        BoundingBox boundingBox;
        GraphicsDevice GraphicsDevice;
        private BasicEffect Effect { get; }

        public AABB(GraphicsDevice gd, Vector3 size){
            this.size = size;
            minExtents = -size;
            maxExtents = size;
            boundingBox = new BoundingBox(minExtents,maxExtents);
            GraphicsDevice = gd;
            if (Config.drawAABB)
            {
                Effect = new BasicEffect(GraphicsDevice);
                CreateLines();
            }
        }
        public AABB(GraphicsDevice gd, Vector3 min, Vector3 max)
        {
            minExtents = min;
            maxExtents = max;
            boundingBox = new BoundingBox(minExtents, maxExtents);
            size = Vector3Abs(maxExtents - minExtents)/3;
            GraphicsDevice = gd;
            if (Config.drawAABB)
            {
                Effect = new BasicEffect(GraphicsDevice);
                CreateLines();
            }
        }
        public void SetManually(Vector3 min, Vector3 max)
        {
            minExtents = min;
            maxExtents = max;
            if (Config.drawAABB)
            {
                CreateLines();
            }
        }

        private void CreateLines()
        {
            var color = Color.White;

            var BoxPoints = new[]
            {
                //top part
                maxExtents,
                maxExtents - 2 * Vector3.UnitX * size.X,
                maxExtents - 2 * Vector3.UnitZ * size.Z,
                maxExtents - Vector3.UnitZ * size.Z * 2 - Vector3.UnitX * size.X * 2,
                //bottom part
                minExtents,
                minExtents + 2 * Vector3.UnitX * size.X,
                minExtents + 2 * Vector3.UnitZ * size.Z,
                minExtents + Vector3.UnitZ * size.Z * 2 + Vector3.UnitX * size.X * 2,
                //sides
                maxExtents - 2 * Vector3.UnitY * size.Y,
                maxExtents - Vector3.UnitX * size.X * 2 - Vector3.UnitY * size.Y * 2,
                maxExtents - Vector3.UnitZ * size.Z * 2 - Vector3.UnitY * size.Y * 2,
                maxExtents - Vector3.UnitZ * size.Z * 2 - Vector3.UnitX * size.X * 2 - Vector3.UnitY * size.Y * 2
            };


            var vertices = new[]
            {
                new VertexPositionColor(BoxPoints[0], color), new VertexPositionColor(BoxPoints[1], color),
                new VertexPositionColor(BoxPoints[0], color), new VertexPositionColor(BoxPoints[2], color),
                new VertexPositionColor(BoxPoints[1], color), new VertexPositionColor(BoxPoints[3], color),
                new VertexPositionColor(BoxPoints[2], color), new VertexPositionColor(BoxPoints[3], color),

                new VertexPositionColor(BoxPoints[4], color), new VertexPositionColor(BoxPoints[5], color),
                new VertexPositionColor(BoxPoints[4], color), new VertexPositionColor(BoxPoints[6], color),
                new VertexPositionColor(BoxPoints[5], color), new VertexPositionColor(BoxPoints[7], color),
                new VertexPositionColor(BoxPoints[6], color), new VertexPositionColor(BoxPoints[7], color),

                new VertexPositionColor(BoxPoints[0], color), new VertexPositionColor(BoxPoints[8], color),
                new VertexPositionColor(BoxPoints[1], color), new VertexPositionColor(BoxPoints[9], color),
                new VertexPositionColor(BoxPoints[2], color), new VertexPositionColor(BoxPoints[10], color),
                new VertexPositionColor(BoxPoints[3], color), new VertexPositionColor(BoxPoints[11], color),
            };

            Vertices = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);

            Vertices.SetData(vertices);


            var indices = new ushort[36];

            // Top.
            indices[0] = 2;
            indices[1] = 1;
            indices[2] = 0;
            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 1;
            // Back.
            indices[6] = 18;
            indices[7] = 17;
            indices[8] = 16;
            indices[9] = 18;
            indices[10] = 19;
            indices[11] = 17;
            // Left.
            indices[12] = 10;
            indices[13] = 9;
            indices[14] = 8;
            indices[15] = 10;
            indices[16] = 11;
            indices[17] = 9;
            // Front.
            indices[18] = 22;
            indices[19] = 21;
            indices[20] = 20;
            indices[21] = 22;
            indices[22] = 23;
            indices[23] = 21;
            // Right.
            indices[24] = 14;
            indices[25] = 13;
            indices[26] = 12;
            indices[27] = 14;
            indices[28] = 15;
            indices[29] = 13;
            // Bottom.
            indices[30] = 6;
            indices[31] = 5;
            indices[32] = 4;
            indices[33] = 6;
            indices[34] = 7;
            indices[35] = 5;

            Indices = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            Indices.SetData(indices);

        }

        public bool IntersectAABB(AABB other){
            Vector3 distance1 = other.minExtents - maxExtents;
            Vector3 distance2 = minExtents - other.maxExtents;
            Vector3 distance = Vector3.Max(distance1,distance2);
            Console.WriteLine(boundingBox.Intersects(other.boundingBox));
            return MaxCoord(distance) < 0;
        }
        public float? IntersectRay(Ray ray){
            return boundingBox.Intersects(ray);
        }
        public void Translation(Vector3 position){
            minExtents = position - size;
            maxExtents = position + size;
            boundingBox = new BoundingBox(minExtents, maxExtents);
            CreateLines();
        }
        public void Translation(Matrix world){
            Vector3 pos = Vector3.Transform(Vector3.Zero, world);
            minExtents = pos - size;
            maxExtents = pos + size;
            boundingBox = new BoundingBox(minExtents, maxExtents);
            CreateLines();
        }
        public void Rotate(Matrix rotation) {
            minExtents = Vector3.Transform(Vector3.Zero, Matrix.CreateTranslation(minExtents) * rotation);
            maxExtents = Vector3.Transform(Vector3.Zero, Matrix.CreateTranslation(maxExtents) * rotation);
            boundingBox = new BoundingBox(minExtents, maxExtents);
            CreateLines();
        }
        private float MaxCoord(Vector3 v){
            if(v.X > v.Y && v.X > v.Z) {
                return v.X;
            } else{
                return v.Y > v.Z ? v.Y : v.Z;
            }
        }
        private Vector3 Vector3Abs(Vector3 v)
        {
            return new Vector3(Math.Abs(v.X),Math.Abs(v.Y),Math.Abs(v.Z));
        }
        public void Draw(Matrix View, Matrix Projection)
        {
            if (Config.drawAABB)
            {
                Effect.View = View;
                Effect.Projection = Projection;
                // Set our vertex declaration, vertex buffer, and index buffer.
                GraphicsDevice.SetVertexBuffer(Vertices);
                GraphicsDevice.Indices = Indices;

                foreach (var effectPass in Effect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 36 / 3);
                }
            }
        }
    }
}