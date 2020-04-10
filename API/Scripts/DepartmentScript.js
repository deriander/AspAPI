$(document).ready(function () {
    $('#deptTable').dataTable({
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
            { "data": "CreateDate", "name": "Create Date" },
            { "data": "UpdateDate", "name": "Update Date" },
        ]
    });
    
});

/*
function loadDepartment() {
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
                html += '<td>' + Dept.CreateDate + '</td>';
                html += '<td>' + Dept.UpdateDate + '</td>';
                html += '<td><button type="button" class="btn btn-warning" id="Update" onclick="return GetById(' + Dept.Id + ')"> Edit </button> ';
                html += '<button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + Dept.Id + ')"> Delete</button></td>';
                html += '</tr>'
            })
            $('.deptbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })

}