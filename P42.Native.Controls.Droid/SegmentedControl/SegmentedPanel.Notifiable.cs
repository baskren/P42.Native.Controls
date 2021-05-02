using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace P42.Native.Controls.Droid
{
    public partial class SegmentedPanel : INotifyPropertyChanged
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

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("null propertyName in SetField");

            field = value;
            HasChanged = true;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected bool SetRedrawField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (SetField(ref field, value, propertyName, callerPath))
            {
                if (hasDrawn) PostInvalidate();
                return true;
            }
            return false;
        }

        protected bool SetLayoutField<T>(ref T field, T value, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (SetField(ref field, value, propertyName, callerPath))
            {
                if (hasDrawn) RequestLayout();
                return true;
            }
            return false;
        }
        #endregion
    }
}
