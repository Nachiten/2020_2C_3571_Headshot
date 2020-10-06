using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace TGC.MonoGame.TP.Utils
{
    public static class ShapeCreatorHelper
    {

        public static VertexPositionTexture[] CreatePlane(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, Vector2 textureRepetitions) {
            var vertices = new VertexPositionTexture[6];

            vertices[0].Position = point1;
            vertices[1].Position = point2;
            vertices[2].Position = point3;
            vertices[3].Position = vertices[1].Position;
            vertices[4].Position = point4;
            vertices[5].Position = vertices[2].Position;

            var repitions = 1;

            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(0, repitions);
            vertices[2].TextureCoordinate = new Vector2(repitions, 0);

            vertices[3].TextureCoordinate = vertices[1].TextureCoordinate;
            vertices[4].TextureCoordinate = new Vector2(repitions, repitions);
            vertices[5].TextureCoordinate = vertices[2].TextureCoordinate;

            return vertices;
        }
    }
}
