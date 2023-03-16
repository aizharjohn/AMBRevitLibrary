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
    internal static class Active
    {
        public static ExternalCommandData doc;

        public static ExternalCommandData GetCommandData()
        {
            //get application
            var uiApp = Active.doc.Application;
            var app = uiApp.Application;

            //get UI document
            var uiDoc = uiApp.ActiveUIDocument;

            var doc = uiDoc.Document;

            return Active.doc;
        }
    }
}
