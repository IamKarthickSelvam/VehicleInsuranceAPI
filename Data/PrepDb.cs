namespace API.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                //SeedData(serviceScope.ServiceProvider.GetService<DataContext>(), isProd);
            }
        }
    }
}
