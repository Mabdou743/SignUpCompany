namespace SignUpCompany.API
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public T Data { get; set; }
    }
}
