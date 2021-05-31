using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace P42.Native.Controls
{
    public interface INotifiable : INotifyPropertyChanged, INotifyPropertyChanging
    {

        void OnPropertyChanging([CallerMemberName] string propertyName = null) { }

        void OnPropertyChanged([CallerMemberName] string propertyName = null) { }

        
    }
}
