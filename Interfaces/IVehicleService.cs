using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IVehicleService
    {
        Task<Dictionary<string, string>> GetImages();
        Task<ActionResult<VehicleDto>> CalculatePremium(VehicleDto vehicleDto);
        Task<ActionResult<VehicleDBDto>> Add(VehicleDBDto addVDto);
        Task<ActionResult<IEnumerable<string>>> GetVehicleModels();
        Task<byte[]> GeneratePdf(VehicleDto vehicleDto);
    }
}
