namespace WebShop.ViewModels
{
    public class ErrorResponse<T>
    {
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public T RequestParams { get; set; }
        public ErrorResponse(int errorCode, string errorMsg, T requestParams)
        {
            ErrorCode = errorCode;
            ErrorMsg = errorMsg;
            RequestParams = requestParams;
        }
    }


    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public ErrorResponse(int errorCode, string errorMsg)
        {
            ErrorCode = errorCode;
            ErrorMsg = errorMsg;
        }
    }
}
