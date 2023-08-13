using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    // TODO: доступ к командам автокада
    public class Class_Access_the_AutoCAD_Command_Line
    {
        [CommandMethod("SendACommandToAutoCAD")]

        public static void SendACommandToAutoCAD()

        {

            Document acDoc = Application.DocumentManager.MdiActiveDocument;



            // Draws a circle and zooms to the extents or 

            // limits of the drawing

            acDoc.SendStringToExecute("._circle 2,2,0 4 ", true, false, false);
            acDoc.SendStringToExecute("._circle 2,6,0 4 ", true, false, false);
            acDoc.SendStringToExecute("._circle 2,10,0 4 ", true, false, false);

            acDoc.SendStringToExecute("._zoom _all ", true, false, false);

        }
    }
}
