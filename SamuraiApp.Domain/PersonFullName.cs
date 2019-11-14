using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class PersonFullName
    {
        public PersonFullName(string givenName, string surName)
        {
            SurName = surName;
            GivenName = givenName;
        }

        private PersonFullName() { }

        public string SurName { get; set; }
        public string GivenName { get; set; }

        public string FullName => $"{GivenName} {SurName}";
        public string FullNameReverse => $"{SurName} {GivenName}";
    }
}
