using System;

namespace PersistenceLayer
{
    public class Project
    {
        public String Description { get; set; }
        public PublicUserInfo Manager { get; set; }
        public String Name { get; set; }
        private Double _budget;

        public Double Budget
        {
            get { return _budget; }
            set { _budget = value; }
        }

        public Project() {
            Budget = 500;
    }
        public override string ToString()
        {
            
            return Name;
        }
    }
}
