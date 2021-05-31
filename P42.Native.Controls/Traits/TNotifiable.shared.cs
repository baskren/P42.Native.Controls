using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [SimpleTrait]
    partial class TNotifiable : INotifiable
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        #endregion

        
        #region Properties
        bool b_HasDrawn;
        public bool HasDrawn
        {
            get => b_HasDrawn;
            set => SetField(ref b_HasDrawn, value, () =>
            {
                if (HasDrawn)
                    HasDrawnTaskCompletionSource?.TrySetResult(true);
            });
        }

        public bool HasChanged { get; set; }
        #endregion

        
        #region Methods
        TaskCompletionSource<bool> HasDrawnTaskCompletionSource;
        public async Task WaitForDrawComplete()
        {
            if (HasDrawn)
                return;
            HasDrawnTaskCompletionSource = HasDrawnTaskCompletionSource ?? new TaskCompletionSource<bool>();
            await HasDrawnTaskCompletionSource.Task;
        }


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
            => HasChanged = false;

        protected bool SetField<T>(ref T field, T value, Action action = null, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("null propertyName in SetField");

            ((INotifiable)this).OnPropertyChanging(propertyName);
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            field = value;
            HasChanged = true;
            action?.Invoke();
            ((INotifiable)this).OnPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion
    }
}

