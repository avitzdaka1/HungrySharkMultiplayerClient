using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatform.Fruits
{
    

    public abstract class BaseFruit
    {
        private double x;
        private double y;

        public abstract double getX();
        public abstract double getY();
    }
}
