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

            //building dimensions
            var blgLength = 36000;
            var blgWidth = 18000;

            //unit is set to in by default so convert it to mm
            var unit = UnitTypeId.Millimeters;

            //grid extension
            var ext = UnitUtils.ConvertToInternalUnits(1500, unit);

            //building length divided by 2
            var length = UnitUtils.ConvertToInternalUnits(blgLength/2, unit);
            var width = UnitUtils.ConvertToInternalUnits(blgWidth / 2, unit);

            var point1 = length;
            var point2 = width + ext;
            var point3 = length + ext;
            var point4 = width;

            var gfl = -500;
            var ffl = 0;
            var rfl = 3000;
            var fcl = 2400;

            var gflName = "GFL";
            var fflName = "FFL";
            var fclName = "FCL";
            var rflName = "RFL";

            String[] gridNames = {"1", "2", "A", "B" };


            var tr = new Transaction(document);

            using (tr)
            {
                try
                {
                    tr.Start("GridsAndLevels");

                    //CREATE LEVELS

                    //gf level
                    Helpers.createLevel(document, gfl, gflName);

                    //finish floor level
                    Helpers.createLevel(document, ffl, fflName);

                    //ceiling level
                    Helpers.createLevel(document, fcl, fclName);

                    //roof level
                    Helpers.createLevel(document, rfl, rflName);

                    //CREATE GRIDS

                    //GRID 1
                    Helpers.createStraightGrid(document, -point1, point2, -point1, -point2, gridNames[0]);

                    //GRID 2
                    Helpers.createStraightGrid(document, point1, point2, point1, -point2, gridNames[1]);

                    //GRID 3
                    Helpers.createStraightGrid(document, -point3, -point4, point3, -point4, gridNames[2]);

                    //GRID 4
                    Helpers.createStraightGrid(document, -point3, point4, point3, point4, gridNames[3]);


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

