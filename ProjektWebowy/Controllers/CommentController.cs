using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjektWebowy.Data;
using ProjektWebowy.Models;

namespace ProjektWebowy.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            comment.UserId = _userManager.GetUserId(User);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Serials", new { id = comment.SerialId });
        }

        // POST: Comment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            // Sprawdź, czy użytkownik jest właścicielem komentarza lub ma rolę Admin
            var isOwner = comment.UserId == _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            if (!isOwner && !isAdmin)
            {
                return Forbid(); // Odmowa dostępu
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Serials", new { id = comment.SerialId });
        }
    }
}
