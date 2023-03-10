using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

using Autodesk.Revit.UI;

namespace AMBRevitLibrary
{
    public static class MyRibbonPanels
    {
        public static void createHelloPanel(UIControlledApplication application)
        {
            var myTab = Constants.RIBBON_TAB;
            var myPanel = Constants.TEST_PANEL;

            //get or create the panel
            RibbonPanel panel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(myTab);
            foreach (RibbonPanel pnl in panels)
            {
                if (pnl.Name == myPanel)
                {
                    panel = pnl;
                    break;
                }
            }

            //if panel not found, create it
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(myTab, myPanel);
            }

            //get the image for the button
            var img = Properties.Resources.icons8_structural_48;
            var imgSrc = GetImageSource(img);

            //create the button data
            var btnData = new PushButtonData(
                "My Button",
                "Hello",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.Hello")
            {
                ToolTip = "Short Description",
                LongDescription = "Long Description",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            //add the button to the ribbon
            var button = (PushButton)panel.AddItem(btnData);
            button.Enabled = true;
        }

        public static void createGridAndLevelPanel(UIControlledApplication application)
        {
            var myTab = Constants.RIBBON_TAB;
            var myPanel = Constants.GRID_AND_LEVEL_PANEL;

            //get or create the panel
            RibbonPanel panel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(myTab);
            foreach (RibbonPanel pnl in panels)
            {
                if (pnl.Name == myPanel)
                {
                    panel = pnl;
                    break;
                }
            }

            //if panel not found, create it
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(myTab, myPanel);
            }

            //get the image for the button
            var img = Properties.Resources.icons8_structural_48;
            var imgSrc = GetImageSource(img);

            //create the button data
            var btnData = new PushButtonData(
                "Setup",
                "Setup",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.GridsAndLevels")
            {
                ToolTip = "Short Description",
                LongDescription = "Long Description",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            //add the button to the ribbon
            var button = (PushButton)panel.AddItem(btnData);
            button.Enabled = true;
        }

        public static void createWallsPanel(UIControlledApplication application)
        {
            var myTab = Constants.RIBBON_TAB;
            var myPanel = Constants.WALL_RIBBON_PANEL;

            //get or create the panel
            RibbonPanel panel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(myTab);
            foreach (RibbonPanel pnl in panels)
            {
                if (pnl.Name == myPanel)
                {
                    panel = pnl;
                    break;
                }
            }

            //if panel not found, create it
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(myTab, myPanel);
            }

            //get the image for the button
            var img = Properties.Resources.icons8_structural_48;
            var imgSrc = GetImageSource(img);

            //create the button data
            var btnData = new PushButtonData(
                "WallButton1",
                "Type 1",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.CreateWall")
            {
                ToolTip = "Wall type 1",
                LongDescription = "Wall type 1",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            //create the button data
            var newWallBtnData = new PushButtonData(
                "WallButton2",
                "Type 2",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.CreateNewWall2")
            {
                ToolTip = "Wall type 2",
                LongDescription = "Wall type 2",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            ////add the button to the ribbon
            //var button = (PushButton)panel.AddItem(btnData);
            //button.Enabled = true;

            ////add the new wall command button to the ribbon
            //var newWallButton = (PushButton)panel.AddItem(newWallBtnData);
            //newWallButton.Enabled = true;

            List<RibbonItem> wallButtons = new List<RibbonItem>();

            wallButtons.AddRange(panel.AddStackedItems(btnData, newWallBtnData));
        }

        public static void createFloorPanel(UIControlledApplication application)
        {
            var myTab = Constants.RIBBON_TAB;
            var myPanel = Constants.FLOOR_PANEL;

            //get or create the panel
            RibbonPanel panel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(myTab);
            foreach (RibbonPanel pnl in panels)
            {
                if (pnl.Name == myPanel)
                {
                    panel = pnl;
                    break;
                }
            }

            //if panel not found, create it
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(myTab, myPanel);
            }

            //get the image for the button
            var img = Properties.Resources.icons8_structural_48;
            var imgSrc = GetImageSource(img);

            //create the button data
            var btnData = new PushButtonData(
                "Floor Button 1",
                "Create Floor",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.CreateFloor")
            {
                ToolTip = "Short Description",
                LongDescription = "Long Description",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            //add the button to the panel
            var button = (PushButton)panel.AddItem(btnData);
            button.Enabled = true;
        }

        public static void createCeilingPanel(UIControlledApplication application)
        {
            var myTab = Constants.RIBBON_TAB;
            var myPanel = Constants.CEILING_PANEL;

            //get or create the panel
            RibbonPanel panel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(myTab);
            foreach (RibbonPanel pnl in panels)
            {
                if (pnl.Name == myPanel)
                {
                    panel = pnl;
                    break;
                }
            }

            //if panel not found, create it
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(myTab, myPanel);
            }

            //get the image for the button
            var img = Properties.Resources.icons8_structural_48;
            var imgSrc = GetImageSource(img);

            //create the button data
            var btnData = new PushButtonData(
                "Ceiling Button 1",
                "Create Ceiling",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.CreateCeiling")
            {
                ToolTip = "Short Description",
                LongDescription = "Long Description",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            //add the button to the panel
            var button = (PushButton)panel.AddItem(btnData);
            button.Enabled = true;
        }

        public static void createRoofPanel(UIControlledApplication application)
        {
            var myTab = Constants.RIBBON_TAB;
            var myPanel = Constants.ROOF_PANEL;

            //get or create the panel
            RibbonPanel panel = null;
            List<RibbonPanel> panels = application.GetRibbonPanels(myTab);
            foreach (RibbonPanel pnl in panels)
            {
                if (pnl.Name == myPanel)
                {
                    panel = pnl;
                    break;
                }
            }

            //if panel not found, create it
            if (panel == null)
            {
                panel = application.CreateRibbonPanel(myTab, myPanel);
            }

            //get the image for the button
            var img = Properties.Resources.icons8_structural_48;
            var imgSrc = GetImageSource(img);

            //create the button data
            var btnData = new PushButtonData(
                "Roof Button 1",
                "Create Roof",
                Assembly.GetExecutingAssembly().Location,
                "AMBRevitLibrary.CreateRoof")
            {
                ToolTip = "Short Description",
                LongDescription = "Long Description",
                Image = imgSrc,
                LargeImage = imgSrc
            };

            //add the button to the panel
            var button = (PushButton)panel.AddItem(btnData);
            button.Enabled = true;
        }

        private static BitmapSource GetImageSource(Image img)
        {
            var bmp = new BitmapImage();

            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;
                bmp.EndInit();
            }

            return bmp;
        }
    }
}
