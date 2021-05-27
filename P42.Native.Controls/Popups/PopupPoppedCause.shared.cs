using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    /// <summary>
    /// Causes for P42.Uno.Controls popup being popped
    /// </summary>
    public enum PopupPoppedCause
    {
        /// <summary>
        /// popup is not pushed
        /// </summary>
        NotPushed,
        /// <summary>
        /// popup's background was touched
        /// </summary>
        BackgroundTouch,
        /// <summary>
        /// Devices [back] button was pressed
        /// </summary>
        HardwareBackButtonPressed,
        /// <summary>
        /// popups IsVisible property was set to false
        /// </summary>
        VisibilityPropertySet,
        /// <summary>
        /// A method (passed via trigger property) was called that popped the popup
        /// </summary>
        MethodCalled,
        /// <summary>
        /// The popup timed out
        /// </summary>
        Timeout,
        /// <summary>
        /// One of the popup's pre-packaged buttons was tapped
        /// </summary>
        ButtonTapped,
        /// <summary>
        /// One of the popup's pre-packaged segments was tapped
        /// </summary>
        SegmentTapped,
        /// <summary>
        /// User supplied trigger was given to the popup's CancelAsync method
        /// </summary>
        Custom,
        /*
        /// <summary>
        /// The popup was disposed
        /// </summary>
        Disposed
        */
        /// <summary>
        /// User moved pointer outside of Target
        /// </summary>
        PointerMoved,
    }
}
