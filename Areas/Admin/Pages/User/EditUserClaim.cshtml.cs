using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EFWebRazor.models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;

namespace App.Admin.User
{
    public class EditUserClaimModel : PageModel
    {

        private readonly MyDbContext _myDbContext;
        private readonly UserManager<MyAppUser> _userManager;

        public EditUserClaimModel(MyDbContext myDbContext,UserManager<MyAppUser> userManager)
        {
            _myDbContext = myDbContext;
            _userManager = userManager;

        }
        public  NotFoundObjectResult  OnGet() =>  NotFound("Khong tìm thấy dữ liệu");
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
        [TempData]
        public string StatusMessage { get; set; }



        [BindProperty]
        public InputModel Input{set;get;}
        public MyAppUser user{set;get;}

        public async Task<IActionResult> OnGetAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if(user == null) return NotFound("Không tìm thấy User của bạn");
            return Page();
        }

         public async Task<IActionResult> OnPostAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if(user == null) return NotFound("Không tìm thấy User của bạn");
            if(!ModelState.IsValid) return Page();

            var claim = _myDbContext.UserClaims.Where(c=> c.UserId == user.Id);

            if(claim.Any(c => c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "đặc tính đã có");
                return Page();
            }

            await _userManager.AddClaimAsync(user , new Claim(Input.ClaimType , Input.ClaimValue));
            StatusMessage = "Chỉnh sửa thành công";
            return RedirectToPage("./AddRole", new {Id = user.Id});
        }
   
          public IdentityUserClaim<string> userClaim{set;get;}
          public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
        {
            
            if(claimid == null) return NotFound("Không tìm thấy User của bạn");

            userClaim =  _myDbContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();

            user = await _userManager.FindByIdAsync(userClaim.UserId);

            if(user == null) return NotFound("Không tìm thấy User của bạn");

            Input = new InputModel()
            {
                ClaimType = userClaim.ClaimType,
                ClaimValue = userClaim.ClaimValue
            };

            return Page();

      

            
        }  
    
    
        public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
        {
            
            if(claimid == null) return NotFound("Không tìm thấy User của bạn");

            userClaim =  _myDbContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            user = await _userManager.FindByIdAsync(userClaim.UserId);

            if(user == null) return NotFound("Không tìm thấy User của bạn");

            if(!ModelState.IsValid) return Page();

            if((_myDbContext.UserClaims.Any(c => c.UserId == user.Id && userClaim.ClaimType == Input.ClaimType 
            && userClaim.ClaimValue == Input.ClaimValue
            && c.Id != userClaim.Id )))
            {
                ModelState.AddModelError(string.Empty, "Claim bị trùng trên hệ thống");
                return Page();
            }



            userClaim.ClaimType = Input.ClaimType; 
            userClaim.ClaimValue = Input.ClaimValue;

            await _myDbContext.SaveChangesAsync();
            StatusMessage = "Cập nhật thành công";


            return RedirectToPage("./AddRole", new{Id = user.Id});

      

            
        }  
    
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            
            if(claimid == null) return NotFound("Không tìm thấy User của bạn");

            userClaim =  _myDbContext.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            user = await _userManager.FindByIdAsync(userClaim.UserId);

            if(user == null) return NotFound("Không tìm thấy User của bạn");

           

          
           await _userManager.RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            StatusMessage = "Xóa dữ liệu thành công";


            return RedirectToPage("./AddRole", new{Id = user.Id});

      

            
        }  
    }
}
