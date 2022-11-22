using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Diagnostics;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class GridsAndLevels : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var app = uiapp.Application;
            var document = uidoc.Document;

            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var pt1 = UnitUtils.ConvertToInternalUnits(-6000, unit);
            var pt2 = UnitUtils.ConvertToInternalUnits(3300, unit);
            var pt3 = UnitUtils.ConvertToInternalUnits(6000, unit);
            var pt4 = UnitUtils.ConvertToInternalUnits(-3300, unit);
            var pt5 = UnitUtils.ConvertToInternalUnits(-7500, unit);
            var pt6 = UnitUtils.ConvertToInternalUnits(-1800, unit);
            var pt7 = UnitUtils.ConvertToInternalUnits(7500, unit);
            var pt8 = UnitUtils.ConvertToInternalUnits(1800, unit);

            var roofLevel = UnitUtils.ConvertToInternalUnits(3000, unit);
            var groundLevel = UnitUtils.ConvertToInternalUnits(-500, unit);

            var tr = new Transaction(document);

            using (tr)
            {
                try
                {
                    tr.Start("GridsAndLevels");

                    //create levels

                    //ground floor level
                    var gfLevel = Level.Create(document, groundLevel);
                    gfLevel.Name = "GFL";

                    //finish floor level
                    var ffLevel = Level.Create(document, 0.00);
                    ffLevel.Name = "FFL";

                    //roof level
                    var rfLevel = Level.Create(document, roofLevel);
                    rfLevel.Name = "RFL";


                    //GRID 1
                    //create the geometry line which the grid locates
                    var line1start = new XYZ(pt1, pt2, 0);
                    var line1end = new XYZ(pt1, pt4, 0);
                    var grid1Line = Line.CreateBound(line1start, line1end);

                    //create grid using the geometry line
                    var gridLine1 = Grid.Create(document, grid1Line);
                    if (null == gridLine1)
                    {
                        throw new Exception("Create a new straight grid failed.");
                    }

                    //modify the name of the created grid
                    gridLine1.Name = "1";


                    //GRID 2
                    //create the geometry line which the grid locates
                    var line2start = new XYZ(pt3, pt2, 0);
                    var line2end = new XYZ(pt3, pt4, 0);
                    var grid2Line = Line.CreateBound(line2start, line2end);

                    //create grid using the geometry line
                    var gridLine2 = Grid.Create(document, grid2Line);
                    if (null == gridLine2)
                    {
                        throw new Exception("Create a new straight grid failed.");
                    }

                    //modify the name of the created grid
                    gridLine2.Name = "2";


                    //GRID 3
                    //create the geometry line which the grid locates
                    var line3start = new XYZ(pt5, pt6, 0);
                    var line3end = new XYZ(pt7, pt6, 0);
                    var grid3Line = Line.CreateBound(line3start, line3end);

                    //create grid using the geometry line
                    var gridLine3 = Grid.Create(document, grid3Line);
                    if (null == gridLine3)
                    {
                        throw new Exception("Create a new straight grid failed.");
                    }

                    //modify the name of the created grid
                    gridLine3.Name = "B";


                    //GRID 4
                    //create the geometry line which the grid locates
                    var line4start = new XYZ(pt5, pt8, 0);
                    var line4end = new XYZ(pt7, pt8, 0);
                    var grid4Line = Line.CreateBound(line4start, line4end);

                    //create grid using the geometry line
                    var gridLine4 = Grid.Create(document, grid4Line);
                    if (null == gridLine4)
                    {
                        throw new Exception("Create a new straight grid failed.");
                    }

                    //modify the name of the created grid
                    gridLine4.Name = "A";

                    //delete default levels
                    //DeleteElement.deleteLevel(document, "Level 1");
                    //DeleteElement.deleteLevel(document, "Level 2");

                    tr.Commit();
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                    tr.RollBack();
                }

                return Result.Succeeded;
            }
        }
    }
}

