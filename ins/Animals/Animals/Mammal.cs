using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    namespace Mammal
    {
        abstract class Mammal : Animal
        {
            abstract public void live(Body body);
            abstract public void breath(Air air);
        }
    }
}
