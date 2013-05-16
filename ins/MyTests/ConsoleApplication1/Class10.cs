using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    namespace Inner15
    {
        class Class10:Inner16.Class9
        {
        }

        namespace Inner17
        {
            using Inner16;
            class Class12 : Class9
            {
            }

        }
    }
    namespace Inner18
    {
        using Inner15;
        namespace Inner19
        {
            using Inner15.Inner16;
            class Class12 : Class9
            {
            }

        }
    }
}
