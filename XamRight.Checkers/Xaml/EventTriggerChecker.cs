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
    internal class EventTriggerNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;

        private static readonly WarningDefinition emptyEventOnEventTriggerDef = new WarningDefinition()
        {
            Number = 5103,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.EventTriggerEventEmptyWarning,
            Category = WarningCategory.OtherXAMLErrors,
        };

        private static readonly WarningDefinition noEventPropertyOnEventTriggerDef = new WarningDefinition()
        {
            Number = 5102,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.NoEventPropertyOnEventTriggerWarning,
            Category = WarningCategory.OtherXAMLErrors,
        };

        private IXmlSyntaxAttribute _eventAttribute;
        private IXmlSyntaxNode _eventAsNodeAttribute;

        public EventTriggerNodeChecker(IXmlSyntaxNode rootNode, WarningService warningService, ContextService contextService)
        {
            _rootNode = rootNode;
            _warningService = warningService;
            _contextService = contextService;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            if (attribute.Name == "Event")
            {
                _eventAttribute = attribute;
                if (String.IsNullOrEmpty(attribute.Value))
                    _warningService.AddAttributeNameWarning(node, attribute, emptyEventOnEventTriggerDef);
            }
        }

        public void CheckerComplete()
        {
            ;
        }

        public void CheckNode(IXmlSyntaxNode node)
        {
            if (node.Tag == "EventTrigger.Event")
                _eventAsNodeAttribute = node;
        }

        public void NodeComplete(IXmlSyntaxNode node)
        {
            if(node == _rootNode && _eventAttribute == null && _eventAsNodeAttribute == null)
            {
                _warningService.AddNodeTagWarning(node, noEventPropertyOnEventTriggerDef);
                _warningService.AddInsertAttributeCodeFix(node, 
                                                          noEventPropertyOnEventTriggerDef, 
                                                          "Event", 
                                                          "", 
                                                          MessageResources.InsertPropertyCodeFixMessage, 
                                                          new string[1] { "Event" });
            }
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return node == _rootNode;
        }
    }

    [XamRightChecker]
    public class EventTriggerChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;
            if (node.Tag == "EventTrigger")
                return new EventTriggerNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
