﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DealerLead.Web.Controllers
{
    public class DealershipsController : Controller
    {

        private readonly DealerLeadDbContext _context;

        public DealershipsController(DealerLeadDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address1,Address2,City,State,Zip,CreatorId")] Dealership dealership)
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

            /*    ViewData["MakeID"] = new SelectList(_context.SupportedMake, "ID", "MakeName", supportedModel.MakeID);
            return View(supportedModel);*/
            return RedirectToAction(nameof(Index));
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
