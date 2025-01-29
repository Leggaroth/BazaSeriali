using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektWebowy.Data;
using ProjektWebowy.Models;

namespace ProjektWebowy.Controllers
{
    public class SerialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SerialsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Serials
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber, int? categoryId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ScoreSortParm"] = sortOrder == "score_desc" ? "Score" : "score_desc";

            if (searchString != null)
            {
                pageNumber = 1;
            } else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CategoryFilter"] = categoryId;

            var serials = _context.Serials.Include(s => s.Category).Include(s => s.Ratings).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                serials = serials.Where(s => s.Title.Contains(searchString));
            }

            if (categoryId.HasValue)
            {
                serials = serials.Where(s => s.CategoryId == categoryId.Value);
            }


            switch (sortOrder)
            {
                case "title_desc":
                    serials = serials.OrderByDescending(s => s.Title);
                    break;
                case "Score":
                    serials = serials.OrderBy(s => s.Ratings.Any() ? s.Ratings.Average(r => r.Value) : 0);
                    break;
                case "score_desc":
                    serials = serials.OrderByDescending(s => s.Ratings.Any() ? s.Ratings.Average(r => r.Value) : 0);
                    break;
                default:
                    serials = serials.OrderBy(s => s.Title);
                    break;
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;

            int pageSize = 5;
            return View(await PaginatedList<Serial>.CreateAsync(serials,pageNumber ?? 1, pageSize));
        }

        // GET: Serials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var serial = await _context.Serials
                .Include(s => s.Category)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .Include(s => s.Ratings)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serial == null) return NotFound();

            var ratingDistribution = new int[10];
            foreach (var rating in serial.Ratings)
            {
                if (rating.Value >= 1 && rating.Value <= 10)
                {
                    ratingDistribution[rating.Value - 1]++;
                }
            }

            ViewBag.RatingDistribution = ratingDistribution;

            return View(serial);
        }

        // GET: Serials/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Serials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Serial serial)
        {
            _context.Add(serial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Serials/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serial = await _context.Serials.FindAsync(id);
            if (serial == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(serial);
        }

        // POST: Serials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Serial serial)
        {
            if (id != serial.Id)
            {
                return NotFound();
            }
            try
            {
                _context.Update(serial);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerialExists(serial.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Serials/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serial = await _context.Serials
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serial == null)
            {
                return NotFound();
            }

            return View(serial);
        }

        // POST: Serials/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serial = await _context.Serials.FindAsync(id);
            if (serial != null)
            {
                _context.Serials.Remove(serial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerialExists(int id)
        {
            return _context.Serials.Any(e => e.Id == id);
        }
    }
}
