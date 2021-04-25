using System.ComponentModel.DataAnnotations;

namespace O3.SeaBattle.Service.Dto
{
    public class Shot
    {
        [Required]
        public string coord { get; set; }
    }
}
