using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Specialized;
namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
   public class Class_Create_table_of_block_attribute
    {
       
            // Set up some formatting constants
            // for the table

            private const double colWidth = 15.0;
            private const double rowHeight = 3.0;
            private const double textHeight = 1.0;

            private const CellAlignment cellAlign =
              CellAlignment.MiddleCenter;

            // Helper function to set text height
            // and alignment of specific cells,
            // as well as inserting the text

            static public void SetCellText(Table tb, int row, int col, string value)
            {
                tb.SetAlignment(row, col, cellAlign);
                tb.SetTextHeight(row, col, textHeight);
                tb.SetTextString(row, col, value);
            }

            [CommandMethod("BAT_F")]
            static public void BlockAttributeTable()
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;

                PromptStringOptions opt = new PromptStringOptions("\nEnter name of block to list: ");
                PromptResult pr = ed.GetString(opt);

                if (pr.Status == PromptStatus.OK)
                {
                    string blockToFind = pr.StringResult.ToUpper();

                    Transaction tr = doc.TransactionManager.StartTransaction();
                    using (tr)
                    {
                        // Let's check the block exists

                        BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);

                        if (!bt.Has(blockToFind))
                        {
                            ed.WriteMessage("\nBlock " + blockToFind + " does not exist.");
                        }
                        else
                        {
                            // And go through looking for
                            // attribute definitions

                            StringCollection colNames = new StringCollection();

                            BlockTableRecord bd = (BlockTableRecord)tr.GetObject(bt[blockToFind], OpenMode.ForRead);
                            foreach (ObjectId adId in bd)
                            {
                                DBObject adObj = tr.GetObject(adId, OpenMode.ForRead);

                            // For each attribute definition we find...
                            
                            AttributeDefinition ad = adObj as AttributeDefinition;

                            if (ad != null)
                                {
                                    // ... we add its name to the list

                                    colNames.Add(ad.Tag);
                                //TODO: блок тестируемый TA_Кабель
                                //  ed.WriteMessage("\n" + ad.Tag + " val " + ad.TextString);
                                ed.WriteMessage("\n" + ad.Tag + " val " + ad.TextString);
                            }
                        }
                            if (colNames.Count == 0)
                            {
                                ed.WriteMessage("\nThe block " + blockToFind + " contains no attribute definitions.");
                            }
                            else
                            {
                                // Ask the user for the insertion point
                                // and then create the table

                                PromptPointResult ppr;
                                PromptPointOptions ppo = new PromptPointOptions("");
                                ppo.Message = "\n Select the place for print output:";
                                //get the coordinates from user
                                ppr = ed.GetPoint(ppo);
                                if (ppr.Status != PromptStatus.OK)
                                    return;
                                Point3d startPoint = ppr.Value.TransformBy(ed.CurrentUserCoordinateSystem);
                                //Point3d startPoint1 = startPoint.Subtract();
                                Vector3d disp = new Vector3d(0.0, -2.0 * db.Textsize, 0.0);
                                Vector3d disp2 = new Vector3d(0.0, -2.0 * db.Textsize, 0.0);

                                if (ppr.Status == PromptStatus.OK)
                                {
                                    Table tb = new Table();
                                    tb.TableStyle = db.Tablestyle;
                                    tb.NumRows = 1;
                                    tb.NumColumns = colNames.Count;
                                    tb.SetRowHeight(rowHeight);
                                    tb.SetColumnWidth(colWidth);
                                    tb.Position = startPoint;

                                    // Let's add our column headings

                                    for (int i = 0; i < 5; i++)
                                    {
                                        SetCellText(tb, 0, i, colNames[i]);
                                        ed.WriteMessage("\n" + colNames[i]);
                                    }

                                    // Now let's search for instances of
                                    // our block in the modelspace

                                    BlockTableRecord ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForRead);

                                    int rowNum = 1;
                                    foreach (ObjectId objId in ms)
                                    {
                                        DBObject obj = tr.GetObject(objId, OpenMode.ForRead);
                                        BlockReference br = obj as BlockReference;
                                        if (br != null)
                                        {
                                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                                            using (btr)
                                            {
                                            // TODO^ не надо  if (btr.Name.ToUpper() == blockToFind)
                                            if (btr.Name == blockToFind)
                                                {
                                                    // We have found one of our blocks,
                                                    // so add a row for it in the table

                                                    tb.InsertRows(rowNum, rowHeight, 2);

                                                    // Assume that the attribute refs
                                                    // follow the same order as the
                                                    // attribute defs in the block

                                                    int attNum = 0;
                                                    foreach (ObjectId arId in br.AttributeCollection)
                                                    {
                                                        DBObject arObj = tr.GetObject(arId, OpenMode.ForRead);
                                                        AttributeReference ar = arObj as AttributeReference;
                                                        if (ar != null)
                                                        {
                                                            string strCell;

                                                            strCell = ar.TextString;

                                                            string strArId = arId.ToString();

                                                            strArId = strArId.Trim(new char[] { '(', ')' });

                                                            SetCellText(tb, rowNum, attNum, strCell);
                                                            ed.WriteMessage("\n" + ar.Tag +" val22 "+ ar.TextString);
                                                        }
                                                        attNum++;
                                                    }
                                                    rowNum++;
                                                }
                                            }
                                        }
                                    }
                                    tb.GenerateLayout();

                                    ms.UpgradeOpen();
                                    ms.AppendEntity(tb);
                                    tr.AddNewlyCreatedDBObject(tb, true);
                                    tr.Commit();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

