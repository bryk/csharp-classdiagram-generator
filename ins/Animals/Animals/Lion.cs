using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    namespace Lion
    {
        using Wildness;
        class Lion : Mammal.Mammal, Predator.Predator, Wild
        {

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
