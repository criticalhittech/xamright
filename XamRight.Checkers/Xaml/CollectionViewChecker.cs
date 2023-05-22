// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamRight.Checkers.Infrastructure;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    public class CollectionViewNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private const string _stdListViewDataTemplateWarningBase = "https://criticalhittech.com/xamright/warnings/itemtemplate-and-datatemplate#XR";

        private static readonly WarningDefinition collectionViewItemTemplateContentErrDef = new WarningDefinition()
        {
            Number = 5069,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.CollectionViewItemTemplateContentWarning,
            Category = WarningCategory.OtherXAMLErrors,
            HelpLinkUri = _stdListViewDataTemplateWarningBase + "5069"
        };

        private static readonly WarningDefinition collectionViewDataTemplateContentErrDef = new WarningDefinition()
        {
            Number = 5068,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.CollectionViewDataTemplateContentWarning,
            Category = WarningCategory.OtherXAMLErrors,
            HelpLinkUri = _stdListViewDataTemplateWarningBase + "5068"
        };

        private static readonly WarningDefinition collectionViewSelectionChangedErrDef = new WarningDefinition()
        {
            Number = 5099,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.CollectionViewSelectionChangedWarning,
            Category = WarningCategory.OtherXAMLErrors,
        };

        private static readonly WarningDefinition collectionViewSelectionChangedNotFiredErrDef = new WarningDefinition()
        {
            Number = 5109,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.CollectionViewSelectionChangedDoesNotGetFiredWhenUsingFrameLayout,
            Category = WarningCategory.OtherXAMLErrors,
            HelpLinkUri = "https://github.com/dotnet/maui/issues/9567"
        };

        public CollectionViewNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public static bool IsCollectionView(IXmlSyntaxNode node, ContextService contextService)
        {
            bool isCollectionView = false;

            if (contextService.IsXamarinForms)
                isCollectionView = contextService.DoesNodeSymbolDeriveFromBaseType(node, "Xamarin.Forms.CollectionView");
            else if (contextService.IsMaui)
                isCollectionView = contextService.DoesNodeSymbolDeriveFromBaseType(node, "Microsoft.Maui.Controls.CollectionView");

            return isCollectionView;
        }

        private IXmlSyntaxAttribute _collectionViewSelectionChangedAttribute;
        private IXmlSyntaxAttribute _collectionViewSelectionModeAttribute;
        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            if (attribute.Name == "SelectionChanged")
                _collectionViewSelectionChangedAttribute = attribute;
            if (attribute.Name == "SelectionMode")
                _collectionViewSelectionModeAttribute = attribute;
        }

        public void CheckerComplete()
        {
            ;
        }

        public void CheckNode(IXmlSyntaxNode node)
        {
            if (node.Parent != _rootNode)
                return;

            if (node.Tag.EndsWith(".ItemTemplate"))
            {
                AddDataTemplateWarning(node);
                AddSelectionChangedDoesNotGetFiredWarning(node);
            }

            if (node.Children.Any()
                && node.Children[0].Children.Any()
                && node.Children[0].Children[0].Tag == "SwipeView")
                CheckSwipeViewHasAutomationProperties(node.Children[0].Children[0]);
        }

        private void AddDataTemplateWarning(IXmlSyntaxNode node)
        {
            if (node.Children != null && node.Children.Count > 0)
            {
                var dataTemplateNode = node.Children[0];

                if (node.Children.Count > 1 || !ListViewNodeChecker.IsDataTemplateNode(dataTemplateNode, _contextService))
                {
                    _warningService.AddNodeTagWarning(dataTemplateNode, collectionViewItemTemplateContentErrDef, null);
                    return;
                }

                if (dataTemplateNode.Children != null && dataTemplateNode.Children.Count > 0)
                {
                    if (ListViewNodeChecker.IsCellNode(dataTemplateNode.Children[0], _contextService))
                    {
                        _warningService.AddNodeTagWarning(dataTemplateNode.Children[0], collectionViewDataTemplateContentErrDef, null);
                    }
                }
            }
        }

        private void AddSelectionChangedDoesNotGetFiredWarning(IXmlSyntaxNode itemTemplateNode)
        {
            if (!_contextService.IsMaui)
                return;

            if (_collectionViewSelectionChangedAttribute == null)
                return;

            AddSelectionChangedDoesNotGetFiredWarningIfIsFrameLayout(itemTemplateNode);
        }

        private void AddSelectionChangedDoesNotGetFiredWarningIfIsFrameLayout(IXmlSyntaxNode node)
        {
            if (node.Children == null || node.Children.Count == 0)
                return;

            foreach (var childNode in node.Children)
            {
                if(_contextService.DoesNodeSymbolDeriveFromBaseType(childNode, "Microsoft.Maui.Controls.Frame"))
                {
                    _warningService.AddNodeTagWarning(childNode, collectionViewSelectionChangedNotFiredErrDef, null);
                }
                AddSelectionChangedDoesNotGetFiredWarningIfIsFrameLayout(childNode);
            }
        }

        private void CheckSwipeViewHasAutomationProperties(IXmlSyntaxNode node)
        {
            var automationProperties = _contextService.IsMaui ? AccessibilityProperties.MauiAutomationProperties : AccessibilityProperties.XamAutomationProperties;

            if (!node.Children.Any(child => child.Attributes.Any(attr => automationProperties.Contains(attr.Name))))
            {
                _warningService.AddNodeTagWarning(node,
                                                  ListViewNodeChecker.MissingAutomationPropertiesToHintListViewContextActionsDef,
                                                  new string[2] { node.Tag, _rootNode.Tag });
            }
        }

        public void NodeComplete(IXmlSyntaxNode node)
        {
            if (node != _rootNode)
                return;

            if (_collectionViewSelectionChangedAttribute == null)
                return;

            if (_collectionViewSelectionModeAttribute == null ||
                (_collectionViewSelectionModeAttribute.Value != "Single" && _collectionViewSelectionModeAttribute.Value != "Multiple"))
            {
                _warningService.AddAttributeNameWarning(node, _collectionViewSelectionChangedAttribute, collectionViewSelectionChangedErrDef);
            }
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return true;
        }
    }

    [XamRightChecker]
    public class CollectionViewChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (node.Children.Count == 0)
                return null;

            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;

            //Xamarin.Forms only supports CollectionView starting from Version 4.3
            if (contextService.IsXamarinForms && !contextService.IsXamarinFormsVersionSupported(XamarinFormsVersionEnum.V_4_3))
                return null;

            if (CollectionViewNodeChecker.IsCollectionView(node, contextService))
                return new CollectionViewNodeChecker(node, warningService, contextService);

            return null;
        }
    }
}
