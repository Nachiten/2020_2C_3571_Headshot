using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Utils;
using TGC.MonoGame.TP.FPS;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using TGC.MonoGame.TP.FPS.Scenarios;
using System.Linq;

namespace TGC.MonoGame.TP
{
    public class Enemigo: Ashootable, IElementEffect
    {
        private const string ContentFolder3D = "Models/";

        private Vector3 posicion;
        private Vector3 OldPosition = Vector3.Zero;

        private Matrix World { get; set; }

        public ModelCollidable Model { get; set; }

        private Weapon Weapon { get; set; }

        private Effect Effect;

        private Vector3 mirandoInicial = new Vector3(0, 0, -1);
        private float Velocidad = 3;
        private float anguloRotacionRadianes;
        private Vector3 vectorDireccion = Vector3.Zero;

        private int offSetY = 0;
        private Matrix ScaleFactor = Matrix.CreateScale(.8f);

        private int maxHealth = 100;
        private float health;
        
        private bool shooting = false;
        private Ray LineOfSight;
        private Vector3 GunOffset = new Vector3(33, 73, -10);
        private float shootTimeSeconds = 0.5f;

        private int indexPath = 0;
        private bool foward = true;
        public List<PathTrace> path = new List<PathTrace>();

        private Vector3 posicionObjetivo = Vector3.Zero;
        private bool StartedMoving = false;
        Vector3[] PosibleDirections = new[]
        {
            Vector3.UnitX,
            Vector3.UnitZ,
            -Vector3.UnitX,
            -Vector3.UnitZ
        };
        #region animation
        public bool TriggerDead = false;
        double frameTimePlayed = 0; //Amount of time (out of animationTime) that the animation has been playing for
        bool IsAnimating = false; //Self-Explanitory
        int animationTime = 500; //For .5 seconds of animation.
        Matrix PreviousWorld;
        float TriggerDissapear = 0f;
        float TriggerStopFlatten = 0f;
        #endregion

        public Enemigo(List<PathTrace> _path)
        {
            health = maxHealth;

            var posicionInicial = _path.First();
            anguloRotacionRadianes = PointTo(posicionInicial.normal);
            posicion = convertVector3(posicionInicial.posicion);
            World = ScaleFactor * Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationY(anguloRotacionRadianes) * Matrix.CreateTranslation(posicion);

            LineOfSight = new Ray();

            path = _path;
            Weapon = new Weapon(new M4(posicion));
        }

        public void SetEffect(Effect Effect)
        {
            this.Effect = Effect;
            Weapon.SetEffect(Effect);
        }
        public void SetLight(Light Light)
        {
            Model.SetLight(Light);
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            Model = new ModelCollidable(GraphicsDevice, Content, ContentFolder3D + "Knight/Knight_01", World);

            Aabb = Model.Aabb;

            Collision.Instance.AppendStatic(Model.Aabb);
            Collision.Instance.AppendShootable(this);
            Weapon.Gun.LoadContent(Content, GraphicsDevice);
            Collision.Instance.RemoveCollectable(Weapon.Gun);

            Model.SetEffect(Effect);
            Model.SetLightParameters(.2f, .75f, .05f, 100f);
            Model.SetTexture(Content.Load<Texture2D>(ContentFolder3D + "Knight/Knight01_albedo"));
        }

        private Vector3 convertVector3(Vector2 posicion) {

            return new Vector3(posicion.X, offSetY, posicion.Y);
        }
        public void avanzarPath() 
        {
            
            if (indexPath + 1 < path.Count && foward) {
                if (Vector3.Distance(convertVector3(path[indexPath + 1].posicion), posicion) <= 2)
                {
                    indexPath++;
                    anguloRotacionRadianes = PointTo(path[indexPath].normal);
                }
            }
            else if(indexPath + 1 == path.Count && foward)
            {
                foward = false;
                indexPath--;
                anguloRotacionRadianes = PointTo(-path[indexPath].normal);
            }
            if (!foward) {
                if (indexPath > 0)
                {
                    if (Vector3.Distance(convertVector3(path[indexPath].posicion), posicion) <= 2)
                    {
                        indexPath--;
                        anguloRotacionRadianes = PointTo(-path[indexPath].normal);
                    }
                }
                else if(Vector3.Distance(convertVector3(path[indexPath].posicion), posicion) <=2)
                { 
                    foward = true;
                    indexPath = 0;
                    anguloRotacionRadianes = PointTo(path[indexPath].normal);
                }
                
            }
            UpdateWorld(posicion + (vectorDireccion * Velocidad), anguloRotacionRadianes);
        }

