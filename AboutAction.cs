using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace Sitecore.Rocks.Resharper
{
  /// <summary>
  /// Class AboutAction.
  /// </summary>
  [ActionHandler("Sitecore.Rocks.Resharper.About")]
  public class AboutAction : IActionHandler
  {
    /// <summary>
    /// Updates action visual presentation. If presentation.Enabled is set to false, Execute
    ///             will not be called.
    /// </summary>
    /// <param name="context">DataContext</param>
    /// <param name="presentation">presentation to update</param>
    /// <param name="nextUpdate">delegate to call</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // return true or false to enable/disable this action
      return true;
    }

    /// <summary>
    /// Executes action. Called after Update, that set ActionPresentation.Enabled to true.
    /// </summary>
    /// <param name="context">DataContext</param>
    /// <param name="nextExecute">delegate to call</param>
    public void Execute(IDataContext context, DelegateExecute nextExecute)
    {
      MessageBox.Show(
        "Sitecore Rocks Resharper\nSitecore A/S\n\nIntegrates Sitecore Rocks deep into the Visual Studio Text Editor.",
        "About Sitecore Rocks Resharper",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
    }
  }
}
