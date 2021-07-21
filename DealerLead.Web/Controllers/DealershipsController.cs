using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DealerLead.Web.Controllers
{
    public class DealershipsController : Controller
    {

        private readonly DealerLeadDbContext _context;

        public DealershipsController(DealerLeadDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Dealership.ToListAsync());
          
        }

        public IActionResult Create()
        {
            ViewData["StateSelectList"] = new SelectList(_context.SupportedState, "Abbreviation", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address1,Address2,City,StateAbbreviation,Zip,CreatorId")] Dealership dealership)
        {            
            if (ModelState.IsValid)
            {
                ClaimsPrincipal principal = User as ClaimsPrincipal;
                Guid Oid = (Guid)IdentityHelper.GetAzureOIDToken(principal);
                var creator = _context.DealerLeadUser.FirstOrDefault(u => u.Oid == Oid);
                var creatorID = creator.Id;

                dealership.CreatorId = creatorID;

                _context.Add(dealership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateSelectList"] = new SelectList(_context.SupportedState, "Abbreviation", "Name", dealership.State);
            /*    ViewData["MakeID"] = new SelectList(_context.SupportedMake, "ID", "MakeName", supportedModel.MakeID);
            return View(supportedModel);*/
            return View(dealership);
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
