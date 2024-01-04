using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class VehicleData
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
    }
}
