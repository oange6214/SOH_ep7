
using SohatNotebook.Entities.Dtos.Errors;

namespace SohatNotebook.Entities.Dtos.Generic;

/// <summary>
/// Single item return
/// </summary>
public class Result<T>
{
    public T Content { get; set; }
    public Error Error { get; set; }
    public bool IsSuccess => Error == null;
    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
}
