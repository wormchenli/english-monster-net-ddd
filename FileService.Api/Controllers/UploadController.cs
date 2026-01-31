using FileService.Api.Dtos;
using FileService.Domain.DomainService;
using FileService.Domain.Repository;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middlewares.UnitOfWork;

namespace FileService.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
[UnitOfWork(typeof(FileServiceDbContext))]
[Authorize(Roles = "Admin")]
public class UploadController : ControllerBase
{
    private readonly FsDomainService _fsDomainService;
    private readonly IUploadedItemRepository _uploadedItemRepository;

    public UploadController(FsDomainService fsDomainService, IUploadedItemRepository uploadedItemRepository)
    {
        _fsDomainService = fsDomainService;
        _uploadedItemRepository = uploadedItemRepository;
    }

    [HttpGet]
    public async Task<ActionResult<FileExistsDto>> FileExists(string sha256Hash, long fileSize)
    {
        var item = await _uploadedItemRepository.FindFileAsync(sha256Hash, fileSize);
        if (item == null)
        {
            return new FileExistsDto(false, null);
        }

        Console.WriteLine("****************** found existing file ******************");
        return new FileExistsDto(true, item.FileAccessPath);

    }
    // validation will be executed automatically
    // before the action method is invoked
    // RequestSizeLimit executes before model binding
    // FileSizeAttribute on the dto executes after model binding
    [HttpPost]
    [RequestSizeLimit(60_000_000)] // 60 MB
    public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var file = request.File;
        var fileName = file.FileName;
        await using var fileStream = file.OpenReadStream();
        var uploadedItem = await _fsDomainService.UpLoadFileAsync(fileStream, fileName, cancellationToken);
        
        if (uploadedItem == null) return BadRequest("File upload failed.");
        await _uploadedItemRepository.AddAsync(uploadedItem);
        Console.WriteLine(uploadedItem.FileAccessPath);
        return uploadedItem.FileAccessPath;
    }
}
