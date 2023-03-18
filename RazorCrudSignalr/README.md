- B1: Tạo project razor nhớ là `.NET 6.0` (Cái này trường đã tạo nên không cần tạo lại)
- B2: Tải thư viện cần thiết: 
    - **EntityFrameworkCore.Design** và **EntityFrameworkCore.SqlServer**. 
    - Khi tải xong nhớ save và **ReBuild** lại không kỳ sau học lại (Trường cũng đã cài không cài lại).
    - Chuột phải ở **Project (Q2)** chọn **Rebuild**

- B3: Mở terminal lên nhớ đi đến project của mình chứ không phải solution:
    - ví dụ: 
        ```bash
        cd Q2
        ```
    - đảm bảng đang ở thư mục:
        ```bash
        PS C:\Users\giaki\Desktop\PRN221_PE_GivenSolution\PRN221_PE_GivenSolution\Q2>
        ```
    - Hoặc có thể chuột phải vào project Q2: chọn open terminal ở dưới
    - Sau khi mở terminal lên chạy dòng lệnh sau để generator sql vào models:
        ```cmd
        dotnet ef dbcontext scaffold "Server=HAGIAKINH;database=PRN221_Spr22;Integrated security=true;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -o Models
        ```
- B4: Sau khi generator models thành công Vào `appsettings.json` để thêm config sql:
    ```json
    "ConnectionStrings": {
        "SqlConnection": "Server=HAGIAKINH;database=PRN221_Spr22;Integrated security=true;TrustServerCertificate=true"
    }
    ```

    * **Lưu ý**: Nhớ đảm bảo `applications.json` phải được **coppy if never** hoặc **always** nên để **always** cho chắc.
    Để check thì chuột phải ở `appsettings.json` chọn `Properties` ở dưới cùng

    Sau khi thêm vào `appsettings.json` thì **Rebuild** lại lần nữa cho chắc.

- B5: Vào `PRN221_Spr22Context.cs` ở folder Models ở hàm OnConfiguring xóa bỏ các dòng ở body. Thay thế bằng dòng này. "method này OnConfiguring(DbContextOptionsBuilder optionsBuilder)"

    ```cSharp
    if (!optionsBuilder.IsConfigured)
    {
      var conf = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json").Build();
      optionsBuilder.UseSqlServer(conf.GetConnectionString("SqlConnection")); 
    }	
    ```

    Lưu ý: **SqlConnection** nó ở trong `appsettings.json` nên trường có thay đổi bằng thằng khác thì nhớ thay ở đây và trong `appsettings.json`

- B6: Vào `Programs.cs` nơi mà cài đặt config ứng dụng Razor page:
    - Thêm dòng này vào ở phía trên var `app = builder.Build();` Nó sẽ có hình dạng sau đây. Nhớ thêm đúng thứ tự không sai lại không chạy được.

        ```cSharp
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<PRN221_Spr22Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
        builder.Services.AddScoped<PRN221_Spr22Context>();

        var app = builder.Build();
        ```

    **Rebuild** lại lần nữa cho chắc cú

- B7: Phần `configs` ứng dụng đã xong. Bây giờ nếu trường là sử dụng `Signalr` Thì sẽ thực hiện tiếp bước này. Còn không thì bỏ qua `B7` và `B8`.

    - Tạo 1 folder Hubs. Cùng cấp với `Programs.cs`, `Model.cs` đại lại là chuột phải ở **Project (Q2)** xong tạo folder chứ không để nó nằm lung tung ở nơi khác.

    - Tạo 1 class là SignalrServer.cs trong folder Hubs
    `Hubs/SignalrServer.cs`

    - Trong thư class `SignalrServer.cs` cho nó kế thừa interface `Hub`

        ```cSharp
        using Microsoft.AspNetCore.SignalR;

        namespace SignalRCrud.Hubs
        {
            public class SignalrServer : Hub
            {
            }
        }
        ```

    - Để như thế và không cần làm gì thêm.

