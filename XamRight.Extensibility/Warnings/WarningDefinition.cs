// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;

namespace XamRight.Extensibility.Warnings
{
    public enum WarningSeverity
    {
        Informational,
        Warning,
        Error
    };

    public enum WarningCategory
    {
        Layout,
        Accessibility,
        OtherXAMLErrors
    }

    public class WarningDefinition
    {
        public int Number { get; set; }
        public WarningSeverity Severity { get; set; }
        public string MessageFormat { get; set; }
        public WarningCategory Category { get; set; }
        public string HelpLinkUri { get; set; }
    }
}
