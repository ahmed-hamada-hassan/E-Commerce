using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Application.Common;

public record PaginationSettings
{
    [Range(1, 50, ErrorMessage = "Numbers from 1 to 50 are allowed")]
    public int MaxSize { get; init; }

    [Range(1, 50, ErrorMessage = "Numbers from 1 to 50 are allowed")]
    public int DefaultSize { get; init; }
}