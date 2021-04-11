using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace Budgets.GUI.WPF.Navigation
{
    public abstract class NavigationBase<TObject> : BindableBase where TObject : Enum
    {
        private List<INavigatable<TObject>> _viewModels = new();
        private INavigatable<TObject> _currentViewModel;

        public INavigatable<TObject> CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        protected NavigationBase()
        {

        }

        public void Navigate(TObject type)
        {
            if (CurrentViewModel != null && CurrentViewModel.Type.Equals(type))
            {
                return;
            }

            INavigatable<TObject> viewModelToChange = _viewModels.FirstOrDefault(viewModel => viewModel.Type.Equals(type));

            if (viewModelToChange == null)
            {
                viewModelToChange = CreateViewModel(type);
                _viewModels.Add(viewModelToChange);
            }

            viewModelToChange.ClearSensitiveData();
            CurrentViewModel = viewModelToChange;
        }

        protected abstract INavigatable<TObject> CreateViewModel(TObject type);
    }
}
