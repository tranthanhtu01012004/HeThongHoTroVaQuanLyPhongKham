namespace HeThongHoTroVaQuanLyPhongKham.Common
{
    public class ApiResponse<TDto> where TDto : class
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public TDto? Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public ApiResponse(bool Status, string? Message, TDto? data)
        {
            this.Status = Status;
            this.Message = Message;
            Data = data;
        }

        public ApiResponse(bool status, string? message, TDto? data, int page, int pageSize, int totalPages, int totalItems)
                    : this(status, message, data)
        {
            Page = page;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        public static ApiResponse<TDto> Success(TDto? Data, string? message = "Lấy dữ liệu thành công.")
        {
            return new ApiResponse<TDto>(true, message, Data);
        }

        public static ApiResponse<TDto> Success(TDto? data, int page, int pageSize, int totalPages, int totalItems, string? message = "Lấy dữ liệu thành công.")
        {
            return new ApiResponse<TDto>(true, message, data, page, pageSize, totalPages, totalItems);
        }

        public static ApiResponse<TDto> Fail(string message)
        {
            return new ApiResponse<TDto>(false, message, null);
        }
    }
}
