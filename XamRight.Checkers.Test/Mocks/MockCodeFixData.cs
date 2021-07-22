// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Test.Mocks
{
    public class MockCodeFixData
    {
        public enum FixKind
        {
            AddAttribute,
            DeleteAttribute,
            ReplaceAttribute
        }

        public FixKind Kind { get; set; }
        public IXmlSyntaxNode Node { get; set; }
        public IXmlSyntaxAttribute Attribute { get; set; }
        public string CodeFixNameParam { get; set; }
        public string CodeFixValueParam { get; set; }
        public WarningDefinition WarningDefinition { get; set; }
        public string HintFormat { get; set; }
        public string[] HintParams { get; set; }
    }
}
