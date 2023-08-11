using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    //TODO: метод считывает (HANDL ID DXF) из всех обьектов на чертеже
    public class Class_Query_objects
    {
        [CommandMethod("OpenTransactionManager")]

        public static void OpenTransactionManager()

        {

            // Get the current document and database

            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            Database acCurDb = acDoc.Database;



            // Start a transaction

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())

            {

                // Open the Block table for read

                BlockTable acBlkTbl;

                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,

                                             OpenMode.ForRead) as BlockTable;



                // Open the Block table record Model space for read

                BlockTableRecord acBlkTblRec;

                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],

                                                OpenMode.ForRead) as BlockTableRecord;



                // Step through the Block table record

                foreach (ObjectId asObjId in acBlkTblRec)

                {

                    acDoc.Editor.WriteMessage("\nDXF name: " + asObjId.ObjectClass.DxfName);

                    acDoc.Editor.WriteMessage("\nObjectID: " + asObjId.ToString());

                    acDoc.Editor.WriteMessage("\nHandle: " + asObjId.Handle.ToString());

                    acDoc.Editor.WriteMessage("\n");

                }



                // Dispose of the transaction

            }

        }
    }
}
