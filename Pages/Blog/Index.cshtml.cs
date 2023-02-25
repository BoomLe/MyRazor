using System.Net;
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

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EFWebRazor.models.MyDbContext _context;
          // tổng số bài viết trên 1 trang:
        public const int ITEMS_PER_PAGE = 10;

        [BindProperty(SupportsGet =true, Name = "p")]
        public int currentPage{set;get;}

        public int countPages{set;get;}

        public IndexModel(EFWebRazor.models.MyDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        public async Task OnGetAsync(string SearchString)
        {
            
                // Article = await _context.articles.ToListAsync();

                int totalPages = await _context.articles.CountAsync();
                countPages = (int)Math.Ceiling((double)totalPages/ITEMS_PER_PAGE);
                if(currentPage < 1)
                currentPage = 1;
                if (currentPage > countPages)
                currentPage = countPages;
                


                
                var qr = (from p in _context.articles 
                orderby p.Created descending
                select p)
                .Skip((currentPage - 1) * ITEMS_PER_PAGE)
                .Take(ITEMS_PER_PAGE);

                if(!string.IsNullOrEmpty(SearchString))
                 
                {

                 Article = qr.Where(a=> a.Title.Contains(SearchString)).ToList();

                //  var Article = (from a in qr
                //  where a=> (a.Title.Contains(Searching)).ToString()
                //  select a).ToList();
                
                }
                else
                {
                    Article = await qr.ToListAsync();

                }

                
            
        }
    }
}
