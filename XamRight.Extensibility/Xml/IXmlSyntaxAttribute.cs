// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;

namespace XamRight.Extensibility.Xml
{
    public interface IXmlSyntaxAttribute
    {
        string Name { get; }
        string Value { get; }
        IXmlFilePosition NamePosition { get; }
        IXmlFilePosition ValuePosition { get; }
    }
}
