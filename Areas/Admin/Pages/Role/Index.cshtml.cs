using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace App.Admin.Roles
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, MyDbContext mydbcontext) : base(roleManager, mydbcontext)
        {
        }

        public List<IdentityRole> roles {set;get;}

        public async Task OnGet()
        {
            roles = await _roleManager.Roles.OrderBy(p => p.Name).ToListAsync();
        }

        public void OnPost()=> RedirectToPage();
    }
}