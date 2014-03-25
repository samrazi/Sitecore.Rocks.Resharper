namespace Sitecore.Rocks.Resharper.Layouts
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using JetBrains.Application.Progress;
  using JetBrains.ProjectModel;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search.SearchRequests;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Search;

  /// <summary>
  /// Class FindItemUsagesRequest.
  /// </summary>
  public class FindItemUsagesRequest : SearchUsagesRequest
  {
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FindItemUsagesRequest" /> class.
    /// </summary>
    /// <param name="solution">The solution.</param>
    /// <param name="searchDomain">The search domain.</param>
    public FindItemUsagesRequest(ISolution solution, ISearchDomain searchDomain) : base(solution, searchDomain)
    {
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the search targets.
    /// </summary>
    /// <value>The search targets.</value>
    public override ICollection SearchTargets
    {
      get
      {
        return new IDeclaredElementEnvoy[0];
      }
    }

    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>The title.</value>
    public override string Title
    {
      get
      {
        return "Sitecore Items";
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Searches the specified progress indicator.
    /// </summary>
    /// <param name="progressIndicator">The progress indicator.</param>
    /// <returns>ICollection&lt;IOccurence&gt;.</returns>
    public override ICollection<IOccurence> Search(IProgressIndicator progressIndicator)
    {
      foreach (var project in this.Solution.GetAllProjects())
      {
        project.Accept(new ItemProjectVisitor(this.Solution));
      }

      var occurence = new ItemOccurence();

      return new[]
      {
        occurence
      };
    }

    /// <summary>
    /// Class ItemProjectVisitor.
    /// </summary>
    public class ItemProjectVisitor : RecursiveProjectVisitor
    {
      private readonly ISolution solution;

      /// <summary>
      /// Initializes a new instance of the <see cref="ItemProjectVisitor"/> class.
      /// </summary>
      /// <param name="solution">The solution.</param>
      public ItemProjectVisitor(ISolution solution)
      {
        this.solution = solution;
      }

      /// <summary>
      /// Visits the project file.
      /// </summary>
      /// <param name="projectFile">The project file.</param>
      public override void VisitProjectFile(IProjectFile projectFile)
      {
        var location = projectFile.Location;
        if (location == null || location.IsEmpty)
        {
          return;
        }

        if (!location.ToString().EndsWith(".layout.xml", StringComparison.InvariantCultureIgnoreCase))
        {
          return;
        }

      }
    }

    #endregion
  }
}