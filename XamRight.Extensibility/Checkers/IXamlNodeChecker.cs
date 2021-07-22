// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.Xml;

namespace XamRight.Extensibility.Checkers
{
    /// <summary>
    /// Implemented by Xaml Extensibility checkers, these are created to analyze the Xaml tree with an initial scope
    /// given by the XamlChecker when it was created.
    /// </summary>
    public interface IXamlNodeChecker
    {
        void CheckNode(IXmlSyntaxNode node);
        bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node);
        void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute);
        void NodeComplete(IXmlSyntaxNode node);
        void CheckerComplete();
    }
}
