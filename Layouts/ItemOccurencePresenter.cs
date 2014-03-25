namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Drawing;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search;
  using JetBrains.ReSharper.Feature.Services.Occurences;
  using JetBrains.ReSharper.Feature.Services.Occurences.Presentation;
  using JetBrains.UI.PopupMenu;
  using JetBrains.UI.RichText;

  /// <summary>
  /// Class ItemOccurencePresenter.
  /// </summary>
  [OccurencePresenter(Priority = 0.0)]
  public class ItemOccurencePresenter : IOccurencePresenter
  {
    #region Public Methods and Operators

    /// <summary>
    /// Determines whether the specified occurence is applicable.
    /// </summary>
    /// <param name="occurence">The occurence.</param>
    /// <returns><c>true</c> if the specified occurence is applicable; otherwise, <c>false</c>.</returns>
    public bool IsApplicable(IOccurence occurence)
    {
      return occurence is ItemOccurence;
    }

    /// <summary>
    /// Presents the specified descriptor.
    /// </summary>
    /// <param name="descriptor">The descriptor.</param>
    /// <param name="occurence">The occurence.</param>
    /// <param name="occurencePresentationOptions">The occurence presentation options.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public bool Present(IMenuItemDescriptor descriptor, IOccurence occurence, OccurencePresentationOptions occurencePresentationOptions)
    {
      var itemOccurence = occurence as ItemOccurence;
      if (itemOccurence == null)
      {
        return false;
      }

      var greyTextStyle = TextStyle.FromForeColor(SystemColors.GrayText);

      var richText = new RichText(itemOccurence.ItemName, TextStyle.FromForeColor(Color.Tomato));

      if (!string.IsNullOrEmpty(itemOccurence.ParentPath))
      {
        richText.Append(string.Format(" (in {0})", itemOccurence.ParentPath), greyTextStyle);
      }

      descriptor.Text = richText;
      descriptor.Style = MenuItemStyle.Enabled;
      descriptor.ShortcutText = new RichText(itemOccurence.ItemUri.Site.Name + "/" + itemOccurence.ItemUri.DatabaseName, greyTextStyle);

      return true;
    }

    #endregion
  }
}