using Microsoft.AspNetCore.Mvc;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private StoreContext storeContext;

        public CategoryController(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }

        [HttpPost]
        [Route("category")]
        public ActionResult<CategoryResponse> AddCategory(CategoryCreateRequest request)
        {
            Category category = request.CategoryGetEntity();
            try
            {
                var result = storeContext.Categories.Add(category).Entity;
                storeContext.SaveChanges();
                return Ok(new CategoryResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("category/{id}")]

        public ActionResult<CategoryResponse> GetCategory(int id)
        {
            var result = storeContext.Categories.FirstOrDefault(c => c.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new CategoryResponse(result));
            }

        }

        [HttpGet]
        [Route("categories")]

        public ActionResult<IEnumerable<CategoryResponse>> GetCategories()
        {
            var result = storeContext.Categories;

            return Ok(result.Select(result => new CategoryResponse(result)));
        }

        [HttpDelete]
        [Route("category/{id}")]
        public ActionResult<CategoryResponse> DeleteCategory(int id)
        {
            var result = storeContext.Categories.FirstOrDefault(c => c.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                storeContext.Categories.Remove(result);
                storeContext.SaveChanges();
                return Ok();
            }

        }

        [HttpPut]
        [Route("category/{id}")]
        public ActionResult<CategoryResponse> UpdateCategory(int id, CategoryUpdateRequest request)
        {
            Category category = request.CategoryGetEntity(id);
            try
            {
                var result = storeContext.Categories.Update(category).Entity;
                storeContext.SaveChanges();
                return Ok(new CategoryResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
