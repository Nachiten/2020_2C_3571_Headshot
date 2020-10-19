using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using System.Diagnostics;
using TGC.MonoGame.TP.FPS.Scenarios;

namespace TGC.MonoGame.TP
{
    public abstract class ARecolectable
    {
        // Path para modelo
        protected const string ContentFolder3D = "Models/";
        // Modelo propiamente dicho
        public ModelCollidable Modelo { get; set; }
        // Rotacion del modelo
        protected float Rotation { get; set; }
        // Matriz de mundo
        protected Matrix World { get; set; }
        // Posicion actual
        protected Vector3 posicion;
        // Path del modelo dentro de Models
        protected string pathModelo;
        // Escala del modelo
        protected float tamanioModelo;
        // Color del modelo
        protected Vector3 modelColor;
        // Offset para ajustar el centro correctamente
        protected Vector3 offsetPosicion = Vector3.Zero;
        // Matriz para ajustar el offset
        protected Matrix matrizOffsetPosicion = Matrix.CreateTranslation(Vector3.Zero);

        public virtual void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice) {
            // Genero el modelo
            World *= Matrix.CreateScale(tamanioModelo) * matrizOffsetPosicion;
            Modelo = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + pathModelo, World);
            //Debug.WriteLine("Path: " + pathModelo + " | offset: " + offsetPosicion);
            Modelo.Aabb.Translation(World * Matrix.CreateTranslation(posicion + offsetPosicion));

            // Se agrega el colisionable a la lista
            Collision.Instance.AppendCollectable(this);

            // Se genera los efectos y texturas default
            var modelEffectArmor = (BasicEffect)Modelo.Model.Meshes[0].Effects[0];
            modelEffectArmor.DiffuseColor = modelColor;
            modelEffectArmor.EnableDefaultLighting();
        }

        public void Update(GameTime gameTime)
        {
            // Basado en el tiempo que pasa se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 0.7f;
            
            if (!Config.rotacionRecolectables)
                Rotation = 0;

            Modelo.Transform(World * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posicion), false);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo el modelo
            Modelo.Draw(view, projection);
        }

        public abstract void recolectar(IStage Stage);

        protected void eliminarRecolectableDeLista(IStage Stage) {
            Stage.RemoveRecolectable(this);
        }
    }
}
