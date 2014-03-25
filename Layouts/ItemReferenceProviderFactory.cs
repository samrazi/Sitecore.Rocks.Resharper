namespace Sitecore.Rocks.Resharper.Layouts
{
  using System;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Resolve;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.ReSharper.Psi.Xml.Tree;

  /// <summary>
  /// Class ItemReferenceProviderFactory.
  /// </summary>
  // [ReferenceProviderFactory]
  public class ItemReferenceProviderFactory : IReferenceProviderFactory
  {
    #region Public Events

    /// <summary>
    /// Fired when factory settings is changed, and for all active files reference providers should be re-evaluated
    /// </summary>
    public event Action OnChanged;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Creates implementation of <see cref="T:JetBrains.ReSharper.Psi.Resolve.IReferenceFactory"/> for the <paramref name="sourceFile"/> and <paramref name="file"/>.
    /// </summary>
    /// <param name="sourceFile">the specified project file</param><param name="file">the specified PSI file</param>
    /// <returns>
    /// implementation of <see cref="T:JetBrains.ReSharper.Psi.Resolve.IReferenceFactory"/> or <c>null</c>
    /// </returns>
    /// <remarks>
    /// <paramref name="file"/>.IsValid() is <c>false</c> for this method call!
    /// </remarks>
    public IReferenceFactory CreateFactory(IPsiSourceFile sourceFile, IFile file)
    {
      if (!(file is IXmlFile))
      {
        return null;
      }

      if (!sourceFile.Name.EndsWith(".layout.xml", StringComparison.InvariantCultureIgnoreCase))
      {
        return null;
      }

      return new ItemReferenceFactory();
    }

    #endregion
  }
}