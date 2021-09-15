
$(document).ready(function () {
    $('#InputFile').change(function () {
        var fileName = this.files[0].name;

        // recuperamos la extensión del archivo
        var ext = fileName.split('.').pop();

        // Convertimos en minúscula porque
        // la extensión del archivo puede estar en mayúscula
        ext = ext.toLowerCase();

        // console.log(ext);
        switch (ext) {
            case 'csv':
            case 'CSV':
                break;
            default:
                Swal.fire({
                    title: '¡Error!',
                    text: 'El archivo debe ser de extension CSV',
                    icon: 'error',
                    confirmButtonText: 'Cool'
                });

                this.value = ''; // reset del valor
                this.files[0].name = '';
        }


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
        '<div class="row"><div class="col-sm-4"></div><div class="col-sm-4" style="text-align: center;"><img src="http://www.lacosox.org/sites/default/files/cargando.gif"></div><div class="col-sm-4"></div></div>');

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