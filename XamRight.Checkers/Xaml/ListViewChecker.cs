// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    public class ListViewNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private const string _stdListViewDataTemplateWarningBase = "https://criticalhittech.com/xamright/warnings/itemtemplate-and-datatemplate#XR";

        public static readonly WarningDefinition MissingAutomationPropertiesToHintListViewContextActionsDef = new WarningDefinition()
        {
            Number = 5919,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.MissingAutomationPropertiesToHintListViewContextActionsWarning,
            Category = WarningCategory.Accessibility
        };

        private static readonly WarningDefinition listViewItemTemplateContentErrDef = new WarningDefinition()
        {
            Number = 5035,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.ListViewItemTemplateContentWarning,
            Category = WarningCategory.OtherXAMLErrors,
            HelpLinkUri = _stdListViewDataTemplateWarningBase + "5035"
        };

        private static readonly WarningDefinition listViewDataTemplateMoreThanOneChildErrDef = new WarningDefinition()
        {
            Number = 5106,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.ListViewDataTemplateMoreThanOneChildWarnning,
            Category = WarningCategory.OtherXAMLErrors,
        };

        private static readonly WarningDefinition listViewDataTemplateContentErrDef = new WarningDefinition()
        {
            Number = 5036,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.ListViewDataTemplateContentWarning,
            Category = WarningCategory.OtherXAMLErrors,
            HelpLinkUri = _stdListViewDataTemplateWarningBase + "5036"
        };

        private static readonly WarningDefinition listViewDataTemplateForHeaderFooterContentErrDef = new WarningDefinition()
        {
            Number = 5093,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.ListViewDataTemplateForHeaderFooterContentWarning,
            Category = WarningCategory.OtherXAMLErrors,
        };

        private static readonly WarningDefinition listViewDoesNotSetHasUnevenRowsErrDef = new WarningDefinition()
        {
            Number = 5104,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.ListViewDoesNotSetHasUnevenRowsWarning,
            Category = WarningCategory.Accessibility,
        };

        public ListViewNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public static bool IsListView(IXmlSyntaxNode node, ContextService contextService)
        {
            bool isListView = false;

            if (contextService.IsXamarinForms)
                isListView = contextService.DoesNodeSymbolDeriveFromBaseType(node, "Xamarin.Forms.ListView");
            else if (contextService.IsMaui)
                isListView = contextService.DoesNodeSymbolDeriveFromBaseType(node, "Microsoft.Maui.Controls.ListView");

            return isListView;
        }

        public static bool IsDataTemplateNode(IXmlSyntaxNode node, ContextService contextService)
        {
            if (node.Tag == "DataTemplate")
                return true;

            if (contextService.IsXamarinForms)
                return contextService.DoesNodeSymbolDeriveFromBaseType(node, "Xamarin.Forms.DataTemplate");
            else if (contextService.IsMaui)
                return contextService.DoesNodeSymbolDeriveFromBaseType(node, "Microsoft.Maui.Controls.DataTemplate");

            return false;
        }

        public static bool IsCellNode(IXmlSyntaxNode node, ContextService contextService)
        {
            if (contextService.IsXamarinForms)
                return contextService.DoesNodeSymbolDeriveFromBaseType(node, "Xamarin.Forms.Cell");
            else if (contextService.IsMaui)
                return contextService.DoesNodeSymbolDeriveFromBaseType(node, "Microsoft.Maui.Controls.Cell");

            return false;
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
                CheckHasUnevenRowsInListView(node);

            if (node.Parent != _rootNode)
                return;

            if (node.Tag.EndsWith(".ItemTemplate")
                  || node.Tag.EndsWith(".HeaderTemplate")
                  || node.Tag.EndsWith(".FooterTemplate")
                  || node.Tag.EndsWith(".GroupHeaderTemplate"))
            {
                AddDataTemplateWarning(node);
            }

            if (node.Children.Any()
                && node.Children[0].Children.Any()
                && node.Children[0].Children[0].Tag == "ViewCell")
            {
                CheckViewCellHasAutomationProperties(node.Children[0].Children[0]);
            }
        }

        private void AddDataTemplateWarning(IXmlSyntaxNode node)
        {
            if (node.Children != null && node.Children.Count > 0)
            {
                var dataTemplateNode = node.Children[0];

                if (node.Children.Count > 1 || !IsDataTemplateNode(dataTemplateNode, _contextService))
                {
                    _warningService.AddNodeTagWarning(node.Children[0], listViewItemTemplateContentErrDef);
                    return;
                }

                if (dataTemplateNode.Children != null && dataTemplateNode.Children.Count > 0)
                {
                    if (dataTemplateNode.Children.Count > 1)
                    {
                        _warningService.AddNodeTagWarning(dataTemplateNode.Children[0], listViewDataTemplateMoreThanOneChildErrDef, new string[1] { node.Tag });
                        return;
                    }

                    var dataTemplateContentNode = dataTemplateNode.Children[0];

                    //ItemTemplate and GroupHeaderTemplate must be wrapped in a ViewCell
                    if ((node.Tag.EndsWith(".ItemTemplate") || node.Tag.EndsWith(".GroupHeaderTemplate"))
                        && !IsCellNode(dataTemplateContentNode, _contextService))
                    {
                        _warningService.AddNodeTagWarning(dataTemplateContentNode, listViewDataTemplateContentErrDef, new string[1] { node.Tag });
                    }

                    //HeaderTemplate and FooterTemplate should not be wrapped in a ViewCell
                    if ((node.Tag.EndsWith(".HeaderTemplate") || node.Tag.EndsWith(".FooterTemplate"))
                        && IsCellNode(dataTemplateContentNode, _contextService))
                    {
                        _warningService.AddNodeTagWarning(dataTemplateContentNode, listViewDataTemplateForHeaderFooterContentErrDef, new string[1] { node.Tag });
                    }
                }
            }
        }

        private void CheckHasUnevenRowsInListView(IXmlSyntaxNode node)
        {
            var hasUnevenRowsAttr = node.Attributes.FirstOrDefault(attr => attr.Name == "HasUnevenRows");

            if (hasUnevenRowsAttr == null || String.Equals(hasUnevenRowsAttr.Value, "False", StringComparison.OrdinalIgnoreCase))
            {
                if (hasUnevenRowsAttr == null)
                {
                    _warningService.AddNodeTagWarning(node, listViewDoesNotSetHasUnevenRowsErrDef, new string[1] { node.Tag });
                    _warningService.AddInsertAttributeCodeFix(node,
                                                              listViewDoesNotSetHasUnevenRowsErrDef,
                                                              "HasUnevenRows",
                                                              "True",
                                                              MessageResources.InsertPropertyCodeFixMessage,
                                                              new string[1] { "HasUnevenRows" });
                }
                else
                {
                    _warningService.AddAttributeValueWarning(node, hasUnevenRowsAttr, listViewDoesNotSetHasUnevenRowsErrDef, new string[1] { node.Tag });
                    _warningService.AddReplaceAttributeValueCodeFix(node,
                                                                    hasUnevenRowsAttr,
                                                                    listViewDoesNotSetHasUnevenRowsErrDef,
                                                                    "True",
                                                                    MessageResources.ReplaceAttributeValueCodeFixMessage,
                                                                    new string[2] { hasUnevenRowsAttr.Name, "True" });
                }
            }
        }

        private void CheckViewCellHasAutomationProperties(IXmlSyntaxNode viewCellNode)
        {
            if (viewCellNode.Children.Any(child => child.Tag.EndsWith(".ContextActions"))
                && !viewCellNode.Children.Any(child => child.Attributes.Any(attr => attr.Name == "AutomationProperties.Name" ||
                                                                                   attr.Name == "AutomationProperties.HelpText")))
            {
                _warningService.AddNodeTagWarning(viewCellNode, 
                                                  MissingAutomationPropertiesToHintListViewContextActionsDef, 
                                                  new string[2] { viewCellNode.Tag, _rootNode.Tag });
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
    public class ListViewChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (node.Children.Count == 0)
                return null;

            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;

            if (ListViewNodeChecker.IsListView(node, contextService))
                return new ListViewNodeChecker(node, warningService, contextService);

            return null;
        }
    }
}
