using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Utils
{
    class LinePrimitive
    {
        BasicEffect BasicEffect;
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public LinePrimitive(GraphicsDevice gd, Vector3 startpoint, Vector3 endpoint)
        {
            StartPoint = startpoint;
            EndPoint = endpoint;

            BasicEffect = new BasicEffect(gd);
        }
        public void Draw(Matrix view, Matrix projection)
        {
            BasicEffect.View = view;
            BasicEffect.Projection = projection;
            BasicEffect.CurrentTechnique.Passes[0].Apply();
            var vertices = new[] { new VertexPositionColor(StartPoint, Color.White), new VertexPositionColor(EndPoint, Color.White) };
            BasicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
        }
    }
}
