try
{
    var builder = DistributedApplication.CreateBuilder(args);

    var apiService = builder.AddProject<Projects.RecipeGiverApp_ApiService>("apiservice");

    builder.AddProject<Projects.RecipeGiverApp_Web>("webfrontend")
        .WithExternalHttpEndpoints()
        .WithReference(apiService);

    builder.Build().Run();
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex);
	throw;
}