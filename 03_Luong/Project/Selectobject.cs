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

            foreach (ObjectId ob in ss.GetObjectIds())
            {
                if (ob.ObjectClass.Name == "AcDbCircle")
                {
                    using(Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        Circle cc = tr.GetObject(ob,OpenMode.ForRead) as Circle;
                        ed.WriteMessage("\nRadius la:" + cc.Radius);
                    }
                }
            }
        }
    }
}
