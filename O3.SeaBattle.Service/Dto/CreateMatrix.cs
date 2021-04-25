using System.ComponentModel.DataAnnotations;

namespace O3.SeaBattle.Service.Dto
{
    public class CreateMatrix
    {
        [Required]
        public int range { get; set; }
    }
}
