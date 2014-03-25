namespace Sitecore.Rocks.Resharper.Layouts
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using JetBrains.Application;
  using JetBrains.Application.Threading.Tasks;
  using JetBrains.DataFlow;
  using JetBrains.ReSharper.Feature.Services.Goto;
  using JetBrains.ReSharper.Feature.Services.Navigation.Search;
  using JetBrains.ReSharper.Psi;
  using JetBrains.Text;
  using JetBrains.Util;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Data.DataServices;
  using Sitecore.VisualStudio.Extensions.StringExtensions;

  /// <summary>
  /// Class ItemNavigationProvider.
  /// </summary>
  [ShellFeaturePart]
  public class ItemOccurenceProvider : IGotoEverythingProvider
  {
    #region Static Fields

    /// <summary>
    /// The sitecore item occurrences
    /// </summary>
    [NotNull]
    private static readonly Key<List<ItemOccurence>> SitecoreItemOccurrences = new Key<List<ItemOccurence>>("SitecoreItemOccurrences");

    #endregion

    #region Fields

    /// <summary>
    /// My lifetime
    /// </summary>
    [NotNull]
    private readonly Lifetime myLifetime;

    /// <summary>
    /// My shell locks
    /// </summary>
    [NotNull]
    private readonly IShellLocks myShellLocks;

    /// <summary>
    /// My task host
    /// </summary>
    [NotNull]
    private readonly ITaskHost myTaskHost;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemOccurenceProvider"/> class.
    /// </summary>
    /// <param name="lifetime">The lifetime.</param>
    /// <param name="taskHost">The task host.</param>
    /// <param name="shellLocks">The shell locks.</param>
    public ItemOccurenceProvider([NotNull] Lifetime lifetime, [NotNull] ITaskHost taskHost, [NotNull] IShellLocks shellLocks)
    {
      this.myLifetime = lifetime;
      this.myTaskHost = taskHost;
      this.myShellLocks = shellLocks;
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// The lower is result int -&gt; the higher is the priority
    /// </summary>
    [NotNull]
    public Func<int, int> ItemsPriorityFunc
    {
      get
      {
        return i => i;
      }
    }

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Finds matched items and returns a corresponing list of matchingInfos.
    ///             Controller (or some other entity that will use this provider) recieves these items, scores them, sorts 
    ///             and than invoke GetOccurences for the top scored of them.
    /// </summary>
    /// <param name="matcher">matcher to use</param><param name="scope">defines a scope to search in</param><param name="gotoContext"/><param name="checkForInterrupt"/>
    /// <returns/>
    public IEnumerable<MatchingInfo> FindMatchingInfos(IdentifierMatcher matcher, INavigationScope scope, GotoContext gotoContext, Func<bool> checkForInterrupt)
    {
      var occurrences = new List<ItemOccurence>();

      var databases = new List<DatabaseUri>();
      foreach (var project in VisualStudio.Projects.ProjectManager.Projects)
      {
        if (project.Site != null)
        {
          databases.Add(new DatabaseUri(project.Site, DatabaseName.Master));
        }
      }

      using (var fibers = this.myTaskHost.CreateBarrier(this.myLifetime, checkForInterrupt, false, false))
      {
        foreach (var databaseUri in databases)
        {
          var busy = true;
          var db = databaseUri;

          ExecuteCompleted completed = delegate(string response, ExecuteResult executeResult)
          {
            if (!DataService.HandleExecute(response, executeResult, true))
            {
              busy = false;
              return;
            }

            var r = response.ToXElement();
            if (r == null)
            {
              busy = false;
              return;
            }

            foreach (var element in r.Elements())
            {
              occurrences.Add(new ItemOccurence(ItemHeader.Parse(db, element)));
            }

            busy = false;
          };

          databaseUri.Site.DataService.ExecuteAsync("Search.Search", completed, matcher.Filter, string.Empty, string.Empty, string.Empty, string.Empty, 0);
          AppHost.DoEvents(ref busy);
        }
      }

      gotoContext.PutData(SitecoreItemOccurrences, occurrences);

      var matchingInfo = new MatchingInfo(matcher.Filter, EmptyList<IdentifierMatch>.InstanceList);
      return new[] { matchingInfo };
    }

    /// <summar>Gets occurences by given matchingInfo </summar><param name="navigationInfo"/><param name="scope"/><param name="gotoContext"/><param name="checkForInterrupt"/>
    /// <returns/>
    public IEnumerable<IOccurence> GetOccurencesByMatchingInfo(MatchingInfo navigationInfo, INavigationScope scope, GotoContext gotoContext, Func<bool> checkForInterrupt)
    {
      var occurrences = gotoContext.GetData(SitecoreItemOccurrences);

      return occurrences ?? EmptyList<ItemOccurence>.InstanceList;
    }

    /// <summary>
    /// Determines whether the specified scope is applicable.
    /// </summary>
    /// <param name="scope">The scope.</param>
    /// <param name="gotoContext">The goto context.</param>
    /// <param name="matcher">The matcher.</param>
    /// <returns><c>true</c> if the specified scope is applicable; otherwise, <c>false</c>.</returns>
    public bool IsApplicable(INavigationScope scope, GotoContext gotoContext, IdentifierMatcher matcher)
    {
      if (string.IsNullOrEmpty(matcher.Filter))
      {
        return false;
      }

      if (matcher.Filter.Length < 3)
      {
        return false;
      }

      if (scope is ProjectModelNavigationScope)
      {
        return true;
      }

      if (scope is SolutionNavigationScope)
      {
        return true;
      }

      return VisualStudio.Projects.ProjectManager.Projects.Any();
    }

    #endregion
  }
}