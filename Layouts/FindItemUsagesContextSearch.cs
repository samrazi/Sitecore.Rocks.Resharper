namespace Sitecore.Rocks.Resharper.Layouts
{
  using System;
  using JetBrains.Application.DataContext;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches.BaseSearches;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search.SearchRequests;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Impl.Search.SearchDomain;

  /// <summary>
  /// Class XamlImplementationContextSearch.
  /// </summary>
  [ShellFeaturePart]
  public class FindItemUsagesContextSearch : FindUsagesContextSearch
  {
    #region Public Methods and Operators

    /// <summary>
    /// Determines whether the specified data context is available.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns><c>true</c> if the specified data context is available; otherwise, <c>false</c>.</returns>
    public override bool IsAvailable(IDataContext dataContext)
    {
      var projectFile = dataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.PROJECT_MODEL_ELEMENT) as IProjectFile;
      if (projectFile == null)
      {
        return false;
      }

      var location = projectFile.Location;
      if (location == null || location.IsEmpty)
      {
        return false;
      }
      
      return location.ToString().EndsWith(".layout.xml", StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Creates the search request.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="declaredElement">The declared element.</param>
    /// <param name="initialTarget">The initial target.</param>
    /// <returns>SearchUsagesRequest.</returns>
    protected override SearchUsagesRequest CreateSearchRequest(IDataContext dataContext, IDeclaredElement declaredElement, IDeclaredElement initialTarget)
    {
      var solution = dataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);

      return new FindItemUsagesRequest(solution, EmptySearchDomain.Instance);
    }

    #endregion
  }
}