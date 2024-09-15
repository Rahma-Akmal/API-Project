using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Model
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
