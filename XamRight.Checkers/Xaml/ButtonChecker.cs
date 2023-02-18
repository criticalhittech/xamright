using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Xaml
{
    public class ButtonNodeChecker : IXamlNodeChecker
    {
        private IXmlSyntaxNode _rootNode;
        private WarningService _warningService;
        private ContextService _contextService;
        private bool _hasCommand;
        private IXmlSyntaxAttribute _isEnabledAttribute;
        private bool _reported;

        private static readonly WarningDefinition buttonWithIsEnabledDef = new WarningDefinition()
        {
            Number = 5108,
            Severity = WarningSeverity.Warning,
            MessageFormat = MessageResources.ButtonCommandIsEnabledWarning,
            Category = WarningCategory.OtherXAMLErrors,
            HelpLinkUri = "https://criticalhittech.com/xamright/warnings/inefficient-layout#XR5311"
        };

        public ButtonNodeChecker(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            _rootNode = node;
            _warningService = warningService;
            _contextService = contextService;
        }

        public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
        {
            if (attribute.Name == "IsEnabled")
                _isEnabledAttribute = attribute;
            if (attribute.Name == "Command")
                _hasCommand = true;

            if (_hasCommand && _isEnabledAttribute != null && !_reported)
            {
                _warningService.AddAttributeNameWarning(node, _isEnabledAttribute, buttonWithIsEnabledDef);
                _reported = true;
            }
        }

        public void CheckerComplete()
        {
        }

        public void CheckNode(IXmlSyntaxNode node)
        {
        }

        public void NodeComplete(IXmlSyntaxNode node)
        {
        }

        public bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node)
        {
            return node == _rootNode;
        }
    }

    /// <summary>
    /// Check for combining IsEnabled and Command on a Button node.
    /// https://www.damirscorner.com/blog/posts/20201127-DisableAButtonWithCommandInXamarinForms.html
    /// </summary>
    [XamRightChecker]
    public class ButtonChecker : IXamlChecker
    {
        public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
        {
            if (!contextService.IsXamarinForms && !contextService.IsMaui)
                return null;
            if (node.Tag == "Button")
                return new ButtonNodeChecker(node, warningService, contextService);
            return null;
        }
    }
}
