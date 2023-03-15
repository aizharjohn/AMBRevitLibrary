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
            var document = uidoc.Document;

            //unit is set to in by default so convert it to mm
            var unit = UnitTypeId.Millimeters;

            var pt1 = UnitUtils.ConvertToInternalUnits(6000, unit);
            var pt2 = UnitUtils.ConvertToInternalUnits(3300, unit);
            var pt3 = UnitUtils.ConvertToInternalUnits(7500, unit);
            var pt4 = UnitUtils.ConvertToInternalUnits(1800, unit);
            
            var rfl = UnitUtils.ConvertToInternalUnits(3000, unit);
            var gfl = UnitUtils.ConvertToInternalUnits(-500, unit);
            var ffl = UnitUtils.ConvertToInternalUnits(0, unit);

            var roofLevel = rfl;
            var groundLevel = gfl;
            var finishLevel = ffl;

            var gflName = "GFL";
            var fflName = "FFL";
            var rflName = "RFL";

            String[] gridNames = {"1", "2", "A", "B" };


            var tr = new Transaction(document);

            using (tr)
            {
                try
                {
                    tr.Start("GridsAndLevels");

                    //delete default levels
                    //DeleteElement.deleteLevel(document, "Level 1");
                    //DeleteElement.deleteLevel(document, "Level 2");

                    //CREATE LEVELS

                    //gf level
                    Helpers.createLevel(document, groundLevel, gflName);

                    //finish floor level
                    Helpers.createLevel(document, finishLevel, fflName);

                    //roof level
                    Helpers.createLevel(document, roofLevel, rflName);

                    //CREATE GRIDS

                    //GRID 1
                    Helpers.createStraightGrid(document, -pt1, pt2, -pt1, -pt2, gridNames[0]);

                    //GRID 2
                    Helpers.createStraightGrid(document, pt1, pt2, pt1, -pt2, gridNames[1]);

                    //GRID 3
                    Helpers.createStraightGrid(document, -pt3, -pt4, pt3, -pt4, gridNames[2]);

                    //GRID 4
                    Helpers.createStraightGrid(document, -pt3, pt4, pt3, pt4, gridNames[3]);


                    tr.Commit();
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                    tr.RollBack();
                    return Result.Failed;
                }

                return Result.Succeeded;
            }
        }
    }
}

