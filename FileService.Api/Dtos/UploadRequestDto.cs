using FileService.Api.Validations;

namespace FileService.Api.Dtos;

public sealed class UploadRequestDto
{
    [FileSize("Upload:MaxFileSizeBytes")]
    public IFormFile File { get; init; } = null!;
}
