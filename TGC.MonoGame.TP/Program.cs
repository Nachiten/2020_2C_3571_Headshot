﻿using System;

namespace TGC.MonoGame.TP
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new FPSManager())
                game.Run();
        }
    }
}
