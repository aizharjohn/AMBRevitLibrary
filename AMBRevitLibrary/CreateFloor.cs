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

            //get document
            var doc = uidoc.Document;

            //floor dimensions
            var floorLength = 24000;
            var floorWidth = 8000;

            //convert units to millimeters
            var unit = UnitTypeId.Millimeters;
            var width = UnitUtils.ConvertToInternalUnits(floorWidth / 2, unit);
            var length = UnitUtils.ConvertToInternalUnits(floorLength / 2, unit);

            //name of level
            var myLevel = "FFL";

            //name of floor type
            var floorType = "160mm Concrete With 50mm Metal Deck";

            var tr = new Transaction(doc);

            using ( tr )
            {
                try
                {
                    tr.Start("CreateFloor");
                    Helpers.createArchitecturalFloor(doc, length, width, floorType, myLevel);

                    //commit transaction
                    tr.Commit();
                }
                catch (Exception e)
                {
                    message = e.Message;
                    tr.RollBack();
                    return Result.Failed;
                    
                }
            }

            return Result.Succeeded;
        }
    }
}
