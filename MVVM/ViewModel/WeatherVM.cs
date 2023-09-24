using MVVM.Model;
using MVVM.ViewModel.Commands;
using MVVM.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.ViewModel
{
    public class WeatherVM: INotifyPropertyChanged
    {
        #region CommandProperties

        public SearchCommand SearchCommand { get; set; }
        #endregion

        #region AutoProperties
        public ObservableCollection<City> Cities { get; set; }
        #endregion

        #region FullProperties

        private string query;

        public string Query
        {
            get { return query; }
            set { query = value; OnPropertyChanged("Query"); }
        }

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set { 
                currentConditions = value;  
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set 
            {
                Debug.WriteLine("Here 55");
                Debug.WriteLine(value);
                if(selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged("SelectedCity");
                    Debug.WriteLine(value.LocalizedName);
                    Debug.WriteLine(value.Key);
                    GetCurrentConditions();
                }
            }
        }

        #endregion

        #region EventProperties
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public WeatherVM()
        {
            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        public async void GetCities()
        {
            var tempCities = await AccuWeatherHelper.GetCities(Query);
            Cities.Clear();
            foreach (City city in tempCities)
            {
                Cities.Add(city);
            }
        }

        public async void GetCurrentConditions()
        {
            CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }

    }
}
