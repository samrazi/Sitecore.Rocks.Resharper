namespace Sitecore.Rocks.Resharper.Layouts
{
  using JetBrains.ReSharper.Psi;

  /// <summary>
  /// Interface IItemDeclaredElement
  /// </summary>
  public interface IItemDeclaredElement : IDeclaredElement
  {
    #region Public Properties

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    /// <value>The name of the item.</value>
    string ItemName { get; }

    #endregion
  }
}