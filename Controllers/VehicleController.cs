using API.Data;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class VehicleController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;

        public VehicleController(DataContext context,
            IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }

        [HttpGet("Blob")]
        public async Task<Dictionary<string, string>> GetImages()
        {
            try
            {
                return await _vehicleService.GetImages();
            }
            catch (Exception ex)
            {
                return new Dictionary<string, string>
                    {
                        { "Error message", ex.Message },
                    };
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetBasicDetails()
        {
            try
            {
                return await _vehicleService.GetVehicleModels();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<VehicleDto>> CalculatePremium([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                var response = await _vehicleService.CalculatePremium(vehicleDto);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("generate")]
        //public ActionResult Generate([FromBody] VehicleDto vehicleDto)
        //{
        //    try
        //    {
        //        //byte[] invoiceBytes = _vehicleService.GenerateDoc(vehicleDto);
        //        //return File(invoiceBytes, "application/blob", "Invoice.docx");
        //        return BadRequest("Error in generating file:");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Error in generating file: " + ex.Message);
        //    }
        //}

        [HttpPost("Add")]
        public async Task<ActionResult<VehicleDBDto>> Add(VehicleDBDto addVDto)
        {
            try
            {
                var response = await _vehicleService.Add(addVDto);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pdf")]
        public async Task<ActionResult<VehicleDto>> GeneratePDF([FromBody] VehicleDto vehicleDto)
        {
            try
            {
                var response = await _vehicleService.GeneratePdf(vehicleDto);
                return File(response, "application/pdf", "Test.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //FIGURE OUT DELETE ASAP
        //[HttpDelete]
        //public async Task<ActionResult> Delete(VehicleDBDto vehicleDBDto)
        //{
        //    if (!await VehicleExists(vehicleDBDto.Model))
        //        return BadRequest("Vehicle does not exist");

        //    var vehicle = new VehicleData
        //    {
        //        Model = vehicleDBDto.Model,
        //        Type = vehicleDBDto.Type,
        //        Price = vehicleDBDto.Price,
        //    };

        //    _context.Vehicles.Remove(vehicle);
        //    await _context.SaveChangesAsync();

        //    return StatusCode(200);
        //}
    }
}
