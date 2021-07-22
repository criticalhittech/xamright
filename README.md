# XamRight Extensibility #

## Development Environment ##

__Windows__

Visual Studio 2017 is the primary development platform.  While you can do your development
in VS 2019, XamRight production builds use VS 2017 in order to be able to be used as a
VS 2017 extension.  This means that in the end you can't take dependencies on any language
feature not available in VS 2017.

__Mac__

Visual Studio Mac 2019 is the development environment, but see notes above about not
using new language features.

## Component Overview ##

### XamRight.Extensibility ###

This project defines the interface into XamRight that you can use to write your checkers.

At a high level, the parts of the Extensibilty project have the following roles:

__AnalysisContext__

Information available to the checker provided by the XamRight analysis engine. An instance of `ContextService`
is passed to the checker at runtime and may be queried as needed to perform analysis.

__Checkers__

These are the main classes and interfaces you need to implement checkers. 
Each XamRight checker requires two primary classes to define its behavior:

```
[XamRightChecker]
public SampleChecker : IXamlChecker
{
    IXamlNodeChecker GetCheckerForNode(
        IXmlSyntaxNode node, WarningService warningService, ContextService contextService);
}

public SampleNodeChecker : IXamlNodeChecker
{
    void CheckNode(IXmlSyntaxNode node);
    bool ShouldCheckAttributesOnNode(IXmlSyntaxNode node);
    void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute);
    void NodeComplete(IXmlSyntaxNode node);
    void CheckerComplete();
}
```

The checkers to run are identified by XamRight using the `[XamRightChecker]` attribute on implementations
of `IXamlChecker`.

Conceptually, an IXamlChecker object is a "hook" that XamRight uses to interact with the checker, but is intended to be
stateless. It is created without any state in terms of a specific Xaml file, Xaml framework, Visual Studio project, or solution.
Instead, when there is a need to perform analysis, the IXamlChecker method `GetCheckerForNode` is called with the
context of a specific node in a Xaml file, along with additional context and services to do the analysis.

The method `GetCheckerForNode` returns an `IXamlNodeChecker`, which is bound to the specific context in which it
was created.  These are intended to be short-lived objects.

If the checker does not have a need to analyze a specific node, it returns null for `GetCheckerForNode`. However if
it returns a valid `IXamlNodeChecker`, that object will be asked to analyze each node and attribute in the subtree
rooted at the node for which it was created.  As a consequence, a given checker might have multiple node checkers
called for each node in a subtree.

The Xaml tree is visited in pre-order depth-first traversal, with attributes on a node being analyzed before the
node's children. If a node checker does not have the need to analyze any attributes on a given node, it should
return false for `ShouldCheckAttributesOnNode`, in which case that node checker will not receive any
`CheckAttribute` calls for that Xml node.

Once a subtree is complete, including all attributes and children, `NodeComplete` is invoked with that node. The
final call received by a node checker is `CheckerComplete`, after `NodeComplete` is called for the node that
originally created that node.

For a simplified implementation of the algorithm that drives this,
see `XamRight.Checkers.Test\XamlUtilities\CheckerRunner.cs`.

__Warnings__

An instance of the `WarningService` class is provided at runtime to checkers to be able to report warnings
found in Xaml files as well as potential fixes to be provided to users.

New warnings are defined in checkers with the `WarningDefinition` class.  All properties are required, except
`HelpLinkUri`, which may be left as null.

__Xml__

These are interfaces for classes defined by XamRight to represent the syntax of Xaml files (which is, of course, Xml).

### XamRight.Checkers ###

This is the project that contains the checkers that XamRight runs out of the box based on the
Extensibility project.  

### XamRight.Checkers.Test ###

Each Checker in XamRight.Checkers needs to have a unit test source file with one or
more TestMethods defined.  The checker unit test source files are in
`XamRight.Checkers.Test\Xaml`.  In order to make it easier to write test cases
using XML rather than having to generate test data entirely in C#, XML data files
can be added to `XamRight.Checkers.Test\Xaml\TestData`.

Each TestData file must be valid XML (e.g. starting with `<?xml ...`) but do not need
to be valid XAML.  They just need to have enough of the Xaml document to run the
appropriate test scenario.

When adding XML TestData files, make sure to set the Build Action to `Copy Always`
(the default is `Don't Copy`), or else it won't be visible when the unit tests
are executed.

