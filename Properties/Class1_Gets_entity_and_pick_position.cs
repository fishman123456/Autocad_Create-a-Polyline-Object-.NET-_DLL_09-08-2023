using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using System;
using AcadApp = Autodesk.AutoCAD.ApplicationServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023.Properties
{
   public class Class1_Gets_entity_and_pick_position
    {
        [CommandMethod("MyID")]
        /// <summary>
        /// Gets entity and pick position.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The entity ID and the pick position.</returns>
        /// 
        public static  Tuple<ObjectId, Point3d> GetPick(string message)
        {
            Database db = HostApplicationServices.WorkingDatabase;

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            
            var res = ed.GetEntity(message);
            if (res.Status == PromptStatus.OK)
            {
                return Tuple.Create(res.ObjectId, res.PickedPoint);
            }

            return null;
        }
    }
}
