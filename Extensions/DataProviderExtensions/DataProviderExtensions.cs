// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DataProviderExtensions class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sitecore.Rocks.Extensions.DataProviderExtensions
{
  using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
  using JetBrains.ReSharper.Psi;
  using Sitecore.VisualStudio.Annotations;
  using Sitecore.VisualStudio.Diagnostics;
  using Sitecore.VisualStudio.Projects;

  /// <summary>
  /// Defines the DataProviderExtensions class.
  /// </summary>
  public static class DataProviderExtensions
  {
    #region Public Methods and Operators

    /// <summary>Gets the project.</summary>
    /// <param name="provider">The provider.</param>
    /// <returns>Returns the project.</returns>
    [CanBeNull]
    public static Project GetProject([NotNull] this ICSharpContextActionDataProvider provider)
    {
      Assert.ArgumentNotNull(provider, "provider");

      var sourceFile = provider.Document.GetPsiSourceFile(provider.Solution);

      var projectFile = sourceFile.ToProjectFile();
      if (projectFile == null)
      {
        return null;
      }

      var project = projectFile.GetProject();
      if (project == null)
      {
        return null;
      }

      var projectPath = project.ProjectFileLocation.ToString();
      if (string.IsNullOrEmpty(projectPath))
      {
        return null;
      }

      return ProjectManager.GetProject(projectPath);
    }

    #endregion
  }
}