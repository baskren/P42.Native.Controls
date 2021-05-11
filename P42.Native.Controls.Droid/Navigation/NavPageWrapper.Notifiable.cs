using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace P42.Native.Controls.Droid
{
    public partial class NavPageWrapper : INotifyPropertyChanged
    {
        #region Property Change Handler
        internal protected bool hasDrawn;

        [JsonIgnore]
        public bool HasChanged { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
            => HasChanged = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, Action action = null, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("null propertyName in SetField");

            field = value;
            HasChanged = true;
            action?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetRedrawField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (hasDrawn) PostInvalidate(); }, propertyName, callerPath);

        protected bool SetLayoutField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
            => SetField(ref field, value, () => { if (hasDrawn) RequestLayout(); }, propertyName, callerPath);

        #endregion
    }
}
