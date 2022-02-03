using FSchad.Models;
using FShad.Data;
using FShad.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSchad.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private FSContext FSContext { get; set; }

        public InvoiceController(ILogger<InvoiceController> logger, FSContext fSContext)
        {
            _logger = logger;
            FSContext = fSContext;
        }

        public IActionResult Index()
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

            ViewData["Customers"] = GetCustomersSelectedList();

            return View(GetAll());
        }

        [HttpPost]
        public IActionResult Create(InvoiceVM viewModel)
        {
            Add(viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            Remove(id);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Update")]
        public IActionResult Update(InvoiceVM viewModel)
        {
            Edit(viewModel);
            return RedirectToAction("Index");
        }

        #region CRUD - DB
        public List<SelectListItem> GetCustomersSelectedList()
        {
            try
            {
                var model = FSContext.Customers.ToList();
                var selectListItems = new List<SelectListItem>();

                foreach (var element in model)
                {
                    selectListItems.Add(new SelectListItem() { Text = element.CustName, Value = element.Id.ToString() });
                }

                return selectListItems;
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
                return null;
            }
        }
        private List<InvoiceVM> GetAll()
        {
            try
            {
                var all = FSContext.Invoice
                    .Include(x => x.Customer).ToList();
                return all.Adapt<List<InvoiceVM>>();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
                return new List<InvoiceVM>();
            }
        }

        private void Add(InvoiceVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<Invoice>();
                var customerModel = FSContext.Customers.FirstOrDefault(x => x.Id == viewModel.CustomerId);

                if (customerModel != null)
                {
                    FSContext.Add(model);
                    FSContext.SaveChanges();
                }
                else
                    TempData["ErrorMessage"] = "Can't Create, 'Customer' Not Found";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
            }
        }

        private void Edit(InvoiceVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<Invoice>();
                var customerModel = FSContext.Customers.FirstOrDefault(x => x.Id == viewModel.CustomerId);

                if (customerModel != null)
                {
                    FSContext.Invoice.Update(model);
                    FSContext.SaveChanges();
                }
                else
                    TempData["ErrorMessage"] = "Can't Update, 'Customer' Not Found";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
            }
        }

        private void Remove(int id)
        {
            try
            {
                var model = FSContext.Invoice.Find(id);

                if (model == null)
                    TempData["ErrorMessage"] = "Can't delete, 'Invoice' Not Found";
                else
                {
                    var modelInvoice = FSContext.InvoiceDetails.FirstOrDefault(x => x.InvoiceId == id);

                    if (modelInvoice != null)
                        TempData["ErrorMessage"] = "Can't delete, 'Invoice' is related to Invoice Detail(s)";
                    else
                    {
                        FSContext.Invoice.Remove(model);
                        FSContext.SaveChanges();
                    }
                }
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
            }
        }
        #endregion
    }
}
