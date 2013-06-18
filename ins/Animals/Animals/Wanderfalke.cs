using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    namespace Wanderfalke
    {
        using Wildness;
        class Wanderfalke : Bird, Wild,Predator.Predator
        {
            public void attack(Animal animal)
            {
            }
            public void live(Body body)
            {
            }
            public void breath(Air air) { }
            private void fly()
            {
            }
        }
    }
}
