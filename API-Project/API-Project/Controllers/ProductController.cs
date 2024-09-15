using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;
using API_Project.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductrepo _prorepo;
        public ProductController(IProductrepo proRepository)
        {
            _prorepo = proRepository;

        }
        [HttpGet]
        public IActionResult Get()
        {
            var pro = _prorepo.GetAll();
            return Ok(pro);
        }
        [HttpGet("product/{id:int}", Name = "GetProductById")]

        public IActionResult GetByID(int id)
        {
            var pro = _prorepo.GetById(id);
            return Ok(pro);
        }
        [HttpPost("Addproduct")]
        //[Authorize("Admin")]

        public IActionResult AddPro(ProductDTO pro)
        {
            if (ModelState.IsValid)
            {
                _prorepo.AddProduct(pro);
                var link = Url.Link("GetProductById", new { id = pro.ProductId });
                return Created(link, pro);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("updateproduct")]
        //[Authorize("Admin")]

        public IActionResult UpdatePro(ProductDTO pro, int id)
        {
            _prorepo.UpdateProduct(pro, id);
            return Ok("Item Updated Successfully");
        }
        [HttpDelete("removeproduct")]
        public IActionResult Deletepro(int id)
        {
            _prorepo.DeleteProduct(id);
            return Ok();
        }

    }
}
