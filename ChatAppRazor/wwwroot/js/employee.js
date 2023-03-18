$(() => {
    LoadProdData();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();

    connection.on("LoadEmployees", function () {
        LoadProdData();
    })
    LoadProdData();

    function LoadProdData() {
        var tr = '';
        $.ajax({
            url: '/api/Employees',
            method: 'GET',
            success: (result) => {
                console.log(result);    
                $.each(result, (k, v) => {
                    tr +=
                        `<tr> 
                        <td> ${v.name}</td>
                        <td> ${v.gender}</td>
                        <td> ${v.dob}</td>
                        <td> ${v.phone}</td>
                        <td> ${v.idnumber}</td>
                        <td>
                            <a href='../Employees/Edit?id=${v.id}'>Edit</a> |
                            <a href='../Employees/Details?id=${v.id}'>Details</a> |
                            <a href='../Employees/Delete?id=${v.id}'>Delete</a> |
                        </td>
                    </tr>`
                })
                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
})