$(document).ready(function () {
    $('#TB_Department').DataTable({
        "ajax": {
            url: "http://localhost:8082/api/Department",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",
            headers: { "Authorization": "Bearer " + sessionStorage.getItem("key") }            
        },       
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "name" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return '<button class="btn btn-warning " data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return getbyID(' + row.id + ')"><i class="fa fa-pen"></i></button >' + '&nbsp;' +
                        '<button class="btn btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')"><i class="fa fa-trash"></i></button >'
                }
            }
            ]
    })
    
})

function ClearScreen() {
    $('#ID').val('');
    $('#Name').val('');
    $('#Update').hide();
    $('#Save').show();
}

function getbyID(ID) {
    //debugger;
    $.ajax({
        url: "http://localhost:8082/api/Department/" + ID,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            var obj = result.data; //data yg dapet dr id
            $('#Id').val(obj.id); //ngambil data dr api
            $('#Name').val(obj.name);
            $('#Modal').modal('show');
            $('#Save').hide();
            $('#Update').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
        })
}

function Save() {  
    var Department = new Object(); //bikin objek baru
    Department.Name = $('#Name').val(); //value dari database
    $.ajax({
        type: 'POST',
        url: 'http://localhost:8082/api/Department',
        data: JSON.stringify(Department), //ngirim data ke api
        contentType: "application/json; charset=utf-8"
    }).then((result) => {
        debugger;
        if (result.status == result.status == 201 || result.status == 204 || result.status == 200) {
            //alert("Data Berhasil Dimasukkan");
            Swal.fire({
                icon: 'success',
                title: 'Berhasil',
                text: 'Data berhasil dimasukkan',
                showConfirmButton: false,
                timer: 1500
            })
            $('#TB_Department').DataTable().ajax.reload();
        }
        else {
            Swal.fire(
                'Error!',
                result.message,
                'error'
            )
        }
    })
}


function Delete(ID) {
    //debugger;
    Swal.fire({
        title: 'Kamu yakin?',
        text: "Anda tidak akan bisa mengembalikannya jika memilih Ya!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Ya',
        cancelButtonText:'Tidak'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "http://localhost:8082/api/Department/" + ID,
                type: "DELETE",
                dataType: "json",
            }).then((result) => {
                debugger;
                if (result.status == 200) {
                    Swal.fire(
                        'Berhasil',
                        'Data sudah dihapus.',
                        'success'
                    )
                    $('#TB_Department').DataTable().ajax.reload();
                }
                else {
                    Swal.fire(
                        'Error!',
                        result.message,
                        'error'
                    )
                }
            });
        }
    })
}

function Update() {
    //debugger;
    var Department = new Object();
    Department.Id = $('#Id').val();
    Department.Name = $('#Name').val();
    $.ajax({
        url: 'http://localhost:8082/api/Department/',
        type: 'PUT',
        data: JSON.stringify(Department),
        contentType: "application/json; charset=utf-8",

    }).then((result) => {
        debugger;
        if (result.status == 200) {
            //alert("Data Berhasil Diperbaharui");
            //table.ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'Berhasil',
                text: 'Data berhasil diupdate',
                showConfirmButton: false,
                timer: 1500
            })
            $('#Modal').modal('hide');
            $('#TB_Department').DataTable().ajax.reload();
        }
        else {
            Swal.fire(
                'Error!',
                result.message,
                'error'
            )
            table.ajax.reload();
        }
    });
}