- B8: Vào `Startup.cs` để cấu hình `Signalr` cho ứng dụng Razor page. Theo thứ tự như sau

    ```cSharp
    // Add services to the container.
    builder.Services.AddRazorPages();
    
    builder.Services.AddSignalR();
    
    builder.Services.AddDbContext<PRN221_Spr22Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
    builder.Services.AddScoped<PRN221_Spr22Context>();

    var app = builder.Build();

    ....

    app.UseAuthorization();
    app.MapHub<SignalrServer>("/signalrServer");

    app.Run();
    ```
    Chúng ta sẽ sử dụng  `builder.Services.AddSignalR()` để thêm `Signalr` và ứng dụng.

    `app.MapHub<SignalrServer>("/signalrServer");` khi config như này thì chúng ta sẽ connect đến server sẽ có đường dẫn là `/signalrServer`. Sau đó sẽ có 1 file `signalr.js` được tạo ra trong folder `wwwroot` của project. Nó sẽ được sử dụng để connect đến server.

    Thế là chúng ta đã config xong 1 Signalr để thực hiện **Crud** bằng `Signalr`
    
    **Giải thích**: Việc connect ở đây chúng ra sẽ connect ở `wwwroot/js/site.js` và `wwwroot/index.html` nhưng trường đã config sẵn nên mình không cần làm gì thêm. Hoặc thêm vào html mà cần connect đến server signalrs. Khi connect đến `Siganlr` thì sẽ có 1 vòng lặp để lăng nghe `event` được gửi từ nơi khác đến. Ví dụ như ở đây là `SignalrServer.cs` sẽ gửi 1 `event` đến `site.js` để thực hiện `Crud` và `site.js` sẽ gửi 1 `event` đến `index.html` để hiển thị lên giao diện. Việc gửi `event` thì là do mình gửi. 

    - Ví dụ: Khi mình mở 2 trình duyệt: 1 là vào màn create employee `http://localhost/Employee/Create` 2 là vào màn list `http://localhost/Employee` 
    - Khi mình thêm 1 bản ghi ở trình duyệt 1 thì trình duyệt 2 sẽ tự động cập nhật lại list. Vì trình duyệt 2 đang lắng nghe sự kiện `event` được gửi từ trình duyệt 1. Và ngược lại. *(Event này mình sẽ gửi đi ở trong hàm save. Cái này là mình code bằng cơm nhé. Không phải magic gì ở đây cả)*
    - Như đã nói việc lăng nghe sự kiện mình lắng nghe ở html và js màn hình cần lăng nghe. Cụ thể ở đây là `Employee.cshtml` hoặc `site.js` và import `site.js` vào ` Emplyee.cshtml`. Mình connect bằng `Javascript`
    - Oke. Vậy là ý tưởng sẽ như thế bây giờ thực hiện tiếp `B9`

- B9: Sau khi config xong rồi thì mình thêm thư viện `Js` và `Boostrap` nếu đề bài trường nhìn đẹp đẹp thì thêm boostrap cho chắc.
    - Thêm thư viện js `Signalr` để connect với Server Signalr phía trình duyệt(Client)
        - Ở đây trường thêm rồi nên không cần thêm. Check lại ở folder `wwwroot/lib/microsoft/signalr`
        - Tí nữa import ở `_Layout.cshtml` cho toàn bộ folder. Vì nó là thư viện chung cho toàn bộ folder. Nên mình import ở đây. Nếu import ở mỗi file thì mỗi file sẽ phải import lại thư viện này. Nên mình import ở đây cho tiện.
        - Nếu chưa thêm thì thực hiện các bước sau:
            - B1: Chuột phải ở folder `lib` trong `wwwroot`
            - B2: Add -> Client-Side Library -> chọn Provider là unpkg -> gõ ở ô input signalr -> signalr
            - B3: Chọn Choose Specific file -> chọn `dist` 
            - b4: Chọn Install
    - Thêm thư viện bootstrap: (Để ý giao diện đẹp thì mới thêm nhé)
        - Hoặc làm theo các bước sau đây:
            - B1: Chuột phải ở folder `lib` trong `wwwroot`
            - B2: Add -> Client-Side Library -> chọn Provider là unpkg -> gõ ở ô input boostrap -> Bootstrap -> 5.2.3
            - B3: Chọn Choose Specific file -> chọn `dist` 
            - b4: Chọn Install
    - Thêm thư viện Jquey, jquery-validation, jquery-validation-unobtrusive tương tự bootstrap
    - Có thẻ coppy ở project khác vào
    - **Lưu ý**
        - Config Shared (`_Layout.cshtml` , `_ValidationScriptsPartial.cshtml`), `_ViewImports.cshtml`, `_ViewStart.cshtml` coppy mấy thằng này từ project khác vào folder Pages nếu được sử dụng Boostrap
            - Khi coppy lưu ý sửa lại file: `_ViewImports.cshtml` như sau: Sử dụng import đúng tên Project của mình `RazorCrudSignalr`
            ```cSharp
            @using RazorCrudSignalr
            @namespace RazorCrudSignalr.Pages
            @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
            ```
            - Sửa cả `_Layout.cshtml` cho đúng tên project của mình
        - Coppy luôn `wwwroot/css` Phục vụ viết css ở trong file này
        - Coppy luôn `wwwroot/js/site.js` cho đủ bộ. Phục vụ viết javascript ở trong file này
        - Sau khi coppy nhớ `Rebuild` lại nhé
