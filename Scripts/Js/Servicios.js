
$(document).ready(function () {
    $('#cboServicios').change(function () {
        var cboServicios = $("#cboServicios").val();
        var peticion = "";
        var descripcion = "";
        if (cboServicios == 'Consulta de productos Stock') {
            peticion = "../api/Producto?PrecioInicial&PrecioFinal&Marca&Nombre";
            descripcion = "Servicio que permita consultar de manera paginada los productos existentes"
                + "en la base de datos.El servicio debe soportar la consulta de productos por" +
                "los siguientes criterios: Por coincidencia del nombre completo o parte de él, Por rango de precios, Por marca.";
        } else if (cboServicios == "Agregar producto al carrito") {
            peticion = "../api/Carrito?IdUsuario=1&IdCarrito=1&Cantidad=1&IdProducto=2";
            descripcion = "Servicio para agregar un producto al carrito de compras. El servicio debe"
            +"validar que haya existencias suficientes del producto antes de ser agregado"+
            "al carrito.";
        }
        else if (cboServicios == "Consultar items del carrito") {
            peticion = "../api/Carrito?IdCarrito=1";
            descripcion = "Servicio para consultar los productos agregados en el carrito de compras.";
        } else if (cboServicios == "Vaciar carrito") {
            peticion = "../api/Carrito?IdCarrito=1";
            descripcion = "Servicio para vaciar el carrito de compras.";
        }else if (cboServicios == "Finalizar la compra") {
            peticion = "../api/Carrito?IdCarrito=1";
            descripcion = "Servicio que permita finalizar la compra de los productos existentes en el"
           +" carrito de compras afectando de manera oficial las existencias de los"
            + "productos en la base de datos.";
        } else if (cboServicios == "Consultar carrito") {
            peticion = "../api/Carrito?IdUsuario=1";
            descripcion = "Consulta carrito por usuario registrado, en caso de no tener uno disponible lo crea y retorna el numero creado";
        }


        $("#txtPeticion").val(peticion);
        $("#divDescripcionServicio").html(descripcion);
        
    });
});
