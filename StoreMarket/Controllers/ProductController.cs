using Microsoft.AspNetCore.Mvc;
using StoreMarket.Abstroctions;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using System.Text;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet(template: "GetProduct/{id}")]
        public ActionResult<ProductResponse> GetProduct(int id)
        {
            var result = _productServices.GetProductId(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet(template: "GetProducts")]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var result = _productServices.GetProducts();

            return Ok(result);
        }

        [HttpPost(template: "AddProduct")]
        public ActionResult<int> AddProduct(ProductCreateRequest request)
        {
            try
            {
                var id = _productServices.AddProduct(request);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(template: "DeleteProduct/{id}")]
        public ActionResult<ProductResponse> DeleteProduct(int id)
        {
            var product = _productServices.DeleteProduct(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
                return NotFound();
        }


        [HttpPut(template: "UpdateProduct")]
        public ActionResult<ProductResponse> UpdateProduct(int id, ProductUpdateRequest request)
        {
            try
            {
                var product = _productServices.UpdateProduct(id, request);
                return product;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet(template: "GetProductsCSV")]
        public FileContentResult GetProductsCSV()
        {
            var result = GetCsv(_productServices.GetProducts());

            return File(new System.Text.UTF8Encoding().GetBytes(result), "txt/csv", "report.csv");
        }

        private string GetCsv(IEnumerable<ProductResponse> products)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var product in products)
            {
                stringBuilder.AppendLine(product.Id + ";" +
                                         product.Name + ";" +
                                         product.Description + ";" +
                                         product.CategoryId + "\n");
            }
            return stringBuilder.ToString();
        }
    }
}
