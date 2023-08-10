using Autodesk.AutoCAD.ApplicationServices;
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

            do

            {

                // pint the point

                res = ed.GetPoint("Pick a 3d point");

                // if point picked

                if (res.Status == PromptStatus.OK)

                {

                    // add it to our list of points

                    pnts.Add(res.Value);

                }

                else

                    break;



            } while (res.Status == PromptStatus.OK);



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
