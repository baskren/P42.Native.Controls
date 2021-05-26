using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [Trait]
    abstract partial class TNotifiable : INotifiable
    {



        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;




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

        [Overrideable]
        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null) { }

        [Overrideable]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) { }

        public bool SetField<T>(ref T field, T value, Action action = null, [CallerMemberName] string propertyName = null, [CallerFilePath] string callerPath = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            if (propertyName == null)
                throw new Exception("null propertyName in SetField");

            OnPropertyChanging(propertyName);
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            field = value;
            HasChanged = true;
            action?.Invoke();
            OnPropertyChanged(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

    }
}