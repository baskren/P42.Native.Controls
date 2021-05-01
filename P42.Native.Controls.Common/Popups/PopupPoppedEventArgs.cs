using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    /// <summary>
    /// Event arguments passed by Popped event and WaitUntilPoppedAsyc() method in P42.Uno.Controls popups.
    /// </summary>
    public class PopupPoppedEventArgs : EventArgs
    {
        /// <summary>
        /// What was the cause of the Popup being popped?
        /// </summary>
        public PopupPoppedCause Cause { get; private set; }

        /// <summary>
        /// What object triggered the popup being popped
        /// </summary>
        public object Trigger { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cause"></param>
        /// <param name="trigger"></param>
        public PopupPoppedEventArgs(PopupPoppedCause cause, object trigger)
        {
            Cause = cause;
            Trigger = trigger;
        }
    }
}
