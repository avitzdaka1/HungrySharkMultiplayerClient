using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Fruits
{
    

    public class Apple: BaseFruit, IEquatable<Apple>
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

        public bool Equals(Apple other)
        {
            if (other.x == x && other.y == y)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            //if (Object.ReferenceEquals(this, null)) return 0;

            //Get hash code for the Numf field if it is not null. 
            int hashNumf = x == 0 ? 0 : x.GetHashCode();
            hashNumf += y == 0 ? 0 : y.GetHashCode();
            return hashNumf;
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
