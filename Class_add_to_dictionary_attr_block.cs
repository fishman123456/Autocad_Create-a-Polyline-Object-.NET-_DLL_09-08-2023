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
        public static void getDWGInfo()
        {
            // Declarations
            int dwgCount = 0;
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            DocumentCollection docs = Application.DocumentManager;
            var dwgList = new List<string>();

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
                        dwgList.Add(doc.Name);
                        dwgCount++;
                        acDoc.Editor.WriteMessage("\nname: " + acBlkTbl.ObjectId);
                    }
                    else
                    {
                        Application.ShowAlertDialog("The drawing does not have a TA_Кабель titleblock with a REV attribute, couldn't add to list.");
                    }
                }
            }
            // ##### Return dwgDict to the method that called it?
        }
    }
}


/// <summary>
/// Creates a block and then inserts a reference to the model space.
/// </summary>
/// <param name="entities">The entities.</param>
/// <param name="blockName">The block name.</param>
/// <param name="blockBasePoint">The block base point.</param>
/// <param name="insertPosition">The insert position.</param>
/// <param name="rotation">The rotation.</param>
/// <param name="scale">The scale.</param>
/// <param name="overwrite">Whether to overwrite.</param>
/// <returns>The insert object ID.</returns>
/// 
////public static ObjectId CreateBlockAndInsertReference(IEnumerable<Entity> entities, string blockName, Point3d blockBasePoint, Point3d insertPosition, double rotation = 0, double scale = 1, bool overwrite = true)
////{
////    Draw.Block(entities, blockName, blockBasePoint, overwrite);
////    return Draw.Insert(blockName, insertPosition, rotation, scale);
////}
