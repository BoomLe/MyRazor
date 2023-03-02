using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.models;
namespace App.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext mydbContext;

    public IndexModel(ILogger<IndexModel> logger,AppDbContext _mydbContext)
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
