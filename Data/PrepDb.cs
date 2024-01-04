using API.Entities;

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
            context.Vehicles.AddRange(
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
