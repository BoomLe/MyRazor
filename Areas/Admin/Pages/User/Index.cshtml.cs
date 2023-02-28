using System.Linq;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace App.Admin.User
{
    [Authorize]
    public class IndexModel : PageModel
    {
       

        private readonly UserManager<MyAppUser> _appuserManager;
        public IndexModel(UserManager<MyAppUser> appuserManager)
        {
            _appuserManager = appuserManager;
        }

        
        
        public const int ITEMS_PER_PAGE = 15;

        [BindProperty(SupportsGet =true, Name = "p")]
        public int currentPage{set;get;}

        public int countPage{set;get;}

        public int totalUsers{set;get;}

        [TempData]
        public string StatusMessage{set;get;}

        public class UserAndRole : MyAppUser
        {
            public string? RoleNames{set;get;} 
        }
        public List<UserAndRole> users {set;get;}

        public async Task OnGet()
        {
        //    users = await _appuserManager.Users.OrderBy(u => u.UserName).ToListAsync();
        var qr = _appuserManager.Users.OrderBy(u => u.UserName);
        
        totalUsers = await qr.CountAsync();
        countPage = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);
        
        if(currentPage < 1)
        currentPage =1;
        if(currentPage > countPage)
        currentPage = countPage;
        var qr1 = qr.Skip((currentPage -1) * ITEMS_PER_PAGE)
        .Take(ITEMS_PER_PAGE)
        .Select(u => new UserAndRole()
        {
            Id = u.Id,
            UserName = u.UserName
        });

        users =await qr1.ToListAsync();

        foreach(var user in users)
        {
            var roles = await _appuserManager.GetRolesAsync(user);
            user.RoleNames = string.Join(",", roles);       
             }
        
        }

        public void OnPost()=> RedirectToPage();
    }
}