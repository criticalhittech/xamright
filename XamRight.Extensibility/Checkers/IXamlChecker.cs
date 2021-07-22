// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Extensibility.Checkers
{
    /// <summary>
    /// Primary interface between XamRight and Xaml Extensibility checkers. These classes must have a default
    /// constructor (i.e. no parameters) which will create node checkers to perform the actual checks.
    /// </summary>
    public interface IXamlChecker
    {
        IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService);
    }
}
