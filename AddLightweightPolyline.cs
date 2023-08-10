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

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    public class AddLightweightPolylineUser
    {
        [CommandMethod("AddLightweightPolyline")]
        
        
        public static void AddLightweightPolyline()
        {
            Class_Polyline3d.My3dPoly();
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            // открываем окно
           
            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
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
                }

                // Save the new object to the database
                acTrans.Commit();
                UserControl1 userControl1 = new UserControl1();
                AcadApp.Application.ShowModalWindow(userControl1);
            }
        }
    }
}
