using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

// using AutoCad
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows.Data;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using Autodesk.AutoCAD.Colors;

// Thực hành tạo tool create new layer
// Tùy theo cá nhân mỗi người có một cách viết logic khác nhau

namespace Learning_API_Training
{
    public class SelectionAutoCad
    {
        [CommandMethod("SEL")]
        public void selection()
        {
            // Đi tới bản vẽ hiện hành thực hiện câu lệnh
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = "Quet chon doi tuong";
            pso.AllowDuplicates = false;
            pso.RejectObjectsOnLockedLayers = true;

            PromptSelectionResult psr = ed.GetSelection(pso);
            SelectionSet ss = psr.Value;

            //Đặt câu điều kiện: nếu kết quả đúng
            if (psr.Status == PromptStatus.OK)
            {
                ArrayList radiusArr = new ArrayList();
                foreach (ObjectId ob in ss.GetObjectIds())
                {

                    if (ob.ObjectClass.Name == "AcDbCircle")
                    {
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            Circle cc = tr.GetObject(ob, OpenMode.ForRead) as Circle;
                            ed.WriteMessage("\nRadius la:" + cc.Area);
                            radiusArr.Add(cc.Area);
                            tr.Commit();
                        }
                    }
                }
                double tong = 0;
                foreach (double Area in radiusArr)
                {
                    tong = tong + Area;
                }
                MessageBox.Show("Tổng bán kính là:" + tong, "Bảng tính tổng");

                //for (int i = 0; i < radiusArr.Count; i++)
                //{
                //    tong = tong + double.Parse(radiusArr{ i}.ToString();
                //}

            }
            else
            {
                //Lệnh đầu tiên là dòng thứ 2, lệnh sau là hàng dưới
                MessageBox.Show("Bạn vừa hủy lệnh bằng phím ESE", "Thông báo");
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

                    // Create a line that starts at 5,5 and ends at 12,3
                    Line acLine = new Line(new Point3d(0, 5, 0),
                                           new Point3d(2, 3, 0));
                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acLine);
                    tr.AddNewlyCreatedDBObject(acLine, true);

                    tr.Commit();
                }
            }

        }
    }
}
