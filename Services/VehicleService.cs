using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using API.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace API.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly DataContext _context;
        private readonly IFeatureManager _featureManager;
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IDistributedCache _cache;

        public VehicleService(DataContext context,
            IFeatureManager featureManager,
            IConfiguration configuration,
            BlobServiceClient blobServiceClient,
            IDistributedCache cache)
        {
            _context = context;
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
            _cache = cache;
            _featureManager = featureManager;
        }

        public async Task<Dictionary<string, string>> GetImages()
        {
            var azureEnabled = await _featureManager.IsEnabledAsync("AzureEnabled");
            var blobList = new Dictionary<string, string>();

            if (azureEnabled)
            {
                var containerName = _configuration.GetValue<string>("AzureBlob:ContainerName");
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobs = blobContainerClient.GetBlobsAsync();

                await foreach (var item in blobs)
                {
                    var blobClient = blobContainerClient.GetBlobClient(item.Name);
                    blobList.Add(Path.GetFileNameWithoutExtension(blobClient.Name.ToLower()), blobClient.Uri.AbsoluteUri);
                }
                return blobList;
            }
            else
            {
                return new Dictionary<string, string>
                    {
                        { "bike", "./assets/img/Bike.jpg" },
                        { "car", "./assets/img/Car.jpg" },
                        { "apple_pay", "./assets/img/apple_pay.svg" },
                        { "google_pay", "./assets/img/google_pay.svg" },
                        { "mastercard", "./assets/img/mastercard.svg" },
                        { "paypal", "./assets/img/paypal.svg" },
                        { "paytm", "./assets/img/paytm.svg" },
                        { "rupay", "./assets/img/rupay.png" },
                        { "visa", "./assets/img/visa.svg" },
                    };
            }
        }

        public async Task<ActionResult<VehicleDto>> CalculatePremium(VehicleDto vehicleDto) 
        {
            var multipliers = new Multipliers();
            var vehicle = await GetVehicle(vehicleDto.Model);

            if (vehicle == null)
                throw new Exception("Error in Vehicle data");

            multipliers.TypeM = vehicleDto.Type switch
            {
                "Bike" => 1,
                "Car" => 1.2f,
                _ => throw new Exception("Vehicle Type not specified")
            };
            vehicleDto.Age = vehicleDto.RegYear == DateTime.Now.Year ? 0 : DateTime.Now.Year - vehicleDto.RegYear;
            multipliers.AgeM = vehicleDto.Age switch
            {
                0 or 1 or 2 => 1,
                3 or 4 or 5 => 1.2f,
                _ => 1.5f
            };

            var BikeValue = vehicle.Price * (1 - (multipliers.AgeM - 1));
            float minBikeValue = vehicle.Price * 0.5f;
            vehicleDto.Value = BikeValue;
            vehicleDto.BasePrem = vehicle.Price * 0.1f;

            if (BikeValue == vehicle.Price)
            {
                multipliers.ValueM = 1;
            }
            else if (BikeValue >= minBikeValue)
            {
                multipliers.ValueM = 1.2f;
            }
            else
            {
                multipliers.ValueM = 1.5f;
            }

            string[] nearbyStates = { "AP", "TL", "KL", "KA" };
            string location = vehicleDto.RegNo.ToUpper().Substring(0, 2);

            if (location == "TN")
            {
                multipliers.LocM = 1f;
            }
            else if (nearbyStates.Contains(location))
            {
                multipliers.LocM = 1.1f;
            }
            else
            {
                multipliers.LocM = 1.2f;
            }

            float totalPrem = vehicleDto.BasePrem;
            totalPrem = (float)Math.Round(totalPrem * multipliers.TypeM);
            totalPrem = (float)Math.Round(totalPrem * multipliers.AgeM);
            totalPrem = (float)Math.Round(totalPrem * multipliers.ValueM);
            totalPrem = (float)Math.Round(totalPrem * multipliers.LocM);

            //Get premium for 3 years
            //Third Party
            vehicleDto.TPrem1Year = (totalPrem * 1);
            vehicleDto.TPrem2Year = (totalPrem * 2);
            vehicleDto.TPrem3Year = (totalPrem * 3);

            //Comprehensive
            vehicleDto.CPrem1Year = (float)Math.Round(vehicleDto.TPrem1Year * 1.2f);
            vehicleDto.CPrem2Year = (float)Math.Round(vehicleDto.TPrem2Year * 1.2f);
            vehicleDto.CPrem3Year = (float)Math.Round(vehicleDto.TPrem3Year * 1.2f);

            return vehicleDto;
        }

        public async Task<ActionResult<VehicleDBDto>> Add(VehicleDBDto addVDto)
        {
            if (await VehicleExists(addVDto.Model))
                throw new Exception("Vehicle already exists, Please modify");

            var vehicle = new VehicleData
            {
                Model = addVDto.Model,
                Type = addVDto.Type,
                Price = addVDto.Price,
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return new VehicleDBDto
            {
                Model = addVDto.Model,
                Type = addVDto.Type,
                Price = addVDto.Price,
            };
        }

        public async Task<ActionResult> Delete(string vehicleModel)
        {
            var result = await GetVehicle(vehicleModel);

            if (result == null)
                throw new Exception("Vehicle does not exist");

            _context.Entry(result).State = EntityState.Detached;

            var vehicle = new VehicleData
            {
                Id = result.Id,
                Model = result.Model,
                Type = result.Type,
                Price = result.Price,
            };

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return null;
        }

        private async Task<bool> VehicleExists(string model)
        {
            return await _context.Vehicles.AnyAsync(x => x.Model == model);
        }

        private async Task<VehicleData> GetVehicle(string model)
        {
            return await _context.Vehicles.SingleOrDefaultAsync(x => x.Model == model);
        }
        
        public async Task<List<string>> GetVehicleModels(string vehicleType)
        {
            var azureEnabled = await _featureManager.IsEnabledAsync("AzureEnabled");
            if (azureEnabled)
            {
                List<string> vehicleList = new();
                var cachedVehicledModelsList = _cache.GetStringAsync("vehicleModelList");

                //Try to get cached response if available
                if (!string.IsNullOrEmpty(await cachedVehicledModelsList))
                {
                    vehicleList = JsonConvert.DeserializeObject<List<string>>(await cachedVehicledModelsList);
                }
                else
                {
                    vehicleList = await _context.Vehicles.Where(x => x.Type == vehicleType).Select(x => x.Model).ToListAsync();
                    _cache.SetString("vehicleModelList", JsonConvert.SerializeObject(vehicleList));
                }

                return vehicleList;
            }
            else
            {
                return await _context.Vehicles.Where(x => x.Type == vehicleType).Select(x => x.Model).ToListAsync();
            }
        }

        public async Task<byte[]> GeneratePdf(VehicleDto vehicleDto)
        {
            var document = new PdfDocument();
            var azureStorageEnabled = await _featureManager.IsEnabledAsync("AzureEnabled");
            var HtmlContent = "";
            if (azureStorageEnabled)
            {
                var containerName = _configuration.GetValue<string>("AzureBlob:ContainerName");
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = blobContainerClient.GetBlobClient(_configuration.GetValue<string>("AzureBlob:InsuranceTemplateName"));
                BlobDownloadInfo blobDownloadInfo = blobClient.Download();
                var content = blobDownloadInfo.Content;
                using (var streamReader = new StreamReader(content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        HtmlContent = await streamReader.ReadToEndAsync();
                    }
                }
            }
            else
            {
                var template = new InsTemplateHtml();
                HtmlContent = template.Html;
            }

            var vehicleType = vehicleDto.Type switch
            {
                "Bike" => "2 Wheeler",
                "Car" => "4 Wheeler",
                _ => ""
            };
            HtmlContent = HtmlContent.Replace("{{Type}}", vehicleType);

            var planType = vehicleDto.SelectedPlan.Substring(vehicleDto.SelectedPlan.IndexOf('-') + 1, 2);
            if (planType == "CP")
            {
                HtmlContent = HtmlContent.Replace("{{InsType}}", "Comprehensive");
            }
            else
            {
                HtmlContent = HtmlContent.Replace("{{InsType}}", "Third Party");
            }

            HtmlContent = HtmlContent.Replace("{{Model}}", vehicleDto.Model);
            HtmlContent = HtmlContent.Replace("{{RegNo}}", vehicleDto.RegNo);
            HtmlContent = HtmlContent.Replace("{{fullName}}", vehicleDto.FullName);
            HtmlContent = HtmlContent.Replace("{{StartDate}}", DateTime.Now.ToString("dd-MM-yyyy"));
            HtmlContent = HtmlContent.Replace("{{EndDate}}", DateTime.Now.AddYears(1).ToString("dd-MM-yyyy"));
            HtmlContent = HtmlContent.Replace("{{AccCover}}", vehicleDto.AccCover ? "Yes" : "No");
            HtmlContent = HtmlContent.Replace("{{Status}}", vehicleDto.Status);

            var noOfYears = vehicleDto.SelectedPlan.Substring(0, 1);
            HtmlContent = HtmlContent.Replace("{{Years}}", noOfYears);

            float premium = noOfYears switch
            {
                "1" => planType == "CP" ? vehicleDto.CPrem1Year : vehicleDto.TPrem1Year,
                "2" => planType == "CP" ? vehicleDto.CPrem2Year : vehicleDto.TPrem2Year,
                "3" => planType == "CP" ? vehicleDto.CPrem3Year : vehicleDto.TPrem3Year,
                _ => 0
            };

            HtmlContent = HtmlContent.Replace("{{Premium}}", premium.ToString());
            HtmlContent = HtmlContent.Replace("{{GST}}", ((int)(premium * 0.18)).ToString());
            HtmlContent = HtmlContent.Replace("{{Total}}", (((int)(premium + (premium * 0.18)))).ToString());
            HtmlContent = HtmlContent.Replace("{{fullName}}", vehicleDto.FullName);
            HtmlContent = HtmlContent.Replace("{{email}}", vehicleDto.Email);
            HtmlContent = HtmlContent.Replace("{{phone}}", vehicleDto.MobNo);
            HtmlContent = HtmlContent.Replace("{{pincode}}", vehicleDto.Pincode);

            PdfGenerator.AddPdfPages(document, HtmlContent, PdfSharpCore.PageSize.A4);
            byte[] response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            return response;
        }
    }
}
