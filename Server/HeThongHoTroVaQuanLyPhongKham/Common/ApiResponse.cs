namespace HeThongHoTroVaQuanLyPhongKham.Common
{
    public class ApiResponse<TDto> where TDto : class
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public TDto? Data { get; set; }

        public ApiResponse(bool Status, string? Message, TDto? data)
        {
            this.Status = Status;
            this.Message = Message;
            Data = data;
        }

        public static ApiResponse<TDto> Success(TDto? Data, string? message = "Lấy dữ liệu thành công.")
        {
            return new ApiResponse<TDto>(true, message, Data);
        }

        public static ApiResponse<TDto> Fail(string message)
        {
            return new ApiResponse<TDto>(false, message, null);
        }
    }
}
