const loadProdData = () => {
    fetch('/api/Employees')
        .then(response => response.json())
        .then(employees => {
            let tr = '';
            employees.forEach((employee) => {
                tr += `
                      <tr>
                        <td>${employee.name}</td>
                        <td>${employee.gender}</td>
                        <td>${employee.dob}</td>
                        <td>${employee.phone}</td>
                        <td>${employee.idnumber}</td>
                        <td>
                          <a href="../Employees/Edit?id=${employee.id}">Edit</a> |
                          <a href="../Employees/Details?id=${employee.id}">Details</a> |
                          <a href="../Employees/Delete?id=${employee.id}">Delete</a>
                        </td>
                      </tr>
                    `;
            });
            document.querySelector('#tableBody').innerHTML = tr;
        })
        .catch(error => console.log(error));
};

loadProdData();
var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
connection.start();

connection.on("LoadEmployees", function () {
    loadProdData();
})
loadProdData();