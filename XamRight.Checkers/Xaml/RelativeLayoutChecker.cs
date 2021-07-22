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
    public class RelativeLayoutNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private static readonly WarningDefinition redundantLayoutOpsDef = new WarningDefinition()
        {
            Number = 5312,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.RedundantLayoutOptionsOnRelativeLayoutChildWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = "https://criticalhittech.com/xamright/warnings/inefficient-layout#XR5312"
        };

        public RelativeLayoutNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            if (attribute.Name == LayoutOptionsValues.VerticalOptsStr)
            {
                _warningService.AddAttributeNameWarning(node, 
                                                        attribute, 
                                                        redundantLayoutOpsDef, 
                                                        new string[1] { LayoutOptionsValues.VerticalOptsStr });
                _warningService.AddDeleteAttributeCodeFix(node, 
                                                          attribute, 
                                                          redundantLayoutOpsDef, 
                                                          MessageResources.DeleteUnnecessaryAttributeCodeFixMessage, 
                                                          new string[1] { LayoutOptionsValues.VerticalOptsStr });
            }
            else if (attribute.Name == LayoutOptionsValues.HorizontalOptsStr)
            {
                _warningService.AddAttributeNameWarning(node, 
                                                        attribute, 
                                                        redundantLayoutOpsDef, 
                                                        new string[1] { LayoutOptionsValues.HorizontalOptsStr });
                _warningService.AddDeleteAttributeCodeFix(node, 
                                                          attribute, 
                                                          redundantLayoutOpsDef,
                                                          MessageResources.DeleteUnnecessaryAttributeCodeFixMessage, 
                                                          new string[1] { LayoutOptionsValues.HorizontalOptsStr });
            }
        }

        public void CheckerComplete()
        {
            ;
        }

        public void CheckNode(IXmlSyntaxNode node)
        {
            ;
        }

        public void NodeComplete(IXmlSyntaxNode node)
        {
            ;
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return node.Parent == _rootNode;
        }
    }

    [XamRightChecker]
    public class RelativeLayoutChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;
            if (node.Tag == "RelativeLayout" && node.Children.Count > 0)
                return new RelativeLayoutNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
