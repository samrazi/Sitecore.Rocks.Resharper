// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertPathToIdFix.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.Rocks.Resharper.TextEditors.ConvertPaths
{
  using System;
  using JetBrains.Application.Progress;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.Bulbs;
  using JetBrains.ReSharper.Feature.Services.LinqTools;
  using JetBrains.ReSharper.Intentions.Extensibility;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.TextControl;
  using JetBrains.Util;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Data.DataServices;
  using Sitecore.VisualStudio.Extensions.StringExtensions;

  /// <summary>
  /// The convert path to id fix.
  /// </summary>
  [QuickFix]
  public sealed class ConvertPathToIdFix : QuickFixBase
  {
    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ConvertPathToIdFix"/> class.</summary>
    /// <param name="highlight">The highlight.</param>
    public ConvertPathToIdFix([NotNull] ConvertPathToIdHighlight highlight)
    {
      this.Highlight = highlight;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Popup menu item text
    /// </summary>
    public override string Text
    {
      get
      {
        return "Use Sitecore ID [Sitecore Rocks]";
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the highlight.
    /// </summary>
    /// <value>The highlight.</value>
    [NotNull]
    private ConvertPathToIdHighlight Highlight { get; set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>Check if this action is available at the constructed context.
    ///             Actions could store precalculated info in <paramref name="cache"/> to share it between different actions</summary>
    /// <param name="cache">The cache.</param>
    /// <returns>true if this bulb action is available, false otherwise.</returns>
    public override bool IsAvailable(IUserDataHolder cache)
    {
      return true;
    }

    #endregion

    #region Methods

    /// <summary>Executes QuickFix or ContextAction. Returns post-execute method.</summary>
    /// <param name="solution">The solution.</param>
    /// <param name="progress">The progress.</param>
    /// <returns>Action to execute after document and PSI transaction finish. Use to open TextControls, navigate caret, etc.</returns>
    protected override Action<ITextControl> ExecutePsiTransaction([NotNull] ISolution solution, [NotNull] IProgressIndicator progress)
    {
      var literal = this.Highlight.LiteralExpression;

      var text = literal.ConstantValue.Value as string;
      if (string.IsNullOrEmpty(text))
      {
        return null;
      }

      var psiSourceFile = literal.GetSourceFile();
      if (psiSourceFile == null)
      {
        return null;
      }

      var projectFile = psiSourceFile.ToProjectFile();
      if (projectFile == null)
      {
        return null;
      }

      var p = projectFile.GetProject();
      if (p == null)
      {
        return null;
      }

      var projectFileLocation = p.ProjectFileLocation.ToString();
      if (string.IsNullOrEmpty(projectFileLocation))
      {
        return null;
      }

      var project = VisualStudio.Projects.ProjectManager.GetProject(projectFileLocation);
      if (project == null)
      {
        return null;
      }

      var site = project.Site;
      if (site == null)
      {
        return null;
      }

      var busy = true;
      var path = text;

      ExecuteCompleted completed = delegate(string response, ExecuteResult result)
      {
        if (!DataService.HandleExecute(response, result, true))
        {
          busy = false;
          return;
        }

        var root = response.ToXElement();
        if (root == null)
        {
          busy = false;
          return;
        }

        var itemHeader = ItemHeader.Parse(new DatabaseUri(site, new DatabaseName("master")), root);

        path = itemHeader.ItemId.ToString();

        busy = false;
      };

      site.DataService.ExecuteAsync("Items.GetItemHeader", completed, path, "master");

      while (busy)
      {
        AppHost.DoEvents();
      }

      var factory = CSharpElementFactory.GetInstance(literal.GetPsiModule());
      var expression = factory.CreateExpressionAsIs("\"" + path + "\"");
      this.Highlight.LiteralExpression.ReplaceBy(expression);

      return null;
    }

    #endregion
  }
}