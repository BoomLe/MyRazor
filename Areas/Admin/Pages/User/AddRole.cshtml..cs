using System.Collections.Generic;
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EFWebRazor.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.User
{
    [Authorize]
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<MyAppUser> _userManager;
        private readonly SignInManager<MyAppUser> _signInManager;

        private readonly RoleManager<IdentityRole> _identityRole;

        public AddRoleModel(
            UserManager<MyAppUser> userManager,
            SignInManager<MyAppUser> signInManager,
            RoleManager<IdentityRole> identityRole)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityRole = identityRole;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
      
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
     
        public MyAppUser user{set;get;}

        [BindProperty]
        [DisplayName("Gán Roles")]
        public string[] RoleName{set;get;}

        public SelectList allRoles{set;get;} 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if(string.IsNullOrEmpty(id))
            // if(id != null)
            {
                return NotFound("Không tìm thấy User");
            }
             user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($" Không tìm thấy User , id = {id}.");
            }

           string[] RoleName = (await _userManager.GetRolesAsync(user)).ToArray<string>();


            List<string> roleName =  await _identityRole.Roles.Select(x => x.Name).ToListAsync();
           allRoles = new SelectList(roleName);

           

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
             if(string.IsNullOrEmpty(id))
            {
                return NotFound("Không tìm thấy User");
            }
             user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($" Không tìm thấy User , id = {id}.");
            }
            
            var oldRoleName = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRole = oldRoleName.Where(r => !RoleName.Contains(r));
            var addRole = RoleName.Where(r => !oldRoleName.Contains(r) );

           List<string> roleName =  await _identityRole.Roles.Select(x => x.Name).ToListAsync();
           allRoles = new SelectList(roleName);

            var resultdelete = await _userManager.RemoveFromRolesAsync(user,deleteRole) ;
            if (!resultdelete.Succeeded)
            {
               resultdelete.Errors.ToList().ForEach(error => 
               {
                ModelState.AddModelError(string.Empty, error.Description);

               });
               return Page();
                
            }

              var resultAdd = await _userManager.AddToRolesAsync(user, addRole);
            if (!resultAdd.Succeeded)
            {
               resultAdd.Errors.ToList().ForEach(error => 
               {
                ModelState.AddModelError(string.Empty, error.Description);

               });
               return Page();
                
            }

            StatusMessage = $"Vừa cập nhật Role cho user :{user.UserName}";
            return RedirectToPage("./Index");
            

        
        }
    }
}
