// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamRight.Checkers.Test.Mocks;
using XamRight.Checkers.Test.XamlUtilities;
using XamRight.Checkers.Xaml;

namespace XamRight.Checkers.Test.Xaml
{
    [TestClass]
    public class BindableLayoutCheckerTests
    {
        private readonly BindableLayoutChecker _checker = new BindableLayoutChecker();

        [TestMethod]
        public void CheckerSetupTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\BindableLayoutTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var nodeChecker = _checker.GetCheckerForNode(rootNode, warningService, contextService);
            Assert.IsNotNull(nodeChecker);
            Assert.IsFalse(nodeChecker.ShouldCheckAttributesOnNode(rootNode));
        }

        [TestMethod]
        public void BindableLayoutItemTemplateWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\BindableLayoutTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(1, warningService.Warnings.Count);
            Assert.AreEqual(5072, warningService.Warnings[0].WarningDefinition.Number);
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }

        [TestMethod]
        public void BindableLayoutDataTemplateWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\BindableLayoutTest2.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(1, warningService.Warnings.Count);
            Assert.AreEqual(5071, warningService.Warnings[0].WarningDefinition.Number);
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }
    }
}
