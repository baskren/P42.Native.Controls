using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace P42.Native.Controls
{
    public interface INotifiable : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Property Change Handler
        bool HasDrawn { get; }

        bool HasChanged { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
            => HasChanged = false;

        /*
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        */
        void OnPropertyChanging([CallerMemberName] string propertyName = null);

        void OnPropertyChanged([CallerMemberName] string propertyName = null);

        public bool SetField<T>(ref T field, T value, Action action = null, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("null propertyName in SetField");

            OnPropertyChanging(propertyName);
            field = value;
            HasChanged = true;
            action?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public bool SetRedrawField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (HasDrawn) RedrawElement(); }, propertyName, callerPath);

        public bool SetLayoutField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (HasDrawn) RelayoutElement(); }, propertyName, callerPath);

        void RedrawElement();

        void RelayoutElement();
        #endregion
    }
}
