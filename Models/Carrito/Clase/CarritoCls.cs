using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models.Carrito.Clase
{
    public class CarritoCls
    {
        public int IdCarrito { get; set; }
        public int IdUsuario { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public string EstadoCompra { get; set; }
        public decimal TotalCompra { get; set; }
    }
}