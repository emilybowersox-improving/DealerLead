using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealerLead;
using System.Security.Claims;
using static DealerLead.Web.Controllers.HomeController;

namespace DealerLead.Web.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public VehiclesController(DealerLeadDbContext context)
        {
            _context = context;
        }

       


        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            Guid Oid = (Guid)IdentityHelper.GetAzureOIDToken(principal);
            var dealerUser = _context.DealerLeadUser.FirstOrDefault(u => u.Oid == Oid);
            var currentUserId = dealerUser.Id;

            // where 

            var dealerLeadDbContext = _context.Vehicle.Include(v => v.Dealership).Include(v => v.Model).Where(v => v.Dealership.CreatorId == currentUserId);

            return View(await dealerLeadDbContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Dealership).Include(v => v.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ClaimsPrincipal principal = User as ClaimsPrincipal;
            Guid Oid = (Guid)IdentityHelper.GetAzureOIDToken(principal);
            var dealerUser = _context.DealerLeadUser.FirstOrDefault(u => u.Oid == Oid);
            var currentUserId = dealerUser.Id;

            var usersDealerships = _context.Dealership.Where(d => d.CreatorId == currentUserId);

            ViewData["DealershipSelectList"] = new SelectList(usersDealerships, "Id", "Name");
            ViewData["ModelSelectList"] = new SelectList(_context.SupportedModel, "Id", "Name");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelId,MSRP,StockNumber,Color,DealershipId,SellDate")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DealershipSelectList"] = new SelectList(_context.Dealership, "Id", "Name", vehicle.DealershipId);
            ViewData["ModelSelectList"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ClaimsPrincipal principal = User as ClaimsPrincipal;
            Guid Oid = (Guid)IdentityHelper.GetAzureOIDToken(principal);
            var dealerUser = _context.DealerLeadUser.FirstOrDefault(u => u.Oid == Oid);
            var currentUserId = dealerUser.Id;

            var usersDealerships = _context.Dealership.Where(d => d.CreatorId == currentUserId);

            var vehicle = await _context.Vehicle.FindAsync(id);
            ViewData["DealershipSelectList"] = new SelectList(usersDealerships, "Id", "Name");
            ViewData["ModelSelectList"] = new SelectList(_context.SupportedModel, "Id", "Name");

            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelId,MSRP,StockNumber,Color,DealershipId,SellDate")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            ViewData["DealershipSelectList"] = new SelectList(_context.Dealership, "Id", "Name", vehicle.DealershipId);
            ViewData["ModelSelectList"] = new SelectList(_context.SupportedModel, "Id", "Name", vehicle.ModelId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Dealership).Include(v => v.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }





        public static class IdentityHelper
        {
            public static Guid? GetAzureOIDToken(ClaimsPrincipal claimsPrincipal)
            {
                if (claimsPrincipal == null)
                {
                    return null;
                }
                if (claimsPrincipal.Identity.IsAuthenticated == false)
                {
                    return null;
                }
                var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
                if (claimsIdentity == null)
                {
                    return null;
                }
                var oidClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
                if (oidClaim == null)
                {
                    return null;
                }
                return Guid.Parse(oidClaim.Value);
            }
        }



    }

}
