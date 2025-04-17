using clothes_backend.Utils.Enum;

namespace clothes_backend.Service
{
    public static class ErrorMessage
    {
        //match error message
        public static readonly Dictionary<ErrorType, string> dictionary = new Dictionary<ErrorType, string>()
        {
            {  
                ErrorType.None,"Lỗi không xác định." 
            },
            {  
                ErrorType.NotFound,"Không tìm thấy dữ liệu" 
            },
            {
                ErrorType.Invalid,"Dữ liệu không hợp lệ."
            },
            {
                ErrorType.Unauthorized,"Không có quyền truy cập."
            }
        };
        //get message
        public static string getMessage(ErrorType error)
        {
            if(dictionary.TryGetValue(error,out var message))
            {
                return message;
            }
            return "Lỗi không xác định.";
        }
    }
}