        public void Update(GameTime gameTime, Vector3 posicionCamara)
        {

            Model.SetCameraPos(Player.Instance.GetCameraPos());

            if(!IsAnimating)
                avanzarPath();
            HandleDeadAnimation(gameTime);


            if (Math.Round(gameTime.TotalGameTime.TotalMilliseconds) % (shootTimeSeconds * 1000) == 0 && !shooting)
            {
                shooting = true;
                Collision.Instance.CheckShootable(LineOfSight, this, ShootableCollisionCB);
            }
            else
            {
                shooting = false;
            }
        }
        public void UpdateWorld(Vector3 hacia, float angulo)
        {
            posicion = hacia;
            // Aplico la rotacion que corresponde
            World = ScaleFactor * Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationY(MathHelper.Pi + angulo);

            // Muevo el modelo
            World *= Matrix.CreateTranslation(posicion);
            Model.Transform(World, true);

            // Actualizo la vista
            UpdateLineOfSight();
        }
        private void HandleDeadAnimation(GameTime GameTime)
        {
            //If it is not already animating and there is a trigger, start animating
            if (!IsAnimating && TriggerDead)
            {
                IsAnimating = true;
                PreviousWorld = World;
            }

            //Increment the frameTimePlayed by the time (in milliseconds) since the last frame
            if (IsAnimating)
                frameTimePlayed += GameTime.ElapsedGameTime.TotalMilliseconds;
            //If playing and we have not exceeded the time limit
            if (IsAnimating && frameTimePlayed < animationTime)
            {
                Vector3 scale;
                Quaternion rotation;
                Vector3 translation;
                World.Decompose(out scale, out rotation, out translation);
                if(vectorDireccion.X == 1 || vectorDireccion.X == -1)
                    World = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateRotationZ(MathHelper.PiOver2 * (float)(frameTimePlayed / (animationTime * 16))) * Matrix.CreateTranslation(translation);
                else
                    World = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateRotationX(MathHelper.PiOver2 * (float)(frameTimePlayed / (animationTime * 16))) * Matrix.CreateTranslation(translation);
                Model.Transform(World, true);
            }
            //If exceeded time, reset variables and stop playing
            else if (IsAnimating && frameTimePlayed >= animationTime)
            {
                //World = PreviousWorld;
                TriggerDead = false;
                TriggerDissapear = 1f;
            }
            if (TriggerDissapear == 1f && frameTimePlayed >= animationTime * 4)
            {
                TriggerStopFlatten = 1f;
            }
            if (TriggerDissapear == 1f && frameTimePlayed >= animationTime * 8)
            {
                frameTimePlayed = 0;
                IsAnimating = false;
                TriggerDissapear = 0f;
                TriggerStopFlatten = 0f;
                Reiniciar();
            }
        }
        private float MoveTowards(Vector3 v)
        {
            vectorDireccion = Vector3.Normalize(v - posicion);
            float angle = (float)Math.Acos(Vector3.Dot(vectorDireccion, mirandoInicial)
                    / (Vector3.Distance(vectorDireccion, Vector3.Zero) * Vector3.Distance(mirandoInicial, Vector3.Zero)));

            // Si posX del objetivo es mayor a posX actual => * -1
            // Si el objetivo está en el tercer o cuarto cuadrante (angulo > 180) entonces debo invertir el angulo
            if (v.X > posicion.X)
            {
                angle *= -1;
            }
            return angle;
        }
        private float PointTo(Vector3 v)
        {
            Vector3 dirant = vectorDireccion;
            vectorDireccion = v;
            float angle = (float)Math.Acos(Vector3.Dot(vectorDireccion, mirandoInicial)
                    / (Vector3.Distance(vectorDireccion, Vector3.Zero) * Vector3.Distance(mirandoInicial, Vector3.Zero)));

            // Si posX del objetivo es mayor a posX actual => * -1
            // Si el objetivo está en el tercer o cuarto cuadrante (angulo > 180) entonces debo invertir el angulo
            if (v.X > dirant.X)
            {
                angle *= -1;
            }
            return angle;
        }

        private void UpdateLineOfSight()
        {
            LineOfSight.Position = posicion + GunOffset;
            LineOfSight.Direction = vectorDireccion;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            // Update Parameters
            Effect.Parameters["Enemy"]?.SetValue(1f);
            Effect.Parameters["Dead"]?.SetValue(TriggerDissapear);
            Effect.Parameters["StopFlatten"]?.SetValue(TriggerStopFlatten);
            Effect.Parameters["Time"]?.SetValue((float)frameTimePlayed/1000);
            // Draw
            Model.Draw(view, projection);
            // Unset Parameters
            Effect.Parameters["Enemy"]?.SetValue(0f);
            Effect.Parameters["Dead"]?.SetValue(0f);
            Effect.Parameters["StopFlatten"]?.SetValue(0f);

            World.Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);

            Matrix ww = Matrix.CreateTranslation(GunOffset) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(translation);

            Weapon.Draw(ww, view, projection);
        }

        public override void GetDamaged(int damage)
        {
            if (health - damage < 0)
            {
                health = 0;
            } else
            {
                health -= damage;
            }
        }
        public bool IsDead()
        {
            return health == 0;
        }
        public void Reiniciar()
        {
            health = maxHealth;
            indexPath = 0;

            var posicionInicial = path.First();
            anguloRotacionRadianes = PointTo(posicionInicial.normal);
            posicion = convertVector3(posicionInicial.posicion);
            World = ScaleFactor * Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationY(anguloRotacionRadianes) * Matrix.CreateTranslation(posicion);
            Model.Transform(World, true);

            LineOfSight = new Ray();
        }
        public void Revivir()
        {
            health = maxHealth;
        }


        private int StaticCollisionCB(AABB a, AABB b)
        {
            //TODO: Handle Collision
            posicion = OldPosition;
            Model.Transform(World * Matrix.CreateTranslation(posicion), true);
            return 0;
        }
        public int ShootableCollisionCB(Ashootable e)
        {
            e.GetDamaged(Weapon.Damage);
            return 0;
        }
    }
}
