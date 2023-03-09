using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class CreateFloor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //get application
            var uiapp = commandData.Application;

            //get UI document
            var uidoc = uiapp.ActiveUIDocument;
            
            //var app = uiapp.Application;

            //get document
            var doc = uidoc.Document;

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

            //grab all floors
            var colFloors = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Floor));

            // get floor
            var floorType = new FilteredElementCollector(doc)
                .OfClass(typeof(FloorType))
                .Cast<FloorType>().FirstOrDefault(q => q.Name == "Generic 150mm");

            //get element and cast the ID
            var floorType1 = (FloorType)doc.GetElement(floorType.Id);


            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var width = UnitUtils.ConvertToInternalUnits(1800, unit);
            var length = UnitUtils.ConvertToInternalUnits(6000, unit);
            

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
                    tr.Start("CreateFloor");

                    Floor.Create(doc, new List<CurveLoop> {profile}, floorType1.Id, lvlId);

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
