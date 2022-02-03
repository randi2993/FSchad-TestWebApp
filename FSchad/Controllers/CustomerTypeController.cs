using FSchad.Models;
using FShad.Data;
using FShad.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSchad.Controllers
{
    public class CustomerTypeController : Controller
    {
        private readonly ILogger<CustomerTypeController> _logger;
        private FSContext FSContext { get; set; }

        public CustomerTypeController(ILogger<CustomerTypeController> logger, FSContext fSContext)
        {
            _logger = logger;
            FSContext = fSContext;
        }

        public IActionResult Index()
        {
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(GetAll());
        }

        [HttpPost]
        public IActionResult Create(CustomerTypesVM viewModel)
        {
            Add(viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            Remove(id);
            return RedirectToAction("Index", ViewBag);
        }

        [HttpPost, ActionName("Update")]
        public IActionResult Update(CustomerTypesVM viewModel)
        {
            Edit(viewModel);
            return RedirectToAction("Index");
        }

        #region CRUD - DB
        private List<CustomerTypesVM> GetAll()
        {
            try
            {
                var all = FSContext.CustomerTypes.ToList();
                return all.Adapt<List<CustomerTypesVM>>();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
                return new List<CustomerTypesVM>();
            }
        }

        private void Add(CustomerTypesVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<CustomerTypes>();
                FSContext.Add(model);
                FSContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Exception, {ex.Message ?? ex.InnerException?.Message}";
            }
        }

        private void Edit(CustomerTypesVM viewModel)
        {
            try
            {
                var model = viewModel.Adapt<CustomerTypes>();
                FSContext.CustomerTypes.Update(model);
                FSContext.SaveChanges();
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
                var model = FSContext.CustomerTypes.Find(id);

                if (model == null)
                    TempData["ErrorMessage"] = "Can't delete, 'Customer Type' Not Found";
                else
                {
                    var modelCustomer = FSContext.Customers.FirstOrDefault(x => x.CustomerTypeId == id);

                    if (modelCustomer != null)
                        TempData["ErrorMessage"] = "Can't delete, 'Customer Type' is related to Customer(s)";
                    else
                    {
                        FSContext.CustomerTypes.Remove(model);
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
