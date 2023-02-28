using System.Reflection.Metadata.Ecma335;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, MyDbContext mydbcontext) : base(roleManager, mydbcontext)
        {
        }

   
        public IdentityRole role{set;get;}

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy Roles");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null)
            {
               return NotFound("Không tìm thấy Roles");
                
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy Role ");
            role = await _roleManager.FindByIdAsync(roleid);
             if(role == null) return NotFound("Không tìm thấy Role ");
          
           
           
           var result = await _roleManager.DeleteAsync(role);

            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa : {role.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                });
            }


            return Page();
        }
    }
}
