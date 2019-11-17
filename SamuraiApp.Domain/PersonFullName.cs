using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class PersonFullName
    {
        public static PersonFullName Create(string givenName, string surName)
        {
            return new PersonFullName(givenName, surName);
        }

        public static PersonFullName Empty()
        {
            return new PersonFullName("", "");
        }
        private PersonFullName(string givenName, string surName)
        {
            SurName = surName;
            GivenName = givenName;
        }

        public bool isEmpty()
        {
            return SurName == "" && GivenName == "";
        }

        public override bool Equals(object obj)
        {
            return obj is PersonFullName name &&
                   SurName == name.SurName &&
                   GivenName == name.GivenName;
        }

        public override int GetHashCode()
        {
            var hashCode = -1052426677;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SurName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GivenName);
            return hashCode;
        }

        private PersonFullName() { }

        public string SurName { get; private set; }
        public string GivenName { get; private set; }

        public string FullName => $"{GivenName} {SurName}";
        public string FullNameReverse => $"{SurName} {GivenName}";

        public static bool operator ==(PersonFullName left, PersonFullName right)
        {
            return EqualityComparer<PersonFullName>.Default.Equals(left, right);
        }

        public static bool operator !=(PersonFullName left, PersonFullName right)
        {
            return !(left == right);
        }
    }
}
