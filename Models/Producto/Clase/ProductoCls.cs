using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models.Producto.Clase
{
    public class ProductoCls
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Precio { get; set; }
        public string CantStock { get; set; }
        public string Estado { get; set; }
        public string PorcentajeDescuento { get; set; }
        public string Resultado { get; set; }
    }
}