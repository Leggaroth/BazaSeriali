using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektWebowy.Data;
using ProjektWebowy.Models;

namespace ProjektWebowy.Controllers
{
    
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Rating/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int serialId, int value)
        {
            var userId = _userManager.GetUserId(User);
            var existingRating = await _context.Ratings.Where(r => r.UserId == userId).ToListAsync();

            var rating = new Rating
            {
                SerialId = serialId,
                UserId = userId,
                Value = value
            };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Serials", new { id = rating.SerialId });
        }

        // POST: Rating/Delete
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string username)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (rating.UserId != userId)
            {
                return Forbid();
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return Redirect("/Rating/Index/" + username);
        }

        // GET: Rating/Index
        public async Task<IActionResult> Index(string id)
        {
            var user = _userManager.FindByNameAsync(id).Result;
            var ratedSeries = await _context.Ratings
                .Include(r => r.Serial)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();
            ViewBag.UserName = id;

            return View(ratedSeries);
        }
    }
}
