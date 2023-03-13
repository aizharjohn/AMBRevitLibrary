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
using System.IO;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class ExportToPdf : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //get application
            var uiapp = commandData.Application;

            //get UI document
            var uidoc = uiapp.ActiveUIDocument;

            //var app = uiapp.Application;

            //get document
            var doc = uidoc.Document;

            // get all the elements in the model database
            var collector = new FilteredElementCollector(doc);

            // filter out all elements except Views
            ICollection<Element> collection = collector.OfClass(typeof(ViewSheet)).ToElements();

            try
            {
                var tr = new Transaction(doc, "Export Sheets");

                using (tr)
                {
                    // create a list to hold the sheets
                    List<ElementId> sheetsToPDF = new List<ElementId>();

                    // create DWG export options
                    var pdfOptions = new PDFExportOptions();

                    pdfOptions.HideCropBoundaries = true;
                    pdfOptions.HideReferencePlane = true;
                    pdfOptions.HideScopeBoxes = true;
                    pdfOptions.ColorDepth = ColorDepthType.Color;
                    pdfOptions.ExportQuality = PDFExportQualityType.DPI300;
                    pdfOptions.PaperFormat = ExportPaperFormat.ISO_A0;
                    pdfOptions.PaperPlacement = PaperPlacementType.Center;
                    pdfOptions.PaperOrientation = PageOrientationType.Landscape;
                    pdfOptions.ZoomType = ZoomType.Zoom;
                    pdfOptions.ZoomPercentage = 1;

                    // add a counter to count the sheets exported
                    var x = 0;

                    //loop through each view in the model
                    foreach (var e in collection)
                    {
                        try
                        {
                            var viewSheet = (ViewSheet)e;

                            //only add sheets to list
                            if (viewSheet.IsPlaceholder == false)
                            {
                                sheetsToPDF.Add(e.Id);
                                x++;
                            }
                        }
                        catch (Exception exception)
                        {
                            message = exception.Message;
                        }
                    }

                    var path = "";
                    var file = "";

                    //get the current date and time
                    var dtNow = DateTime.Now;
                    var dt = string.Format("{0:yyyyMMdd HHmm}", dtNow);

                    if (doc.PathName != "")
                    {
                        //use model path + date and time
                        //path = Path.GetDirectoryName(doc.PathName) + "\\" + dt;
                        path = Path.GetDirectoryName("C:\\Users\\aizhar\\Desktop\\") + "\\" + dt;
                    }
                    else
                    {
                        //C: \Users\aizhar\Desktop
                        //model has not been saved
                        // use C:\DWG_Export + date and time
                        path = "C:\\Users\\aizhar\\Desktop\\" + dt;
                        file = "NONAME";

                    }

                    //create folder
                    Directory.CreateDirectory(path);

                    //export
                    //doc.Export(path, file, sheetsToPDF, pdfOptions);
                    doc.Export(file, sheetsToPDF, pdfOptions);

                    TaskDialog.Show("Export Sheets to PDF", x + " sheets exported to:\n" + path);

                    // String interpolation:
                    //Console.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");


                    //commit to the transaction
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
