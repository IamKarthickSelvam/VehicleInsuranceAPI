using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class VehicleDto
    {
        [Required]
        public string RegNo { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int RegYear { get; set; }
        [Required]
        public string Status { get; set; }
        public string ImgUri { get; set; }
        public int Age { get; set; }
        public float Value { get; set; }
        public float BasePrem { get; set; }
        public float CPrem1Year { get; set; }
        public float CPrem2Year { get; set; }
        public float CPrem3Year { get; set; }
        public float TPrem1Year { get; set; }
        public float TPrem2Year { get; set; }
        public float TPrem3Year { get; set; }
        public float TotalPremium { get; set; }
        public bool AccCover { get; set; }
        public string SelectedPlan { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobNo { get; set; }
        public string Pincode { get; set; }
    }
}
