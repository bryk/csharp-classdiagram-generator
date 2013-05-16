using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    namespace Inner2
    {
        using Inner1;
        class Class5: Class4
        {
        }
        namespace Inner3
        {
            class Class6 : Class4
            {

            }
            namespace Inner4
            {
                class Class7 : Class1
                {
                }
            }
        }
    }
}
