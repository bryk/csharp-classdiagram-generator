using System;

namespace PersistenceLayer
{
    [Serializable]
    public class PublicUserInfo
    {
        public String Login { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public Double Ballance { get; set; }

        public PublicUserInfo()
        {
        }
        public PublicUserInfo(String Name, String Surname, String Login)
        {
            this.Login = Login;
            this.Name = Name;
            this.Surname = Surname;
        }
        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}
