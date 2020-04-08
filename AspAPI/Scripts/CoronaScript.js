$(document).ready(function () {
    $('#coronaTable').dataTable({
        "ajax": loadDataCorona(),
        "responsive": true,
    });
    $('[data-toggle="tooltip]"').tooltip();
});

function loadDataCorona() {
        $.ajax({
            url: "/Corona/LoadCorona",
            type: "GET",
            contentType: "application/json;charset-utf-8",
            dataType: "json",
            success: function (result) {
                debugger;
                var html = '';
                $.each(result, function (key, Corona){
                    html += '<tr>';
                    html += '<td>' + Corona.id + '</td>';
                    html += '<td>' + Corona.start_Date + '</td>';
                    html += '<td>' + Corona.end_Date + '</td>';
                    html += '<td><button type="button" class="btn btn-warning" id="Update" onclick="return GetById(' + Corona.id + ')">Edit</button>';
                    html += '<button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + Corona.id + ')"> Delete</button></td>';
                    html += '</tr>'
                })
                $('.coronabody').html(html);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        })

    }