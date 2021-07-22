// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    public class BindableLayoutNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private const string _stdListViewDataTemplateWarningBase = "https://criticalhittech.com/xamright/warnings/itemtemplate-and-datatemplate#XR";

        private static readonly WarningDefinition bindableLayoutDataTemplateContentErrorDef = new WarningDefinition()
        {
            Number = 5071,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.BindableLayoutDataTemplateWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = _stdListViewDataTemplateWarningBase + "5071"
        };

        private static readonly WarningDefinition bindableLayoutItemTemplateContentErrorDef = new WarningDefinition()
        {
            Number = 5072,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.BindableLayoutItemTemplateWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = _stdListViewDataTemplateWarningBase + "5072"
        };

        public BindableLayoutNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
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
            if (node.Parent != _rootNode)
                return;

            if (node.Tag == "BindableLayout.ItemTemplate")
                AddDataTemplateWarning(node);
        }

        private void AddDataTemplateWarning(IXmlSyntaxNode node)
        {
            if (node.Children?.Count == 0)
                return;

            var dataTemplateNode = node.Children[0];

            if (node.Children.Count > 1 || !ListViewNodeChecker.IsDataTemplateNode(dataTemplateNode, _contextService))
            {
                _warningService.AddNodeTagWarning(node.Children[0], bindableLayoutItemTemplateContentErrorDef);
                return;
            }

            if (dataTemplateNode.Children?.Count > 0)
            {
                var dataTemplateContent = dataTemplateNode.Children[0];

                if (ListViewNodeChecker.IsCellNode(dataTemplateContent, _contextService))
                    _warningService.AddNodeTagWarning(dataTemplateContent, bindableLayoutDataTemplateContentErrorDef);
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
    public class BindableLayoutChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (node.Children.Count == 0)
                return null;

            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;

            //Xamarin.Forms only supports BindableLayout starting from Version 3.5
            if (contextService.IsXamarinForms && !contextService.IsXamarinFormsVersionSupported(VersionEnum.V_3_5))
                return null;

            return new BindableLayoutNodeChecker(node, warningService, contextService);
        }
    }
}
