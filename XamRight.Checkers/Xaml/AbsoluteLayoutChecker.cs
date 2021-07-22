// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Checkers.CheckerHelpers;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    public class AbsoluteLayoutNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private static readonly WarningDefinition redundantLayoutOpsDef = new WarningDefinition()
        {
            Number = 5311,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.IgnoredLayoutAttrOnAbsoluteLayoutWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = "https://criticalhittech.com/xamright/warnings/inefficient-layout#XR5311"
        };

        public AbsoluteLayoutNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return node.Parent == _rootNode;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            if (attribute.Name == LayoutOptionsValues.VerticalOptsStr || attribute.Name == LayoutOptionsValues.HorizontalOptsStr)
            {
                _warningService.AddAttributeNameWarning(node, attribute, redundantLayoutOpsDef, attribute.Name);
                _warningService.AddDeleteAttributeCodeFix(node, attribute, redundantLayoutOpsDef, MessageResources.DeleteUnnecessaryAttributeCodeFixMessage, attribute.Name);
            }
        }

        public void CheckNode(IXmlSyntaxNode node)
        {
            ;
        }

        public void NodeComplete(IXmlSyntaxNode node)
        {
            ;
        }

        public void CheckerComplete()
        {
            ;
        }
    }

    /// <summary>
    /// Check for layout-related attributes on children of an AbsoluteLayout node that are not applicable to AbsoluteLayout.
    /// </summary>
    [XamRightChecker]
    public class AbsoluteLayoutChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;
            if (node.Tag == "AbsoluteLayout" && node.Children.Count > 0)
                return new AbsoluteLayoutNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
