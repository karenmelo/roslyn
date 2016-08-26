' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis.Structure
Imports Microsoft.CodeAnalysis.VisualBasic.Structure
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports MaSOutliners = Microsoft.CodeAnalysis.VisualBasic.Structure.MetadataAsSource

Namespace Microsoft.CodeAnalysis.Editor.VisualBasic.UnitTests.Outlining.MetadataAsSource
    Public Class DelegateDeclarationOutlinerTests
        Inherits AbstractVisualBasicSyntaxNodeOutlinerTests(Of DelegateStatementSyntax)

        Protected Overrides ReadOnly Property WorkspaceKind As String
            Get
                Return CodeAnalysis.WorkspaceKind.MetadataAsSource
            End Get
        End Property

        Friend Overrides Function CreateOutliner() As AbstractSyntaxStructureProvider
            Return New MaSOutliners.DelegateDeclarationOutliner()
        End Function

        <Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)>
        Public Async Function NoCommentsOrAttributes() As Task
            Dim code = "
Delegate Sub $$Bar()
"

            Await VerifyNoRegionsAsync(code)
        End Function

        Public Delegate Sub Bar(x As Int16)

        <Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)>
        Public Async Function WithAttributes() As Task
            Dim code = "
{|hint:{|collapse:<Foo>
|}Delegate Sub $$Bar()|}
"

            Await VerifyRegionsAsync(code,
                Region("collapse", "hint", VisualBasicOutliningHelpers.Ellipsis, autoCollapse:=True))
        End Function

        <Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)>
        Public Async Function WithCommentsAndAttributes() As Task
            Dim code = "
{|hint:{|collapse:' Summary:
'     This is a summary.
<Foo>
|}Delegate Sub $$Bar()|}
"

            Await VerifyRegionsAsync(code,
                Region("collapse", "hint", VisualBasicOutliningHelpers.Ellipsis, autoCollapse:=True))
        End Function

        <Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)>
        Public Async Function WithCommentsAttributesAndModifiers() As Task
            Dim code = "
{|hint:{|collapse:' Summary:
'     This is a summary.
<Foo>
|}Public Delegate Sub $$Bar()|}
"

            Await VerifyRegionsAsync(code,
                Region("collapse", "hint", VisualBasicOutliningHelpers.Ellipsis, autoCollapse:=True))
        End Function
    End Class
End Namespace