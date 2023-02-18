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

Note that the source code provided is only for reference. Any checker implementation must
link to a XamRight.Extensibility.dll library built as part of the official signed build
process, and included in lib with this repo.

At a high level, the parts of the Extensibilty project have the following roles:

__AnalysisContext__

Information available to the checker provided by the XamRight analysis engine. An instance of `ContextService`
is passed to the checker at runtime and may be queried as needed to perform analysis.

__Checkers__

These are the interfaces you need to implement in order to define a checker. 
Each XamRight checker requires two primary classes to define its behavior:
<a name="checkerCode"></a>
```
[XamRightChecker]
 public class SampleChecker : IXamlChecker
 {
    public IXamlNodeChecker GetCheckerForNode(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
    {
        return new SampleNodeChecker(node, warningService, contextService);
    }
 }

 public class SampleNodeChecker : IXamlNodeChecker
 {
    private IXmlSyntaxNode _rootNode;
    private WarningService _warningService;
    private ContextService _contextService;

    public SampleNodeChecker(IXmlSyntaxNode node, WarningService warningService, ContextService contextService)
    {
        _rootNode = node;
        _warningService = warningService;
        _contextService = contextService;
    }

    public void CheckAttribute(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute)
    {
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
        return false;
    }
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
Unlike the interfaces in Checkers, these are implemented by the XamRight engine and passed to checkers,
not defined by implementers of checkers.


### XamRight.Checkers ###

This is the project that contains the checkers that XamRight runs out of the box based on the
Extensibility project.  

Use these as examples of the power of checkers.  You can also build and run these with
the unit test project (below) in the debugger to understand how these are called.

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

## Developing Custom Checkers

We recommend using this solution as a framework, with a checker project and a
unit test project, customizing the names and namespaces.

By using the unit test scaffolding provided in XamRight.Checkers.Test, you can
validate your checker quickly and easily.  And besides, unit test are good :-)

<ins> Step by step instructions on how to build a Custom Checker </ins>

1. Clone this repository onto your machine.
2. Open the solution with Visual Studio 2017 or 2019.
3. Add a new .NET Standard class library project to the solution (This is the project that will be built and later added as a Custom Extension).
4. Add a reference to the XamRight.Extensibility.dll located in the /lib folder (Adding a direct reference to the XamRight.Extensibility project will not work).
5. Add a new file for the Checker you want to add to the project.
6. Implement an instance of IXamlChecker and IXamlNodeChecker or you can start by copying & pasting the [boilerplate code](#checkerCode) above. You can also use the Checkers in XamRight.Checkers/Xaml as a reference.
7. After you've completed writing up your Checkers, set the build to Release and proceed to build your class library.
8. The compiled .dll file should be located in the bin/Release/netstandard2.0/ folder

Then in Visual Studio, open the window at:

>    Tools > Options > XamRight > Custom XamRight Extensions

Select the [...] button to browse to your new checker dll, select it, and load.
XamRight will copy the dll into its cache, so you don't have to worry about
overwriting it.

You can add multiple checkers, enable and disable them (using the check mark next to each),
replace them, or delete them.  Two important notes:
1. In order to replace or delete a checker, you have to restart Visual Studio after making the change in the XamRight UI.
2. XamRight only copies the checker DLL itself. If you add references to your checker besides
what's available in .NET Standard 1.3 and XamRight.Extensibility, your checker may fail to load.

## Updating XamRight.Checkers

If there are bugs in XamRight.Checkers, or additions you would like to submit,
we welcome Pull Requests.  To test your version of XamRight.Checkers, you need
to deactivate the built in XamRight.Checker.dll before trying to load your version.

<ins> Step by step instructions on how to extend XamRight.Checker.dll </ins>

1. Clone this repository onto your machine.
2. Open the solution with Visual Studio 2017 or 2019.
3. Make the necessary changes to the XamRight.Checker project.
4. After you're done, set the build to Release and proceed to build the XamRight.Checker project.
5. The compiled XamRight.Checker.dll file should be located in the bin/Release/netstandard2.0/ folder.
6. To test out your changes, disable the build in XamRight.Checker.dll on the Custom XamRight Extensions page and restart Visual Studio.
7. After you've restarted Visual Studio, you can proceed to add your updated version of XamRight.Checker.dll with the built in one disabled 
  (Note that when testing out an updated version of a built in Checker, it is required that the file name matches the built in one, in this case XamRight.Checker.dll).