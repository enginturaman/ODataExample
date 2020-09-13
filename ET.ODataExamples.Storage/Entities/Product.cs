using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ET.ODataExamples.Storage.Entities
{
    public class ProductDmo : BaseInfoEntity
    {
        [Column(TypeName = "nvarhcar(100)")]
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        [Required]
        public decimal Price { get; set; }
    }
}
