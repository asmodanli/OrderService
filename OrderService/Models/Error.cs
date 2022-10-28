namespace OrderService.Models
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }

        public Error() { }
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
