// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamRight.Checkers.Xaml;
using XamRight.Checkers.Test.Mocks;
using XamRight.Checkers.Test.XamlUtilities;

namespace XamRight.Checkers.Test.Xaml
{
    /// <summary>
    /// Summary description for AbsoluteLayoutCheckerTests
    /// </summary>
    [TestClass]
    public class AbsoluteLayoutCheckerTests
    {
        private readonly AbsoluteLayoutChecker _checker = new AbsoluteLayoutChecker();

        [TestMethod]
        public void CheckerSetupTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\AbsoluteLayoutTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var nodeChecker = _checker.GetCheckerForNode(rootNode, warningService, contextService);
            Assert.IsNotNull(nodeChecker);
            Assert.IsFalse(nodeChecker.ShouldCheckAttributesOnNode(rootNode));
            Assert.IsTrue(nodeChecker.ShouldCheckAttributesOnNode(rootNode.Children[0]));
        }

        [TestMethod]
        public void AbsoluteLayoutWarningTest()
        {
            var warningService = new MockWarningService();
            var contextService = new MockContextService(MockContextService.XamlMode.XamarinForms);

            var testFile = @"Xaml\TestData\AbsoluteLayoutTest1.xml";
            var parser = new XmlParser(testFile);
            var rootNode = parser.ParseFile();

            var checkerRunner = new CheckerRunner(_checker, warningService, contextService);
            checkerRunner.CheckTree(rootNode);

            Assert.AreEqual(1, warningService.Warnings.Count);
            Assert.AreEqual(1, warningService.CodeFixes.Count);
        }
    }
}
