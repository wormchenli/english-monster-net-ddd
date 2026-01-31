namespace FileService.Api.Dtos;

public record FileExistsDto(bool IsExists, Uri? Url);