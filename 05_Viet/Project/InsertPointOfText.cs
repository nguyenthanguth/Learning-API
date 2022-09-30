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
using Autodesk.AutoCAD.Windows.Data;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using Autodesk.AutoCAD.Colors;

namespace Learning_API_Training
{
    public class InsertPointOfText
    {
        [CommandMethod("IPT")]
        public void cmdRun()
        {
            // Đi tới bản vẽ hiện hành thực hiện câu lệnh
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptSelectionOptions quetChon1 = new PromptSelectionOptions();
            quetChon1.AllowDuplicates = false;
            quetChon1.MessageForAdding = "Select Source Text";
            quetChon1.RejectObjectsOnLockedLayers = true;

            PromptSelectionOptions quetChon2 = new PromptSelectionOptions();
            quetChon2.AllowDuplicates = false;
            quetChon2.MessageForAdding = "Select Drawing";
            quetChon2.RejectObjectsOnLockedLayers = true;

            PromptSelectionResult KQ1 = ed.GetSelection(quetChon1);
            PromptSelectionResult KQ2 = ed.GetSelection(quetChon2);

            SelectionSet SSkq1 = KQ1.Value;
            SelectionSet SSkq2 = KQ2.Value;

            if (KQ1.Status == PromptStatus.Cancel || KQ2.Status == PromptStatus.Cancel)
            {
                ed.WriteMessage("Bạn Vừa Hủy Lệnh Bằng Phím ESC");
                return;
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(db.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                foreach (ObjectId oid1 in SSkq1.GetObjectIds())
                {
                    if (oid1.ObjectClass.Name == "AcDbText")
                    {
                        DBText text1 = tr.GetObject(oid1, OpenMode.ForRead) as DBText;
                        string content1 = text1.TextString;

                        foreach (ObjectId oid2 in SSkq2.GetObjectIds())
                        {
                            if (oid2.ObjectClass.Name == "AcDbText")
                            {
                                DBText text2 = tr.GetObject(oid2, OpenMode.ForRead) as DBText;
                                string content2 = text2.TextString;
                                Point3d position2 = text2.Position;

                                if (content1 == content2)
                                {
                                    DBPoint Pis = new DBPoint(position2);
                                    // Add the new object to the block table record and the transaction
                                    acBlkTblRec.AppendEntity(Pis);
                                    tr.AddNewlyCreatedDBObject(Pis, true);
                                    // Set the style for all point objects in the drawing
                                    db.Pdmode = 34;
                                    db.Pdsize = 5;
                                }
                            }
                        }

                    }
                }
                // Save the new object to the database
                tr.Commit();
            }
        }
    }
}
