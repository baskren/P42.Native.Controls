﻿using System;
using System.Collections.Generic;
using System.Text;
using SmartTraitsDefs;

namespace P42.Native.Controls
{
    [AddTrait(typeof(TNotifiable))]
    public partial class SegmentedButton : DINotifiable
    {
    }
}