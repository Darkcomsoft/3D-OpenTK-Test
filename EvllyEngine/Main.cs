using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    class MainApplication
    {
        public static void Main()
        {
            using (Engine game = new Engine(1000, 600, "EvllyEngine FPS: 0"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60);
            }
        }
    }
}
