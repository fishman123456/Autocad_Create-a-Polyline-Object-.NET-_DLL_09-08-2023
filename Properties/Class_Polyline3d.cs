﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.EditorInput;
// TODO: справка по командам автокад
// TODO: https://github.dev/luanshixia/AutoCADCodePack/blob/341b5bd153994f4016fc2aa86038e60b36e855c9/AutoCADCommands/Commands.cs#L1169#L1204
namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023.Properties
{

    public class Class_Polyline3d
    {
        [CommandMethod("My3dPoly")]

        // 10-08-2023 нужно мышкой щелкать для создания полилинии, переделаем в листбокс, лучше в текстбокс
        public static void My3dPoly()

        {

            // get the std items

            Database db = HostApplicationServices.WorkingDatabase;

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;



            Point3dCollection pnts = new Point3dCollection();

            // now input some points

            PromptPointResult res = null;
            //// делаем пока ескейп не нажмем
            //do
            //{
            //    // pint the point
            //    res = ed.GetPoint("нажмите для точки");
            //    // if point picked
            //    if (res.Status == PromptStatus.OK)
            //    {
                   
            //        // add it to our list of points
            //        //добавлени е точек щелчками мыхи
            //        // pnts.Add(res.Value);

            //    }
            //    else
            //        break;

            //} while (res.Status == PromptStatus.Cancel);
            // берем координаты жестко забитые
            pnts.Add(new Point3d(0.0, 0.0, 0.0));
            pnts.Add(new Point3d(0.0, 50.0, 0.0));
            pnts.Add(new Point3d(50.0, 50.0, 0.0));
            pnts.Add(new Point3d(50.0, 0.0, 0.0));
            pnts.Add(new Point3d(0.0, 0.0, 0.0));
            // поднимаемся по Z
            pnts.Add(new Point3d(0.0, 0.0, 100.0));
            pnts.Add(new Point3d(0.0, 0.0, 100.0));
            pnts.Add(new Point3d(0.0, 50.0, 100.0));
            pnts.Add(new Point3d(50.0, 50.0, 100.0));
            pnts.Add(new Point3d(50.0, 0.0, 100.0));
            pnts.Add(new Point3d(0.0, 0.0, 100.0));

            // if we have enough points

            if (pnts.Count >= 2)

            {
                // Create a 3D polyline with two segments (3 points)
                using (Polyline3d poly3d = new Polyline3d())
                {
                    poly3d.SetDatabaseDefaults();
                    // Add the new object to the current space block table record
                    using (BlockTableRecord curSpace = db.CurrentSpaceId.
                                               Open(OpenMode.ForWrite) as BlockTableRecord)
                    {
                        // because before adding vertexes, the polyline must be in the drawing
                        curSpace.AppendEntity(poly3d);
                        foreach (Point3d pnt in pnts)
                        {
                            // now create the vertices
                            using (PolylineVertex3d poly3dVertex = new PolylineVertex3d(pnt))
                                // and add them to the 3dpoly (this adds them to the db also
                                poly3d.AppendVertex(poly3dVertex);

                        }

                    }

                }

            }

        }
    }

}
