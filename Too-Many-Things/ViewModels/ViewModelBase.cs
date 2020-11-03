using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Too_Many_Things.ViewModels
{
        /*
         * Base class all ViewModels shall inherit from.  Implements clean,
         * quick, and easy property change handling for properties that belong
         * to individual viewModels
         */
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
}
