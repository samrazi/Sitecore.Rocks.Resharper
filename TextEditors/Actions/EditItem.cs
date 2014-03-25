// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditItem.cs" company="Sitecore A/S">
//   Copyright (C) by Sitecore A/S
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Rocks.Resharper.TextEditors.Actions
{
  using System;
  using JetBrains.Application.Progress;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.Bulbs;
  using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
  using JetBrains.ReSharper.Intentions.Extensibility;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.TextControl;
  using JetBrains.Util;
  using Sitecore.Rocks.Extensions.DataProviderExtensions;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Sites;

  /// <summary>The convert id to path.</summary>
  [ContextAction(Name = "Edit Sitecore Item [Sitecore Rocks]", Description = "Edit the Sitecore item.", Group = "C#")]
  public class EditItem : ContextActionBase
  {
    #region Fields

    /// <summary>The _provider.</summary>
    private readonly ICSharpContextActionDataProvider provider;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="EditItem"/> class.</summary>
    /// <param name="provider">The provider.</param>
    public EditItem(ICSharpContextActionDataProvider provider)
    {
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

    /// <summary>Gets the text.</summary>
    public override string Text
    {
      get
      {
        return "Edit Sitecore Item [Sitecore Rocks]";
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>The is available.</summary>
    /// <param name="cache">The cache.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public override bool IsAvailable(IUserDataHolder cache)
    {
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
      var site = this.Site;

      var itemUri = new ItemUri(new DatabaseUri(site, new DatabaseName("master")), new ItemId(this.Guid));
      var itemVersionUri = new ItemVersionUri(itemUri, LanguageManager.CurrentLanguage, VisualStudio.Data.Version.Latest);

      AppHost.OpenContentEditor(itemVersionUri);

      return null;
    }

    #endregion
  }
}