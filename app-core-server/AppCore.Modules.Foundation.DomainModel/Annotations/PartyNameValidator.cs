using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Foundation.DomainModel.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PartyNameValidator : ValidationAttribute
    {
        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is BaseParty)
            {
                var party = (value as BaseParty);
                switch (party.PartyType)
                {
                    case "I":
                        bool hasFirstName = (!String.IsNullOrEmpty(party.FirstName));
                        bool hasSurname = (!String.IsNullOrEmpty(party.Surname));
                        if ((!hasFirstName) || (!hasSurname))
                        {
                            List<string> members = new List<string>();
                            if (!hasFirstName)
                                members.Add("FirstName");
                            if (!hasSurname)
                                members.Add("Surname");

                            return new ValidationResult("Please provide a name", members);
                        }
                        break;
                    case "C":
                        bool hasCoName = (!String.IsNullOrEmpty(party.Name));                        
                        if (!hasCoName)
                        {
                            List<string> members = new List<string>();                            
                            members.Add("Name");                            
                            return new ValidationResult("Please provide a name", members);
                        }
                        break;
                }                
            }
            return ValidationResult.Success;
        }
    }
}
