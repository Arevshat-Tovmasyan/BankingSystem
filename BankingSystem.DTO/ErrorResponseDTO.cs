namespace BankingSystem.DTO
{
    public class ErrorResponseDTO(string message)
    {
        public string Message { get; set; } = message;
    }
}
