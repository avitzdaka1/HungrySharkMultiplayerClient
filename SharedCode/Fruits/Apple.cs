using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Fruits
{
    

    public class Apple: BaseFruit
    {
        private double x;
        private double y;
        private Vector2 position;
        public Apple(double x, double y):base()
        {
            position = new Vector2((float)x, (float)y);
            this.x = x;
            this.y = y;
            
        }





        public override double getX()
        {
            return x;
        }
        public override double getY()
        {
            return y;
        }

        public Vector2 getPosition()
        {
            return position;
        }

    }
}
