using Store.Data_Access.Repository;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private ProductRepository repository =new ProductRepository();

        public ActionResult List()
        {
            IEnumerable<Product> model = repository.GetAllProducts();

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(Product newProduct)
        {

            if (!ModelState.IsValid)
            {
                return View(newProduct);
            }

            repository.AddProduct(newProduct);

            return View("List");
        }

        [HttpGet]
        public ActionResult EditProduct(int productID)
        {
            Product product = repository.GetProduct(productID);

            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            repository.UpdateProduct(product);

            return RedirectToAction("List");
        }

        public ActionResult DeleteProduct(int? productID)
        {
            if (productID.HasValue)
            {
                repository.Delete(productID.Value);
            }

            return RedirectToAction("List");
        }

        public ActionResult GenerateProducts()
        {
            repository.GenerateProduct();

            return RedirectToAction("List");
        }
    }
}