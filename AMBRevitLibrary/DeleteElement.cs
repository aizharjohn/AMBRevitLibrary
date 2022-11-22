using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace AMBRevitLibrary
{
    public static class DeleteElement
    {
        public static void deleteLevel(Document doc, String elName)
        {
            var element = FindElementByName(doc, typeof(Level), elName);

            var tr = new Transaction(doc, "Delete element");

            using (tr)
            {
                tr.Start();
                doc.Delete(element.Id);
                tr.Commit();
            }
        }

        private static Element FindElementByName(Document doc, Type targetType, string targetName)
        {
            return new FilteredElementCollector(doc)
                .OfClass(targetType)
                .FirstOrDefault<Element>(e => e.Name.Equals(targetName));
        }
    }
}
