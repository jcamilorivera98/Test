
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

function CargarProductos() {
    if ($("#InputFile").val() == '') {
        Swal.fire(
            '¡Error!',
            'Seleccione archivo',
            'Error'
        )
        return false;
    }


    $("#DivResult").html(
        '<div class="row"><div class="col-sm-4"></div><div class="col-sm-4"><img src="http://www.lacosox.org/sites/default/files/cargando.gif"></div><div class="col-sm-4"></div></div>');

    var formData = new FormData();

    var file = document.getElementById("InputFile").files[0];
    formData.delete("MyFile", file);
    formData.append("MyFile", file);



    $("#InputFile").val("");

    $.ajax({
        type: "POST",
        url: '/CSV/CargarProductoCSV',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        cache: false,
        success: function (response) {
            Swal.fire(
                '¡Correcto!',
                'Archivo cargado',
                'success'
            )
            var r = "OK";
            var myhtml = '';

            var resultado = "ok";
            //myhtml = '<div class="alert alert-success">' + response.responseMessage + '</div>';
            var ca = "";
            var lista = response;
            if (lista.length > 0) {
                ca += " <table id='table3' style='max-width: 759px;' class='display table compact table-striped table-bordered table-hover dtVerticalScrollExample2'>";
                ca += "    <thead>";
                ca += "        <tr>";
                ca += "            <th> Nombre </th>";
                ca += "            <th> Marca </th>";
                ca += "            <th> Precio </th>";
                ca += "            <th> Cant stock </th>";
                ca += "            <th> Estado </th>";
                ca += "            <th> Descuento % </th>";
                ca += "            <th> Resultado </th>";

                ca += "        </tr>";
                ca += "    </thead>";
                ca += "    <tbody>";
                for (var i = 0; i < lista.length; i++) {


                    ca += "        <tr>";
                    ca += "            <td>" + lista[i].Nombre + "</td>";
                    ca += "            <td>" + lista[i].Marca + "</td>";
                    ca += "            <td>" + lista[i].Precio + "</td>";
                    ca += "            <td>" + lista[i].CantStock + "</td>";
                    ca += "            <td>" + lista[i].Estado + "</td>";
                    ca += "            <td>" + lista[i].PorcentajeDescuento + "</td>";
                    ca += "            <td>" + lista[i].Resultado + "</td>";

                    ca += "        </tr>";
                }
                ca += "    </tbody>";
                ca += "</table>";
            }
            $("#DivResult").html(ca);
            $('#table3').DataTable({

            });



        },
        error: function (error) {


            Swal.fire(
                '¡Error!',
                'Archivo no cargado ' + error,
                'Error'
            )
        }
    });

    $("#InputFile").val("");






}