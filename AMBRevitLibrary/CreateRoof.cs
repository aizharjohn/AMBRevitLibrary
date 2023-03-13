using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.ObjectModel;
using Autodesk.Revit.Creation;
using System.Reflection.Emit;

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
            //.WhereElementIsNotElementType()
            //.OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(RoofType));

            var roofType = (RoofType)colRoofTypes.FirstElement();

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

                    var modelCurve = (ModelCurve)element;
                    if (modelCurve != null)
                    {
                        footPrint.Append(modelCurve.GeometryCurve);
                    }

                }

            } 
            else
            {
                throw new Exception("You should select a curve loop, or a wall loop, or loops combination \nof walls and curves to create a footprint roof.");
            }

            try
            {
                var tr = new Transaction(doc);

                using (tr)
                {
                    var footPrintToModelCurveMapping = new ModelCurveArray();

                    var footPrintRoof = doc.Create.NewFootPrintRoof(footPrint, rfl, roofType, out footPrintToModelCurveMapping);

                    var iterator = footPrintToModelCurveMapping.ForwardIterator();

                    iterator.Reset();

                    while (iterator.MoveNext())
                    {
                        var modelCurve = (ModelCurve)iterator.Current;
                        footPrintRoof.set_DefinesSlope(modelCurve, true);
                        footPrintRoof.set_SlopeAngle(modelCurve, 0.5);
                    }

                    tr.Commit();
                }
            }

            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

                return Result.Succeeded;
        }
    }
}
