// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Test.XamlUtilities
{
    public class XmlFilePosition : IXmlFilePosition
    {
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
    }

    public class XmlSyntaxAttribute : IXmlSyntaxAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public IXmlFilePosition NamePosition { get; set; }
        public IXmlFilePosition ValuePosition { get; set; }
    }

    public class XmlNode : IXmlSyntaxNode
    {
        public string Tag { get; set; }
        public string Text { get; set; }
        public IXmlFilePosition PositionOfOpenTag { get; set; }
        public IXmlFilePosition PositionOfCloseTag { get; set; }
        public IReadOnlyList<IXmlSyntaxAttribute> Attributes { get; set; }
        public IReadOnlyList<IXmlSyntaxNode> Children { get; set; }
        public IXmlSyntaxNode Parent { get; set; }
        public bool IsEmptyElement { get; set; }
    }
}
