using CurrencyRateConverter.Models;
using CurrencyRateConverter.Services;
using DryIoc;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Xamarin.Forms;

namespace CurrencyRateConverter.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private DelegateCommand _convertCommand;
        private DelegateCommand _cleanCommand;
        private DelegateCommand _closeCommand;

        private ApiObj _obj;
        private int _monto;
        private double _total;
        
        private string _baseD;

        public string BaseD
        {
            get { return _baseD; }
            set { SetProperty(ref _baseD, value); }
        }

        private string _baseO;

        public string BaseO
        {
            get { return _baseO; }
            set { SetProperty(ref _baseO, value); }
        }

        private DateTime _fecha;

        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetProperty(ref _fecha, value); }
        }


        private DateTime _now;
        private string _fechaStr;

        public string FechaStr
        {
            get  { return _fechaStr; }
            set { SetProperty(ref _fechaStr, value); }
        }

        private string _nowStr;

        public string NowStr
        {
            get { return _nowStr; }
            set { SetProperty(ref _nowStr, value); }
        }


        public DateTime Now

        {
            get { return _now; }
            set { SetProperty(ref _now, value); }
        }



        public double Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }


        public int Monto
        {
            get { return _monto; }
            set { SetProperty(ref _monto, value); }
        }


        public HomePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Convertidor de tipo de moneda";
        }

        public ApiObj Obj
        {
            get => _obj;
            set => SetProperty(ref _obj, value);
        }

        public DelegateCommand ConvertCommand => _convertCommand ?? (_convertCommand = new DelegateCommand(ConvertAsync));
        public DelegateCommand CleanCommand => _cleanCommand ?? (_cleanCommand = new DelegateCommand(CleanData));

        private void CleanData()
        {
            Total = 0;
            Monto = 0;
            BaseO = "";
            BaseD = "";
            Fecha = DateTime.Now;
        }

        public DelegateCommand CloseCommand => _closeCommand ?? (_closeCommand = new DelegateCommand(CloseAsync));

        private void CloseAsync()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        private async  void ConvertAsync()
        {
            Now = DateTime.Now;
            NowStr = Now.ToString("yyyy-MM-dd");
            FechaStr = Fecha.ToString("yyyy-MM-dd");
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetCurrencyAsync(url, FechaStr, NowStr, BaseO);
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Obj = (ApiObj)response.Result;

            var ratesByDate = Obj.Rates[Fecha.Date];
            PropertyInfo property = ratesByDate.GetType().GetProperty(BaseD);
            var value = property.GetValue(ratesByDate, null);
            var rate = Convert.ToDouble(value);
            //Total = Monto * rate;
            await App.Database.SaveItemAsync(Obj.Rates[Fecha.Date]);
            var cosa = await App.Database.GetItemAsync(1);
            PropertyInfo proper = cosa.GetType().GetProperty(BaseD);
            var valu = proper.GetValue(cosa, null);
            var rat = Convert.ToDouble(valu);
            Total = Monto * rat;
        }
    }
}

