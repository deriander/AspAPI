$(document).ready(function () {
    $.fn.DataTable.ext.errMode = 'none';
    $('#deptTable').DataTable({
        "ajax": {
            url: "/Department/LoadDepartment",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 3 },
            { "searchable": false, "targets": 3 }
        ],
        "columns": [
            { "data": "Name", "name": "Name" },
            {
                "data": "CreateDate", "render": function (data) {
                    return moment(data).format('MMMM Do YYYY, h:mm:ss a');
                }
            },
            {
                "data": "UpdateDate", "render": function (data) {
                    var notupdate = "Not update yet";
                    if (data == null) {
                        return notupdate;
                    } else {
                        return moment(data).format('MMMM Do YYYY, h:mm:ss a');
                    }
                }
            },
            {
                "data": null, "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="EditBtn" data-toggle="tooltip" data-placement="top" title="Edit" onclick="return GetById(' + row.Id + ')"><i class="mdi mdi-pencil"></i></button> <button type="button" class="btn btn-danger" id="DeleteBtn" data-toggle="tooltip" data-placement="top" title="Delete" onclick="return Delete(' + row.Id + ')"><i class="mdi mdi-delete"></i></button>';
                }
            }           
        ]
    });
});

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})



/*
function loadDataDepartment() {
    $.ajax({
        url: "/Department/LoadDepartment",
        type: "GET",
        contentType: "application/json;charset-utf-8",
        dataType: "json",
        success: function (result) {
            debugger;
            var html = '';
            $.each(result, function (key, Dept) {
                html += '<tr>';
                html += '<td>' + Dept.Name + '</td>';
                html += '<td>' + moment(Dept.CreateDate).format('DD-MM-YYYY') + '</td>';
                if (Dept.UpdateDate == null) {
                    html += '<td> Not update yet </td>';
                }
                else {
                    html += '<td>' + moment(Dept.UpdateDate).format('DD-MM-YYYY') + '</td>';
                }
                html += '<td><button type="button" class="btn btn-warning" id="EditBtn" onclick="return GetById(' + Dept.Id + ')">Edit</button>';
                html += '<button type="button" class="btn btn-danger" id="DeleteBtn" onclick="return Delete(' + Dept.Id + ')"> Delete</button></td>';
                html += '</tr>';
            })
            $('.deptbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}
*/

function ShowModal() {
    $('#myModal').modal('show');
    $('#Id').val('');
    $('#Name').val('');
    $('#UpdateBtn').hide();
    $('#SaveBtn').show();
}


function MyTableReload() {
    var rtable = $('#deptTable').DataTable({
        ajax: "data.json"
    });

    rtable.ajax.reload();
}

function Save() {
    debugger;
    var Department = new Object();
    Department.Name = $('#Name').val();  
    $.ajax({
        type: 'POST',
        url: '/Department/InsertOrEdit/',
        data: Department
    }).then((result) => {
        debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Department Added Successfully',
                timer: 5000
            }).then(function () {
                MyTableReload();
            });
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            MyTableReload();
        }
    })
}

function Edit() {
    debugger;
    var Department = new Object();
    Department.Name = $('#Name').val();
    Department.Id = $('#Id').val();
    $.ajax({
        type: 'POST',
        url: '/Department/InsertOrEdit/',
        data: Department
    }).then((result) => {
        debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Department Updated Successfully',
                timer: 5000
            }).then(function () {
                MyTableReload();
            });
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            MyTableReload();
        }
    })
}

function Delete(Id) {
    Swal.fire({
        title: "Are you sure?",
        showCanceButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: '/Department/Delete/',
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Department Deleted Successfully'
                    }).then((result) => {
                        if (result.value) {
                            MyTableReload();
                        }                       
                    });
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ShowModal();
                }
            })
        };
    });   
}



function GetById(Id) {
    $.ajax({
        url: "/Department/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.Name);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}