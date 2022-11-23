﻿using System;
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

            //grab level
            var collLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //grab first level in collection
            var firstLevel = (Level)collLevels.FirstElement();

            //set unit to millimeters
            var unit = UnitTypeId.Millimeters;

            //wall length
            var length = UnitUtils.ConvertToInternalUnits(1200, unit);

            //wall height
            var height = UnitUtils.ConvertToInternalUnits(2400, unit);

            //wall offset
            var offset = UnitUtils.ConvertToInternalUnits(0, unit);

            try
            {
                var tr = new Transaction(doc);
                using (tr)
                {
                    tr.Start("CreateWall");


                    //create points
                    var start = new XYZ(0, 0, 0);
                    var end = new XYZ(length, 0, 0);

                    //create line
                    var geomLine = Line.CreateBound(start, end);


                    //create wall
                    Wall.Create(doc, geomLine, firstLevel.Id, false);

                    //Wall.Create(doc, geomLine, firstLevel.Id, firstLevel.Id, height, offset, false, true);

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
