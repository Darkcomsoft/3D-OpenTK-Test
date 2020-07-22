using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvllyEngine
{
    public class ScriptTest : ScriptBase
    {
        public override void Update()
        {
            //gameObject._transform._Rotation = new Quaternion(MathHelper.DegreesToRadians(Time._Tick * 200), MathHelper.DegreesToRadians(Time._Tick * 200), MathHelper.DegreesToRadians(Time._Tick * 200), 0);
            base.Update();
        }
    }
}
