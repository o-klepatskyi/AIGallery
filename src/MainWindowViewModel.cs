using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AIGallery
{
    public class MainViewModel : ObservableObject
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand<Type> ChangeViewModelCommand { get; }

        public MainViewModel()
        {
            CurrentViewModel = new GenerateViewModel();

            ChangeViewModelCommand = new RelayCommand<Type>((viewModelType) =>
            {
                CurrentViewModel = Activator.CreateInstance(viewModelType);
            });
        }
    }
}
