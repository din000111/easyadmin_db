namespace EasyAdmin.Shared.Common
{
    public class ServicesResponse
    {
        public bool isSuccess{ get; set; }
        public object resultObject { get; set; }
        public string errorMessage { get; set; }
        public int errorCode { get; set; }
    }
    public class ServicesResponse<T>
    {
        public bool isSuccess { get; set; }
        public T resultObject { get; set; }
        public string errorMessage { get; set; }
        public int errorCode { get; set; }
    }
}
