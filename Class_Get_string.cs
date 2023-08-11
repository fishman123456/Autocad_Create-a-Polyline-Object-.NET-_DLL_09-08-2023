using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    public class Class_Get_string
    {

        // TODO: метод ввода строки пользователем
        [CommandMethod("GetStringFromUser")]

        public static void GetStringFromUser()

        {

            Document acDoc = Application.DocumentManager.MdiActiveDocument;



            PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter your name: ");

            pStrOpts.AllowSpaces = true;

            PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts);



            Application.ShowAlertDialog("The name entered was: " +

                                        pStrRes.StringResult);

        }
    }
}
