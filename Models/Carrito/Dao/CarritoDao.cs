using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test.Models.Carrito.Clase;
using Test.Models.Log.Clase;
using Test.Models.Log.Dao;

namespace Test.Models.Carrito.Dao
{
    public class CarritoDao
    {
        static Entity db = new Entity();

        public static tblCarrito GetCarrito(int IdUsuario)
        {
            var carrito = db.tblCarrito.Where(d => d.IdUsuario == IdUsuario && d.EstadoCompra == "PENDIENTE").FirstOrDefault();
            var tblcarrito = new tblCarrito();
            if (carrito == null)
            {
               
                tblcarrito.IdUsuario = IdUsuario;
                tblcarrito.FechaRegistro = DateTime.Now;
                tblcarrito.EstadoCompra = "PENDIENTE";
                tblcarrito.TotalCompra = 0;
                db.tblCarrito.Add(tblcarrito);
                db.SaveChanges();
                LogCls log = new LogCls();
                log.Operacion = "INSERCION";
                log.Modulo = "CARRITO DE COMPRAS";
                log.IdentificadorRegistro = tblcarrito.IdCarrito;
                log.NombreTablaRegistro = "tblcarrito";
                log.UsuarioRegistro = IdUsuario;
                log.Comentario = "CREACION DE CARRITO DE COMPRA";
                LogDao.GuardarLog(log);
                return tblcarrito;
            }
            else
            {
                return carrito;

            }
          
        }

        public static Resultado AddItemCarrito(int IdUsuario, int IdCarrito, int Cantidad, int IdProducto)
        {
            Resultado result = new Resultado();

            var ItemCarrito = db.tblItemCarrito.Where(d => d.IdProducto == IdProducto && d.IdCarrito == IdCarrito).FirstOrDefault();
            var producto = db.tblProducto.Where(d => d.IdProducto == IdProducto && d.CantStock >= Cantidad).FirstOrDefault();
            var carrito = db.tblCarrito.Where(d => d.IdCarrito == IdCarrito && d.EstadoCompra=="PENDIENTE").FirstOrDefault();
            if (carrito !=null)
            {
                if (ItemCarrito != null)
                {
                    Cantidad = Cantidad + ItemCarrito.Cantidad;
                    producto = db.tblProducto.Where(d => d.IdProducto == IdProducto && d.CantStock >= Cantidad).FirstOrDefault();

                    if (producto == null)
                    {

                        result.IdItem = 0;
                        result.Descripcion = "NO HAY DISPONIBILIDAD";
                    }
                    else
                    {
                        ItemCarrito.Cantidad = Cantidad;
                        db.SaveChanges();
                        result.IdItem = ItemCarrito.IdItem;
                        result.Descripcion = "ITEM AGREGADO";
                        LogCls log = new LogCls();
                        log.Operacion = "ACTUALIZACION";
                        log.Modulo = "CARRITO DE COMPRAS";
                        log.IdentificadorRegistro = ItemCarrito.IdItem;
                        log.NombreTablaRegistro = "tblItemCarrito";
                        log.UsuarioRegistro = IdUsuario;
                        log.Comentario = "ACTUALIZACIÓN DE ITEM AL CARRITO";
                        LogDao.GuardarLog(log);
                    }


                }
                else
                {
                    if (producto == null)
                    {
                        result.IdItem = 0;
                        result.Descripcion = "NO HAY DISPONIBILIDAD";
                    }
                    else
                    {
                        var tblItemCarrito = new tblItemCarrito();
                        tblItemCarrito.IdProducto = IdProducto;
                        tblItemCarrito.IdCarrito = IdCarrito;
                        tblItemCarrito.Cantidad = Cantidad;
                        tblItemCarrito.FechaRegistro = DateTime.Now;

                       


                        db.tblItemCarrito.Add(tblItemCarrito);
                        db.SaveChanges();
                        
                        result.IdItem = tblItemCarrito.IdItem;
                        result.Descripcion = "ITEM AGREGADO";
                        LogCls log = new LogCls();
                        log.Operacion = "INSERCION";
                        log.Modulo = "CARRITO DE COMPRAS";
                        log.IdentificadorRegistro = tblItemCarrito.IdItem;
                        log.NombreTablaRegistro = "tblItemCarrito";
                        log.UsuarioRegistro = IdUsuario;
                        log.Comentario = "INSERCION DE ITEM AL CARRITO";
                        LogDao.GuardarLog(log);

                    }

                }

            }
            else
            {
                result.Descripcion = "No existe carrito en estado pendiente,";
                result.IdItem = 0;

            }
            

            return result;

        }

