namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Text;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Impl;
  using JetBrains.ReSharper.Psi.Resolve;
  using JetBrains.Util;

  /// <summary>
  /// Class ItemDeclaredElementPresenter.
  /// </summary>
  public class ItemDeclaredElementPresenter : IDeclaredElementPresenter
  {
    #region Static Fields

    /// <summary>
    /// The instance
    /// </summary>
    public static readonly IDeclaredElementPresenter Instance = new ItemDeclaredElementPresenter();

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes static members of the <see cref="ItemDeclaredElementPresenter"/> class.
    /// </summary>
    static ItemDeclaredElementPresenter()
    {
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Returns a string containing declared element text presentation made according to this presenter settings.
    ///              This method is usefull when additional processing is required for the returned string,
    ///              e.g. as is done in the following method:
    ///              
    /// <code>
    ///              RichText Foo(IMethod method)
    ///              {
    ///                DeclaredElementPresenterMarking marking;
    ///                RichTextParameters rtp = new RichTextParameters(ourFont);
    ///                // make rich text with declared element presentation
    ///                RichText result = new RichText(ourInvocableFormatter.Format(method, out marking),rtp);
    ///                // highlight name of declared element in rich text
    ///                result.SetColors(SystemColors.HighlightText,SystemColors.Info,marking.NameRange.StartOffset,marking.NameRange.EndOffset);
    ///                return result;
    ///              }
    ///              </code>
    /// </summary>
    /// <param name="style">The style.</param>
    /// <param name="element">Contains <see cref="T:JetBrains.ReSharper.Psi.IDeclaredElement" /> to provide string presentation of.</param>
    /// <param name="substitution">The substitution.</param>
    /// <param name="marking">Returns the markup of the string with a <see cref="T:JetBrains.ReSharper.Psi.IDeclaredElement" /> presentation.</param>
    /// <returns>System.String.</returns>
    public string Format(DeclaredElementPresenterStyle style, IDeclaredElement element, ISubstitution substitution, out DeclaredElementPresenterMarking marking)
    {
      marking = new DeclaredElementPresenterMarking();

      var itemDeclaredElement = (IItemDeclaredElement)element;
      var sb = new StringBuilder();
      
      if (style.ShowEntityKind != EntityKindForm.NONE)
      {
        marking.EntityKindRange = AppendString(sb, style.ShowEntityKind == EntityKindForm.NORMAL_IN_BRACKETS ? "[Item] " : "Item ");
      }
      
      if (style.ShowName != NameStyle.NONE)
      {
        if (style.ShowNameInQuotes)
        {
          sb.Append('"');
        }

        marking.NameRange = style.ShowName == NameStyle.SHORT || style.ShowName == NameStyle.SHORT_RAW ? AppendString(sb, itemDeclaredElement.ShortName) : AppendString(sb, itemDeclaredElement.ItemName);
        if (style.ShowNameInQuotes)
        {
          sb.Append('"');
        }
      }
      
      return sb.ToString();
    }

    /// <summary>
    /// Returns language specific presentation for a given parameter kind
    /// </summary>
    /// <param name="parameterKind">Kind of the parameter.</param>
    /// <returns>System.String.</returns>
    public string Format(ParameterKind parameterKind)
    {
      return string.Empty;
    }

    /// <summary>
    /// Returns language specific presentation for a given access rights value
    /// </summary>
    /// <param name="accessRights">The access rights.</param>
    /// <returns>System.String.</returns>
    public string Format(AccessRights accessRights)
    {
      return string.Empty;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Appends the string.
    /// </summary>
    /// <param name="sb">The sb.</param>
    /// <param name="substr">The substr.</param>
    /// <returns>TextRange.</returns>
    private static TextRange AppendString(StringBuilder sb, string substr)
    {
      var length = sb.Length;
      
      sb.Append(substr);
      
      if (substr.Length != 0)
      {
        return new TextRange(length, sb.Length);
      }

      return TextRange.InvalidRange;
    }

    #endregion
  }
}