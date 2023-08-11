using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.AutoCAD.Runtime;

using Autodesk.AutoCAD.Geometry;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    //http://docs.autodesk.com/ACD/2010/ENU/AutoCAD%20.NET%20Developer's%20Guide/index.html?url=WS1a9193826455f5ff2566ffd511ff6f8c7ca-41f1.htm,topicNumber=d0e11617
    // деление экрана модели пополам
    public class Class_Viewport
    {
        [CommandMethod("CreateModelViewport")]

        public static void CreateModelViewport()

        {

            // Get the current database

            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            Database acCurDb = acDoc.Database;



            // Start a transaction

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())

            {

                // Open the Viewport table for read

                ViewportTable acVportTbl;

                acVportTbl = acTrans.GetObject(acCurDb.ViewportTableId,

                                               OpenMode.ForRead) as ViewportTable;



                // Check to see if the named view 'TEST_VIEWPORT' exists

                if (acVportTbl.Has("TEST_VIEWPORT") == false)

                {

                    // Open the View table for write

                    acVportTbl.UpgradeOpen();



                    // Add the new viewport to the Viewport table and the transaction

                    ViewportTableRecord acVportTblRecLwr = new ViewportTableRecord();

                    acVportTbl.Add(acVportTblRecLwr);

                    acTrans.AddNewlyCreatedDBObject(acVportTblRecLwr, true);



                    // Name the new viewport 'TEST_VIEWPORT' and assign it to be

                    // the lower half of the drawing window

                    acVportTblRecLwr.Name = "TEST_VIEWPORT";

                    acVportTblRecLwr.LowerLeftCorner = new Point2d(0, 0);

                    acVportTblRecLwr.UpperRightCorner = new Point2d(1, 0.5);



                    // Add the new viewport to the Viewport table and the transaction

                    ViewportTableRecord acVportTblRecUpr = new ViewportTableRecord();

                    acVportTbl.Add(acVportTblRecUpr);

                    acTrans.AddNewlyCreatedDBObject(acVportTblRecUpr, true);



                    // Name the new viewport 'TEST_VIEWPORT' and assign it to be

                    // the upper half of the drawing window

                    acVportTblRecUpr.Name = "TEST_VIEWPORT";

                    acVportTblRecUpr.LowerLeftCorner = new Point2d(0, 0.5);

                    acVportTblRecUpr.UpperRightCorner = new Point2d(1, 1);



                    // To assign the new viewports as the active viewports, the 

                    // viewports named '*Active' need to be removed and recreated

                    // based on 'TEST_VIEWPORT'.



                    // Step through each object in the symbol table

                    foreach (ObjectId acObjId in acVportTbl)

                    {

                        // Open the object for read

                        ViewportTableRecord acVportTblRec;

                        acVportTblRec = acTrans.GetObject(acObjId,

                                                          OpenMode.ForRead) as ViewportTableRecord;



                        // See if it is one of the active viewports, and if so erase it

                        if (acVportTblRec.Name == "*Active")

                        {

                            acVportTblRec.UpgradeOpen();

                            acVportTblRec.Erase();

                        }

                    }



                    // Clone the new viewports as the active viewports

                    foreach (ObjectId acObjId in acVportTbl)

                    {

                        // Open the object for read

                        ViewportTableRecord acVportTblRec;

                        acVportTblRec = acTrans.GetObject(acObjId,

                                                          OpenMode.ForRead) as ViewportTableRecord;



                        // See if it is one of the active viewports, and if so erase it

                        if (acVportTblRec.Name == "TEST_VIEWPORT")

                        {

                            ViewportTableRecord acVportTblRecClone;

                            acVportTblRecClone = acVportTblRec.Clone() as ViewportTableRecord;



                            // Add the new viewport to the Viewport table and the transaction

                            acVportTbl.Add(acVportTblRecClone);

                            acVportTblRecClone.Name = "*Active";

                            acTrans.AddNewlyCreatedDBObject(acVportTblRecClone, true);

                        }

                    }



                    // Update the display with the new tiled viewports arrangement

                    acDoc.Editor.UpdateTiledViewportsFromDatabase();



                    // Commit the changes

                    acTrans.Commit();

                }



                // Dispose of the transaction

            }

        }
    }
}
