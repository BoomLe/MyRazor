using System.Threading.Tasks;
using System.Security.Claims;
using App.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace App.Sercurity.Requirement
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
       private readonly UserManager<MyAppUser> _userManager;
    private readonly ILogger<AppAuthorizationHandler> _logger;

    // Inject UserManager vào AppAuthorizationHandler
    public AppAuthorizationHandler(UserManager<MyAppUser> userManager, ILogger<AppAuthorizationHandler> logger)
    {
      _userManager = userManager;
      _logger = logger;
    }
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
             // lấy các requirement chưa được kiểm tra trong ngữ cảnh xác thực hiện tại
            var requirement = context.PendingRequirements.ToList();

            foreach (var requirements in requirement)
            {
                if(requirements is GeZRequirement)
                { 
                    if(IsGenz(context.User, (GeZRequirement)requirements))
                    context.Succeed(requirements);
                

                }
                if(requirements is ArticleCantupdate)
                {
                    bool cantUpdate = cantUpdateArticle(context.User, context.Resource, (ArticleCantupdate)requirements);
                }
                
            }
            return Task.CompletedTask;
        }

        private bool cantUpdateArticle(ClaimsPrincipal user, object? resource, ArticleCantupdate requirements)
        {
            if(user.IsInRole("Admin"))
            {
                _logger.LogInformation("Admin được cập nhật ..");
                return true;
            } 
            var article = resource as Article;
            var datecreated = article.Created;
            var datacantupdated = new DateTime(requirements.Year, requirements.Month, requirements.Date);

            if(datecreated <= datacantupdated)
            {
                _logger.LogInformation("Quá ngày cập nhật");
                return false;
            }
            return true;
        }

        private bool IsGenz(ClaimsPrincipal user, GeZRequirement  requirements)

        {

            var AppUserTask = _userManager.GetUserAsync(user);
            Task.WaitAll(AppUserTask);
            var AppUser = AppUserTask.Result;
            if(AppUser.BrithDate == null) 
            {
                _logger.LogInformation($"{AppUser.UserName} Khong có ngày sinh thỏa mảng GenZRequirement  ");
                return false;}
            int year = AppUser.BrithDate.Value.Year;
           var success = (year >= requirements.FromYear && year <= requirements.ToYear);
           if(success)
           {
                _logger.LogInformation($"{AppUser.UserName} thỏa mảng GenZRequirement  ");
  
           }
           else
           {
            _logger.LogInformation($"{AppUser.UserName} Khong có ngày sinh thỏa mảng GenZRequirement  ");
           }
           
           return success;

        }
    }
}