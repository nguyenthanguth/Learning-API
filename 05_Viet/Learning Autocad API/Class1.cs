using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using AutoCad
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows.Data;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace Learning_Autocad_API
{
    public class Class1
    {
        [CommandMethod("HELLO")]
        public void cmdHELLO()
        {
            // Đi tới bản vẽ hiện hành
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // In ra màn hình console Autocad bằng hàm `WriteMessage`
            ed.WriteMessage("Hello World");

            // Tạo một đường line trên Autocad
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // Create a line that starts at 0,0,0 and ends at 1000,1000,0
                Line line = new Line(new Point3d(0, 0, 0), new Point3d(1000, 1000, 0));

                // Add the new object to the block table record and the transaction
                btr.AppendEntity(line);
                tr.AddNewlyCreatedDBObject(line, true);

                // Save the new object to the database
                tr.Commit();
            }

            // Tạo một Layer trên AutoCad
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;
                LayerTableRecord ltr = new LayerTableRecord();

                ltr.Name = "SlabLayout";
                ltr.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(Autodesk.AutoCAD.Colors.ColorMethod.ByAci, 2);

                lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);

                tr.Commit();
            }

            // Tạo TextStyle trên Autocad
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                TextStyleTable tst = tr.GetObject(db.TextStyleTableId, OpenMode.ForWrite) as TextStyleTable;
                TextStyleTableRecord tstr = new TextStyleTableRecord();

                tstr.Name = "TCVN";
                // tstr.FileName = "TCVN 7284";
                tstr.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor("TCVN 7284", false, false, 0, 0);

                tst.Add(tstr);
                tr.AddNewlyCreatedDBObject(tstr, true);

                tr.Commit();
            }

            // Tạo DimStyle trên AutoCad
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DimStyleTable dst = tr.GetObject(db.DimStyleTableId, OpenMode.ForWrite) as DimStyleTable;
                DimStyleTableRecord dstr = new DimStyleTableRecord();

                dstr.Name = "1-100";

                // Lines
                dstr.Dimdli = 3.75;
                dstr.Dimexe = 1.25;
                dstr.Dimexo = 0.625;

                // Symbols and Arrows
                dstr.Dimasz = 2.5;

                // Text
                dstr.Dimtxt = 2.5;
                dstr.Dimgap = 0.625;

                // Fit
                dstr.Dimscale = 100;
                
                dst.Add(dstr);
                tr.AddNewlyCreatedDBObject(dstr, true);

                tr.Commit();
            }
        }
    }
}
