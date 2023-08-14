using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    public class Class_add_to_dictionary_attr_block
    {

        [CommandMethod("ATTR_TO_DICTIONARY")]


        // Gets the attributes from the drawing
        public void getDWGInfo()
        {
            // Declarations
            int dwgCount = 0;

            DocumentCollection docs = Application.DocumentManager;
            var dwgDict = new Dictionary<string, string>();

            foreach (Document doc in docs)
            {
                Database acCurDb;
                acCurDb = Application.DocumentManager.MdiActiveDocument.Database;

                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the block table for read
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                    if (acBlkTbl.Has("TA_Кабель"))
                    {
                        // ##### Get the value of attribute "REV" from "STAR2" & Add to dictionary below?
                        dwgDict.Add(doc.Name,acBlkTbl.Database.BlockTableId.ToString());
                        dwgCount++;
                    }
                    else
                    {
                        Application.ShowAlertDialog("The drawing does not have a STAR2 titleblock with a REV attribute, couldn't add to list.");
                    }
                }
            }
            // ##### Return dwgDict to the method that called it?
        }
    }
}
