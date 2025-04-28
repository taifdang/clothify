using clothes_backend.Utils.Enum;

namespace clothes_backend.Heplers.General
{
    public static class ErrorMsg
    {
        //match error message
        public static readonly Dictionary<StatusCode, string> dictionary = new Dictionary<StatusCode, string>()
        {
            {  
                StatusCode.Success,"Thành công" 
            },
            {
                StatusCode.None,"Lỗi không xác định"
            },
            {  
                StatusCode.NotFound,"Không tìm thấy dữ liệu" 
            },
            {
                StatusCode.Isvalid,"Dữ liệu không hợp lệ"
            },
            {
                StatusCode.Unauthorized,"Xác thực thất bại, không có quyền truy cập"
            },
            {
                StatusCode.Conflict,"Xung đột dữ liệu hoặc dữ liệu đã được thay đổi bởi một tiến trình khác"
            }

        };
        //GetAllBase message
        public static string getMessage(StatusCode error)
        {
            if(dictionary.TryGetValue(error,out var message))
            {
                return message;
            }
            return dictionary[StatusCode.None];
        }       
    }
}