        public static List<vwCarritoItems> GetItemsCarrito(int IdCarrito)
        {
            List<vwCarritoItems> list = new List<vwCarritoItems>();

            list = db.vwCarritoItems.Where(d => d.IdCarrito == IdCarrito).ToList();

            return list;

        }

        public static Resultado VaciarCarrito(int IdCarrito)
        {
            Resultado resultado = new Resultado();
            int contador = 0; ;
            var itemcarrito = db.tblItemCarrito.Where(d => d.IdCarrito == IdCarrito).ToList();
            var carrito = db.tblCarrito.Where(d => d.IdCarrito == IdCarrito && d.EstadoCompra=="PENDIENTE" ).FirstOrDefault();
            if (carrito !=null)
            {
                foreach (var item in itemcarrito)
                {

                    db.tblItemCarrito.Remove(item);
                    db.SaveChanges();
                    contador++;
                    LogCls log = new LogCls();
                    log.Operacion = "ELIMINACION";
                    log.Modulo = "CARRITO DE COMPRAS";
                    log.IdentificadorRegistro = item.IdItem;
                    log.NombreTablaRegistro = "tblItemCarrito";
                    log.UsuarioRegistro = carrito.IdUsuario;
                    log.Comentario = "ELIMINACION DE ITEM CARRITO DE COMPRA";
                    LogDao.GuardarLog(log);
                }

                resultado.Descripcion = "Eliminado: " + contador + " items del carrito.";
                resultado.IdItem = IdCarrito;

            }
            else
            {

                resultado.Descripcion = "No existe carrito en estado pendiente,";
                resultado.IdItem = 0;

            }
            

            return resultado;
        }

        public static Resultado CerrarCompra(int IdCarrito)
        {
            Resultado resultado = new Resultado();           
            var itemcarrito = db.tblItemCarrito.Where(d => d.IdCarrito == IdCarrito).ToList();
            var carrito = db.tblCarrito.Where(d => d.IdCarrito == IdCarrito).FirstOrDefault();
            LogCls log = new LogCls();
            if (itemcarrito.Count>=1 && carrito !=null)
            {
                foreach (var item in itemcarrito)
                {

                    var producto = db.tblProducto.Where(d => d.IdProducto == item.IdProducto).FirstOrDefault();
                    producto.CantStock = producto.CantStock - item.Cantidad;                    
                    carrito.TotalCompra = carrito.TotalCompra + ((producto.Precio * (producto.PorcentajeDescuento/100)) - producto.Precio);


                    log.Operacion = "ACTUALIZACION";
                    log.Modulo = "PRODUCTO";
                    log.IdentificadorRegistro = producto.IdProducto;
                    log.NombreTablaRegistro = "tblProducto";
                    log.UsuarioRegistro = carrito.IdUsuario;
                    log.Comentario = "ACTUALIZACION STOCK POR COMPRA";
                    LogDao.GuardarLog(log);
                }

                carrito.EstadoCompra = "FINALIZADA";
              
                log.Operacion = "ACTUALIZACION";
                log.Modulo = "CARRITO";
                log.IdentificadorRegistro = carrito.IdCarrito;
                log.NombreTablaRegistro = "tblCarrito";
                log.UsuarioRegistro = carrito.IdUsuario;
                log.Comentario = "ACTUALIZACION CARRITO POR FINALIZACIÓN DE COMPRA";
                LogDao.GuardarLog(log);
                db.SaveChanges();
                resultado.Descripcion = "Compra finalizada";
                resultado.IdItem = IdCarrito;
            }
            else
            {
                resultado.Descripcion = "Carrito vacio o no existente";
                resultado.IdItem = 0;
            }           

           
            return resultado;
        }

    }
}