- B10: Sau khi import đây đủ thì bây giờ tạo Razor page
    - Tạo folder `Pages`
    - Tạo thư mục `Employees`. Nếu là thư mục `Employees` trong thư mục `Pages` thì sẽ có mặc định `uri` là : `https://localhost:7129/Employees`
    - Tạo file `Index.cshtml` trong thư mục `Employees`: Cái này là default khi đường dẫn là `https://localhost:7129/Employees` thì sẽ hiển thị file `Index.cshtml` trong thư mục `Employees`. 
        - Còn nếu file `Create.cshtml` thì có đường dẫn là `https://localhost:7129/Employees/Create` thì sẽ hiển thị file `Create.cshtml` trong thư mục `Employees`
    - Tạo file bằng cách:
        - B1: Chuột phải ở folder `Pages/Employees` chọn Add -> chọn `Razor Pages` ở phí trên cùng
        - B2: 
            - Chọn `Razor Page` -> chọn `Empty` -> chọn `Add` Nếu là tạo ra 1 `Index.cshtml` rỗng
            - Chọn `Razor Page` -> chọn `Entity FrameWork` -> chọn `Add` Nếu là tạo ra 1 `Index.cshtml` theo ý mình CRUD (List hoặc Add Hoặc Edit Hoặc Delete).
                - Razor Page Name: Index
                - Template: **List/Create/Edit/Delete** (Index thường ng ta sẽ chọn List)
                - Model class: Employee (Data.Models)
                - Data context class: PRN221_Spr22Context (Data)
            - Chọn `Razor Page` -> chọn `Entity FrameWork (CRUD)` -> chọn `Add` Nếu là tạo ra toàn bộ CRUD.
                - Model class: Employee (Data.Models)
                - Data context class: PRN221_Spr22Context (Data)
    - ở đây ta tạo CRUD luôn cho dễ nhé.

