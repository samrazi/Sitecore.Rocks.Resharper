// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertIdToPath.cs" company="Sitecore A/S">
//  Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   The convert id to path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.Rocks.Resharper.TextEditors.Actions
{
  using System;
  using JetBrains.Application.Progress;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.Bulbs;
  using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
  using JetBrains.ReSharper.Feature.Services.LinqTools;
  using JetBrains.ReSharper.Intentions.Extensibility;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.TextControl;
  using JetBrains.Util;
  using Sitecore.Rocks.Extensions.DataProviderExtensions;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Data.DataServices;
  using Sitecore.VisualStudio.Diagnostics;
  using Sitecore.VisualStudio.Extensions.StringExtensions;
  using Sitecore.VisualStudio.Sites;

  /// <summary>The convert id to path.</summary>
  [ContextAction(Name = "Convert ID to Sitecore Path [Sitecore Rocks]", Description = "Convert an ID to a Sitecore path.", Group = "C#")]
  public class ConvertIdToPath : ContextActionBase
  {
    #region Fields

    /// <summary>The _provider.</summary>
    private readonly ICSharpContextActionDataProvider provider;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="ConvertIdToPath"/> class.</summary>
    /// <param name="provider">The provider.</param>
    public ConvertIdToPath([NotNull] ICSharpContextActionDataProvider provider)
    {
      Assert.ArgumentNotNull(provider, "provider");

      this.provider = provider;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the GUID.
    /// </summary>
    /// <value>The GUID.</value>
    public Guid Guid { get; private set; }

    /// <summary>
    /// Gets the site.
    /// </summary>
    /// <value>The site.</value>
    [NotNull]
    public Site Site { get; private set; }

    /// <summary>
    /// Gets the string literal.
    /// </summary>
    /// <value>The string literal.</value>
    [NotNull]
    public ILiteralExpression StringLiteral { get; private set; }

    /// <summary>Gets the text.</summary>
    public override string Text
    {
      get
      {
        return "Convert ID to Sitecore Path [Sitecore Rocks]";
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>The is available.</summary>
    /// <param name="cache">The cache.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public override bool IsAvailable(IUserDataHolder cache)
    {
      Assert.ArgumentNotNull(cache, "cache");
      var literal = this.provider.GetSelectedElement<ILiteralExpression>(true, true);
      if (literal == null || !literal.IsConstantValue() || !literal.ConstantValue.IsString())
      {
        return false;
      }

      var text = literal.ConstantValue.Value as string;
      if (string.IsNullOrEmpty(text))
      {
        return false;
      }

      if (!text.StartsWith("{"))
      {
        return false;
      }

      Guid guid;
      if (!Guid.TryParse(text, out guid))
      {
        return false;
      }

      var project = this.provider.GetProject();
      if (project == null)
      {
        return false;
      }

      var site = project.Site;
      if (site == null)
      {
        return false;
      }

      this.Site = site;
      this.StringLiteral = literal;
      this.Guid = guid;

      return true;
    }

    #endregion

    #region Methods

    /// <summary>The execute psi transaction.</summary>
    /// <param name="solution">The solution.</param>
    /// <param name="progress">The progress.</param>
    /// <returns>The <see cref="Action"/>.</returns>
    protected override Action<ITextControl> ExecutePsiTransaction([NotNull] ISolution solution, [NotNull] IProgressIndicator progress)
    {
      Debug.ArgumentNotNull(solution, "solution");
      Debug.ArgumentNotNull(progress, "progress");

      var site = this.Site;

      var busy = true;
      var path = this.Guid.ToString();

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

        path = itemHeader.Path;

        busy = false;
      };

      site.DataService.ExecuteAsync("Items.GetItemHeader", completed, path, "master");

      while (busy)
      {
        AppHost.DoEvents();
      }

      var factory = CSharpElementFactory.GetInstance(this.provider.PsiModule);
      var expression = factory.CreateExpressionAsIs("\"" + path + "\"");
      this.StringLiteral.ReplaceBy(expression);

      return null;
    }

    #endregion
  }
}