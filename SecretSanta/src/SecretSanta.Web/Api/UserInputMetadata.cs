using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.Api
{
    [ModelMetadataType(typeof(UserInputMetadata))]
    public partial class UserInput
    {
    }
    public class UserInputMetadata
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
    }
}



