<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> MarkdownSnippets

[![Build status](https://ci.appveyor.com/api/projects/status/8ijthhby6mhw8fk3/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/MarkdownSnippets)
[![NuGet Status](https://img.shields.io/nuget/v/MarkdownSnippets.Tool.svg?label=dotnet%20tool)](https://www.nuget.org/packages/MarkdownSnippets.Tool/)
[![NuGet Status](https://img.shields.io/nuget/v/MarkdownSnippets.MsBuild.svg?label=MsBuild%20Task)](https://www.nuget.org/packages/MarkdownSnippets.MsBuild/)
[![NuGet Status](https://img.shields.io/nuget/v/MarkdownSnippets.svg?label=.net%20API)](https://www.nuget.org/packages/MarkdownSnippets/)

A [dotnet tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) that extract snippets from code files and merges them into markdown documents.

Support is available via a [Tidelift Subscription](https://tidelift.com/subscription/pkg/nuget-markdownsnippets?utm_source=nuget-markdownsnippets&utm_medium=referral&utm_campaign=enterprise).

<!-- toc -->
## Contents

  * [Value Proposition](#value-proposition)
  * [Installation](#installation)
  * [Usage](#usage)
  * [Defining Snippets](#defining-snippets)
  * [Using Snippets](#using-snippets)
  * [More Documentation](#more-documentation)
  * [Security contact information](#security-contact-information)<!-- endtoc -->


## Value Proposition

Automatically extract snippets from code and injecting them into markdown documents has several benefits:

 * Snippets can be verified by a compiler or parser.
 * Tests can be run on snippets, or snippets can be pulled from existing tests.
 * Changes in code are automatically reflected in documentation.
 * Snippets are less likely to get out of sync with the main code-base.
 * Snippets in markdown is easier to create and maintain since any preferred editor can be used to edit them.


## Installation

Ensure [dotnet CLI is installed](https://docs.microsoft.com/en-us/dotnet/core/tools/).

Install [MarkdownSnippets.Tool](https://nuget.org/packages/MarkdownSnippets.Tool/)

```ps
dotnet tool install -g MarkdownSnippets.Tool
```


## Usage

```ps
mdsnippets C:\Code\TargetDirectory
```

If no directory is passed the current directory will be used, but only if it exists with a git repository directory tree. If not an error is returned.


### Behavior

 * Recursively scan the target directory for all non [ignored files](#ignore-paths) for snippets.
 * Recursively scan the target directory for all `*.source.md` files.
 * Merge the snippets with the `.source.md` to produce `.md` files. So for example `readme.source.md` would be merged with snippets to produce `readme.md`. Note that this process will overwrite any existing `.md` files that have matching `.source.md` files.


### mdsource directory convention

There is a secondary convention that leverages the use of a directory named `mdsource`. Where `.source.md` files are placed in a `mdsource` sub-directory, the `mdsource` part of the file path will be removed when calculating the target path. This allows the `.source.md` to be grouped in a sub directory and avoid cluttering up the main documentation directory.

When using the `mdsource` convention, all references to other files, such as links and images, should specify the full path from the root of the repository. This will allow those links to work correctly in both the source and generated markdown files. Relative paths cannot work for both the source and the target file.


### Mark resulting files as read only

To mark the resulting `.md` files as read only use `-r` or `--readonly`.

This can be helpful in preventing incorrectly editing the `.md` file instead of the `.source.md` file.

```ps
mdsnippets -r true
```



## Defining Snippets

Any code wrapped in a convention based comment will be picked up. The comment needs to start with `begin-snippet:` which is followed by the key. The snippet is then terminated by `end-snippet`.

```
// begin-snippet: MySnippetName
My Snippet Code
// end-snippet
```

Named [C# regions](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-region) will also be picked up, with the name of the region used as the key.

To stop regions collapsing in Visual Studio [disable 'enter outlining mode when files open'](/docs/stop-regions-collapsing.png). See [Visual Studio outlining](https://docs.microsoft.com/en-us/visualstudio/ide/outlining).


### UrlsAsSnippets

Urls to files to be included as snippets. Space ` ` separated for multiple values.

```ps
mdsnippets --urls-as-snippets "https://github.com/SimonCropp/MarkdownSnippets/snippet.cs"
```


## Using Snippets

The keyed snippets can be used in any documentation `.md` file by adding the text `snippet: KEY`.

Then snippets with that key.

For example

<pre>
Some blurb about the below snippet
snippet&#58; MySnippetName
</pre>

The resulting markdown will be:

    <!-- snippet: MySnippetName -->
    Some blurb about the below snippet
    <a id='snippet-MySnippetName'/></a>
    ```
    My Snippet Code
    ```
    <sup><a href='/relativeUrlToFile#L1-L11' title='File snippet `MySnippetName` was extracted from'>snippet source</a> | <a href='#snippet-MySnippetName' title='Navigate to start of snippet `MySnippetName`'>anchor</a></sup>
    <!-- endsnippet -->

Notes:

 * The vertical bar ( | ) is used to separate adjacent links as per web accessibility recommendations: https://webaim.org/techniques/hypertext/hypertext_links#groups
 * [H33: Supplementing link text with the title attribute](https://www.w3.org/TR/WCAG20-TECHS/H33.html)


### LinkFormat

Defines the format of `snippet source` links that appear under each snippet.

<!-- snippet: LinkFormat.cs -->
<a id='snippet-LinkFormat.cs'/></a>
```cs
namespace MarkdownSnippets
{
    public enum LinkFormat
    {
        GitHub,
        Tfs,
        Bitbucket,
        GitLab
    }
}
```
<sup><a href='/src/MarkdownSnippets/Processing/LinkFormat.cs#L1-L10' title='File snippet `LinkFormat.cs` was extracted from'>snippet source</a> | <a href='#snippet-LinkFormat.cs' title='Navigate to start of snippet `LinkFormat.cs`'>anchor</a></sup>
<!-- endsnippet -->

<!-- snippet: BuildLink -->
<a id='snippet-buildlink'/></a>
```cs
if (linkFormat == LinkFormat.GitHub)
{
    return $"{path}#L{snippet.StartLine}-L{snippet.EndLine}";
}

if (linkFormat == LinkFormat.Tfs)
{
    return $"{path}&line={snippet.StartLine}&lineEnd={snippet.EndLine}";
}

if (linkFormat == LinkFormat.Bitbucket)
{
    return $"{path}#lines={snippet.StartLine}:{snippet.EndLine}";
}

if (linkFormat == LinkFormat.GitLab)
{
    return $"{path}#L{snippet.StartLine}-{snippet.EndLine}";
}
```
<sup><a href='/src/MarkdownSnippets/Processing/SnippetMarkdownHandling.cs#L103-L123' title='File snippet `buildlink` was extracted from'>snippet source</a> | <a href='#snippet-buildlink' title='Navigate to start of snippet `buildlink`'>anchor</a></sup>
<!-- endsnippet -->


### UrlPrefix

UrlPrefix allows a string to be defined that will prefix all snippet links. This is helpful when the markdown file are being hosted on a site that is no co-located with the source code files. It can be defined in the [config file](/docs/config-file.md), the [MsBuild task](/docs/msbuild.md), and the dotnet tool.


## More Documentation

  * [.net API](/docs/api.md) <!-- include: doc-index. path: /docs/mdsource/doc-index.include.md -->
  * [MsBuild Task](/docs/msbuild.md)
  * [Github Action](/docs/github-action.md)
  * [Config file convention](/docs/config-file.md)
  * [Indentation](/docs/indentation.md)
  * [Max Width](/docs/max-width.md)
  * [iIncludes](/docs/includes.md)
  * [Snippet Exclusion](/docs/snippet-exclusion.md)
  * [Header](/docs/header.md)
  * [Table of contents](/docs/toc.md) <!-- end include: doc-index. path: /docs/mdsource/doc-index.include.md -->


## Security contact information

To report a security vulnerability, use the [Tidelift security contact](https://tidelift.com/security). Tidelift will coordinate the fix and disclosure.


## Credits

Loosely based on some code from https://github.com/shiftkey/scribble.


## Icon

[Down](https://thenounproject.com/AlfredoCreates/collection/arrows-5-glyph/) by [Alfredo Creates](https://thenounproject.com/AlfredoCreates) from [The Noun Project](https://thenounproject.com/).
