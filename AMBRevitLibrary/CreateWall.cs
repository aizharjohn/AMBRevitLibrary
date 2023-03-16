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
            var uiDoc = uiApp.ActiveUIDocument;

            //get document
            var doc = uiDoc.Document;

            //set unit to millimeters
            var unit = UnitTypeId.Millimeters;


            var point1 = 9000;
            var point2 = 3600;
            
            var height = 2400;

            var pt1 = point1 / 2;
            var pt2 = point2 / 2;

            //point1
            var ptX = UnitUtils.ConvertToInternalUnits(pt1, unit);

            //point 2
            var ptY = UnitUtils.ConvertToInternalUnits(pt2, unit);

            //z point
            var ptZ = UnitUtils.ConvertToInternalUnits(0, unit);

            

            // wall width / 2
            var wallWidth = UnitUtils.ConvertToInternalUnits(264, unit);

            var wallWd = wallWidth / 2;

            var wallType = "CW 102-50-100p";
            var level = "FFL";

            var tr = new Transaction(doc);

                using (tr)
                {
                    try
                    {
                        tr.Start("CreateWall");

                        //create points
                        var start1 = new XYZ(-ptX, ptY - wallWd, ptZ);
                        var end1 = new XYZ(ptX, ptY - wallWd, ptZ);

                        var start2 = new XYZ(ptX, -ptY + wallWd, ptZ);
                        var end2 = new XYZ(-ptX, -ptY + wallWd, ptZ);

                        var start3 = new XYZ(ptX - wallWd, ptY, ptZ);
                        var end3 = new XYZ(ptX - wallWd, -ptY, ptZ);

                        var start4 = new XYZ(-ptX + wallWd, -ptY, ptZ);
                        var end4 = new XYZ(-ptX + wallWd, ptY, ptZ);

                        //create line
                        var line1 = Line.CreateBound(start1, end1);
                        var line2 = Line.CreateBound(start2, end2);
                        var line3 = Line.CreateBound(start3, end3);
                        var line4 = Line.CreateBound(start4, end4);

                        //create rectangle wall
                        Helpers.createWall(doc, line1, height, wallType, level);
                        Helpers.createWall(doc, line2, height, wallType, level);
                        Helpers.createWall(doc, line3, height, wallType, level);
                        Helpers.createWall(doc, line4, height, wallType, level);
                    }
                    catch (Exception e)
                    {
                        message = e.Message;
                        tr.RollBack();
                        return Result.Failed;
                    }
                   
                    //commit transaction                                   
                    tr.Commit();
                }
            
            return Result.Succeeded;
        }
    }
}
