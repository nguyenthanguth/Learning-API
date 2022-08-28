using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

// using AutoCad
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
    public class CreateNewLayer
    {
        // Cách 1: tạo hàm static `cmdCreateNewLayer` thực hiện câu lệnh lặp lại nhiều lần mỗi khi gọi hàm
        [CommandMethod("CNL")]
        public void cmdRun()
        {
            cmdCreateNewLayer("Defpoints", 7, null);
            cmdCreateNewLayer("T1-Thay", 3, null);
            cmdCreateNewLayer("T2-Khuat", 6, "HIDDEN");
            cmdCreateNewLayer("T3-Thep", 1, null);
            cmdCreateNewLayer("T4-Cotthep", 4, null);
            cmdCreateNewLayer("T5-Thepdai", 2, null);
            cmdCreateNewLayer("T6-Text", 7, null);
            cmdCreateNewLayer("T7-Opening", 7, null);
            cmdCreateNewLayer("T8-Hatch", 9, null);
            cmdCreateNewLayer("T9-Dim", 144, null);
            cmdCreateNewLayer("T10-Truc", 8, "CENTER");
            cmdCreateNewLayer("T11-Manh", 9, null);

            cmdCreateNewLayer("Khung", 7, null);
            cmdCreateNewLayer("Work Point", 7, null);

            cmdCreateNewLayer("SlabLayout", 4, null);
            cmdCreateNewLayer("SlabOpening", 7, null);
            cmdCreateNewLayer("RebarRegion", 1, null);
            cmdCreateNewLayer("Slab_Center", 2, null);

            cmdCreateNewLayer("ColLayout", 2, null);
            cmdCreateNewLayer("COLUMNBLOCK", 7, null);

            cmdCreateNewLayer("WallLayout", 1, null);
            cmdCreateNewLayer("BIMBLOCKS", 7, null);

            cmdCreateNewLayer("BeamLayout", 3, null);
        }
        private static string cmdCreateNewLayer(string layer_name, short color, string line_type)
        {
            // Đi tới bản vẽ hiện hành thực hiện câu lệnh
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // Khởi tạo một giao dịch (Transaction)
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Open the Layer Table For Write
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;

                // Kiểm tra `layerName` đã có trong bản vẽ hay chưa
                // Nếu chưa có `layerName` thì tạo | Nếu có thì hiển thị thông báo đã tồn tại `layerName`
                if (lt.Has(layer_name) == false)
                {
                    // Khởi tạo biến `ltr` - kiểu dữ liệu `LayerTableRecord` dùng để đặt các thuộc tính của Layer
                    LayerTableRecord ltr = new LayerTableRecord();

                    // Đặt tên `layerName`
                    ltr.Name = layer_name;

                    // Đặt màu sắc của `layerName`
                    ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, color);

                    // Thêm `LayerTableRecord` vào `LayerTable`
                    lt.Add(ltr);

                    // Thêm `LayerTableRecord` vào `Transaction`
                    tr.AddNewlyCreatedDBObject(ltr, true);

                    // Open the Line type Table For Write
                    LinetypeTable lineType = tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite) as LinetypeTable;

                    if (line_type != null)
                    {
                        // Kiểm tra `line_type` đã có trong bản vẽ hay chưa
                        if (lineType.Has(line_type) == false)
                        {
                            // Load the linetype “CENTER | HIDDEN” from the acad.lin file
                            db.LoadLineTypeFile(line_type, "acad.lin");
                        }
                        // Set the linetype for the layer
                        ltr.LinetypeObjectId = lineType[line_type];
                    }
                }
                else
                {
                    ed.WriteMessage($"\nĐã tồn tại Layer: {layer_name}");
                }
                // Mở (Transaction) thì phải có đóng ~> Commit
                tr.Commit();
            }
            // Thực thi câu lệnh trả về kết quả `layerName`
            return layer_name;
        }
    }
}
