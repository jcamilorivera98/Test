function ConsultarItemsCarrito() {
    // $("#ModalCarrito").modal();
    $("#divDetailCarrito").html(
        '<div class="row"><div class="col-sm-4"></div><div class="col-sm-4" style="text-align: center;"><img src="http://www.lacosox.org/sites/default/files/cargando.gif"></div><div class="col-sm-4"></div></div>');

    $.ajax({
        type: "GET",
        url: "../Tienda/ProductosCarrito",
        data: { IdUsuario: 1 },
        success: function (Object) {

            $("#divDetailCarrito").html(Object);

        },
        error: function (req, status, error) {
            Swal.fire(
                '¡Error!',
                'Se ha presentado un error ',
                'error'
            )
        }
    });
}

function ConsultarCarritoUsuario(IdProducto, IdUsuario) {
    $("#ModalCargando").modal();
    try {
        var IdCarrito = 0;
        $.ajax({
            type: "GET",
            url: "../Api/Carrito/GetEstadoCarrito",
            data: { IdUsuario: 1 },
            success: function (Object) {
                console.log(Object);
                if (Object.IdItem != 0) {
                    IdCarrito = Object.IdItem;
                    AddItemCarrito(IdProducto, IdUsuario, IdCarrito);
                }
            },
            error: function (req, status, error) {
                Swal.fire(
                    '¡Error!',
                    'Se ha presentado un error ',
                    'error'
                )
            }
        });

    } catch {
        Swal.fire(
            '¡Error!',
            'Se ha presentado un error ',
            'error'
        )
    }
   
}

function AddItemCarrito(IdProducto, IdUsuario, IdCarrito) {
    try {
        var cant = $("#txtCant_" + IdProducto).val();
        cant = parseInt(cant);
      
        $.ajax({
           
            url: "../Tienda/AddItemCarrito",
            data: { IdUsuario: IdUsuario, IdCarrito: IdCarrito, Cantidad: cant, IdProducto: IdProducto },
          type: 'POST',
            success: function (Object) {
                console.log(Object);
                if (Object.IdItem != 0) {
                    Swal.fire(
                        'Correcto',
                        'Se ha agregado correctamente',
                        'success'
                    )
                    $(".txtCant").val("1");
                    $("#ModalCargando").modal('toggle');
                } else {
                    Swal.fire(
                        'Error',
                        'No hay items disponibles para este producto en STOCK',
                        'warning'
                    )
                    $("#ModalCargando").modal('toggle');
                }
            },
            error: function (req, status, error) {
                Swal.fire(
                    'Error',
                    'Se ha presentado un error',
                    'error'
                )
                $("#ModalCargando").modal('toggle');
            }
        });

    } catch {
        Swal.fire(
            'Error',
            'Se ha presentado un error',
            'error'
        )
        $("#ModalCargando").modal('toggle');
    }
}

function LimpiarCarrito(IdCarrito) {
    try {
        $("#ModalCargando").modal();
        
        $.ajax({

            url: "../Tienda/DeleteItemsCarrito",
            data: { IdCarrito: IdCarrito},
            type: 'POST',
            success: function (Object) {
                console.log(Object);
                if (Object.IdItem != 0) {
                    Swal.fire(
                        'Correcto',
                        'Se ha limpiado correctamente',
                        'success'
                    )
                    $("#ModalCargando").modal('toggle');
                    ConsultarItemsCarrito();
                } else {
                    $("#ModalCargando").modal('toggle');
                    Swal.fire(
                        'Error',
                        'Se ha presentado  un error al limpiar el carrito',
                        'error'
                    )
                }
            },
            error: function (req, status, error) {
                $("#ModalCargando").modal('toggle');
                Swal.fire(
                    'Error',
                    'Se ha presentado un error',
                    'error'
                )
            }
        });

    } catch {
        $("#ModalCargando").modal('toggle');
        Swal.fire(
            'Error',
            'Se ha presentado un error',
            'error'
        )
    }
}

function FinalizarCompra(IdCarrito) {
    try {
        $("#ModalCargando").modal();

        $.ajax({

            url: "../Tienda/FinalizarCompra",
            data: { IdCarrito: IdCarrito },
            type: 'POST',
            success: function (Object) {
                console.log(Object);
                $("#ModalCargando").modal('toggle');

                if (Object.IdItem != 0) {
                    Swal.fire(
                        'Correcto',
                        'Se ha finalizado la compra correctamente',
                        'success'
                    )
                    ConsultarItemsCarrito();
                    ConsultarProductos();
                } else {
                    Swal.fire(
                        'Error',
                        'Se ha presentado  un error al finalizar la compra',
                        'error'
                    )
                }
            },
            error: function (req, status, error) {
                $("#ModalCargando").modal('toggle');

                Swal.fire(
                    'Error',
                    'Se ha presentado un error',
                    'error'
                )
            }
        });

    } catch {
        $("#ModalCargando").modal('toggle');

        Swal.fire(
            'Error',
            'Se ha presentado un error',
            'error'
        )
    }
}


function ConsultarProductos() {
    var txtNombre = $("#txtNombre").val();
    var cboMarca = $("#cboMarca").val();
    if (cboMarca=="null") {
        cboMarca = null;
    }
    var txtRangoInicial = $("#txtRangoInicial").val();
    var txtRangoFinal = $("#txtRangoFinal").val();
    
    if ((txtRangoInicial >= 0 && txtRangoFinal >= 0) && txtRangoFinal !="") {
        if (txtRangoFinal <= txtRangoInicial) {
            Swal.fire(
                'Error',
                'Valide que el rango inicial sea mayor que el final',
                'warning'
            )

            return;
        }
    }
    
    
    try {
        $("#divProductosStock").html(
            '<div class="row"><div class="col-sm-4"></div><div class="col-sm-4" style="text-align: center;"><img src="http://www.lacosox.org/sites/default/files/cargando.gif"></div><div class="col-sm-4"></div></div>');

        $.ajax({

            url: "../Tienda/GetProductosFiltrados",
            data: { PrecioInicial: txtRangoInicial, PrecioFinal: txtRangoFinal, Marca: cboMarca, Nombre: txtNombre},
            type: 'GET',
            success: function (Object) {
                $("#divProductosStock").html(Object);
               
            },
            error: function (req, status, error) {
                Swal.fire(
                    'Error',
                    'Se ha presentado un error',
                    'error'
                )
            }
        });

    } catch {
        Swal.fire(
            'Error',
            'Se ha presentado un error',
            'error'
        )
    }

}

function LimpiarFiltro() {

    $("#txtNombre").val("");
    $("#cboMarca").val(null);
    $("#txtRangoInicial").val("");
    $("#txtRangoFinal").val("");
    
    ConsultarProductos();
}