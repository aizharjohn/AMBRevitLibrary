using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Reflection.Emit;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class CreateWall : IExternalCommand
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

            //grab all walls
            var colWalls = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Wall));

            //grab first wall in collection
            var wall = colWalls.FirstElement() as Wall;

            //var wallId = wall.Id;

            //grab all wall types
            var colWallTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType));

            //grab first wall type in collection
            //var firstWallType = colWalls.FirstElement() as WallType;

            //get wall
            var wType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>().FirstOrDefault(q => q.Name == "CW 102-50-100p");
          
            //get element and cast the ID
            var wallType1 = (WallType)doc.GetElement(wType.Id);
            
            //set unit to millimeters
            var unit = UnitTypeId.Millimeters;

            //wall length
            var length = UnitUtils.ConvertToInternalUnits(6000, unit);

            //wall height
            var height = UnitUtils.ConvertToInternalUnits(2400, unit);

            //wall offset
            var offset = UnitUtils.ConvertToInternalUnits(0, unit);

            //point1
            var stPt = UnitUtils.ConvertToInternalUnits(-6000, unit);

            //point 2
            var ptY = UnitUtils.ConvertToInternalUnits(1800, unit);

            //z point
            var ptZ = UnitUtils.ConvertToInternalUnits(0, unit);


            try
            {
                var tr = new Transaction(doc);
                using (tr)
                {
                    tr.Start("CreateWall");

                    //create points
                    var start = new XYZ(stPt,ptY, ptZ);
                    var end = new XYZ(length, ptY, ptZ);

                    //create line
                    var geomLine = Line.CreateBound(start, end); 

                    //create wall
                    var myWall = Wall.Create(doc, geomLine, wallType1.Id, lvlId, height, offset, false, false);

                    //set location line to Finish Face Exterior
                    myWall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM).Set(2);
                                       
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
