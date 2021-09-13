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
using Test.Models.Carrito.Clase;
using Test.Models.Carrito.Dao;
using Test.Models.Log.Clase;
using Test.Models.Log.Dao;

namespace Test.Controllers.Api.Carrito
{
    public class CarritoController : ApiController
    {
        private Entity db = new Entity();
        [HttpGet]
        public IHttpActionResult GetEstadoCarrito(int IdUsuario)
        {
            Resultado result = new Resultado();
          
            try
            {
                tblCarrito carrito = new tblCarrito();
                carrito = CarritoDao.GetCarrito(IdUsuario);
                result.IdItem = carrito.IdCarrito;
                result.Descripcion = "CARRITO ENCONTRADO";
                return Ok(result);
            }
            catch (Exception e)
            {
                result.IdItem = 0;
                result.Descripcion = "Error:" + e.ToString();
                return Ok(result);

            }


        }
        [HttpPut]
        public IHttpActionResult AddItemCarrito(int IdUsuario,int IdCarrito,int Cantidad,int IdProducto)
        {
            Resultado result = new Resultado();

            try
            {
                result = CarritoDao.AddItemCarrito(IdUsuario,IdCarrito,Cantidad,IdProducto);


                return Ok(result);

            }
            catch(Exception e)
            {
                result.IdItem = 0;
                result.Descripcion = "Error:" + e.ToString();
                return Ok(result);

            }


        }

        [HttpGet]
        public IHttpActionResult GetItemsCarrito(int IdCarrito)
        {
            List<vwCarritoItems> listItem = new List<vwCarritoItems>();

            try
            {
                listItem = CarritoDao.GetItemsCarrito(IdCarrito);


                return Ok(listItem);
            }
            catch (Exception e)
            {
                return Ok(listItem);

            }


        }
        [HttpDelete]
        public IHttpActionResult DeleteItemsCarrito(int IdCarrito)
        {
            Resultado resultado = new Resultado();

            try
            {
                resultado = CarritoDao.VaciarCarrito(IdCarrito);


                return Ok(resultado);
            }
            catch (Exception e)
            {
                return Ok(resultado);

            }


        }
        [HttpPut]
        public IHttpActionResult FinalizarCompra(int IdCarrito)
        {
            Resultado resultado = new Resultado();

            try
            {
                resultado = CarritoDao.CerrarCompra(IdCarrito);


                return Ok(resultado);
            }
            catch (Exception e)
            {
                return Ok(resultado);

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
