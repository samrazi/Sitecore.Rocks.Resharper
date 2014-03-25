namespace Sitecore.Rocks.Resharper.Layouts
{
  using System.Collections.Generic;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search;
  using JetBrains.ReSharper.Feature.Services.Occurences.OccurenceKindProviders;
  using JetBrains.Util;

  /// <summary>
  /// Class ItemOccurenceKindProvider.
  /// </summary>
  [SolutionComponent]
  public class ItemOccurenceKindProvider : IOccurenceKindProvider
  {
    #region Static Fields

    /// <summary>
    /// The occurence kinds
    /// </summary>
    private static readonly OccurenceKind[] kinds =
    {
      OccurenceKind.Other
    };

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Gets all possible occurence kinds.
    /// </summary>
    /// <returns>IEnumerable&lt;OccurenceKind&gt;.</returns>
    public IEnumerable<OccurenceKind> GetAllPossibleOccurenceKinds()
    {
      return kinds;
    }

    /// <summary>
    /// Gets the occurence kinds.
    /// </summary>
    /// <param name="occurence">The occurence.</param>
    /// <returns>ICollection&lt;OccurenceKind&gt;.</returns>
    public ICollection<OccurenceKind> GetOccurenceKinds(IOccurence occurence)
    {
      var referenceOccurence = occurence as ItemOccurence;
      if (referenceOccurence == null)
      {
        return EmptyList<OccurenceKind>.InstanceList;
      }

      return kinds;
    }

    #endregion
  }
}