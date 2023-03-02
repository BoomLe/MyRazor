using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.models;
using Microsoft.AspNetCore.Authorization;

namespace App.Pages_Blog
{
    public class EditModel : PageModel
    {
        private readonly App.models.AppDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(App.models.AppDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.articles == null)
            {
                return Content("Không Thấy Dữ Liệu");
            }

            var article =  await _context.articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return Content("Không Thấy Dữ Liệu");
            }
            Article = article;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                // kiểm tra cập nhật tạo ra AuthorizationService ở đây
                var cantupdate = await _authorizationService.AuthorizeAsync(this.User,Article, "Cantupdate");
                if(cantupdate.Succeeded)
                {
                    await _context.SaveChangesAsync();

                }
                else
                {
                    return Content("Không có quyền cập nhật ");
                }
                



            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return Content("Không Thấy Dữ Liệu");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
          return (_context.articles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
