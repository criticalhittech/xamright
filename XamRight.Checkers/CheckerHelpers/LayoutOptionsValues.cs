// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;

namespace XamRight.Checkers.CheckerHelpers
{
    public enum XRStackOrientation
    {
        Vertical,
        Horizontal
    }

    public static class LayoutOptionsValues
    {
        public enum XRLayoutOptions
        {
            Start,
            Center,
            End,
            Fill,
            StartAndExpand,
            CenterAndExpand,
            EndAndExpand,
            FillAndExpand
        }

        public const string HorizontalOptsStr = "HorizontalOptions";
        public const string VerticalOptsStr = "VerticalOptions";

        public static readonly Dictionary<string, XRLayoutOptions> XRLayoutOptionsDictionary = new Dictionary<string, XRLayoutOptions>()
        {
            {"Start", XRLayoutOptions.Start },
            {"Center", XRLayoutOptions.Center },
            {"End", XRLayoutOptions.End },
            {"Fill", XRLayoutOptions.Fill },
            {"StartAndExpand", XRLayoutOptions.StartAndExpand },
            {"CenterAndExpand", XRLayoutOptions.CenterAndExpand },
            {"EndAndExpand", XRLayoutOptions.EndAndExpand },
            {"FillAndExpand", XRLayoutOptions.FillAndExpand }
        };
    }
}
