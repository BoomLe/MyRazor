using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EFWebRazor.models;
using Microsoft.AspNetCore.Authorization;

namespace EFWebRazor.Pages_Blog
{
    [Authorize(Policy = "IsGenz")] // năm sinh phải 1997 > 2012 mới được Login
    public class DetailsModel : PageModel
    {
        private readonly EFWebRazor.models.MyDbContext _context;

        public DetailsModel(EFWebRazor.models.MyDbContext context)
        {
            _context = context;
        }

      public Article Article { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.articles == null)
            {
                return NotFound();
            }

            var article = await _context.articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            else 
            {
                Article = article;
            }
            return Page();
        }
    }
}
