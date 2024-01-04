using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IVehicleService
    {
        Task<Dictionary<string, string>> GetImages();
        Task<ActionResult<VehicleDto>> CalculatePremium(VehicleDto vehicleDto);
        Task<ActionResult<VehicleDBDto>> Add(VehicleDBDto addVDto);
        Task<ActionResult> Delete(string vehicleModel);
        Task<List<string>> GetVehicleModels(string vehicleType);
        Task<byte[]> GeneratePdf(VehicleDto vehicleDto);
    }
}
