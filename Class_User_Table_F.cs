﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Autodesk.AutoCAD.DatabaseServices.DBObject
//    Autodesk.AutoCAD.DatabaseServices.Entity
//        Autodesk.AutoCAD.DatabaseServices.BlockReference
//            Autodesk.AutoCAD.DatabaseServices.Table

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    // TODO: из чего состоит Table -  public class Table : BlockReference, IEnumerable;
   // TODO: таблица с именами блоков
    internal class Class_User_Table_F:Table
    {
        const double rowHeight = 3.0, colWidth = 5.0;

        const double textHeight = rowHeight * 0.25;

        [CommandMethod("CBT")]
        static public void CreateBlockTable()

        {

            var doc = Application.DocumentManager.MdiActiveDocument;

            if (doc == null)

                return;



            var db = doc.Database;

            var ed = doc.Editor;



            var pr = ed.GetPoint("\nEnter table insertion point");

            if (pr.Status != PromptStatus.OK)

                return;



            using (var tr = doc.TransactionManager.StartTransaction())

            {

                var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);



                // Create the table, set its style and default row/column size



                var tb = new Table();

                tb.TableStyle = db.Tablestyle;

                tb.SetRowHeight(rowHeight);

                tb.SetColumnWidth(colWidth);

                tb.Position = pr.Value;



                // Set the header cell



                var head = tb.Cells[0, 0];

                head.Value = "Blocks";

                head.Alignment = CellAlignment.MiddleCenter;

                head.TextHeight = textHeight;



                // Insert an additional column



                tb.InsertColumns(0, colWidth, 1);



                // Loop through the blocks in the drawing, creating rows



                foreach (var id in bt)

                {

                    var btr = (BlockTableRecord)tr.GetObject(id, OpenMode.ForRead);



                    // Only care about user-insertable blocks



                    if (!btr.IsLayout && !btr.IsAnonymous)

                    {

                        // Add a row



                        tb.InsertRows(tb.Rows.Count, rowHeight, 1);



                        var rowIdx = tb.Rows.Count - 1;



                        // The first cell will hold the block name



                        var first = tb.Cells[rowIdx, 0];

                        first.Value = btr.Name;

                        first.Alignment = CellAlignment.MiddleCenter;

                        first.TextHeight = textHeight;



                        // The second will contain a thumbnail of the block



                        var second = tb.Cells[rowIdx, 1];

                        second.BlockTableRecordId = id;

                    }

                }



                // Now we add the table to the current space



                var sp =

                  (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                sp.AppendEntity(tb);



                // And to the transaction, which we then commit



                tr.AddNewlyCreatedDBObject(tb, true);

                tr.Commit();

            }

        }

    }
}


