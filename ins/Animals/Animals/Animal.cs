using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    abstract class Animal : Living, Breathing
    {
        abstract public void live(Body body);
        abstract public void breath(Air air);
    }
}
