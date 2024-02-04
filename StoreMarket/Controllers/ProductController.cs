using Microsoft.AspNetCore.Mvc;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private StoreContext storeContext;
        public ProductController(StoreContext context)
        {
            storeContext = context;
        }

        [HttpGet]
        [Route("product/{id}")]

        public ActionResult<ProductResponse> GetProduct(int id)
        {
            var result = storeContext.Products.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new ProductResponse(result));
            }

        }


        [HttpGet]
        [Route("products")]

        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var result = storeContext.Products;

            return Ok(result.Select(result => new ProductResponse(result)));
        }

        [HttpPost]
        [Route("product")]

        public ActionResult<ProductResponse> AddProduct(ProductCreateRequest request)
        {
            Product product = request.ProductGetEntity();
            try
            {
                var result = storeContext.Products.Add(product).Entity;

                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        [Route("product/{id}")]

        public ActionResult<ProductResponse> DeleteProduct(int id)
        {
            var result = storeContext.Products.FirstOrDefault(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                storeContext.Products.Remove(result);
                storeContext.SaveChanges();
                return Ok();
            }

        }

        [HttpPut]
        [Route("product/{id}")]
        public ActionResult<ProductResponse> UpdateProduct(int id, ProductUpdateRequest request)
        {

            Product product = request.ProductGetEntity(id);
            try
            {
                var result = storeContext.Products.Update(product).Entity;
                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
