using FSchad.Models;
using FShad.Data;
using FShad.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FSchad.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private FSContext FSContext { get; set; }

        public CustomerController(ILogger<CustomerController> logger, FSContext fSContext)
        {
            _logger = logger;
            FSContext = fSContext;
        }

        public IActionResult Index()
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

            ViewData["CustomerTypes"] = GetCustomerTypesSelectedList();

            return View(GetAll());
        }

        [HttpPost]
        public IActionResult Create(CustomerVM viewModel)
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
        public IActionResult Update(CustomerVM viewModel)
        {
            Edit(viewModel);
            return RedirectToAction("Index");
        }

        #region CRUD - DB
        public List<SelectListItem> GetCustomerTypesSelectedList()
        {
            try
            {
                var model = FSContext.CustomerTypes.ToList();
                var selectListItems = new List<SelectListItem>();

                foreach (var element in model)
                {
                    selectListItems.Add(new SelectListItem() { Text = element.Description, Value = element.Id.ToString() });
                }

                return selectListItems;
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
                return null;
            }
        }

        private List<CustomerVM> GetAll()
        {
            try
            {
                var all = FSContext.Customers
                    .Include(x => x.CustomerType).ToList();
                return all.Adapt<List<CustomerVM>>();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
                return new List<CustomerVM>();
            }
        }

        private void Add(CustomerVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<Customer>();
                var customerModel = FSContext.CustomerTypes.FirstOrDefault(x => x.Id == viewModel.CustomerTypeId);

                if (customerModel != null)
                {
                    FSContext.Add(model);
                    FSContext.SaveChanges();
                }
                else
                    TempData["ErrorMessage"] = "Can't Create, 'Customer Type' Not Found";
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
            }
        }

        private void Edit(CustomerVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<Customer>();
                var customerModel = FSContext.CustomerTypes.FirstOrDefault(x => x.Id == viewModel.CustomerTypeId);

                if (customerModel != null)
                {
                    FSContext.Customers.Update(model);
                    FSContext.SaveChanges();
                }
                else
                    TempData["ErrorMessage"] = "Can't Update, 'Customer Type' Not Found";
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
                var model = FSContext.Customers.Find(id);

                if (model == null)
                    TempData["ErrorMessage"] = "Can't delete, 'Customer' Not Found";
                else
                {
                    var modelInvoice = FSContext.Invoice.FirstOrDefault(x => x.CustomerId == id);

                    if (modelInvoice != null)
                        TempData["ErrorMessage"] = "Can't delete, 'Customer' is related to Invoice(s)";
                    else
                    {
                        FSContext.Customers.Remove(model);
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
