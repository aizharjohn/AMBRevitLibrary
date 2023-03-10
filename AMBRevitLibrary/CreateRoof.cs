using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class CreateRoof : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //get application
            var uiApp = commandData.Application;
            var app = uiApp.Application;

            //get UI document
            var uiDoc = uiApp.ActiveUIDocument;

            //get document
            var doc = uiDoc.Document;

            //grab levels
            var collLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //get RFL level
            var rfl = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>().FirstOrDefault(q => q.Name == "RFL");

            var lvlId = rfl.Id; 

            //grab all rooftypes
            var colRoofTypes = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(RoofType));

            RoofType roofType = null;

            foreach (RoofType rt in colRoofTypes)
            {
                roofType= rt;
                break;
            }

            //define the footprint
            var footPrint = app.Create.NewCurveArray();

            ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

            if (selectedIds.Count != 0)
            {
                foreach (var id in selectedIds)
                {
                    var element = doc.GetElement(id);
                    var wall = (Wall) element;

                    if (wall != null)
                    {
                        var wallCurve = (LocationCurve)wall.Location;
                        footPrint.Append(wallCurve.Curve);
                        continue;
                    }

                }

            }

            return Result.Succeeded;
        }
    }
}
