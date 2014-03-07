using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace Sitecore.Rocks.Resharper
{
  [ActionHandler("Sitecore.Rocks.Resharper.About")]
  public class AboutAction : IActionHandler
  {
    public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
    {
      // return true or false to enable/disable this action
      return true;
    }

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
