// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Utilities;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    class AccessibilityDuplicateTextNodeChecker : IXamlNodeChecker
    {
        private WarningService _warningService;
        private ContextService _contextService;
        private Dictionary<string, HashSet<IXmlSyntaxNode>> _textToNode = new Dictionary<string, HashSet<IXmlSyntaxNode>>();

        private static readonly HashSet<string> _textPropertiesToCheckXam = new HashSet<string> { "Text", "AutomationProperties.Name", "AutomationProperties.HelpText" };
        private static readonly HashSet<string> _textPropertiesToCheckWPF = new HashSet<string> { "AutomationProperties.Name", "AutomationProperties.HelpText" };
        private static readonly WarningDefinition multipleIdenticalTextDef = new WarningDefinition()
        {
            Number = 5915,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.MultipleElementsWithIdenticalTextWarning,
            Category = WarningCategory.Accessibility
        };

        public AccessibilityDuplicateTextNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _warningService = warningService;
            _contextService = contextService;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            var textPropertiesToCheck = _contextService.IsXamarinForms ? _textPropertiesToCheckXam : _textPropertiesToCheckWPF;
            if (textPropertiesToCheck.Contains(attribute.Name) &&
                !String.IsNullOrEmpty(attribute.Value) &&
                //ignore markups
                !attribute.Value.StartsWithIgnoreWhitespace('{'))
            {
                HashSet<IXmlSyntaxNode> nodes;

                // if the text has not been saved off before 
                if (!_textToNode.TryGetValue(attribute.Value, out nodes))
                {
                    nodes = new HashSet<IXmlSyntaxNode>();
                    nodes.Add(node);
                    _textToNode.Add(attribute.Value, nodes);
                }
                else
                {
                    var matchingNode = nodes.First();
                    string[] paramValues =
                        {
                            attribute.Name,
                            node.Tag,
                            $"{matchingNode.Tag} (line {matchingNode.PositionOfOpenTag.LineNumber}, column {matchingNode.PositionOfOpenTag.ColumnNumber})"
                        };

                    _warningService.AddAttributeNameWarning(node, attribute, multipleIdenticalTextDef, paramValues);
                }
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
            return true;
        }
    }

    [XamRightChecker]
    public class AccessibilityDuplicateTextChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if ((contextService.IsXamarinForms || contextService.IsWpf || contextService.IsMaui) && node.Parent == null)
                return new AccessibilityDuplicateTextNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
