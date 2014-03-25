// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutInSecurityDisabler.cs" company="Sitecore A/S">
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
  using JetBrains.ReSharper.Feature.Services.LinqTools;
  using JetBrains.ReSharper.Intentions.Extensibility;
  using JetBrains.ReSharper.Intentions.Util;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.CSharp;
  using JetBrains.ReSharper.Psi.CSharp.Tree;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.TextControl;
  using JetBrains.Util;
  using Sitecore.VisualStudio.Annotations;

  /// <summary>The convert id to path.</summary>
  [ContextAction(Name = "Put in Security Disabler [Sitecore Rocks]", Description = "Wraps the code in a Security Disabler.", Group = "C#")]
  public class PutInSecurityDisabler : ContextActionBase
  {
    #region Fields

    /// <summary>The _provider.</summary>
    private readonly ICSharpContextActionDataProvider provider;

    #endregion

    #region Constructors and Destructors

    /// <summary>Initializes a new instance of the <see cref="PutInSecurityDisabler"/> class.</summary>
    /// <param name="provider">The provider.</param>
    public PutInSecurityDisabler(ICSharpContextActionDataProvider provider)
    {
      this.provider = provider;
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the text.</summary>
    public override string Text
    {
      get
      {
        return "Put in Security Disabler [Sitecore Rocks]";
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the invocation expression.
    /// </summary>
    /// <value>The invocation expression.</value>
    [CanBeNull]
    protected IInvocationExpression InvocationExpression { get; private set; }

    #endregion

    #region Public Methods and Operators

    /// <summary>The is available.</summary>
    /// <param name="cache">The cache.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public override bool IsAvailable(IUserDataHolder cache)
    {
      this.InvocationExpression = null;

      var invocationExpression = this.provider.GetSelectedElement<IInvocationExpression>(true, true);
      if (invocationExpression == null)
      {
        return false;
      }

      var referenceExpression = invocationExpression.InvokedExpression as IReferenceExpression;
      if (referenceExpression == null)
      {
        return false;
      }

      var declaredElement = referenceExpression.Reference.Resolve().DeclaredElement;
      if (declaredElement == null)
      {
        return false;
      }

      if (declaredElement.ShortName != "GetItem")
      {
        return false;
      }

      var containingType = this.GetContainingTypeName(declaredElement);
      if (containingType == null)
      {
        return false;
      }

      if (containingType != "Sitecore.Data.Database")
      {
        return false;
      }

      this.InvocationExpression = invocationExpression;

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
      var invocationExpression = this.InvocationExpression;
      if (invocationExpression == null)
      {
        return null;
      }

      var containingStatement = invocationExpression.GetContainingNode<IStatement>();
      if (containingStatement == null)
      {
        return null;
      }

      var code = "using (new Sitecore.SecurityModel.SecurityDisabler())\n{\n" + containingStatement.GetText() + "\n}";

      var factory = CSharpElementFactory.GetInstance(this.provider.PsiModule);
      var put = factory.CreateStatement(code);
      containingStatement.ReplaceBy(put);

      ContextActionUtils.FormatWithDefaultProfile(put);

      return null;
    }

    /// <summary>Gets the name of the containing type.</summary>
    /// <param name="declaredElement">The declared element.</param>
    /// <returns>Returns the string.</returns>
    [CanBeNull]
    private string GetContainingTypeName([NotNull] IDeclaredElement declaredElement)
    {
      var typeMember = declaredElement as ITypeMember;
      if (typeMember == null)
      {
        return null;
      }

      var containingType = typeMember.GetContainingType();

      return containingType == null ? null : containingType.GetClrName().FullName;
    }

    #endregion
  }
}