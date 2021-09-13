using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Test.Models.Log.Clase;
using Test.Models.Log.Dao;
using Test.Models.Producto.Clase;
namespace Test.Models.Producto.Dao
{
    public class ProductoDao
    {
        public static Entity db = new Entity();
        public static List<ProductoCls> GuardarCsv(DataTable dt)
        {
            //LogDao logdao = new LogDao();
            List<ProductoCls> lista = new List<ProductoCls>();
            foreach (DataRow row in dt.Rows)
            {
                LogCls log = new LogCls();
                decimal dec = 0;
                ProductoCls clase = new ProductoCls();                
                clase.Nombre = ((String.IsNullOrEmpty(row["nombre"].ToString()) ? "" : row["nombre"].ToString())).Trim();
                clase.Marca = ((String.IsNullOrEmpty(row["marca"].ToString()) ? "" : row["marca"].ToString())).Trim();
                clase.Precio = ((String.IsNullOrEmpty(row["precio"].ToString()) ? "" : row["precio"].ToString())).Trim();
                clase.CantStock = ((String.IsNullOrEmpty(row["cantidad en stock"].ToString()) ? "" : row["cantidad en stock"].ToString())).Trim();
                clase.Estado = ((String.IsNullOrEmpty(row["estado"].ToString()) ? "" : row["estado"].ToString())).Trim();
                clase.PorcentajeDescuento = ((String.IsNullOrEmpty(row["porcentaje descuento"].ToString()) ? "" : row["porcentaje descuento"].ToString())).Trim();
                try
                {
                    if (!string.IsNullOrEmpty(clase.Nombre)&&
                        !string.IsNullOrEmpty(clase.Marca) &&
                        !string.IsNullOrEmpty(clase.Precio) &&
                        !string.IsNullOrEmpty(clase.CantStock) &&
                        !string.IsNullOrEmpty(clase.Estado) &&
                        !string.IsNullOrEmpty(clase.PorcentajeDescuento)
                        )
                    {
                        using (Entity db = new Entity())
                        {
                            var Producto = new tblProducto();
                            Producto.Nombre = clase.Nombre;
                            Producto.Marca = clase.Marca;
                            Producto.Precio = decimal.Parse(clase.Precio);
                            Producto.CantStock = int.Parse(clase.CantStock);
                            Producto.Estado = clase.Estado;
                            Producto.UsuarioRegistro =1;
                            Producto.UsuarioActualizacion =1;
                            Producto.FechaActualizacion =DateTime.Now;
                            Producto.FechaRegistro =DateTime.Now;
                            Producto.PorcentajeDescuento = int.Parse(clase.PorcentajeDescuento);
                            

                            var temp = db.tblProducto.FirstOrDefault(t=>t.Nombre== Producto.Nombre);
                            if (temp==null)
                            {
                                db.tblProducto.Add(Producto);
                                db.SaveChanges(); 
                                clase.Resultado = "Insertado";
                                log.Operacion = "INSERCION";
                                log.Modulo = "CARGUE CSV";
                                log.IdentificadorRegistro = Producto.IdProducto;
                                log.NombreTablaRegistro = "tblProducto";
                                log.UsuarioRegistro = 1;
                                log.Comentario = "INSERCION DE PRODUCTOS DESDE CSV";

                            }
                            else
                            {
                                temp.Nombre = clase.Nombre;
                                temp.Marca = clase.Marca;
                                temp.Precio = decimal.Parse(clase.Precio);
                                temp.CantStock = int.Parse(clase.CantStock);
                                temp.Estado = clase.Estado;
                                temp.PorcentajeDescuento = int.Parse(clase.PorcentajeDescuento);
                                temp.UsuarioActualizacion = 1;
                                temp.FechaActualizacion = DateTime.Now;
                                
                                db.SaveChanges();
                                clase.Resultado = "Actualizado";
                                
                                log.Operacion = "ACTUALIZACION";
                                log.Modulo = "CARGUE CSV";
                                log.IdentificadorRegistro = temp.IdProducto;
                                log.NombreTablaRegistro = "tblProducto";
                                log.UsuarioRegistro = 1;
                                log.Comentario = "ACTUALIZACION DE PRODUCTOS DESDE CSV";
                            }

                            LogDao.GuardarLog(log);



                        }
                        if (!string.IsNullOrEmpty(clase.Precio))
                        {
                            if (decimal.TryParse(clase.Precio, out dec))
                            {
                                clase.Precio = dec.ToString("N0");
                            }
                            

                        }
                       
                    }
                    else
                    {


                        clase.Resultado = "Error: Campo vacio";
                    }
                   
                }
                catch (Exception e)
                {
                    clase.Resultado = "Error: " + e.ToString(); ;
                }
                lista.Add(clase);
            }
           
            return lista;
        }

        public static List<tblProducto> GetListProductos(decimal? PrecioInicial, decimal? PrecioFinal, string Marca = null, string Nombre = null)
        {
            var productos = db.tblProducto.ToList();

            if (PrecioInicial <= PrecioFinal && PrecioInicial > 0)
            {
                productos = db.tblProducto.Where(d =>
                  (d.Nombre.Contains(Nombre) || Nombre == null) &&
                  (d.Marca.Contains(Marca) || Marca == null) &&
                 (d.Precio >= PrecioInicial && d.Precio <= PrecioFinal)
                  ).OrderBy(d => d.Nombre).ToList();
            }
            else
            {
                productos = db.tblProducto.Where(d =>
                  (d.Nombre.Contains(Nombre) || Nombre == null) &&
                  (d.Marca.Contains(Marca) || Marca == null)
                  ).OrderBy(d => d.Nombre).ToList();
            }

            return productos;

        }
    }
}