namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.ReSharper.Feature.Services.Navigation;
  using JetBrains.ReSharper.Feature.Services.Tree;
  using JetBrains.ReSharper.Feature.Services.Tree.SectionsManagement;
  using JetBrains.ReSharper.Features.Finding.GoToDeclaredElement;
  using JetBrains.ReSharper.Psi;
  using JetBrains.TreeModels;
  using JetBrains.Util;

  /// <summary>
  /// Class ItemOccurenceSectionProvider.
  /// </summary>
  // [ShellFeaturePart]
  public class ItemOccurenceSectionProvider : OccurenceSectionProvider
  {
    #region Public Methods and Operators

    /// <summary>
    /// Update descriptor's sections (sorting, titling, adding new sections) and return them.
    /// </summary>
    /// <param name="descriptor"/>
    /// <returns/>
    public override ICollection<TreeSection> GetTreeSections(OccurenceBrowserDescriptor descriptor)
    {
      var searchDescriptor = descriptor as GotoDeclaredElementsBrowserDescriptor;
      if (searchDescriptor == null)
      {
        return EmptyList<TreeSection>.InstanceList;
      }

      var model = new TreeSimpleModel();

      foreach (var result in descriptor.Items.OfType<ItemOccurence>())
      {
        model.Insert(null, result);
      }

      var tree = new TreeSection(model, "Sitecore Items");

      var list = new List<TreeSection>()
      {
        tree
      };

      return list;
    }

    /// <summary>
    /// Determines whether the specified descriptor is applicable.
    /// </summary>
    /// <param name="descriptor">The descriptor.</param>
    /// <returns><c>true</c> if the specified descriptor is applicable; otherwise, <c>false</c>.</returns>
    public override bool IsApplicable(OccurenceBrowserDescriptor descriptor)
    {
      return descriptor is GotoDeclaredElementsBrowserDescriptor;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Transforms the occurrence sections.
    /// </summary>
    /// <param name="sections">The sections.</param>
    /// <returns>IEnumerable&lt;OccurenceSection&gt;.</returns>
    protected virtual IEnumerable<OccurenceSection> TransformOccurrenceSections(IEnumerable<OccurenceSection> sections)
    {
      return sections;
    }

    #endregion
  }
}