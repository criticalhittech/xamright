// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamRight.Checkers.Xaml;
using XamRight.Checkers.Test.Mocks;
using XamRight.Checkers.Test.XamlUtilities;

namespace XamRight.Checkers.Test.Xaml
{
    [TestClass]
    public class AccessibilityDuplicateTextTests
    {
        private readonly AccessibilityDuplicateTextChecker _checker = new AccessibilityDuplicateTextChecker();

        [TestMethod]
        public void CheckerSetupTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\AccessibilityMultipleIdenticalTextTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var nodeChecker = _checker.GetCheckerForNode(rootNode, warningService, contextService);
            Assert.IsNotNull(nodeChecker);
            Assert.IsTrue(nodeChecker.ShouldCheckAttributesOnNode(rootNode));
            foreach (var c in rootNode.Children)
                Assert.IsTrue(nodeChecker.ShouldCheckAttributesOnNode(c));
        }

        [TestMethod]
        public void MultipleIdenticalTextWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\AccessibilityMultipleIdenticalTextTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(4, warningService.Warnings.Count);
            Assert.AreEqual(0, warningService.CodeFixes.Count);

            Assert.AreEqual("Text", warningService.Warnings[0].MsgParams[0]);
            Assert.AreEqual("Label (line 7, column 14)", warningService.Warnings[0].MsgParams[2]);

            Assert.AreEqual("AutomationProperties.Name", warningService.Warnings[1].MsgParams[0]);
            Assert.AreEqual("Label (line 11, column 14)", warningService.Warnings[1].MsgParams[2]);

            Assert.AreEqual("Text", warningService.Warnings[2].MsgParams[0]);
            Assert.AreEqual("Label (line 11, column 14)", warningService.Warnings[2].MsgParams[2]);

            Assert.AreEqual("AutomationProperties.HelpText", warningService.Warnings[3].MsgParams[0]);
            Assert.AreEqual("Label (line 16, column 14)", warningService.Warnings[3].MsgParams[2]);
        }
    }
}
