namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Collections.Generic;
  using JetBrains.ReSharper.Psi.Resolve;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
  using JetBrains.Util;

  /// <summary>
  /// Class ItemReferenceFactory.
  /// </summary>
  public class ItemReferenceFactory : IReferenceFactory
  {
    #region Public Methods and Operators

    /// <summary>
    /// Incrementally update reference list.
    /// </summary>
    /// <param name="element"/><param name="oldReferences">Old references. Can be invalid.</param>
    public IReference[] GetReferences(ITreeNode element, IReference[] oldReferences)
    {
      var node = element as XmlTagHeaderNode;
      if (node == null)
      {
        return EmptyArray<IReference>.Instance;
      }

      var itemReference = new ItemReference(node);

      return new IReference[]
      {
        itemReference
      };
    }

    /// <summary>
    /// Returns <c>true</c> if this reference provider may have reference on element with one of given names
    /// </summary>
    public bool HasReference(ITreeNode element, ICollection<string> names)
    {
      return element is XmlTagHeaderNode;
    }

    #endregion
  }
}