using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Create_a_Polyline_Object__.NET__DLL_09_08_2023
{
    public class Reserv
        // добавляем блок с аттрибутами 14-08-2023
    {
        [CommandMethod("InsertingBlockWithAnAttribute")]
        public void InsertingBlockWithAnAttribute()
        {
            // Get the current database and start a transaction
            Database acCurDb;
            acCurDb = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                ObjectId blkRecId = ObjectId.Null;

                if (!acBlkTbl.Has("CircleBlockWithAttributes"))
                {
                    using (BlockTableRecord acBlkTblRec = new BlockTableRecord())
                    {
                        acBlkTblRec.Name = "CircleBlockWithAttributes";

                        // Set the insertion point for the block
                        acBlkTblRec.Origin = new Point3d(0, 0, 0);

                        // Add a circle to the block
                        using (Circle acCirc = new Circle())
                        {
                            acCirc.Center = new Point3d(0, 0, 0);
                            acCirc.Radius = 2;

                            acBlkTblRec.AppendEntity(acCirc);

                            // Add an attribute definition to the block
                            using (AttributeDefinition acAttDef = new AttributeDefinition())
                            {
                                acAttDef.Position = new Point3d(0, 0, 0);
                                acAttDef.Prompt = "Door #: ";
                                acAttDef.Tag = "Door#";
                                acAttDef.TextString = "DXX";
                                acAttDef.Height = 1;
                                acAttDef.Justify = AttachmentPoint.MiddleCenter;
                                acBlkTblRec.AppendEntity(acAttDef);

                                acBlkTbl.UpgradeOpen();
                                acBlkTbl.Add(acBlkTblRec);
                                acTrans.AddNewlyCreatedDBObject(acBlkTblRec, true);
                            }
                        }

                        blkRecId = acBlkTblRec.Id;
                    }
                }
                else
                {
                    blkRecId = acBlkTbl["CircleBlockWithAttributes"];
                }

                // Insert the block into the current space
                if (blkRecId != ObjectId.Null)
                {
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(blkRecId, OpenMode.ForRead) as BlockTableRecord;

                    // Create and insert the new block reference
                    using (BlockReference acBlkRef = new BlockReference(new Point3d(2, 2, 0), blkRecId))
                    {
                        BlockTableRecord acCurSpaceBlkTblRec;
                        acCurSpaceBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                        acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                        acTrans.AddNewlyCreatedDBObject(acBlkRef, true);

                        // Verify block table record has attribute definitions associated with it
                        if (acBlkTblRec.HasAttributeDefinitions)
                        {
                            // Add attributes from the block table record
                            foreach (ObjectId objID in acBlkTblRec)
                            {
                                DBObject dbObj = acTrans.GetObject(objID, OpenMode.ForRead) as DBObject;

                                if (dbObj is AttributeDefinition)
                                {
                                    AttributeDefinition acAtt = dbObj as AttributeDefinition;

                                    if (!acAtt.Constant)
                                    {
                                        using (AttributeReference acAttRef = new AttributeReference())
                                        {
                                            acAttRef.SetAttributeFromBlock(acAtt, acBlkRef.BlockTransform);
                                            acAttRef.Position = acAtt.Position.TransformBy(acBlkRef.BlockTransform);

                                            acAttRef.TextString = acAtt.TextString;

                                            acBlkRef.AttributeCollection.AppendAttribute(acAttRef);

                                            acTrans.AddNewlyCreatedDBObject(acAttRef, true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Save the new object to the database
                acTrans.Commit();

                // Dispose of the transaction
            }
        }
    }
    }
