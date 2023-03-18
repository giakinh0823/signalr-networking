using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

/*
 Đây là một đoạn mã đơn giản của giao diện ICommand trong C# sử dụng lớp RelayCommand. 
Giao diện ICommand được sử dụng để đại diện cho một lệnh có thể được thực thi bởi một phần 
tử giao diện người dùng như một nút, một mục menu hoặc một nút thanh công cụ.

Lớp RelayCommand có hai tham số trong hàm tạo của nó: một phương thức Action<object> 
đại diện cho phương thức sẽ được thực thi khi lệnh được gọi, và một phương thức Func<object, bool> đại diện cho 
phương thức để xác định xem liệu lệnh có thể được thực thi hay không.

Phương thức CanExecute trả về giá trị Boolean cho biết liệu lệnh có thể được thực thi hay không, 
dựa trên kết quả của phương thức _canExecute. Nếu _canExecute là null, lệnh luôn có thể được thực thi.

Phương thức Execute thực thi lệnh bằng cách gọi phương thức _execute, truyền vào tham số được chuyển đến phương thức.

Sự kiện CanExecuteChanged được kích hoạt bởi CommandManager khi phải gọi lại phương thức CanExecute 
để xác định liệu lệnh có thể được thực thi hay không. Sự kiện này thường được sử dụng để cập nhật 
trạng thái đã bật/tắt của các phần tử UI được ràng buộc với lệnh.
 */

namespace ChatClient.Core
{
    class RelayCommand : ICommand
    {
        private Func<object, bool> _canExecute;
        private Action<object> _execute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
