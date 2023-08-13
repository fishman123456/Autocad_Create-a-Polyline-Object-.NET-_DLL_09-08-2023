using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using AcadApp = Autodesk.AutoCAD.ApplicationServices;
using Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023.Properties;
using Autodesk.AutoCAD.EditorInput;
using System.Windows;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    public class AddLightweightPolylineUser
    {
        [CommandMethod("AddLightweightPolyline")]
        
        
        public static void AddLightweightPolyline()
        {
            OpeningDataTable dataTable = new OpeningDataTable();
            Class_Polyline3d.My3dPoly();
            // Get the current document and database
            Document acDoc = AcadApp.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor ed = AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
            // открываем окно

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                AcadApp.Application.DocumentManager.MdiActiveDocument.Database.Orthomode = false;
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a polyline with two segments (3 points)
                using (Polyline acPoly = new Polyline())
                {
                    acPoly.AddVertexAt(0, new Point2d(2, 4), 0, 0, 0);
                    acPoly.AddVertexAt(1, new Point2d(4, 2), 0, 0, 0);
                    acPoly.AddVertexAt(2, new Point2d(6, 4), 0, 0, 0);

                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acPoly);
                    acTrans.AddNewlyCreatedDBObject(acPoly, true);
                    // соманды автокад
                    //TODO: Заработали команды 11-08-2023
                    acDoc.SendStringToExecute("_.ZOOM _all " ,true, false, false);
                }

                // Save the new object to the database
                acTrans.Commit();
                AcadApp.Application.ShowAlertDialog("3D полилинии начерчены");

                // TODO: перерисовка чертежа
                // Redraw the drawing

                AcadApp.Application.UpdateScreen();

                AcadApp.Application.DocumentManager.MdiActiveDocument.Editor.UpdateScreen();


                // TODO: регенерация чертежа
                // Regenerate the drawing

                AcadApp.Application.DocumentManager.MdiActiveDocument.Editor.Regen();
                AcadApp.Application.DocumentManager.MdiActiveDocument.Database.Orthomode = true;
                //UserControl1 userControl1 = new UserControl1();
                // AcadApp.Application.ShowModalWindow(userControl1);
            }
        }
    }
}
