using Agero.Core.Validator.Attributes;
using System.Runtime.Serialization;

namespace Agero.Core.ApiResponse.Web.Models
{
    [DataContract]
    public class Name
    {
        [StringValidate("First name should have a minimum of 2 characters and no more than 60 characters.", MinLength = 2, MaxLength = 60, CanBeEmpty = false, CanBeNull = false)]
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [StringValidate("Second name should have a minimum of 2 characters and no more than 60 characters.", MinLength = 2, MaxLength = 60, CanBeEmpty = false, CanBeNull = false)]
        [DataMember(Name = "secondName")]
        public string SecondName { get; set; }
    }
}