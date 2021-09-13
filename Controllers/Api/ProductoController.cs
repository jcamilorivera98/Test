using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Test;
using Test.Models.Producto.Dao;

namespace Test.Controllers.Api
{
    public class ProductoController : ApiController
    {
        private Entity db = new Entity();

        // GET: api/Producto
        public IQueryable<tblProducto> GettblProducto()
        {
            return db.tblProducto;
        }

        // GET: api/Producto/5
        [HttpGet]
        public IHttpActionResult GetProductos(decimal ? PrecioInicial, decimal ? PrecioFinal, string  Marca =null, string Nombre = null)
        {

            
            try
            {
                List<tblProducto> productos = new List<tblProducto>();
                productos = ProductoDao.GetListProductos(PrecioInicial, PrecioFinal, Marca, Nombre);
                if (productos == null || productos.Count==0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(productos);
                }
               


            }
            catch
            {
               
                return NotFound();
                

            }
            
            
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}