using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.FPS.Interface
{
    public struct ControlSettings
    {
        //move
        public static Keys Forward = Keys.W;
        public static Keys Backward = Keys.S;
        public static Keys Right = Keys.D;
        public static Keys Left = Keys.A;
        public static Keys ChangeLastWeapon = Keys.Q;
        public static Keys PrimaryWeapon = Keys.D1;
        public static Keys SecondaryWeapon = Keys.D2;
        public static Keys Knife = Keys.D3;
        public static Keys Jump = Keys.Back;

        //weapon controller
        

        
    }
    public class KeyboardManager
    {
        private Player player { get; set; }
        public KeyboardManager(Player player)
        {
            this.player = player;
        }

        public void Update(GameTime gametime)
        {
            MouseState state = Mouse.GetState();
            
            if(Keyboard.GetState().IsKeyDown(ControlSettings.Forward))
            {
                //fake - ver como mover al personaje
                player.Move();
            }
            if (Keyboard.GetState().IsKeyDown(ControlSettings.Backward))
            {
                
            }
            if (Keyboard.GetState().IsKeyDown(ControlSettings.PrimaryWeapon))
            {
                player.ChangeWeapon(1);
            }
            if (Keyboard.GetState().IsKeyDown(ControlSettings.SecondaryWeapon))
            {
                player.ChangeWeapon(2);
            }
            if (Keyboard.GetState().IsKeyDown(ControlSettings.Knife))
            {
                player.ChangeWeapon(3);
            }
            if(state.RightButton == ButtonState.Pressed)
            {
                //poner zoom al arma
            }
            if (state.LeftButton == ButtonState.Pressed)
            {
                player.Shoot();
            }
        }       
    }
}
