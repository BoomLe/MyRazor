using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class AddRoleModel : RolePageModel
    {
        public AddRoleModel(RoleManager<IdentityRole> roleManager, AppDbContext mydbcontext) : base(roleManager, mydbcontext)
        {
        }

        public class InputModel
        {
            [DisplayName("Cấp quyền (Claim) Role")]
            [Required(ErrorMessage ="Vui lòng nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải từ {2} đến {1} ký tự")]
            public string? ClaimType{set;get;}

            [DisplayName("Giá trị (Claim) Role")]
            [Required(ErrorMessage ="Vui lòng nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải từ {2} đến {1} ký tự")]
            public string? ClaimValue{set;get;}
        }


        [BindProperty]
        public InputModel Input{set;get;}
        public IdentityRole role{set;get;}

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            role = await _roleManager.FindByIdAsync(roleid);
            if(role ==null) return NotFound("Không tìm thấy dữ liệu Roles");
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            role = await _roleManager.FindByIdAsync(roleid);
            if(role == null) return NotFound("Không tìm thấy dữ liệu Roles");
            if(!ModelState.IsValid)
            {
                return Page();

            }
            if((await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == Input.ClaimType && c.Value == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty , "Claim này đã có trông Role");
                return Page();

            }
            var newClaim = new Claim(Input.ClaimType, Input.ClaimValue);
            var result = await _roleManager.AddClaimAsync( role, newClaim);
            if(!result.Succeeded)
            {
                result.Errors.ToList().ForEach(r => 
                {
                    ModelState.AddModelError(string.Empty, r.Description);

                });
                 return Page();

            }

            StatusMessage = "Tạo thành công Role (Claim)";

            return RedirectToPage("./Edit", new {roleid = role.Id});
           


            
        }
    }
}
