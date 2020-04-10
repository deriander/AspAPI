$(document).ready(function () {
    $.fn.DataTable.ext.errMode = 'none';
    $('#divTable').DataTable({
        "ajax": {
            url: "/Division/LoadDivision",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 4 },
            { "searchable": false, "targets": 4 }
        ],
        "columns": [
            { "data": "DivisionName" },
            { "data": "DepartmentName" },
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


function ShowModal() {
    $('#myModal').modal('show');
    $('#Id').val('');
    $('#Name').val('');
    $('#UpdateBtn').hide();
    $('#SaveBtn').show();
}

function MyTableReload() {
    var rtable = $('#divTable').DataTable({
        ajax: "data.json"
    });

    rtable.ajax.reload();
}

var departmentsData = []
function LoadDepartment(element) {
    if (departmentsData.length == 0) {
        debugger;
        $.ajax({
            type: "GET",
            url: "/Department/LoadDepartment",
            success: function (data) {
                departmentsData = data;
                RenderDepartment(element);
            }
        })
    }
    else {
        RenderDepartment(element);
    }
}

function RenderDepartment(element) {
    debugger;
    var $e = $(element);
    $e.empty();
    $e.append($('<option/>').val('0').text('Select Department').hide());
    $.each(departmentsData, function (i, val) {
        $e.append($('<option/>').val(val.Id).text(val.Name));
    })
}
LoadDepartment($('#CBDept'));

function Save() {
    //debugger;
    var Division = new Object();
    Division.Name = $('#Name').val();
    Division.DepartmentID = $('#CBDept').val();
    $.ajax({
        type: 'POST',
        url: '/Division/InsertOrEdit/',
        data: Division
    }).then((result) => {
        //debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Division Added Successfully',
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
    //debugger;
    var Division = new Object();
    Division.Name = $('#Name').val();
    Division.Id = $('#Id').val();
    Division.DepartmentID = $('#CBDept').val();
    $.ajax({
        type: 'POST',
        url: '/Division/InsertOrEdit/',
        data: Division
    }).then((result) => {
        //debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Division Updated Successfully',
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
                url: '/Division/Delete/',
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Division Deleted Successfully'
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
        url: "/Division/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            debugger;
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.DivisionName);
            $('#CBDept').val(obj.DeptID);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}