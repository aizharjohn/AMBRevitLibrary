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
    internal static class Helpers
    {
        public static Level createLevel(Document doc, Int32 elev, string name)
        {
            Autodesk.Revit.Creation.Document createDoc = doc.Create;


            //unit is set to in by default so convert it to mm
            var unit = UnitTypeId.Millimeters;

            var height = UnitUtils.ConvertToInternalUnits(elev, unit);

            var level = Level.Create(doc, height);
            level.Name = name;

            return level;
        }

        public static Grid createStraightGrid(Document doc, double point1, double point2, double point3, double point4, string name)
        {
            var lineStart = new XYZ(point1, point2, 0);
            var lineEnd = new XYZ(point3, point4, 0);
            var line = Line.CreateBound(lineStart, lineEnd);

            // exception for grid in case of failure
            var exception = new Exception("Create a new straight grid failed.");

            var gridLine = Grid.Create(doc, line);

            if (null == gridLine)
            {
                throw exception;
            }

            gridLine.Name = name;

            return gridLine;
        }

        public static Floor createArchitecturalFloor(Document doc, double length, double width, string floorType, string level) 
        {
            //grab levels
            var collLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //get level
            var lvl = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>().FirstOrDefault(q => q.Name == level);

            var lvlId = lvl.Id;

            //grab all floors
            var colFloors = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Floor));

            // get floor
            var floor = new FilteredElementCollector(doc)
                .OfClass(typeof(FloorType))
                .Cast<FloorType>().FirstOrDefault(q => q.Name == floorType);

            //get element and cast the ID
            var floorId = (FloorType)doc.GetElement(floor.Id);

            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var point1 = UnitUtils.ConvertToInternalUnits(length / 2, unit);
            var point2 = UnitUtils.ConvertToInternalUnits(width / 2, unit);

            //create lines from points
            var pt1 = new XYZ(-point1, point2, 0);
            var pt2 = new XYZ(point1, point2, 0);
            var pt3 = new XYZ(point1, -point2, 0);
            var pt4 = new XYZ(-point1, -point2, 0);

            //initiate curveloop
            var profile = new CurveLoop();

            //create curveloop
            var line1 = Line.CreateBound(pt1, pt2);
            var line2 = Line.CreateBound(pt2, pt3);
            var line3 = Line.CreateBound(pt3, pt4);
            var line4 = Line.CreateBound(pt4, pt1);

            profile.Append(line1);
            profile.Append(line2);
            profile.Append(line3);
            profile.Append(line4);

            //create floor
            var createFloor = Floor.Create(doc, new List<CurveLoop> { profile }, floorId.Id, lvlId);

            return createFloor;

        }

        public static Wall createWall(Document doc, Line line, double height, string wallType, string level)
        {
            //grab levels
            var collLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //get level
            var lvl = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
                .Cast<Level>().FirstOrDefault(q => q.Name == level);

            var lvlId = lvl.Id;

            //grab all walls
            var colWalls = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Wall));

            //grab all wall types
            var colWallTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType));

            //get wall
            var wType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>().FirstOrDefault(q => q.Name == wallType);

            //get element and cast the ID
            var wallType1 = (WallType)doc.GetElement(wType.Id);

            //set unit to millimeters
            var unit = UnitTypeId.Millimeters;

            //wall height
            var ht = UnitUtils.ConvertToInternalUnits(height, unit);

            //wall offset
            var offset = UnitUtils.ConvertToInternalUnits(0, unit);

            //create wall
            var wall = Wall.Create(doc, line, wallType1.Id, lvlId, ht, offset, false, false);
            
            wall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM).Set(2);

            return wall;
        }

        public static Ceiling createCeiling(Document doc, double length, double width, string ceilingType, string level)
        {
            //grab levels
            var collLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //get level
            var lvl = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>().FirstOrDefault(q => q.Name == level);

            var lvlId = lvl.Id;

            //grab all ceiling
            var colCeil = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Ceiling));

            //get ceiling
            var ceiling = new FilteredElementCollector(doc)
                .OfClass(typeof(CeilingType))
                .Cast<CeilingType>().FirstOrDefault(q => q.Name == ceilingType);

            //get element and cast the ID
            var ceilId = (CeilingType)doc.GetElement(ceiling.Id);

            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var point1 = UnitUtils.ConvertToInternalUnits(length / 2, unit);
            var point2 = UnitUtils.ConvertToInternalUnits(width / 2, unit);

            var pt1 = new XYZ(-point1, point2, 0);
            var pt2 = new XYZ(point1, point2, 0);
            var pt3 = new XYZ(point1, -point2, 0);
            var pt4 = new XYZ(-point1, -point2, 0);

            var profile = new CurveLoop();

            var line1 = Line.CreateBound(pt1, pt2);
            var line2 = Line.CreateBound(pt2, pt3);
            var line3 = Line.CreateBound(pt3, pt4);
            var line4 = Line.CreateBound(pt4, pt1);

            profile.Append(line1);
            profile.Append(line2);
            profile.Append(line3);
            profile.Append(line4);

            //create ceiling
            var createCeiling = Ceiling.Create(doc, new List<CurveLoop> { profile }, ceilId.Id, lvlId);

            return createCeiling;
        }
    }
}
