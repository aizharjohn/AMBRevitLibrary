using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMBRevitLibrary
{
    [Transaction(TransactionMode.Manual)]
    public class CreateNewWall : IExternalCommand
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

            //set unit to millimeters
            var unit = UnitTypeId.Millimeters;



            //grab all levels
            var colLevels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Level));

            //test grab of materials
            var colMaterials = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Material));

            //grab first level in collection
            var level = colLevels.FirstElement() as Level;

            //grab all walls
            var colWalls = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(Wall));

            //grab first wall in collection
            var wall = colWalls.FirstElement() as Wall;

            //grab all wall types
            var colWallTypes = new FilteredElementCollector(doc)
                //.WhereElementIsNotElementType()
                //.OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(WallType));

            //var colWallTypes = new FilteredElementCollector(doc)
            //    .WhereElementIsElementType()
            //    .OfCategory(BuiltInCategory.OST_Walls)
            //    .ToElements();


            //grab first wall type in collection
            var firstWallType = colWalls.FirstElement() as WallType;

            //set new wall type to null
            WallType newWallType = null;


            try
            {
                var tr = new Transaction(doc);

                using (tr)
                {
                    tr.Start("create new wall");

                    if (newWallType != null)
                    {
                        newWallType = firstWallType.Duplicate("New Wall") as WallType;
                    }
                    else
                    {
                        throw new Exception("Value null");
                    }


                    //grab material of wall
                    var oldLayerMaterialId = firstWallType.GetCompoundStructure().GetLayers()[0].MaterialId;

                    //dimensions
                    var thk1 = UnitUtils.ConvertToInternalUnits(9, unit);
                    var thk2 = UnitUtils.ConvertToInternalUnits(12.5, unit);
                    var thk3 = UnitUtils.ConvertToInternalUnits(89, unit);

                    var length = UnitUtils.ConvertToInternalUnits(1200, unit);


                    //set wall structure
                    //exterior
                    var extLayerFinish1 = new CompoundStructureLayer(thk1, MaterialFunctionAssignment.Finish1, oldLayerMaterialId);

                    //middle
                    var structLayer = new CompoundStructureLayer(thk3, MaterialFunctionAssignment.StructuralDeck, oldLayerMaterialId);

                    //interior
                    var intLayerFinish2 = new CompoundStructureLayer(thk2, MaterialFunctionAssignment.Finish2, oldLayerMaterialId);

                    //grab wall compound structure
                    var compoundStructure = newWallType.GetCompoundStructure();

                    //add all individual layers to wall compound structure
                    IList<CompoundStructureLayer> layers = compoundStructure.GetLayers();

                    //set compound structure layers
                    layers.Insert(0, extLayerFinish1);
                    layers.Insert(1, structLayer);
                    layers.Insert(2, intLayerFinish2);

                    //set layers to compound structure
                    compoundStructure.SetLayers(layers);

                    //set number of ext and int layers
                    compoundStructure.SetNumberOfShellLayers(ShellLayerType.Exterior, 1);
                    compoundStructure.SetNumberOfShellLayers(ShellLayerType.Interior, 1);

                    //debug.print interior and exterior shell layers
                    Debug.Print($"Exterior compound structure layers: {compoundStructure.GetNumberOfShellLayers(ShellLayerType.Exterior)}");
                    Debug.Print($"Interior compound structure layers: {compoundStructure.GetNumberOfShellLayers(ShellLayerType.Interior)}");

                    //set compound structure
                    newWallType.SetCompoundStructure(compoundStructure);


                    //******PLACE THE WALL******//

                    //get locations for line
                    var start = new XYZ(0, 0, 0);
                    var end = new XYZ(length, 0, 0);

                    //create line between points
                    var geomLine = Line.CreateBound(start, end);

                    //set level as first level

                    //set height
                    var height = UnitUtils.ConvertToInternalUnits(2440, unit);

                    //set offset
                    var offset = 0.00;

                    //place wall
                    Wall.Create(doc, geomLine, newWallType.Id, level.Id, height, offset, false, true);

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
