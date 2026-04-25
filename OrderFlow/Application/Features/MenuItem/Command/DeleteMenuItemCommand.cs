using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;
using System.IO;
using System.Linq;
using Application.Interfaces;

namespace Application.Features.MenuItem.Command
{
    public class DeleteMenuItemCommand : IRequest<Response<bool>>
    {
        public int menuItemId { get; set; }

        public class DeleteMenuItemCommandHandler : IRequestHandler<DeleteMenuItemCommand, Response<bool>>
        {
            private readonly IMenuItemRepositoryAsync _menuItemRepository;
            private readonly IFileTempRepositoryAsync _fileTempRepository;
            private readonly IFileUploadService _fileUploadService;

            public DeleteMenuItemCommandHandler(IMenuItemRepositoryAsync menuItemRepository,
                                                IFileTempRepositoryAsync fileTempRepository,
                                                IFileUploadService fileUploadService)
            {
                _menuItemRepository = menuItemRepository;
                _fileTempRepository = fileTempRepository;
                _fileUploadService = fileUploadService;
            }

            public async Task<Response<bool>> Handle(DeleteMenuItemCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var folderPath = Path.GetFullPath("FileUpload");

                    var data = await _menuItemRepository.GetByIdAsync(command.menuItemId) ??
                                                        throw new ApiException($"The requested Menu Item could not be found.");

                    // Retrieve associated file records
                    var fileRecords = await Task.Run(()=> _fileTempRepository.GetFileTempByMenuItemId(command.menuItemId));

                    if (fileRecords != null && fileRecords.Count() > 0)
                    {
                        foreach (var item in fileRecords)
                        {
                            if (!string.IsNullOrEmpty(item.FileUniqueName))
                                _fileUploadService.DeleteFile(item.FileUniqueName);
                        } // Delete file from folder

                        await _fileTempRepository.DeleteRangeAsync(fileRecords); // Delete from DB
                    }

                    await _menuItemRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "Menu Item and associated file(s) deleted successfully");

                }
            }
        }
    }
}

