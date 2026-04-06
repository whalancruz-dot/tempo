public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; } // Aqui vai o Token, Lista de Favoritos, etc.
    public List<string> Errors { get; set; }

    // Construtor para Sucesso
    public static Result<T> Ok(T data, string message = "Operação realizada com sucesso") 
        => new Result<T> { Success = true, Data = data, Message = message };

    public static Result<T> Failure(string message, List<string> errors = null) 
        => new Result<T> { Success = false, Message = message, Errors = errors };
}