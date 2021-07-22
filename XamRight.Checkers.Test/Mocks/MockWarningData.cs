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
    public class MockWarningData
    {
        public IXmlSyntaxNode Node { get; set; }
        public IXmlFilePosition FilePosition { get; set; }
        public int Length { get; set; }
        public WarningDefinition WarningDefinition { get; set; }
        public string[] MsgParams { get; set; }
    }
}
