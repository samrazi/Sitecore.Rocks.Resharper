// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertPathToIdHighlight.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.VisualStudio.TextEditors.ConvertPaths
{
  using JetBrains.DocumentModel;
  using JetBrains.ReSharper.Daemon;
  using JetBrains.ReSharper.Daemon.CSharp.Errors;
  using JetBrains.ReSharper.Daemon.Impl;
  using JetBrains.ReSharper.Psi.Tree;
  using Sitecore.VisualStudio.Annotations;

  /// <summary>Defines the <see cref="ConvertPathToIdHighlight"/> class.</summary>
  [ConfigurableSeverityHighlighting("ConvertPathToId", "CSHARP", OverlapResolve = OverlapResolveKind.WARNING, ToolTipFormatString = "Use Sitecore ID instead of item path [Sitecore Rocks]")]
  public class ConvertPathToIdHighlight : CSharpHighlightingBase, IHighlightingWithRange
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ConvertPathToIdHighlight"/> class.</summary>
    /// <param name="literal">The literal.</param>
    public ConvertPathToIdHighlight([NotNull] ILiteralExpression literal)
    {
      this.LiteralExpression = literal;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the message for this highlighting to show in tooltip and in status bar (if <see cref="P:JetBrains.ReSharper.Daemon.HighlightingAttributeBase.ShowToolTipInStatusBar" /> is <c>true</c>)
    /// </summary>
    /// <value>The error stripe tool tip.</value>
    [NotNull]
    public string ErrorStripeToolTip
    {
      get
      {
        return this.ToolTip;
      }
    }

    /// <summary>
    /// Gets the literal expression.
    /// </summary>
    /// <value>The literal expression.</value>
    [NotNull]
    public ILiteralExpression LiteralExpression { get; private set; }

    /// <summary>
    /// Gets the offset from the Range.StartOffset to set the cursor to when navigating
    /// to this highlighting. Usually returns <c>0</c>
    /// </summary>
    /// <value>The navigation offset patch.</value>
    public int NavigationOffsetPatch
    {
      get
      {
        return 0;
      }
    }

    /// <summary>
    /// Gets the message for this highlighting to show in tooltip and in status bar (if <see cref="P:JetBrains.ReSharper.Daemon.HighlightingAttributeBase.ShowToolTipInStatusBar" /> is <c>true</c>)
    /// To override the default mechanism of tooltip, mark the implementation class with
    /// <see cref="T:JetBrains.ReSharper.Daemon.DaemonTooltipProviderAttribute" /> attribute, and then this property will not be called
    /// </summary>
    /// <value>The tool tip.</value>
    [NotNull]
    public string ToolTip
    {
      get
      {
        return "Use Sitecore ID instead of item path [Sitecore Rocks]";
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>Returns true if data (PSI, text ranges) associated with highlighting is valid</summary>
    /// <returns>The <see cref="bool"/>.</returns>
    public override bool IsValid()
    {
      return this.LiteralExpression.IsValid();
    }

    #endregion

    #region Explicit Interface Methods

    /// <summary>
    /// Calculates the range.
    /// </summary>
    /// <returns>Returns the document range.</returns>
    DocumentRange IHighlightingWithRange.CalculateRange()
    {
      return this.LiteralExpression.GetDocumentRange();
    }

    #endregion
  }
}