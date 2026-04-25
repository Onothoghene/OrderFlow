using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using System.IO;
using System.Linq;

namespace Application.Features.MenuItem.Command
{
    public class AddOrUpdateMenuItemCommand : IRequest<Response<bool>>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileFormat { get; set; }
        public string FileBinary { get; set; }


        public class AddOrUpdateMenuItemCommandHandler : IRequestHandler<AddOrUpdateMenuItemCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IMenuItemRepositoryAsync _menuItemRepository;
            private readonly IFileUploadService _fileUploadService;
            private readonly IFileTempRepositoryAsync _fileTempRepository;

            public AddOrUpdateMenuItemCommandHandler(IMapper mapper, IMenuItemRepositoryAsync menuItemRepository,
                                                    IFileUploadService fileUploadService,
                                                    IFileTempRepositoryAsync fileTempRepository)
            {
                _mapper = mapper;
                _menuItemRepository = menuItemRepository;
                _fileUploadService = fileUploadService;
                _fileTempRepository = fileTempRepository;
            }

            public async Task<Response<bool>> Handle(AddOrUpdateMenuItemCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var folderPath = Path.GetFullPath("FileUpload");

                    var menuItem = command.Id.HasValue && command.Id.Value > 0
                        ? await _menuItemRepository.GetByIdAsync(command.Id.Value)
                          ?? throw new ApiException($"The requested Menu Item could not be found.")
                        : _mapper.Map<Domain.Entities.MenuItem>(command);

                    menuItem.Name = command.Name;
                    menuItem.Price = command.Price;
                    menuItem.Description = command.Description;
                    menuItem.CategoryId = command.CategoryId;

                    // Ensure MenuItem is saved before adding images
                    if (!command.Id.HasValue || command.Id.Value == 0)
                    {
                        await _menuItemRepository.AddAsync(menuItem);
                    }

                    // Delete old image if updating
                    if (command.Id.HasValue && command.Id.Value > 0)
                    {
                        var existingFile = await Task.Run(() => _fileTempRepository.GetFileTempByMenuItemId(menuItem.Id));
                        if (existingFile != null && existingFile.Count() > 0)
                        {
                            foreach (var item in existingFile)
                            {
                                if (!string.IsNullOrEmpty(item.FileUniqueName))
                                    _fileUploadService.DeleteFile(item.FileUniqueName);
                            } // Delete file from folder

                            await _fileTempRepository.DeleteRangeAsync(existingFile); // Delete from DB
                        }
                    }

                    // Handle File Upload
                    if (!string.IsNullOrEmpty(command.FileBinary) && !string.IsNullOrEmpty(command.FileFormat))
                    {
                        var fileTemp = new FileTemp
                        {
                            ImageURL = command.ImageURL,
                            FileName = !string.IsNullOrEmpty(command.FileName) ? command.FileName : null,
                            FileExt = !string.IsNullOrEmpty(command.FileFormat) ? command.FileFormat : null,
                            FileUniqueName = !string.IsNullOrEmpty(command.FileBinary) ? _fileUploadService.ConvertToFile(command.FileBinary, folderPath, command.FileName) : null,
                            MenuItemId = menuItem.Id
                        };

                        await _fileTempRepository.AddAsync(fileTemp);
                    }

                    if (command.Id.HasValue && command.Id.Value > 0)
                        await _menuItemRepository.UpdateAsync(menuItem);

                    ts.Complete();
                }

                return new Response<bool>(true, "Request executed successfully.");
            }


        }

    }
}
