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
    /// <summary>
    /// Summary description for NestedScrollableLayoutsTests
    /// </summary>
    [TestClass]
    public class NestedScrollableLayoutsTests
    {
        private readonly NestedScrollableLayoutsChecker _checker = new NestedScrollableLayoutsChecker();

        [TestMethod]
        public void CheckerSetupTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\NestedScrollableLayoutsTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var nodeChecker = _checker.GetCheckerForNode(rootNode, warningService, contextService);
            Assert.IsNotNull(nodeChecker);
            Assert.IsFalse(nodeChecker.ShouldCheckAttributesOnNode(rootNode));
        }

        [TestMethod]
        public void NestedScrollableLayoutsWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\NestedScrollableLayoutsTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(1, warningService.Warnings.Count);
            Assert.AreEqual(0, warningService.CodeFixes.Count);
        }
    }
}
