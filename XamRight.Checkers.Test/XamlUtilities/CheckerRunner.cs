// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Checkers;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Test.XamlUtilities
{
    public class CheckerRunner
    {
        private IXamlChecker _factory;
        private WarningService _warningService;
        private ContextService _contextService;

        private List<IXamlNodeChecker> _currentCheckers = new List<IXamlNodeChecker>();

        public CheckerRunner(IXamlChecker factory, WarningService warningService, ContextService contextService)
        {
            _factory = factory;
            _warningService = warningService;
            _contextService = contextService;
        }

        public void CheckTree(IXmlSyntaxNode node)
        {
            var newChecker = _factory.GetCheckerForNode(node, _warningService, _contextService);
            if (newChecker != null)
                _currentCheckers.Add(newChecker);

            foreach (var checker in _currentCheckers)
            {
                checker.CheckNode(node);
                if (checker.ShouldCheckAttributesOnNode(node))
                {
                    foreach (var attr in node.Attributes)
                    {
                        checker.CheckAttribute(node, attr);
                    }
                }
            }

            foreach (var c in node.Children)
            {
                CheckTree(c);
            }

            foreach (var c in _currentCheckers)
                c.NodeComplete(node);

            if (newChecker != null)
            {
                newChecker.CheckerComplete();
                _currentCheckers.Remove(newChecker);
            }
        }
    }
}