- B11: Sau khi tạo xong thì mình sẽ tạo 1 `Controller` để xử lý các `Event` mình lắng nghe ở trình duyệt.
    - Tạo folder `Controllers`
	    - Tạo file `EmployeeController.cs` trong thư mục `Controllers`
	    - Tạo file bằng cách:
		    - B1: Chuột phải ở folder `Controllers` chọn Add -> chọn `Controller` ở phí trên cùng
		    - B2: 
                - Chọn `Controller` -> chọn `MVC Controller - Empty` -> chọn `Add`
                - Chọn `Controller` -> chọn `MVC Controller with views, using Entity Framework` -> chọn Model `Employee` -> chọn `Add`
	            - Ở đây mình chọn `MVC Controller - Empty` vì mình sẽ tạo `Action` bằng tay.
        - Sau khi tạo xong thì mình sẽ thêm 1 `Action` để xử lý `Event` mình lắng nghe ở trình duyệt. Thằng này là api nên `Route` mình để `api/Employees`
		    - B1: Chuột phải ở file `EmployeeController.cs` chọn Add -> chọn `Action`
		    - B2: Chọn `Action` -> chọn `Empty` -> chọn `Add`
		    - B3: Đặt tên cho `Action` là `GetEmployees`. Xem ví dụ dưới đây
                ```cSharp
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Threading.Tasks;
                using Microsoft.AspNetCore.Mvc;
                using Microsoft.AspNetCore.Mvc.Rendering;
                using Microsoft.EntityFrameworkCore;
                using ChatAppRazor.Models;

                namespace ChatAppRazor.Controllers
                {
                    [Produces("application/json")] // vì là api nên giao tiếp qua json
                    [Route("api/Employees")] // bỏ đi cũng được vì đã định nghĩa ở program.cs hoặc xài cài này cũng được [Route("api/[controller]")]
                    public class EmployeesController : Controller
                    {
                        private readonly PRN221_Spr22Context _context;

                        public EmployeesController(PRN221_Spr22Context context)
                        {
                            _context = context;
                        }

                        // Get list, url lấy trực tiếp từ route nếu không custom -> api/Employees
                        [HttpGet]
                        public async Task<IActionResult> GetEmployees()
                        {
                            var res = await _context.Employees.ToListAsync();
                            return _context.Employees != null ? Ok(res) : NotFound();
                        }

                        // Get id -> api/Employees/1
                        [HttpGet("{id}")]
                        public async Task<IActionResult> GetEmployee([FromRoute] int id)
                        {
                            if (!ModelState.IsValid)
                            {
                                return BadRequest(ModelState);
                            }

                            var order = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);

                            if (order == null)
                            {
                                return NotFound();
                            }

                            return Ok(order);
                        }
                    }
                }

                ```
    
    - Add config `EmployeesController` vào `Program.cs` theo thứ tự dưới đây
        ```cSharp
        app.MapRazorPages();
        
        // chính nó ở đây
        app.MapControllerRoute(
            name: "Employee",
            pattern: "{controller=Employee}"
        )

        app.MapHub<SignalrServer>("/signalrServer");
        app.Run();
        ```

        - Đây là toàn bộ `Program.cs`
        ```cSharp
        using Microsoft.EntityFrameworkCore;
        using RazorCrudSignalr.Models;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<PRN221_Spr22Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
        builder.Services.AddScoped<PRN221_Spr22Context>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        // 1. Cách dưới này cũng được. Nếu làm cách dưới này thì phải thêm Route[] ở controller cho nó.
        app.MapControllerRoute(
            name: "Employee",
            pattern: "{controller=Employee}"
        );

        //// 2. Cách dưới này cũng được. Nếu làm cách dưới này thì không thêm Route[] ở controller cho nó. vì mình đã định nghĩa parten ở đây r
        //app.MapControllerRoute(
        //    name: "Employees",
        //    pattern: "api/Employees",
        //    defaults: new { controller = "Employees", action = "GetEmployees" }
        //);

        app.Run();

        ```
    - Đến bước này là ta đã config xong api để sử dụng `ajax` hoặc `fetch` trong javascript để lấy dữ liệu `Employees` cập nhật vào list ở `index.cshtml`
        - Nếu Trường không cho xài jquery thì mình sử dụng `fetch`
        - Nếu trường cho xài jquery thì xài `ajax`

- B12: Bây giờ ta sẽ chỉnh thêm và chỉnh sửa file site.js ở thư mục `wwwroot/js/site.js` không có thì ta thêm vào. Mục đích để connect `Signalr` cụ thể ở đây là `/signalrServer` đã được nói đến ở `B7` và `B8` để nhận `Event` và thực hiện cập nhật lại danh sách employees.
    - Cụ thể ở đây ta sẽ connect vào server Signalr `/signalrServer` 
    - Connect bằng `new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();`
    - Filte `site.js` ta viết như sau:
        - Jquery:
            ```javascript
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
            ```
       - Fetch:
           ```javascript
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
           ```
    - Có 2 loại dưới đây dùng loại nào cũng được. Nên xài fetch nếu ko có jquery

