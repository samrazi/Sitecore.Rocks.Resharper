namespace Sitecore.Rocks.Resharper.Layouts
{
  using JetBrains.ActionManagement;
  using JetBrains.ReSharper.Features.Finding.NavigateFromHere;

  /// <summary>
  /// Class SimpleNavigation.
  /// </summary>
  [ActionHandler]
  public class NavigateToRenderingActionHandler : ContextNavigationActionBase<NavigateToRenderingProvider>
  {
  }
}