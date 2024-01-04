using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataContext>());
            }
        }

        private static void SeedData(DataContext context)
        {
            
            if (context.Vehicles.Any())
            {
                Console.WriteLine("Data exists so clearing DB for Seeding...");
                context.Vehicles.ExecuteDeleteAsync();
                context.SaveChanges();
                Console.WriteLine("Successfully cleared DB...");
            }

            Console.WriteLine("Seeding Data...");

            context.Vehicles.AddRange(
                    new VehicleData() { Model = "Volkswagen Virtus", Type = "Car", Price = 2369000 },
                    new VehicleData() { Model = "Skoda Slavia", Type = "Car", Price = 4898089 },
                    new VehicleData() { Model = "Audi Q6", Type = "Car", Price = 6999000 },
                    new VehicleData() { Model = "Toyota Hyrider", Type = "Car", Price = 2363000 },
                    new VehicleData() { Model = "Hyundai Elantra", Type = "Car", Price = 1631000 },
                    new VehicleData() { Model = "Citroen C5s", Type = "Car", Price = 2943000 },
                    new VehicleData() { Model = "Maruti Suzuki Jimny", Type = "Car", Price = 225000 },

                    new VehicleData() { Model = "Aprilia RSV4", Type = "Bike", Price = 2369000 },
                    new VehicleData() { Model = "BMW M1000RR", Type = "Bike", Price = 4898089 },
                    new VehicleData() { Model = "Ducati Panigale V4R", Type = "Bike", Price = 6999000 },
                    new VehicleData() { Model = "Honda CBR-1000RR-R", Type = "Bike", Price = 2363000 },
                    new VehicleData() { Model = "Kawasaki ZX-10R", Type = "Bike", Price = 1631000 },
                    new VehicleData() { Model = "Yamaha R1M", Type = "Bike", Price = 2943000 },
                    new VehicleData() { Model = "Yamaha R15 V4", Type = "Bike", Price = 225000 }
                );

            context.SaveChanges();
        }
    }
}
