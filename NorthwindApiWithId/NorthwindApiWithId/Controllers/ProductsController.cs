using NorthwindApiWithId.Data;
using NorthwindApiWithId.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace NorthwindApiWithId.Controllers
{
    public class ProductsController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: api/Products
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("GetAll")] // Added because these are custom functions now
        [HttpGet] // Added because these are custom functions now

        public IEnumerable<ProductDTO> GetProducts()
        {
            var Products = (from e in db.Products
                            select new ProductDTO
                            {
                                ProductID = e.ProductID,
                                ProductName = e.ProductName,
                                SupplierID = e.SupplierID,
                                CategoryID = e.CategoryID,
                                QuantityPerUnit = e.QuantityPerUnit,
                                UnitPrice = e.UnitPrice,
                                UnitsInStock = e.UnitsInStock,
                                UnitsOnOrder = e.UnitsOnOrder,
                                ReorderLevel = e.ReorderLevel,
                                Discontinued = e.Discontinued
                            }).ToList();
            return Products;
        }

        [Route("GetMore")] // Added because these are custom functions now
        [HttpGet] // Added because these are custom functions now
        public IEnumerable<ProductMoreDTO> GetMoreProductInfo()
        {

            var Products = db.Products.Join(db.Suppliers,
                 p => p.SupplierID,
                 s => s.SupplierID, (p, s) => new ProductMoreDTO
                 {
                     ProductID = p.ProductID,
                     ProductName = p.ProductName,
                     SupplierID = s.SupplierID,
                     CompanyName = s.CompanyName,
                     ContactName = s.ContactName
                 }
                 );
            return Products;
        }

        // GET: api/Products/5
        [Route("FindById/{id}")] // Added because these are custom functions now
        [HttpGet] // Added because these are custom functions now
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Route("FindByNum/{id}")] // Added because these are custom functions now
        [HttpGet] // Added because these are custom functions now
        [ResponseType(typeof(ProductDTO))]
        public ProductDTO getProductById(int id)
        {
            return db.Products.Where(x => x.ProductID == id)
                     .ToList()
                     .Select(x => ToDTO(x))
                     .FirstOrDefault();
        }

        // PUT: api/Products/5
        [Route("Update/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, ProductDTO modified)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modified.ProductID)
            {
                return BadRequest();
            }

            Product prod = db.Products.Where(x => x.ProductID == id)
                     .ToList()
                     .FirstOrDefault();

            prod.ProductName = modified.ProductName;
            prod.UnitPrice = modified.UnitPrice;
            prod.SupplierID = modified.SupplierID;
            prod.CategoryID = modified.CategoryID;
            prod.QuantityPerUnit = modified.QuantityPerUnit;
            prod.UnitPrice = modified.UnitPrice;
            prod.UnitsInStock = modified.UnitsInStock;
            prod.UnitsOnOrder = modified.UnitsOnOrder;
            prod.ReorderLevel = modified.ReorderLevel;
            prod.Discontinued = modified.Discontinued;


            db.Entry(prod).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [Route("Create")]
        [HttpPost]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(ProductDTO productInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product prod = ToEntity(productInfo);
            db.Products.Add(prod);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = prod.ProductID }, prod);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        private Product ToEntity(ProductDTO p)
        {
            return new Product()
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                SupplierID = p.SupplierID,
                CategoryID = p.CategoryID,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                UnitsOnOrder = p.UnitsOnOrder,
                ReorderLevel = p.ReorderLevel,
                Discontinued = p.Discontinued
            };

        }

        private ProductDTO ToDTO(Product p)
        {
            return new ProductDTO()
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                SupplierID = p.SupplierID,
                CategoryID = p.CategoryID,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock,
                UnitsOnOrder = p.UnitsOnOrder,
                ReorderLevel = p.ReorderLevel,
                Discontinued = p.Discontinued
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}
