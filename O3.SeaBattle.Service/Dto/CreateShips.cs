using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace O3.SeaBattle.Service.Dto
{
    public class CreateShips
    {
        [Required]
        [JsonPropertyName(nameof(Coordinates))]
        public string Coordinates { get; set; }
    }
}
