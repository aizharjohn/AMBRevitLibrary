using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class CreateCeiling : IExternalCommand
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

            //ceiling dimensions
            var length = 24000;
            var width = 8000;

            //name of level
            var myLevel = "FFL";

            //ceiling type
            var ceilingType = "600 x 600mm Grid";

            
            var tr = new Transaction(doc);

            using (tr)
            {
                try
                {
                    tr.Start("Create Ceiling");

                    Helpers.createCeiling(doc, length, width, ceilingType, myLevel);

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
