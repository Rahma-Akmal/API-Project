using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _catrepo;
        public CategoryController(ICategoryRepo categoryRepository)
        {
            _catrepo = categoryRepository;
        }
        [HttpGet]
        public IActionResult Get()
        { 
           var cart= _catrepo.GetAll();
            return Ok(cart);
        }
        [HttpGet("category/{id:int}",Name ="GetCategoryById")]
        
        public IActionResult GetByID(int id)
        { 
            var cat=_catrepo.GetById(id);
            return Ok(cat);
        }
        [HttpPost("AddCategory")]
        //[Authorize("Admin")]

        public IActionResult AddCat(Category cat)
        {
            if (ModelState.IsValid)
            {
                _catrepo.AddCategory(cat);
                var link = Url.Link("GetCategoryById", new { id = cat.Id });
                return Created(link, cat);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("updatecategory")]
        //[Authorize("Admin")]

        public IActionResult UpdateCat(CategoryDTO cat,int id) 
        {
            _catrepo.UpdateCategory(cat,id);
            return Ok("Item Updated Successfully");
        }
        [HttpDelete("removeCategeory")]
        public IActionResult DeleteCat(int id) 
        { 
            _catrepo.DeleteCategory(id);
            return Ok();
        }


    }
}
