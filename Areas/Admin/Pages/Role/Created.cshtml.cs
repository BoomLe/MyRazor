using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class CreatedModel : RolePageModel
    {
        public CreatedModel(RoleManager<IdentityRole> roleManager, AppDbContext mydbcontext) : base(roleManager, mydbcontext)
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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var roleNew = new IdentityRole(Input.Name);
            
            var result = await _roleManager.CreateAsync(roleNew);

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
