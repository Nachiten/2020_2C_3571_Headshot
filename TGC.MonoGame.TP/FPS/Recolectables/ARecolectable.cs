using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using System.Diagnostics;
using TGC.MonoGame.TP.FPS.Scenarios;
using TGC.MonoGame.TP.FPS;

namespace TGC.MonoGame.TP
{
    public abstract class ARecolectable : IElementEffect
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
        public float tamanioModelo;
        // Color del modelo
        protected Vector3 modelColor;
        // Offset para ajustar el centro correctamente
        protected Vector3 offsetPosicion = Vector3.Zero;
        // Matriz para ajustar el offset
        protected Matrix matrizOffsetPosicion = Matrix.CreateTranslation(Vector3.Zero);
        protected Effect Effect;

        public virtual void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice) {
            // Genero el modelo
            World *= Matrix.CreateScale(tamanioModelo) * matrizOffsetPosicion;
            Modelo = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + pathModelo, World);
            
            //Debug.WriteLine("Path: " + pathModelo + " | offset: " + offsetPosicion);
            Modelo.Aabb.Translation(World * Matrix.CreateTranslation(posicion + offsetPosicion));

            // Se agrega el colisionable a la lista
            Collision.Instance.AppendCollectable(this);

            // Se genera el efecto
            Modelo.SetEffect(Effect);
        }

        public void Update(GameTime gameTime)
        {
            // Basado en el tiempo que pasa se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) * 0.7f;
            
            if (!Config.rotacionRecolectables)
                Rotation = 0;

            Modelo.Transform(World * Matrix.CreateRotationY(Rotation) * Matrix.CreateTranslation(posicion), false);
            Modelo.SetCameraPos(Player.Instance.GetCameraPos());
            Modelo.SetTime((float)gameTime.TotalGameTime.TotalSeconds);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Dibujo el modelo
            Modelo.SetRecolectable(1f);
            Modelo.Draw(view, projection);
            Modelo.SetRecolectable(0f);
        }
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            // Dibujo el modelo
            Modelo.Draw(world, view, projection);
        }
        public void SetEffect(Effect Effect)
        {
            this.Effect = Effect;
        }
        public void SetLight(Light Light)
        {
            Modelo.SetLight(Light);
        }

        public abstract void recolectar(AStage Stage);

        protected void eliminarRecolectableDeLista(AStage Stage) {
            Stage.RemoveRecolectable(this);
        }
    }
}
