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
    public class InvoiceDetailsController : Controller
    {
        private readonly ILogger<InvoiceDetailsController> _logger;
        private FSContext FSContext { get; set; }

        public InvoiceDetailsController(ILogger<InvoiceDetailsController> logger, FSContext fSContext)
        {
            _logger = logger;
            FSContext = fSContext;
        }

        public IActionResult Index()
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

            ViewData["Customers"] = GetCustomerSelectedList();

            return View(GetAll());
        }

        [HttpPost]
        public IActionResult Create(InvoiceDetailsVM viewModel)
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
        public IActionResult Update(InvoiceDetailsVM viewModel)
        {
            Edit(viewModel);
            return RedirectToAction("Index");
        }

        #region CRUD - DB
        public List<SelectListItem> GetCustomerSelectedList()
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

        private List<InvoiceDetailsVM> GetAll()
        {
            try
            {
                var all = FSContext.InvoiceDetails
                    .Include(x => x.Invoice).ToList();
                return all.Adapt<List<InvoiceDetailsVM>>();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
                return new List<InvoiceDetailsVM>();
            }
        }

        private void Add(InvoiceDetailsVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<InvoiceDetails>();
                var customerModel = FSContext.Customers.FirstOrDefault(x => x.Id == viewModel.CustomerId);

                if (customerModel != null)
                {
                    var invoiceModel = FSContext.Invoice.FirstOrDefault(x => x.Id == viewModel.InvoiceId);

                    model.Total = model.Qty * model.Price;
                    var itbis = (Decimal)0.18;
                    model.TotalItbis = model.Total * itbis;
                    model.SubTotal = model.Total - model.TotalItbis;

                    if (invoiceModel == null)
                    {
                        invoiceModel = new Invoice();
                        invoiceModel.CustomerId = viewModel.CustomerId;
                        invoiceModel.SubTotal = model.SubTotal;
                        invoiceModel.TotalItbis = model.TotalItbis;
                        invoiceModel.Total = model.Total;
                        FSContext.Add(invoiceModel);
                        FSContext.SaveChanges();
                    }
                    else
                    {
                        invoiceModel.SubTotal += model.SubTotal;
                        invoiceModel.TotalItbis += model.TotalItbis;
                        invoiceModel.Total += model.Total;
                        FSContext.Update(invoiceModel);
                        FSContext.SaveChanges();
                    }

                    model.InvoiceId = invoiceModel.Id;
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

        private void Edit(InvoiceDetailsVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<InvoiceDetails>();
                var customerModel = FSContext.Invoice.FirstOrDefault(x => x.Id == viewModel.InvoiceId);

                if (customerModel != null)
                {
                    FSContext.InvoiceDetails.Update(model);
                    FSContext.SaveChanges();
                }
                else
                    TempData["ErrorMessage"] = "Can't Update, 'Invoice' Not Found";
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
                var model = FSContext.InvoiceDetails.Find(id);

                if (model == null)
                    TempData["ErrorMessage"] = "Can't delete, 'Invoice Details' Not Found";
                else
                {
                    var invoiceModel = FSContext.Invoice.FirstOrDefault(x => x.Id == model.InvoiceId);

                    invoiceModel.SubTotal -= model.SubTotal;
                    invoiceModel.TotalItbis -= model.TotalItbis;
                    invoiceModel.Total -= model.Total;
                    FSContext.Update(invoiceModel);
                    FSContext.SaveChanges();

                    FSContext.InvoiceDetails.Remove(model);
                    FSContext.SaveChanges();
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
