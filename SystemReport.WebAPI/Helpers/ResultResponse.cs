namespace SystemReport.WebAPI.Helpers
{
    public class ResultStringResponse
    {
        public string Result { get; set; }
    }
    public class ResultResponse<T>
    {
        public string ResultCode { get; set; }
        public string ResultString { get; set; }
        public T Data { get; set; }
    }
    public enum EResultResponse
    {
        SUCCESS,
        FAIL,
        ERROR,
        DUMPLICATE,
        DUPLICATE,
        PARAM_ERROR,
        NAME_EXIST,
        NOT_EXIST,
        NOT_FOUND
    }
}