- B13: Bây giờ ta chỉnh sửa file `Index.cshtml` thêm id cho table `#tableBody`.
    ```html
    @page
    @model RazorCrudSignalr.Pages.Employees.IndexModel

    @{
        ViewData["Title"] = "Index";
    }

    <h1>Index</h1>

    <p>
        <a asp-page="Create">Create New</a>
    </p>
    <table class="table">
        <thead>
            <tr>
                <th>
                    @Html.DisplayNameFor(model => model.Employee[0].Name)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Employee[0].Gender)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Employee[0].Dob)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Employee[0].Phone)
                </th>
                <th>
                    @Html.DisplayNameFor(model => model.Employee[0].Idnumber)
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody id="tableBody">
    @foreach (var item in Model.Employee) {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => item.Name)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Gender)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Dob)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Phone)
                </td>
                <td>
                    @Html.DisplayFor(modelItem => item.Idnumber)
                </td>
                <td>
                    <a asp-page="./Edit" asp-route-id="@item.Id">Edit</a> |
                    <a asp-page="./Details" asp-route-id="@item.Id">Details</a> |
                    <a asp-page="./Delete" asp-route-id="@item.Id">Delete</a>
                </td>
            </tr>
    }
        </tbody>
    </table>

    ```
    
    - Ở đây vì ta đã thêm `site.js` vào `_Layout.cshtml` vì thế không cần khai báo `site.js` ở `Index.cshtml`. Vì nó áp dụng cho cả project

- B14: Bây giờ ta thêm trigger để bắn `Event` khi Save, Delete, và Edit.
    - Inject SignalHub vào các Razor Page. `Create.cshtml.cs`, `Edit.cshtml.cs`, `Delete.cshtml.cs`
        ```cSharp
        private readonly IHubContext<SignalrServer> _signalRHub;
        ```
    - Theo thứ tự sau đây:
       - `Create.cshtml.cs`
       ```cSharp
       private readonly PRN221_Spr22Context _context;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public CreateModel(PRN221_Spr22Context context, IHubContext<SignalrServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }
       ```
       - `Edit.cshtml.cs`
       ```cSharp
       private readonly PRN221_Spr22Context _context;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public EditModel(PRN221_Spr22Context context, IHubContext<SignalrServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }
       ```
       - `Delete.cshtml.cs`
       ```cSharp
       private readonly PRN221_Spr22Context _context;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public DeleteModel(PRN221_Spr22Context context, IHubContext<SignalrServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }
       ```
    - Sau đó vào các hàm mà có thực hiện `await _context.SaveChangesAsync();` (Save vào database thì sẽ load lại Employees. Cập nhật theo thời gian thực) thì ta sẽ thêm dòng `await _signalRHub.Clients.All.SendAsync("LoadEmployees")` ngay sao đó. Áp dụng cho cả 3 file `Create.cshtml.cs`, `Edit.cshtml.cs`, `Delete.cshtml.cs` 
        ```cSharp
         await _context.SaveChangesAsync();
         await _signalRHub.Clients.All.SendAsync("LoadEmployees");
        ```
        - Ví dụ như `Create.cshtml.cs`:
        ```cSharp
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Employees == null || Employee == null)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();
            await _signalRHub.Clients.All.SendAsync("LoadEmployees");
            return RedirectToPage("./Index");
        }
        ```
    - Ở đây ta bắn sự kiện `await _signalRHub.Clients.All.SendAsync("LoadEmployees");`. Vậy `LoadEmployees` ở đâu. Thì thực chất ở đây mình đặt là gì cũng được. Nếu sửa ở đây là `GetEmployees` thì ở file `site.js` cũng phải sửa thành `GetEmployees` để chúng ta biết đang lắng nghe `Event` nào và đang thực hiện bắn `Event` nào.
    - Thế là đã hoàn thành Crud bằng signalr

**Lưu ý**
- Nếu post không được thì thêm cái này vào form
    ```html
    @Html.AntiForgeryToken()
    ```
