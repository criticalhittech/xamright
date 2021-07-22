// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Xml;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Checkers;

namespace XamRight.Checkers.Xaml
{
    internal class NestedScrollableLayoutsNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private static readonly string[] _xamarinFormsScrollableLayouts = new string[2] { "Xamarin.Forms.ListView", "Xamarin.Forms.ScrollView" };
        private static readonly string[] _mauiScrollableLayouts = new string[2] { "Microsoft.Maui.Controls.ListView", "Microsoft.Maui.Controls.ScrollView" };

        private static readonly WarningDefinition nestedScrollableLayoutErrDef = new WarningDefinition()
        {
            Number = 5308,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.NestedScrollableLayoutsWarning,
            Category = WarningCategory.Layout,
        };

        public NestedScrollableLayoutsNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public static bool IsScrollableView(IXmlSyntaxNode node, ContextService contextService)
        {
            bool isScrollableView = false;

            if (contextService.IsXamarinForms)
                isScrollableView = contextService.DoesNodeSymbolDeriveFromAnyBaseTypes(node, _xamarinFormsScrollableLayouts);
            else if (contextService.IsMaui)
                isScrollableView = contextService.DoesNodeSymbolDeriveFromAnyBaseTypes(node, _mauiScrollableLayouts);
            
            return isScrollableView;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            ;
        }

        public void CheckerComplete()
        {
            ;
        }

        public void CheckNode(IXmlSyntaxNode node)
        {
            if (node == _rootNode)
                return;

            if (IsScrollableView(node, _contextService))
            {
                _warningService.AddNodeTagWarning(node,
                                                  nestedScrollableLayoutErrDef,
                                                  new string[2] { node.Tag, _rootNode.PositionOfOpenTag.LineNumber.ToString() });
            }
        }

        public void NodeComplete(IXmlSyntaxNode node)
        {
            ;
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return false;
        }
    }

    [XamRightChecker]
    public class NestedScrollableLayoutsChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;

            if (NestedScrollableLayoutsNodeChecker.IsScrollableView(node, contextService))
                return new NestedScrollableLayoutsNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
