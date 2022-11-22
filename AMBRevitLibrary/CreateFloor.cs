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
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var app = uiapp.Application;
            var doc = uidoc.Document;

            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var width = UnitUtils.ConvertToInternalUnits(1220, unit);
            var length = UnitUtils.ConvertToInternalUnits(2440, unit);

            var pt1 = new XYZ(0, 0, 0);
            var pt2 = new XYZ(length, 0, 0);
            var pt3 = new XYZ(length, width, 0);
            var pt4 = new XYZ(0, width, 0);

            var line1 = Line.CreateBound(pt1, pt2);
            var line2 = Line.CreateBound(pt2, pt3);
            var line3 = Line.CreateBound(pt3, pt4);
            var line4 = Line.CreateBound(pt4, pt1);



            var floorCurves = new List<Curve>();
            //floorCurves.Add(pt1);
            //var loop = new List<CurveLoop>();
            //loop.Add(line1);
            //loop.Add(line2);
            //loop.Add(line3);
            //loop.Add(line4);

            var level = doc.ActiveView.GenLevel;

            var col = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Floors).OfClass(typeof(FloorType));

            var floorType =  (FloorType)col.FirstElement();

            var tr = new Transaction(doc, "Create Floor");

            using (tr)
            {
                tr.Start();
                //var floor = Floor.Create(doc, loop, floorType.Id, level.Id);
                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
