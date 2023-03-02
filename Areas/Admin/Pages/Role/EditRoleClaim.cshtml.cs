using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class EditRoleClaimModel : RolePageModel
    {
        public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, MyDbContext mydbcontext) : base(roleManager, mydbcontext)
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

        public IdentityRoleClaim<string> Claims {set;get;}

        public async Task<IActionResult> OnGetAsync(int? claimid)
        {
            if(claimid == null) return NotFound("Không tìm Roles");
            Claims =  _mydbcontext.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if(claimid == null) return NotFound("Không tìm Roles");

            role = await _roleManager.FindByIdAsync(Claims.RoleId);
            if(role ==null) return NotFound("Không tìm thấy dữ liệu Roles");

            Input = new InputModel()
            {
                ClaimType = Claims.ClaimType,
                ClaimValue = Claims.ClaimValue,
            };
            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int claimid)
        {
           if(claimid == null) return NotFound("Không tìm Roles");
            Claims =  _mydbcontext.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if(claimid == null) return NotFound("Không tìm Roles");

            role = await _roleManager.FindByIdAsync(Claims.RoleId);
            if(role ==null) return NotFound("Không tìm thấy dữ liệu Roles");
            if(!ModelState.IsValid)
            {
                return Page();

            }
            if((_mydbcontext.RoleClaims.Any(c=> c.Id == claimid && c.ClaimType == Input.ClaimType && Input.ClaimValue == c.ClaimValue)))
            {
                ModelState.AddModelError(string.Empty , "Claim này đã có trông Role");
                return Page();

            }
           Claims.ClaimType = Input.ClaimType;
           Claims.ClaimValue = Input.ClaimValue;

           await _mydbcontext.SaveChangesAsync( );

    
            StatusMessage = "cập nhật thành công Role (Claim)";

            return RedirectToPage("./Edit", new {roleid = role.Id});
           


            
        }

         public async Task<IActionResult> OnPostDeleteAsync(int claimid)
        {
           if(claimid == null) return NotFound("Không tìm Roles");
            Claims =  _mydbcontext.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if(claimid == null) return NotFound("Không tìm Roles");

            role = await _roleManager.FindByIdAsync(Claims.RoleId);
            if(role ==null) return NotFound("Không tìm thấy dữ liệu Roles");

            await _roleManager.RemoveClaimAsync(role, new Claim(Input.ClaimType, Input.ClaimValue));


      
    
            StatusMessage = "xóa thành công Role (Claim)";

            return RedirectToPage("./Edit", new {roleid = role.Id});
           


            
        }





    }
}
