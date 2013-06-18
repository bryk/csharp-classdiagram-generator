using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    namespace HouseAnimal
    {
        using Cat;
        namespace Dog
        {
            class Dog : HouseAnimal, Touch.Touch
            {
                public void touch();

                public void live(Body body)
                {
                }
                public void breath(Air air) { }
                public void feed()
                {
                }
                public void huntCat(Cat.Cat cat) { }
            }
        }
    }
}
