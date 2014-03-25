namespace Sitecore.Rocks.Resharper.Layouts
{
  using JetBrains.ReSharper.Psi;
  using JetBrains.UI.Icons;

  /// <summary>
  /// Class ItemDeclaredElementType.
  /// </summary>
  public class ItemDeclaredElementType : DeclaredElementType
  {
    #region Static Fields

    /// <summary>
    /// The instance
    /// </summary>
    public static readonly DeclaredElementType Instance = new ItemDeclaredElementType();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Prevents a default instance of the <see cref="ItemDeclaredElementType"/> class from being created.
    /// </summary>
    private ItemDeclaredElementType() : base("Item")
    {
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Presentable name of the declared element
    /// </summary>
    /// <value>The name of the presentable.</value>
    public override string PresentableName
    {
      get
      {
        return "Item";
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Default declared element presenter
    /// </summary>
    protected override IDeclaredElementPresenter DefaultPresenter
    {
      get
      {
        return ItemDeclaredElementPresenter.Instance;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Returns presentation of this element is sound for given language type
    /// </summary>
    /// <param name="language">The language.</param>
    /// <returns><c>true</c> if the specified language is presentable; otherwise, <c>false</c>.</returns>
    public override bool IsPresentable(PsiLanguageType language)
    {
      return true;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Image of the declared element
    /// </summary>
    protected override IconId GetImage()
    {
      return null;
    }

    #endregion
  }
}