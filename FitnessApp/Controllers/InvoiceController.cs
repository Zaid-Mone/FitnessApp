using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using FitnessApp.Utility;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SendMemberInvoiceMessage _sendMemberInvoiceMessage;
        public InvoiceController(ApplicationDbContext context, 
            SendMemberInvoiceMessage sendMemberInvoiceMessage)
        {
            _context = context;
            _sendMemberInvoiceMessage = sendMemberInvoiceMessage;
        }

        // GET: Invoice
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices
                .Include(i => i.Member)
                .ThenInclude(q=>q.Person);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {

                invoice.Id = Guid.NewGuid().ToString();
                invoice.UserPayDate = DateTime.Now;
                invoice.SerialNumber = Math.Abs(Guid.NewGuid().GetHashCode()).ToString().Substring(0, 5);
                if (invoice.Userpays == invoice.TotalAmount)
                {
                    invoice.RemainingValue = 0;
                    invoice.IsFullyPaid = true;   
                }
                else if(invoice.Userpays > invoice.TotalAmount)
                {

                    // Display Error Message if something went wrong.
                    ViewBag.msg = false;
                    ViewBag.balance = invoice.TotalAmount.ToString("C");
                    return View(invoice);
                }
                else
                {
                    invoice.RemainingValue = invoice.TotalAmount - invoice.Userpays;
                    invoice.IsFullyPaid = false;
                }
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                // send message to the member
                _sendMemberInvoiceMessage.SendInvoiceMessage(invoice.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }



        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(q=>q.Member)
                .Include(r=>r.Member.Person)
                .SingleOrDefaultAsync(q=>q.Id==id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(string id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }




        public ActionResult GetMemberInvoice()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var invoice = _context.Invoices
                .Include(q => q.Member)
                .Include(q => q.Member.Person)
                .Where(q => q.Member.PersonId == userId).ToList();

            return View(invoice);
        }
    }
}
