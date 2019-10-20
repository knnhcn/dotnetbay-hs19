using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Data.Entity;
using DotNetBay.Data.Provider.FileStorage;
using DotNetBay.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider _serviceProvider { get; private set; }
        public readonly IMainRepository _mainRepository;
        public readonly IAuctionRunner _auctionRunnter;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            this._mainRepository = new FileSystemMainRepository("fileName");
            this._auctionRunnter = new AuctionRunner(this._mainRepository);

            var memberService = new SimpleMemberService(this._mainRepository);
            var service = new AuctionService(this._mainRepository, memberService);

            if (!service.GetAll().Any())
            {
                var me = memberService.GetCurrentMember();
                service.Save(new Auction
                {
                    Title = "My First Auction", 
                    StartDateTimeUtc = DateTime.UtcNow.AddSeconds(10), 
                    EndDateTimeUtc = DateTime.UtcNow.AddDays(14),
                    StartPrice = 72,
                    Seller = me
                });
                service.Save(new Auction
                {
                    Title = "My Second Auction",
                    StartDateTimeUtc = DateTime.UtcNow.AddSeconds(10),
                    EndDateTimeUtc = DateTime.UtcNow.AddDays(14),
                    StartPrice = 200,
                    Seller = me
                });
                service.Save(new Auction
                {
                    Title = "My Third Auction",
                    StartDateTimeUtc = DateTime.UtcNow.AddSeconds(10),
                    EndDateTimeUtc = DateTime.UtcNow.AddDays(14),
                    StartPrice = 5000,
                    Seller = me
                });
            }

            this._auctionRunnter.Start();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuctionRunner, AuctionRunner>();
        }

    }
}
