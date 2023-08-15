using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
// четко прописываем пространство имен чтобы не путаться между автокадом и виндовсом 15-08-2023
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
//Autodesk.AutoCAD.DatabaseServices.DBObject
//    Autodesk.AutoCAD.DatabaseServices.Entity
//        Autodesk.AutoCAD.DatabaseServices.BlockReference
//            Autodesk.AutoCAD.DatabaseServices.Table

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    // TODO: из чего состоит Table -  public class Table : BlockReference, IEnumerable;
   // TODO: таблица с именами блоков
   public class Class_User_Table_F:Table
    {
        const double rowHeight = 20.0, colWidth = 30.0;

        const double textHeight = rowHeight * 0.25;

        [CommandMethod("CBT")]
        static public void CreateBlockTable()

        {

            var doc = Application.DocumentManager.MdiActiveDocument;

            if (doc == null)

                return;


            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;

            var ed = doc.Editor;


            // TODO: задаем точку вручную, щелчком мыши
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
                    var btrAttr = (BlockTableRecord)tr.GetObject(id, OpenMode.ForRead);


                    // Only care about user-insertable blocks


                    // TODO: если не существуют и не анонимные
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

                       // var third = tb.Cells[rowIdx, 2];
                       
                        //third.BlockTableRecordId = id;
                    }

                }
                // Now we add the table to the current space
                var sp =

                  (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                
                sp.AppendEntity(tb);
                // And to the transaction, which we then commit
                tr.AddNewlyCreatedDBObject(tb, true);
                acDoc.SendStringToExecute("._zoom _E ", true, false, false);
                #region
                //TODO: проверка на дату, защита времени

                DateTime dt1 = DateTime.Now;
                DateTime dt2 = DateTime.Parse("9/09/2023");


                if (dt1.Date > dt2.Date)
                {
                    Application.ShowAlertDialog("Your Application is Expire");
                    //MessageBox.Show("Your Application is Expire");
                    // Выход из проложения добавил 12-07-2023. Чтобы порядок был....
                    tr.Abort();
                    //w1.Close();
                }
                else
                {
                    Application.ShowAlertDialog("Работайте до   " + dt2.ToString());
                    tr.Commit();
                }
                //
                #endregion
               

            }

        }

    }
    
}



