using System.ComponentModel.DataAnnotations;

namespace AppData.Models
{
    public class Material
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự.")]
        public string? Name { get; set; }
    }
}
