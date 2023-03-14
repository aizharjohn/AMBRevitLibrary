using System;

//Autodesk Libraries
using Autodesk.Revit.UI;


namespace AMBRevitLibrary
{
    public class MyApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            AddMyRibbon(application);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void AddMyRibbon(UIControlledApplication application)
        {
            
            //get the ribbon tab
            try
            {
                application.CreateRibbonTab(Constants.RIBBON_TAB);
            }
            catch (Exception e)
            {
                _ = e.Message;
            }

            //add the panels here
            MyRibbonPanels.createHelloPanel(application);
            MyRibbonPanels.createGridAndLevelPanel(application);
            MyRibbonPanels.createWallsPanel(application);
            MyRibbonPanels.createFloorPanel(application);
            MyRibbonPanels.createCeilingPanel(application);
            MyRibbonPanels.createRoofPanel(application);
            MyRibbonPanels.exportsPanel(application);
        }
    }
}
