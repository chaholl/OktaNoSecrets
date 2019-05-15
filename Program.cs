using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace OktaNoSecrets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                    {
                        var builtConfig = config.Build();
                        //Pickup local certificate
                        //config.AddAzureKeyVault(
                        //    $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                        //   builtConfig["SpnAppId"],
                        //    new X509Certificate2("spn.pfx"));

                        using (var store = new X509Store(StoreName.My,StoreLocation.CurrentUser))
                        {
                            store.Open(OpenFlags.ReadOnly);
                            var certs = store.Certificates.Find(X509FindType.FindByThumbprint,builtConfig["SpnCertThumbprint"], false);
                            config.AddAzureKeyVault(
                                $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                                builtConfig["SpnAppId"],
                                certs.OfType<X509Certificate2>().Single());

                            store.Close();

                        }
                    }
                })
                .UseStartup<Startup>();
    }
}
