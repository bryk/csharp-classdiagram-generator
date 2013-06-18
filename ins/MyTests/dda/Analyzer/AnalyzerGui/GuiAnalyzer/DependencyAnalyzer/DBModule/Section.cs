using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiAnalyzer.DependencyAnalyzer.DBModule
{
    class Section
    {
        public String SectionName { get; set; }

        public Section(String SectionName)
        {
            this.SectionName = SectionName;
        }
    }
}
