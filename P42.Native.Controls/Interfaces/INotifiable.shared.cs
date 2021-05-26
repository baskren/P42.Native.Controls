using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace P42.Native.Controls
{
    [SmartTraitsDefs.TraitInterface]
    public interface INotifiable : INotifyPropertyChanged, INotifyPropertyChanging
    {
        bool SetField<T>(ref T field, T value, Action action = null, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null);

    }
}
