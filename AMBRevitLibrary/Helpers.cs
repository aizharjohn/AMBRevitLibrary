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
    internal static class Helpers
    {
        public static Level createLevel(Document doc, double elev, string name)
        {
            var level = Level.Create(doc, elev);
            level.Name = name;

            return level;
        }

        public static Grid createStraightGrid(Document doc, double point1, double point2, double point3, double point4, string name)
        {
            var lineStart = new XYZ(point1, point2, 0);
            var lineEnd = new XYZ(point3, point4, 0);
            var line = Line.CreateBound(lineStart, lineEnd);

            // exception for grid in case of failure
            var exception = new Exception("Create a new straight grid failed.");

            var gridLine = Grid.Create(doc, line);

            if (null == gridLine)
            {
                throw exception;
            }

            gridLine.Name = name;

            return gridLine;
        }
    }
}
