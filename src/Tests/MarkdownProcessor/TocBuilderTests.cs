﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class TocBuilderTests
{
    [Fact]
    public Task EmptyHeading()
    {
        var lines = new List<Line>
        {
            new Line("##", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task IgnoreTop()
    {
        var lines = new List<Line>
        {
            new Line("# Heading1", "", 0),
            new Line("## Heading2", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task SanitizeLink()
    {
        return Verifier.Verify(TocBuilder.SanitizeLink("A!@#$%,^&*()_+-={};':\"<>?/b"));
    }

    [Fact]
    public Task StripMarkdown()
    {
        var lines = new List<Line>
        {
            new Line("## **bold** *italic* [Link](link)", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task Exclude()
    {
        var lines = new List<Line>
        {
            new Line("## Heading1", "", 0),
            new Line("### Heading2", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string> {"Heading2"}, Environment.NewLine));
    }

    [Fact]
    public Task Nested()
    {
        var lines = new List<Line>
        {
            new Line("## Heading1", "", 0),
            new Line("### Heading2", "", 0),
            new Line("## Heading3", "", 0),
            new Line("### Heading4", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 2, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task Deep()
    {
        var lines = new List<Line>
        {
            new Line("## Heading1", "", 0),
            new Line("### Heading2", "", 0),
            new Line("#### Heading3", "", 0),
            new Line("##### Heading4", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 10, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task StopAtLevel()
    {
        var lines = new List<Line>
        {
            new Line("## Heading1", "", 0),
            new Line("### Heading2", "", 0),
            new Line("#### Heading3", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 2, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task Single()
    {
        var lines = new List<Line>
        {
            new Line("## Heading", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task WithSpaces()
    {
        var lines = new List<Line>
        {
            new Line("##  A B ", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task DuplicateNested()
    {
        var lines = new List<Line>
        {
            new Line("## Heading", "", 0),
            new Line("### Heading", "", 0),
            new Line("#### Heading", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines,4, new List<string>(), Environment.NewLine));
    }

    [Fact]
    public Task Duplicates()
    {
        var lines = new List<Line>
        {
            new Line("## A", "", 0),
            new Line("## A", "", 0),
            new Line("## a", "", 0)
        };

        return Verifier.Verify(TocBuilder.BuildToc(lines, 1, new List<string>(), Environment.NewLine));
    }
}