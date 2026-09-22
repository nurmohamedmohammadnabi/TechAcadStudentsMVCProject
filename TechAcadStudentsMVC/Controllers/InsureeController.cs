using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechAcadStudentsMVC.Data;
using TechAcadStudentsMVC.Models;

namespace TechAcadStudentsMVC.Controllers
{
    public class InsureeController : Controller
    {
        private readonly InsuranceContext _context;

        public InsureeController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Insuree
        public async Task<IActionResult> Index()
        {
            return View(await _context.Insurees.ToListAsync());
        }

        // GET: Insuree/Admin
        public async Task<IActionResult> Admin()
        {
            return View(await _context.Insurees.ToListAsync());
        }

        // GET: Insuree/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees.FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // GET: Insuree/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            insuree.Quote = CalculateQuote(insuree);

            if (ModelState.IsValid)
            {
                _context.Add(insuree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Admin));
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // POST: Insuree/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (id != insuree.Id)
            {
                return NotFound();
            }

            insuree.Quote = CalculateQuote(insuree);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsureeExists(insuree.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Admin));
            }

            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees.FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree != null)
            {
                _context.Insurees.Remove(insuree);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Admin));
        }

        private bool InsureeExists(int id)
        {
            return _context.Insurees.Any(e => e.Id == id);
        }

        private static decimal CalculateQuote(Insuree insuree)
        {
            // a. Start with a base of $50 / month.
            decimal quote = 50m;

            int age = GetAge(insuree.DateOfBirth);

            // b-d. Age adjustments. Every age falls into one of these ranges.
            if (age <= 18)
            {
                quote += 100m;
            }
            else if (age >= 19 && age <= 25)
            {
                quote += 50m;
            }
            else
            {
                // 26 or older
                quote += 25m;
            }

            // e. Car year before 2000.
            if (insuree.CarYear < 2000)
            {
                quote += 25m;
            }

            // f. Car year after 2015.
            if (insuree.CarYear > 2015)
            {
                quote += 25m;
            }

            // g-h. Porsche adds $25. Porsche 911 Carrera adds another $25 ($50 total).
            if (IsPorsche(insuree.CarMake))
            {
                quote += 25m;

                if (IsPorsche911Carrera(insuree.CarModel))
                {
                    quote += 25m;
                }
            }

            // i. $10 for every speeding ticket.
            quote += 10m * insuree.SpeedingTickets;

            // j. DUI adds 25% to the total.
            if (insuree.DUI)
            {
                quote *= 1.25m;
            }

            // k. Full coverage adds 50% to the total.
            if (insuree.CoverageType)
            {
                quote *= 1.50m;
            }

            return decimal.Round(quote, 2);
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private static bool IsPorsche(string? carMake)
        {
            return !string.IsNullOrWhiteSpace(carMake)
                && carMake.Trim().Equals("Porsche", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPorsche911Carrera(string? carModel)
        {
            return !string.IsNullOrWhiteSpace(carModel)
                && carModel.Trim().Equals("911 Carrera", StringComparison.OrdinalIgnoreCase);
        }
    }
}
