namespace Sitecore.Rocks.Resharper.Layouts
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Windows;
  using JetBrains.Application.DataContext;
  using JetBrains.DocumentModel;
  using JetBrains.ReSharper.Feature.Services.ContextNavigation;
  using JetBrains.ReSharper.Psi;
  using JetBrains.ReSharper.Psi.Files;
  using JetBrains.ReSharper.Psi.Tree;
  using JetBrains.ReSharper.Psi.Xml;
  using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
  using JetBrains.ReSharper.Psi.Xml.Tree;
  using Sitecore.VisualStudio.Data;
  using Sitecore.VisualStudio.Projects;
  using Sitecore.VisualStudio.Sites;

  /// <summary>
  /// Class NavigateToRenderingProvider.
  /// </summary>
  [ContextNavigationProvider]
  public class NavigateToRenderingProvider : INavigateFromHereProvider
  {
    #region Public Methods and Operators

    /// <summary>
    /// Creates the workflow.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <returns>IEnumerable&lt;ContextNavigation&gt;.</returns>
    public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
    {
      if (AppHost.CurrentContentTree == null)
      {
        yield break;
      }

      var document = dataContext.GetData(JetBrains.DocumentModel.DataConstants.DOCUMENT);
      var solution = dataContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
      if (document == null || solution == null)
      {
        yield break;
      }

      var documentOffset = dataContext.GetData(JetBrains.DocumentModel.DataConstants.DOCUMENT_OFFSET);
      if (documentOffset == null)
      {
        yield break;
      }

      var documentRange = new DocumentRange(document, documentOffset.Value);

      var psiFile = solution.GetPsiServices().GetPsiFile<XmlLanguage>(documentRange);
      if (psiFile == null)
      {
        yield break;
      }

      var treeNode = psiFile.FindNodeAt(documentRange);
      if (treeNode is XmlIdentifier)
      {
        treeNode = treeNode.Parent;
      }

      var node = treeNode as XmlTagHeaderNode;
      if (node == null)
      {
        yield break;
      }

      var root = node.Root();
      if (root == null)
      {
        yield break;
      }

      var layout = root.Children().OfType<XmlTag>().First(t => t.Header != null && t.Header.Name != null && t.Header.Name.GetText() == "Layout");
      if (layout == null)
      {
        yield break;
      }

      var attribute = layout.GetAttribute("xmlns");
      if (attribute == null)
      {
        yield break;
      }

      var xmlns = attribute.UnquotedValue;
      if (string.IsNullOrEmpty(xmlns) || !xmlns.StartsWith("http://www.sitecore.net/Sitecore-Speak-Intellisense/"))
      {
        yield break;
      }

      var project = psiFile.GetProject();
      if (project == null)
      {
        yield break;
      }

      var sitecoreProject = ProjectManager.GetProject(project.ProjectFileLocation.ToString());
      if (sitecoreProject == null)
      {
        yield break;
      }

      Action execution = delegate { this.LocateInSitecoreExplorer(sitecoreProject.Site, node.Name.GetText()); };

      yield return new ContextNavigation("Locate Rendering in Sitecore Explorer", null, NavigationActionGroup.Other, execution);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Locates the in sitecore explorer.
    /// </summary>
    /// <param name="site">The site.</param>
    /// <param name="renderingName">The name.</param>
    private void LocateInSitecoreExplorer(Site site, string renderingName)
    {
      Site.RequestCompleted completed = delegate(string response)
      {
        var parts = response.Split('|');
        if (parts.Length != 2)
        {
          AppHost.MessageBox(response, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        var itemUri = new ItemUri(new DatabaseUri(site, new DatabaseName(parts[0])), new ItemId(new Guid(parts[1])));

        AppHost.CurrentContentTree.Activate();
        AppHost.CurrentContentTree.Locate(itemUri);
      };

      site.Execute("XmlLayouts.GetRenderingItemUri", completed, renderingName);
    }

    #endregion
  }
}