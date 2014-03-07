// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertPathToIdAnalyzer.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.VisualStudio.TextEditors.ConvertPaths
{
  using System;
  using JetBrains.ReSharper.Daemon.Stages;
  using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
  using JetBrains.ReSharper.Psi.Tree;

  /// <summary>
  /// Defines the ConvertPathToId class.
  /// </summary>
  [ElementProblemAnalyzer(new[]
  {
    typeof(ILiteralExpression)
  })]
  public class ConvertPathToIdAnalyzer : ElementProblemAnalyzer<ILiteralExpression>
  {
    #region Methods

    /// <summary>Runs the specified element.</summary>
    /// <param name="literal">The literal.</param>
    /// <param name="data">The data.</param>
    /// <param name="consumer">The consumer.</param>
    protected override void Run(ILiteralExpression literal, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
    {
      if (!literal.IsConstantValue() || !literal.ConstantValue.IsString())
      {
        return;
      }

      var text = literal.ConstantValue.Value as string;
      if (string.IsNullOrEmpty(text))
      {
        return;
      }

      if (text != "/sitecore" && !text.StartsWith("/sitecore/", StringComparison.OrdinalIgnoreCase))
      {
        return;
      }

      consumer.AddHighlighting(new ConvertPathToIdHighlight(literal), literal.GetDocumentRange(), literal.GetContainingFile());
    }

    #endregion
  }
}