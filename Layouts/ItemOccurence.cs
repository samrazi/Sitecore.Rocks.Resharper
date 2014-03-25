namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Collections.Generic;
  using JetBrains.IDE;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search;
  using JetBrains.ReSharper.Feature.Services.Occurences;
  using JetBrains.ReSharper.Psi;
  using JetBrains.UI.PopupWindowManager;
  using JetBrains.Util;
  using Sitecore.VisualStudio.Data;

  /// <summary>
  /// Class ItemOccurence.
  /// </summary>
  public class ItemOccurence : IOccurence
  {
    #region Fields

    /// <summary>
    /// The merged items
    /// </summary>
    private readonly IList<IOccurence> mergedItems = new List<IOccurence>();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Object"/> class.
    /// </summary>
    /// <param name="itemHeader"></param>
    public ItemOccurence(ItemHeader itemHeader)
    {
      this.PresentationOptions = OccurencePresentationOptions.DefaultOptions;
      this.ItemName = itemHeader.Name;
      this.ParentPath = itemHeader.ParentPath;
      this.ItemUri = itemHeader.ItemUri;
      this.Icon = itemHeader.Icon;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Object"/> class.
    /// </summary>
    public ItemOccurence()
    {
      this.PresentationOptions = OccurencePresentationOptions.DefaultOptions;
      this.ItemName = "Test";
      this.ParentPath = string.Empty;
      this.ItemUri = ItemUri.Empty;
      this.Icon = Icon.Empty;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the icon.
    /// </summary>
    /// <value>The icon.</value>
    public Icon Icon { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is valid.
    /// </summary>
    /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
    public bool IsValid
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    /// <value>The name of the item.</value>
    public string ItemName { get; private set; }

    /// <summary>
    /// Gets the item URI.
    /// </summary>
    /// <value>The item URI.</value>
    public ItemUri ItemUri { get; private set; }

    /// <summary>
    /// Gets the merge key.
    /// </summary>
    /// <value>The merge key.</value>
    public object MergeKey
    {
      get
      {
        return this.ItemUri;
      }
    }

    /// <summary>
    /// Gets the merged items.
    /// </summary>
    /// <value>The merged items.</value>
    public IList<IOccurence> MergedItems
    {
      get
      {
        return this.mergedItems;
      }
    }

    /// <summary>
    /// Gets the namespace.
    /// </summary>
    /// <value>The namespace.</value>
    public DeclaredElementEnvoy<INamespace> Namespace
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the type of the occurence.
    /// </summary>
    /// <value>The type of the occurence.</value>
    public OccurenceType OccurenceType
    {
      get
      {
        return OccurenceType.TextualOccurence;
      }
    }

    /// <summary>
    /// Gets the item path.
    /// </summary>
    /// <value>The item path.</value>
    public string ParentPath { get; private set; }

    /// <summary>
    /// Gets or sets the presentation options.
    /// </summary>
    /// <value>The presentation options.</value>
    public OccurencePresentationOptions PresentationOptions { get; set; }

    /// <summary>
    /// Gets the project model element envoy.
    /// </summary>
    /// <value>The project model element envoy.</value>
    public ProjectModelElementEnvoy ProjectModelElementEnvoy
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the text range.
    /// </summary>
    /// <value>The text range.</value>
    public TextRange TextRange
    {
      get
      {
        return TextRange.InvalidRange;
      }
    }

    /// <summary>
    /// Gets the type element.
    /// </summary>
    /// <value>The type element.</value>
    public DeclaredElementEnvoy<ITypeElement> TypeElement
    {
      get
      {
        return null;
      }
    }

    /// <summary>
    /// Gets the type member.
    /// </summary>
    /// <value>The type member.</value>
    public DeclaredElementEnvoy<ITypeMember> TypeMember
    {
      get
      {
        return null;
      }
    }

    #endregion

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
      return this.ItemName;
    }

    #region Public Methods and Operators

    /// <summary>
    /// Dumps to string.
    /// </summary>
    /// <returns>System.String.</returns>
    public string DumpToString()
    {
      return this.ItemName + " : " + this.ItemUri;
    }

    /// <summary>
    /// Navigates the specified solution.
    /// </summary>
    /// <param name="solution">The solution.</param>
    /// <param name="windowContext">The window context.</param>
    /// <param name="transferFocus">if set to <c>true</c> [transfer focus].</param>
    /// <param name="tabOptions">The tab options.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public bool Navigate(ISolution solution, PopupWindowContextSource windowContext, bool transferFocus, TabOptions tabOptions = TabOptions.Default)
    {
      AppHost.CurrentContentTree.Activate();
      AppHost.CurrentContentTree.Locate(this.ItemUri);

      return true;
    }

    #endregion
  }
}