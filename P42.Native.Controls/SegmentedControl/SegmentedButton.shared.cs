using System;
using System.Collections.Generic;
using System.Text;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [AddTrait(typeof(TNotifiable))]
    public partial class SegmentButton : DINotifiable
    {
        #region Properties
        Orientation b_Orientation;
        public Orientation Orientation
        {
            get => b_Orientation;
            set => SetField(ref b_Orientation, value, UpdateRadii);
        }

        internal bool IsHorizontal => b_Orientation == Orientation.Horizontal;

        SegmentPosition b_Position;
        internal SegmentPosition Position
        {
            get => b_Position;
            set => SetField(ref b_Position, value, UpdateRadii);
        }

        #region Events
        public event EventHandler<bool> SelectedChanged;
        #endregion

        int b_index = -1;
        public int Index
        {
            get => b_index;
            internal set => SetField(ref b_index, value, UpdateRadii);
        }

        #endregion

        partial void UpdateRadii();
    }
}
