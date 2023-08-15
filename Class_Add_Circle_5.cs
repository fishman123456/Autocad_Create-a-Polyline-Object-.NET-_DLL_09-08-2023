
using cad = Autodesk.AutoCAD.ApplicationServices.Application;
using AppSrv = Autodesk.AutoCAD.ApplicationServices;
using DbSrv = Autodesk.AutoCAD.DatabaseServices;
using EdInp = Autodesk.AutoCAD.EditorInput;
using Geom = Autodesk.AutoCAD.Geometry;
using Rtm = Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    public class Class_Add_Circle_5
    {
        [Rtm.CommandMethod("ADDCIRCLE_5")]
        public void AddCircle_5()
        {
            //TODO: стандартные сокращения

            AppSrv.Document doc = cad.DocumentManager.MdiActiveDocument;
            DbSrv.Database db = doc.Database;
            EdInp.Editor ed = doc.Editor;
            //добавим окружность в пространство Model
            using (DbSrv.Circle circle = new DbSrv.Circle())
            {

                circle.SetDatabaseDefaults();
                circle.Center = new Geom.Point3d(10.0, 10.0, 10.0);
                circle.Radius = 300.0;

                // поместим вызовы  методов ObjectId
                // .GetObject() в контекст обьекта
                // Transaction
                using (DbSrv.Transaction tr = db.TransactionManager.StartTransaction())

                {
                    DbSrv.BlockTable bt = db.BlockTableId.GetObject(DbSrv.OpenMode.ForRead)
                        as DbSrv.BlockTable;
                    DbSrv.BlockTableRecord ms = bt[DbSrv.BlockTableRecord.ModelSpace].GetObject(DbSrv.OpenMode.ForWrite) as DbSrv.BlockTableRecord;
                    ms.AppendEntity(circle);
                    doc.SendStringToExecute("._zoom _E ", true, false, false);
                    #region
                    //TODO: проверка на дату, защита времени

                    DateTime dt1 = DateTime.Now;
                    DateTime dt2 = DateTime.Parse("9/09/2023");


                    if (dt1.Date > dt2.Date)
                    {
                        cad.ShowAlertDialog("Your Application is Expire");
                        //MessageBox.Show("Your Application is Expire");
                        // Выход из проложения добавил 12-07-2023. Чтобы порядок был....
                        tr.Abort();
                        //w1.Close();
                    }
                    else
                    {
                        cad.ShowAlertDialog("Работайте до   " + dt2.ToString());
                        tr.Commit();

                    }
                    //
                    #endregion
                    // сохраняем выполненные изменения
                }
            }
        }
    }
}
