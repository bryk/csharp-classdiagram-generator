using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animals
{
    using Mammal;
    using Wildness;
    using Royal;
    namespace Lion{
            namespace KingLion{
                class KingLion: Mammal.Mammal, Predator.Predator, Wild, Royal.Royal
                    {
                        protected void rule(){}
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

}
