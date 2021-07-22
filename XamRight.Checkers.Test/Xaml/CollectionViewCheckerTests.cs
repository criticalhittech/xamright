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
    public class CollectionViewCheckerTests
    {
        private readonly CollectionViewChecker _checker = new CollectionViewChecker();

        [TestMethod]
        public void CheckerSetupTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\CollectionViewTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var nodeChecker = _checker.GetCheckerForNode(rootNode, warningService, contextService);
            Assert.IsNotNull(nodeChecker);
            Assert.IsTrue(nodeChecker.ShouldCheckAttributesOnNode(rootNode));
        }

        [TestMethod]
        public void CollectionViewItemTemplateContentWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\CollectionViewTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(2, warningService.Warnings.Count);
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5099));
            Assert.IsTrue(warningService.Warnings.Any(w => w.WarningDefinition.Number == 5069));
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }

        [TestMethod]
        public void CollectionViewDataTemplateContentWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\CollectionViewTest2.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(1, warningService.Warnings.Count);
            Assert.AreEqual(5068, warningService.Warnings[0].WarningDefinition.Number);
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }

        [TestMethod]
        public void MissingAutomationPropertyOnSwipeViewWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\CollectionViewTest3.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(1, warningService.Warnings.Count);
            Assert.AreEqual(5913, warningService.Warnings[0].WarningDefinition.Number);
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }
    }
}
