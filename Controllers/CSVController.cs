using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models.Producto.Clase;
using Test.Models.Producto.Dao;

namespace Test.Controllers
{
    public class CSVController : Controller
    {
        // GET: Producto
        public ActionResult CargueProductoIndex()
        {
          
            return View();
        }
        [HttpPost]
        [ActionName("CargarProductoCSV")]
        public JsonResult CargarProductoCSV()
        {
            string filePath = string.Empty;
            List<ProductoCls> lista = new List<ProductoCls>();           
          

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];


                if (file != null)
                {
                    try
                    {

                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        filePath = path + "CargueProducto_" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".csv";
                        string extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filePath);
                        DataTable dt = ConvertCSVtoDataTable(filePath);
                        lista = ProductoDao.GuardarCsv(dt);
                    }
                    catch
                    {
                        throw;

                    }

                  

                }
            }
            return Json(lista);
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }

    }
}
