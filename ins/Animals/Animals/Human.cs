using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    using Mammal;
    namespace Human{
        class Human : Mammal.Mammal, Thinkness.Think, Predator.Predator
        {
            private void think()
            {
            }
            public void attack(Animal animal)
            {
            }
            public void live(Body body)
            {
            }
            public void breath(Air air) { }
        }
    }
}
