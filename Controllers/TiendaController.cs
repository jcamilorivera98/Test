using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models.Carrito.Clase;
using Test.Models.Carrito.Dao;
using Test.Models.Producto.Dao;

namespace Test.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Tienda()
        {
            List<tblProducto> Listproducto = new List<tblProducto>();
            List<string> ListStrings = new List<string>();
            List<string> ListaMarcas = new List<string>();
            try
            {
                Entity db = new Entity();
                Listproducto = db.tblProducto.OrderBy(d => d.Nombre).ToList();
                foreach (var item in Listproducto)
                {
                    ListStrings.Add(item.Marca);
                }

                ListaMarcas = ListStrings.Select(x => x).Distinct().ToList();


            }
            catch
            {

            }
            ViewBag.ListaMarcas = ListaMarcas;

            return View(Listproducto);
        }
        [HttpGet]
        public ActionResult ProductosCarrito(int IdUsuario)
        {
            tblCarrito carrito = new tblCarrito();
            List<vwCarritoItems> listItem = new List<vwCarritoItems>();
            carrito = CarritoDao.GetCarrito(IdUsuario);
            if (carrito != null)
            {
                listItem = CarritoDao.GetItemsCarrito(carrito.IdCarrito);

            }

            return PartialView(listItem);
        }
        public JsonResult AddItemCarrito(int IdUsuario, int IdCarrito, int Cantidad, int IdProducto)
        {
            Resultado result = new Resultado();

            try
            {
                result = CarritoDao.AddItemCarrito(IdUsuario, IdCarrito, Cantidad, IdProducto);


                return Json(result);

            }
            catch (Exception e)
            {
                result.IdItem = 0;
                result.Descripcion = "Error:" + e.ToString();
                return Json(result);

            }
        }
        public JsonResult DeleteItemsCarrito(int IdCarrito)
        {

            Resultado resultado = new Resultado();

            try
            {
                resultado = CarritoDao.VaciarCarrito(IdCarrito);


                return Json(resultado);
            }
            catch (Exception e)
            {
                return Json(resultado);

            }
        }

        public JsonResult FinalizarCompra(int IdCarrito)
        {

            Resultado resultado = new Resultado();

            try
            {
                resultado = CarritoDao.CerrarCompra(IdCarrito);


                return Json(resultado);
            }
            catch (Exception e)
            {
                return Json(resultado);

            }
        }
        // GET: api/Producto/5
        [HttpGet]
        public ActionResult GetProductosFiltrados(decimal? PrecioInicial, decimal? PrecioFinal, string Marca = null, string Nombre = null)
        {
            List<tblProducto> productos = new List<tblProducto>();
            productos = ProductoDao.GetListProductos(PrecioInicial, PrecioFinal, Marca, Nombre);
            return PartialView(productos);

        }
    }
}