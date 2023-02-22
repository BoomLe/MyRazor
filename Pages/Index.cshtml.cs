using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EFWebRazor.models;
namespace EFWebRazor.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MyDbContext mydbContext;

    public IndexModel(ILogger<IndexModel> logger,MyDbContext _mydbContext)
    {
        _logger = logger;
        mydbContext = _mydbContext;
    }

    public void OnGet()
    {
        var qr = (from a in mydbContext.articles
        orderby a.Created descending
         select a).ToList();

         ViewData["post"] = qr;

    }
}
