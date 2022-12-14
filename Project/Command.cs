using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// using AutoCad
// using NameSpace
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows.Data;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using Autodesk.AutoCAD.Colors;

// Ghi chú 1

/*
Ghi chú 2
*/

#region Nhóm code
// Code here
#endregion

namespace Learning_API_Training
{
    public class Command
    {
        [CommandMethod("QUETCHON")]
        public void cmdQUETCHON()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            //PromptPointOptions ppo = new PromptPointOptions("Pick diem tren man hinh autocad");
            //PromptPointResult ppr = ed.GetPoint(ppo);

            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = "Quet Chon Doi Tuong";
            pso.AllowDuplicates = false;
            pso.RejectObjectsOnLockedLayers = true;

            PromptSelectionResult psr = ed.GetSelection(pso);
            SelectionSet ss = psr.Value;

            if (psr.Status == PromptStatus.OK)
            {
                ArrayList lengthArr = new ArrayList();
                foreach (ObjectId oid in ss.GetObjectIds())
                {
                    if (oid.ObjectClass.Name == "AcDbLine")
                    {
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            Line li = tr.GetObject(oid, OpenMode.ForRead) as Line;
                            ed.WriteMessage("\nLength = " + li.Length);

                            lengthArr.Add(li.Length);

                            tr.Commit();
                        }
                    }
                }

                double tong = 0;
                foreach (double length in lengthArr)
                {
                    tong = tong + length;
                }

                for (int i = 0; i < lengthArr.Count; i++)
                {
                    tong = tong + double.Parse(lengthArr[i].ToString());
                }

                MessageBox.Show("Tổng chiều dài là : " + tong, "Tính tổng chiều dài");
            }
            else
            {
                MessageBox.Show("Bạn vừa huỷ lệnh bằng phím ESC", "Thông báo");
            }
        }


































        [CommandMethod("NewLayer")]
        public void cmdNewLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                #region Create New Layer
                // Open the Block table for read
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;
                LayerTableRecord ltr = new LayerTableRecord();

                // Kiểm tra Layer đã có trong bản vẽ hay chưa
                // Nếu có thì return (ngưng thực thi câu lệnh)
                string layerName = "SlabLayout";
                if (lt.Has(layerName) == true)
                {
                    ed.WriteMessage($"Đã có layer name {layerName}");
                    return;
                }

                // Assign the layer the ACI color 1 and a name
                ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, 240);
                ltr.Name = "SlabLayout";

                // Append the new layer to the Layer table and the transaction
                lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                #endregion

                tr.Commit();
            }
        }



        [CommandMethod("QWE")]
        // public void ~> thực hiện câu lệnh
        // public static ~> trả về 1 kết quả của câu lệnh thực hiện (return)
        public void cmdQWE()
        {
            // Hàm chính thực hiện công việc
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // Mở một giao dịch trong AutoCad
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                #region Vẽ Lệnh Line trong AutoCad
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // Create a line that starts at 5,5 and ends at 12,3
                Line acLine = new Line(new Point3d(5, 5, 0), new Point3d(12, 3, 0));

                acLine.SetDatabaseDefaults();

                // Add the new object to the block table record and the transaction
                acBlkTblRec.AppendEntity(acLine);
                tr.AddNewlyCreatedDBObject(acLine, true);
                #endregion

                #region Vẽ Polyline
                // Create a line that starts at 5,5 and ends at 12,3
                Polyline pol = new Polyline();
                pol.AddVertexAt(0, new Point2d(0, 0), 0,0,0);
                pol.AddVertexAt(1, new Point2d(5, 0), 0, 0, 0);
                pol.AddVertexAt(2, new Point2d(5, 5), 0, 0, 0);
                pol.AddVertexAt(3, new Point2d(0, 5), 0, 0, 0);
                pol.Closed = true;

                acLine.SetDatabaseDefaults();

                // Add the new object to the block table record and the transaction
                acBlkTblRec.AppendEntity(pol);
                tr.AddNewlyCreatedDBObject(pol, true);
                #endregion
                // Đóng giao dịch trong AutoCad
                tr.Commit();
            }
        }
        private void cmdTenBanVe()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            string tenBanVe = doc.Name;

            ed.WriteMessage("\nbản vẽ hiện hành là : " + tenBanVe);
        }
    }
}
// Dạng dữ liệu
// event : sự kiện
// properties: thuộc tính
// mothod: phương thức

// Quy tắc đặt tên
// Pascal: KieuDuLieu : thường dùng cho các hàm
// Lạc đà: kieuDuLieu : thường dùng cho các biến của kiểu dữ liệu
// quy_tắc_snake: kieu_du_lieu : thường dùng cho các tham số (parameter)
// QUY_TẮC_SNAKE: KIEU_DU_LIEU : thường dùng cho các hằng số (Const)
