namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Collections.Generic;
  using System.Xml;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
  using JetBrains.Util;
  using JetBrains.Util.DataStructures;

  /// <summary>
  /// Class ItemDeclaredElement.
  /// </summary>
  public class ItemDeclaredElement : IItemDeclaredElement
  {
    #region Fields

    /// <summary>
    /// My psi services
    /// </summary>
    private readonly IPsiServices psiServices;

    private readonly XmlTagHeaderNode owner;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    /// <param name="psiServices">The psi services.</param>
    /// <param name="owner">The source file.</param>
    /// <param name="itemName">Name of the item.</param>
    public ItemDeclaredElement(IPsiServices psiServices, XmlTagHeaderNode owner, string itemName)
    {
      this.psiServices = psiServices;
      this.owner = owner;
      this.ItemName = itemName;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets a value indicating whether [case sensistive name].
    /// </summary>
    /// <value><c>true</c> if [case sensistive name]; otherwise, <c>false</c>.</value>
    public bool CaseSensistiveName
    {
      get
      {
        return false;
      }
    }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    /// <value>The name of the item.</value>
    public string ItemName { get; private set; }

    /// <summary>
    /// Get the language on which this element is declared
    /// </summary>
    public PsiLanguageType PresentationLanguage
    {
      get
      {
        return UnknownLanguage.Instance;
      }
    }

    /// <summary>
    /// Gets the short name.
    /// </summary>
    /// <value>The short name.</value>
    public string ShortName
    {
      get
      {
        return this.ItemName;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Potentially VERY expensive method. In case of a namespace will parse all the files with classes declared in the namespace
    /// </summary>
    public IList<IDeclaration> GetDeclarations()
    {
      return EmptyList<IDeclaration>.InstanceList;
    }

    /// <summary>
    /// Gets the declarations in.
    /// </summary>
    /// <param name="sourceFile">The source file.</param>
    /// <returns>IList&lt;IDeclaration&gt;.</returns>
    public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return EmptyList<IDeclaration>.InstanceList;
    }

    /// <summary>
    /// Gets the type of the element.
    /// </summary>
    /// <returns>DeclaredElementType.</returns>
    public DeclaredElementType GetElementType()
    {
      return ItemDeclaredElementType.Instance;
    }

    /// <summary>
    /// Gets the psi services.
    /// </summary>
    /// <returns>IPsiServices.</returns>
    public IPsiServices GetPsiServices()
    {
      return this.psiServices;
    }

    /// <summary>
    /// Get the set of source files which contains the declaration of this element
    /// </summary>
    public HybridCollection<IPsiSourceFile> GetSourceFiles()
    {
      return new HybridCollection<IPsiSourceFile>(this.owner.GetSourceFile());
    }

    /// <summary>
    /// Gets the XML description summary.
    /// </summary>
    /// <param name="inherit">if set to <c>true</c> [inherit].</param>
    /// <returns>XmlNode.</returns>
    public XmlNode GetXMLDescriptionSummary(bool inherit)
    {
      return null;
    }

    /// <summary>
    /// Gets the XML document.
    /// </summary>
    /// <param name="inherit">if set to <c>true</c> [inherit].</param>
    /// <returns>XmlNode.</returns>
    public XmlNode GetXMLDoc(bool inherit)
    {
      return null;
    }

    /// <summary>
    /// Checks if there are declarations of this element in given <paramref name="sourceFile"/>
    /// </summary>
    public bool HasDeclarationsIn(IPsiSourceFile sourceFile)
    {
      return false;
    }

    /// <summary>
    /// Determines whether this instance is synthetic.
    /// </summary>
    /// <returns><c>true</c> if this instance is synthetic; otherwise, <c>false</c>.</returns>
    public bool IsSynthetic()
    {
      return false;
    }

    /// <summary>
    /// Determines whether this instance is valid.
    /// </summary>
    /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
    public bool IsValid()
    {
      return true;
    }

    #endregion
  }
}