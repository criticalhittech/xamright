// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamRight.Checkers.CheckerHelpers;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    public class StackLayoutNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private const string _stdInefficientLayoutUrlBase = "https://criticalhittech.com/xamright/warnings/inefficient-layout#XR";

        private bool _stackLayHasPaddingAttr;
        private bool _stackLayoutHasBackgroundColorAttr;
        private XRStackOrientation _stackOrientation = XRStackOrientation.Vertical;

        private static readonly WarningDefinition redundantLayoutOptionsDef = new WarningDefinition()
        {
            Number = 5301,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.RedundantLayoutOptionsOnStackLayoutWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = _stdInefficientLayoutUrlBase + "5301"
        };

        private static readonly WarningDefinition redundantLayoutExpansionDef = new WarningDefinition()
        {
            Number = 5302,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.RedundantLayoutExpansionOnStackLayoutWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = _stdInefficientLayoutUrlBase + "5302"
        };

        private static readonly WarningDefinition stackLayoutOneChildDef = new WarningDefinition()
        {
            Number = 5303,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.StackLayoutOneChildWarning,
            Category = WarningCategory.Layout,
            HelpLinkUri = _stdInefficientLayoutUrlBase + "5303"
        };

        private static readonly Dictionary<string, XRStackOrientation> _orientationDic = new Dictionary<string, XRStackOrientation>()
        {
            {"Horizontal", XRStackOrientation.Horizontal },
            {"Vertical", XRStackOrientation.Vertical }
        };

        public StackLayoutNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            if (node == _rootNode)
            {
                if (attribute.Name == "Orientation")
                    _orientationDic.TryGetValue(attribute.Value, out _stackOrientation);
                else if (attribute.Name == "Padding")
                    _stackLayHasPaddingAttr = true;
                else if (attribute.Name == "BackgroundColor")
                    _stackLayoutHasBackgroundColorAttr = true;
            }
            else if (node.Parent == _rootNode)
            {
                if (attribute.Name == LayoutOptionsValues.HorizontalOptsStr &&
                    LayoutOptionsValues.XRLayoutOptionsDictionary.TryGetValue(attribute.Value, out var horizontalOpts))
                {
                    AddHorizontalOptionsWarning(horizontalOpts, attribute, node);
                }
                else if (attribute.Name == LayoutOptionsValues.VerticalOptsStr &&
                    LayoutOptionsValues.XRLayoutOptionsDictionary.TryGetValue(attribute.Value, out var verticalOpts))
                {
                    AddVerticalOptionsWarning(verticalOpts, attribute, node);
                }
            }
        }

        private void AddHorizontalOptionsWarning(LayoutOptionsValues.XRLayoutOptions value, IXmlSyntaxAttribute attr, IXmlSyntaxNode node)
        {
            if (_stackOrientation == XRStackOrientation.Horizontal && node.Tag != "RelativeLayout"
               && (value == LayoutOptionsValues.XRLayoutOptions.Start ||
                   value == LayoutOptionsValues.XRLayoutOptions.Center ||
                   value == LayoutOptionsValues.XRLayoutOptions.End ||
                   value == LayoutOptionsValues.XRLayoutOptions.Fill))
            {
                _warningService.AddAttributeValueWarning(node, attr, redundantLayoutOptionsDef, new string[1] { "Horizontal" });

                _warningService.AddReplaceAttributeValueCodeFix(node, attr, redundantLayoutOptionsDef, attr.Value + "AndExpand", MessageResources.AddSuffixCodeFixMessage, new string[1] { "AndExpand" });
                _warningService.AddDeleteAttributeCodeFix(node, attr, redundantLayoutOptionsDef, MessageResources.RemoveCodeFixMessage, new string[1] { LayoutOptionsValues.HorizontalOptsStr });
            }
            else if (_stackOrientation == XRStackOrientation.Vertical
                && (value == LayoutOptionsValues.XRLayoutOptions.StartAndExpand ||
                    value == LayoutOptionsValues.XRLayoutOptions.CenterAndExpand ||
                    value == LayoutOptionsValues.XRLayoutOptions.EndAndExpand ||
                    value == LayoutOptionsValues.XRLayoutOptions.FillAndExpand))
            {
                _warningService.AddAttributeValueWarning(node, attr, redundantLayoutExpansionDef, new string[1] { "Horizontal" });

                _warningService.AddReplaceAttributeValueCodeFix(node, attr, redundantLayoutExpansionDef, attr.Value.Remove(attr.Value.IndexOf('A')), MessageResources.RemoveSuffixCodeFixMessage, new string[1] { "AndExpand" });
                _warningService.AddDeleteAttributeCodeFix(node, attr, redundantLayoutExpansionDef, MessageResources.RemoveCodeFixMessage, new string[1] { LayoutOptionsValues.HorizontalOptsStr });
            }
        }

        private void AddVerticalOptionsWarning(LayoutOptionsValues.XRLayoutOptions value, IXmlSyntaxAttribute attr, IXmlSyntaxNode node)
        {
            if (_stackOrientation == XRStackOrientation.Vertical && node.Tag != "RelativeLayout"
                && (value == LayoutOptionsValues.XRLayoutOptions.Start ||
                    value == LayoutOptionsValues.XRLayoutOptions.Center ||
                    value == LayoutOptionsValues.XRLayoutOptions.End ||
                    value == LayoutOptionsValues.XRLayoutOptions.Fill))
            {
                _warningService.AddAttributeValueWarning(node, attr, redundantLayoutOptionsDef, new string[1] { "Vertical" });

                _warningService.AddReplaceAttributeValueCodeFix(node, attr, redundantLayoutOptionsDef, attr.Value + "AndExpand", MessageResources.AddSuffixCodeFixMessage, new string[1] { "AndExpand" });
                _warningService.AddDeleteAttributeCodeFix(node, attr, redundantLayoutOptionsDef, MessageResources.RemoveCodeFixMessage, new string[1] { LayoutOptionsValues.VerticalOptsStr });
            }
            else if (_stackOrientation == XRStackOrientation.Horizontal
                && (value == LayoutOptionsValues.XRLayoutOptions.StartAndExpand ||
                    value == LayoutOptionsValues.XRLayoutOptions.CenterAndExpand ||
                    value == LayoutOptionsValues.XRLayoutOptions.EndAndExpand ||
                    value == LayoutOptionsValues.XRLayoutOptions.FillAndExpand))
            {
                _warningService.AddAttributeValueWarning(node, attr, redundantLayoutExpansionDef, new string[1] { "Vertical" });

                _warningService.AddReplaceAttributeValueCodeFix(node, attr, redundantLayoutExpansionDef, attr.Value.Remove(attr.Value.IndexOf('A')), MessageResources.RemoveSuffixCodeFixMessage, new string[1] { "AndExpand" });
                _warningService.AddDeleteAttributeCodeFix(node, attr, redundantLayoutExpansionDef, MessageResources.RemoveCodeFixMessage, new string[1] { LayoutOptionsValues.VerticalOptsStr });
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
            if (node == _rootNode)
            {
                // If there is a single child, then a layer can probably be omitted, unless:
                // - the StackLayout has Padding, which means the child does not occupy the full space
                // - the StackLayout has a BackgroundColor
                // - the Child has a Margin, same as above
                // - the Child tag has a ., which means it's an attached property, not a layout
                if (_rootNode.Children.Count == 1
                    && !_stackLayHasPaddingAttr
                    && !_stackLayoutHasBackgroundColorAttr
                    && !_rootNode.Children[0].Tag.Contains(".")
                    && !_rootNode.Children[0].Attributes.Any(attr => attr.Name == "Margin"))
                {
                    _warningService.AddNodeTagWarning(node, stackLayoutOneChildDef, new string[1] { _rootNode.Children[0].Tag });
                }
            }
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return (node == _rootNode || node.Parent == _rootNode);
        }
    }

    [XamRightChecker]
    public class StackLayoutChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;
            if (node.Tag == "StackLayout" && node.Children.Count > 0)
                return new StackLayoutNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
