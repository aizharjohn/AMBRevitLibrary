using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class Hello : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Revit", "Hello");

            return Result.Succeeded;
        }
    }
}
