
using App.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class RolePageModel :PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;

        protected readonly AppDbContext _mydbcontext;


        [TempData]
        public string StatusMessage{set;get;}

        public RolePageModel(RoleManager<IdentityRole> roleManager, AppDbContext mydbcontext)
        {
            _roleManager = roleManager;
            _mydbcontext = mydbcontext;

        }


    }
}