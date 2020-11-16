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
    public class Enemigo: Ashootable
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

        private int maxHealth = 400;
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

            avanzarPath();


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
            // Dibujo en las coordenadas actuales
            Model.Draw(view, projection);

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
            World = Matrix.CreateRotationY(anguloRotacionRadianes) * ScaleFactor * Matrix.CreateTranslation(posicion);

            LineOfSight = new Ray();
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
            Debug.WriteLine("!!!!Dispare al personaje ");
            e.GetDamaged(Weapon.Damage);
            return 0;
        }
    }
}
