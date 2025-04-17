using clothes_backend.Utils.Enum;

namespace clothes_backend.Service
{
    public static class MessageResponse
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
                StatusCode.Unauthorized,"Không có quyền truy cập"
            },
            {
                StatusCode.Conflict,"Xung đột dữ liệu"
            }

        };
        //get message
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
