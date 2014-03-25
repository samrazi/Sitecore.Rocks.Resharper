namespace Sitecore.Rocks.Resharper.Layouts
{
  using JetBrains.ReSharper.Feature.Services.ContextNavigation;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
  using JetBrains.ReSharper.Psi.Resolve;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.ReSharper.Psi.Xml.Impl.Tree;

  /// <summary>
  /// Class ItemReference.
  /// </summary>
  public class ItemReference : TreeReferenceBase<XmlTagHeaderNode>
  {
    #region Fields

    /// <summary>
    /// The owner
    /// </summary>
    private readonly XmlTagHeaderNode owner;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemReference"/> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    public ItemReference(XmlTagHeaderNode owner) : base(owner)
    {
      this.owner = owner;
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Bind this reference to given Declared Element. May insert using directive.
    /// </summary>
    /// <returns> new "this" </returns>
    public override IReference BindTo(IDeclaredElement element)
    {
      return null;
    }

    /// <summary>
    /// Bind this reference to given Declared Element and substitution. May insert using directive.
    /// </summary>
    /// <returns>
    /// new "this"
    /// </returns>
    public override IReference BindTo(IDeclaredElement element, ISubstitution substitution)
    {
      return null;
    }

    /// <summary>
    /// Returns access context containing reference. It is useful to define, if context
    ///             is static or to determine access rights of context.
    /// </summary>
    public override IAccessContext GetAccessContext()
    {
      return new ElementAccessContext(this.owner);
    }

    /// <summary>
    /// Returns reference name.
    ///             This name usually coincides with short name of corresponding DeclaredElement.
    ///             (Only known exception is constructor initializer, its name is "this" or "base".)
    /// </summary>
    public override string GetName()
    {
      return "/sitecore/content/Home";
    }

    /// <summary>
    /// Returns full symbol table for the reference
    /// </summary>
    public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName)
    {
      var sourceFile = this.owner.GetSourceFile();

      var symbolTable = new SymbolTable(this.owner.GetPsiServices());

      symbolTable.AddSymbol(new ItemDeclaredElement(this.owner.GetPsiServices(), this.owner, this.GetName()));
      symbolTable.AddSymbol(new ItemDeclaredElement(this.owner.GetPsiServices(), this.owner, this.GetName()));

      return symbolTable;
    }

    /// <summary>
    /// Returns text range of reference in the source file.
    /// </summary>
    public override TreeTextRange GetTreeTextRange()
    {
      return this.owner.GetTreeTextRange();
    }

    /// <summary>
    /// Resolves the without cache.
    /// </summary>
    /// <returns>ResolveResultWithInfo.</returns>
    public override ResolveResultWithInfo ResolveWithoutCache()
    {
      var referenceSymbolTable = GetReferenceSymbolTable(true);
      var resolveResult = referenceSymbolTable.GetResolveResult(GetName());
      return resolveResult;
    }

    #endregion
  }
}