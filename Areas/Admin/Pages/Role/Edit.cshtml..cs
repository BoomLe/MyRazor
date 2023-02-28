using System.Reflection.Metadata.Ecma335;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class EditdModel : RolePageModel
    {
        public EditdModel(RoleManager<IdentityRole> roleManager, MyDbContext mydbcontext) : base(roleManager, mydbcontext)
        {
        }

        public class InputModel
        {
            [DisplayName("Tên của Role")]
            [Required(ErrorMessage ="Vui lòng nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải từ {2} đến {1} ký tự")]
            public string Name{set;get;}
        }

        [BindProperty]
        public InputModel Input{set;get;}

        public IdentityRole role{set;get;}

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy Roles");
            role = await _roleManager.FindByIdAsync(roleid);
            if(role != null)
            {
                Input = new InputModel() 
                {
                     Name = role.Name,
                };
                return Page();
            }
            return NotFound("Không tìm thấy Roles");

        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if(roleid == null) return NotFound("Không tìm thấy Role ");
            role = await _roleManager.FindByIdAsync(roleid);
             if(role == null) return NotFound("Không tìm thấy Role ");
          
            if(!ModelState.IsValid)
            {
                return Page();
            }
           
           role.Name = Input.Name;
           var result = await _roleManager.UpdateAsync(role);

            if(result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo thành công : {Input.Name}";
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
