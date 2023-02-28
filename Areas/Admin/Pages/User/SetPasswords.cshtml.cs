// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EFWebRazor.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace App.Admin.User
{
    // [Authorize]
    public class SetPasswordsModel : PageModel
    {
        private readonly UserManager<MyAppUser> _userManager;
        private readonly SignInManager<MyAppUser> _signInManager;

        public SetPasswordsModel(
            UserManager<MyAppUser> userManager,
            SignInManager<MyAppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = " {0} phải từ {2} đến {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string NewPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Lặp lại mật khẩu")]
            [Compare("NewPassword", ErrorMessage = "Lặp lại mật khẩu không chính xác.")]
            public string ConfirmPassword { get; set; }

            // public MyAppUser user{set;get;}
        }
        public MyAppUser user{set;get;}

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



            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userManager.RemovePasswordAsync(user);

           

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            
            StatusMessage = $"Mật khẩu đã thay đổi cho {user.UserName} .";

            return RedirectToPage("./Index");
        }
    }
}
