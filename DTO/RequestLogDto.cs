namespace assignment_3_movievault.DTO
{
    public class RequestLogDto
    {
        public string Username { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string RequestType { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}