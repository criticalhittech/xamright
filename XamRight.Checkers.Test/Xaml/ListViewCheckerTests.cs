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
    public class ListViewCheckerTests
    {
        private readonly ListViewChecker _checker = new ListViewChecker();

        [TestMethod]
        public void CheckerSetupTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\ListViewTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var nodeChecker = _checker.GetCheckerForNode(rootNode, warningService, contextService);
            Assert.IsNotNull(nodeChecker);
            Assert.IsFalse(nodeChecker.ShouldCheckAttributesOnNode(rootNode));
        }

        [TestMethod]
        public void ListViewItemTemplateContentWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\ListViewTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(3, warningService.Warnings.Count);
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5104));
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5035));
            Assert.AreEqual(1, warningService.CodeFixes.Count);
        }

        [TestMethod]
        public void ListViewDataTemplateContentWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\ListViewTest2.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(2, warningService.Warnings.Count);
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5919));
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5036));
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }

        [TestMethod]
        public void ListViewDataTemplateForHeaderFooterContentWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\ListViewTest3.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(2, warningService.Warnings.Count);
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5093));
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }
    }
}
