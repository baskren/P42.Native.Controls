using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    public interface INotifiable : INotifyPropertyChanged, INotifyPropertyChanging
    {

        void OnPropertyChanging([CallerMemberName] string propertyName = null) { }

        void OnPropertyChanged([CallerMemberName] string propertyName = null) { }

        Task DipWaitForDrawComplete();


    }
}
