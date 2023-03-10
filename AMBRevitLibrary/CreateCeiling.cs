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
    public class CreateCeiling : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //get application
            var uiApp = commandData.Application;
            var app = uiApp.Application;

            //get UI document
            var uiDoc = commandData.Application.ActiveUIDocument;

            //get document
            var doc = uiDoc.Document;

            //grab levels
            var collLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //get FFL level
            var ffl = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>().FirstOrDefault(q => q.Name == "FFL");

            var lvlId = ffl.Id;

            //grab all ceiling
            var colCeil = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Ceiling));

            //get ceiling
            var ceiling = new FilteredElementCollector(doc)
                .OfClass(typeof(CeilingType))
                .Cast<CeilingType>().FirstOrDefault(q => q.Name == "600 x 600mm Grid");

            //get element and cast the ID
            var ceilType = (CeilingType)doc.GetElement(ceiling.Id);

            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var width = UnitUtils.ConvertToInternalUnits(1600, unit);
            var length = UnitUtils.ConvertToInternalUnits(5800, unit);

            var pt1 = new XYZ(-length, width, 0);
            var pt2 = new XYZ(length, width, 0);
            var pt3 = new XYZ(length, -width, 0);
            var pt4 = new XYZ(-length, -width, 0);

            var profile = new CurveLoop();

            var line1 = Line.CreateBound(pt1, pt2);
            var line2 = Line.CreateBound(pt2, pt3);
            var line3 = Line.CreateBound(pt3, pt4);
            var line4 = Line.CreateBound(pt4, pt1);

            profile.Append(line1);
            profile.Append(line2);
            profile.Append(line3);
            profile.Append(line4);

            try
            {
                var tr = new Transaction(doc);

                using (tr)
                {
                    tr.Start("Create Ceiling");

                    Ceiling.Create(doc, new List<CurveLoop> { profile }, ceiling.Id, lvlId);

                    //commit transaction
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
