using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название товара обязательно")]
    [StringLength(100, ErrorMessage = "Название не может превышать 100 символов")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Цена товара обязательно")]
    [Range(0.01, 10000, ErrorMessage = "Цена должна быть между 0.01 и 10000")]
    public decimal Price { get; set; }

    [StringLength(500, ErrorMessage = "Описание не может превышать 500 символов")]
    public string Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
