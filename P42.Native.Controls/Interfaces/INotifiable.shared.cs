using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    public interface DINotifiable : INotifyPropertyChanged, INotifyPropertyChanging
    {

        void OnPropertyChanging([CallerMemberName] string propertyName = null) { }

        void OnPropertyChanged([CallerMemberName] string propertyName = null) { }

    }
}
