using System.Collections.Generic;

namespace SystemReport.WebAPI.Helpers
{
    public class ResultMessageResponse
    {
        public string ResultCode { get; set; }
        public string ResultString { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public ResultMessageResponse WithCode(string resultCode)
        {
            if (!string.IsNullOrEmpty(resultCode))
            {
                ResultCode = resultCode;
            }

            return this;
        }
        public ResultMessageResponse WithMessage(string resultString)
        {
            if (!string.IsNullOrEmpty(resultString))
            {
                ResultString = resultString;
            }

            return this;
        }
        public ResultMessageResponse WithErrors(IEnumerable<string> errors)
        {
            if (errors != default)
            {
                Errors = errors;
            }

            return this;
        }
    }
    public class ResultResponse<T> : ResultMessageResponse
    {
        public T Data { get; set; }

        public ResultResponse<T> WithData(T data)
        {
            if (data != null)
            {
                Data = data;
            }

            return this;
        }
        
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
    }
}