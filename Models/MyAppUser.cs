using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace App.models
 {
    public class MyAppUser : IdentityUser
    {
        [Column(TypeName ="nvarchar")]
        [StringLength(400)]
        public string? HomeAddrss{set;get;}


        [DataType(DataType.Date)]
        public DateTime? BrithDate{set;get;}

    }
}