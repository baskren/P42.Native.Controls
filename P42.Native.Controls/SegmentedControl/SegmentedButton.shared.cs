using System;
using System.Collections.Generic;
using System.Text;
using SmartTraitsDefs;
namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TNotifiable))]
    public partial class SegmentButton 
    {
        
        #region Properties
        Orientation b_DipOrientation;
        public Orientation DipOrientation
        {
            get => b_DipOrientation;
            set => SetField(ref b_DipOrientation, value, UpdateRadii);
        }

        internal bool DipIsHorizontal => b_DipOrientation == Orientation.Horizontal;

        SegmentPosition b_DipPosition;
        internal SegmentPosition DipPosition
        {
            get => b_DipPosition;
            set => SetField(ref b_DipPosition, value, UpdateRadii);
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
