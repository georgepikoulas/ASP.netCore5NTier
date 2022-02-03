var dataTable;

$(document).ready(function () {
    $("#myTable").Datatable();
});


function(url) {
    dataTable = $("#tblData").dataTable({
        "ajax": {
            "url": "/inquiry/" + url
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "fullname", "width": "15%" },
            { "data": "phonenumber", "width": "15%" },
            { "data": "email", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href=""/InquiryDetails/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i>
                                </a>
                            </div>
                            `;
                },
                "width": "5%"
            },

        ]
    })
}