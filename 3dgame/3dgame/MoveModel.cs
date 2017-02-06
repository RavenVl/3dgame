using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3dgame
{
    class MoveModel :BasicModel
    {
        //Matrix rotation = Matrix.Identity;
        Matrix move = Matrix.Identity;
        public MoveModel(Model m)
            : base(m)
        {

            
        }

        public override void Update()
        {
            //rotation *= Matrix.CreateRotationY(MathHelper.Pi / 180);
            move *= Matrix.CreateTranslation(new Vector3(0,0,-0.1f));
            //if (this.world.)
            //{
                
            //}
        }

        public override Matrix GetWorld()
        {
            return world *move ;
        }
    }

}
