using System.Security.Claims;
using System.Collections.Generic;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace App.Admin.Roles
{
    // [Authorize(Roles ="Admin")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, MyDbContext mydbcontext) : base(roleManager, mydbcontext)
        {
        }

        public class RoleModel : IdentityRole
        {
            public string[]? Claims {set;get;}
        }

        public List<RoleModel> roles {set;get;}

        public async Task OnGet()
        {
           var  r = await _roleManager.Roles.OrderBy(p => p.Name).ToListAsync();
           roles = new List<RoleModel>();
           foreach (var _r in r)
           {
              var claims = await _roleManager.GetClaimsAsync(_r);
              var claimString = claims.Select(n => n.Type + "=" + n.Value);

              var rm = new RoleModel()
              {
                Id = _r.Id,
                Name = _r.Name,
                Claims = claimString.ToArray()
              };
              roles.Add(rm);

            
           }
            
        }

        public void OnPost()=> RedirectToPage();
    }
}