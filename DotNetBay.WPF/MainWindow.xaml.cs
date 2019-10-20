using DotNetBay.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotNetBay.Core;
using DotNetBay.Interfaces;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Auction> _auctions;
        private readonly IAuctionService _auctionService;

        public MainWindow()
        {
            InitializeComponent();

            IMainRepository mainRepo = ((App) Application.Current)._mainRepository;
            this._auctionService = new AuctionService(mainRepo, new SimpleMemberService(mainRepo));
            this._auctions = new ObservableCollection<Auction>(this._auctionService.GetAll());

            DataContext = this;
        }

        private void NewAuction_Click(object sender, RoutedEventArgs e)
        {
            var sellView = new SellView();
            sellView.ShowDialog(); // Blocking
        }

        public ObservableCollection<Auction> Auctions => this._auctions;
    }
}
