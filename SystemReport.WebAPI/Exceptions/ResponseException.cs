using System;

namespace SystemReport.WebAPI.Exceptions
{
    public class ResponseMessageException : Exception
    {
        public string ResultCode { get; set; }
        public string ResultString { get; set; }

        public ResponseMessageException()
        {
            
        }
        public ResponseMessageException(string resultCode, string resultString)
        {
            this.ResultCode = resultCode;
            this.ResultString = resultString;
        }

        public ResponseMessageException WithCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                this.ResultCode = code;
            }

            return this;
        }
        
        public ResponseMessageException WithMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this.ResultString = message;
            }

            return this;
        }
    }
    public class ResponseException<T> : ResponseMessageException
    {
        public T Data { get; set; }

        public ResponseException()
        {
            
        }
        public ResponseException(string resultCode, string resultString, T data) : base(resultCode, resultString)
        {
            this.Data = data;
        }
        
        public ResponseMessageException WithData(T data)
        {
            if (data != null)
            {
                this.Data = data;
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