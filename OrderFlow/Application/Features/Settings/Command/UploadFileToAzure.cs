//using Application.Interfaces;
//using Application.Wrappers;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Application.Features.Settings.Command
//{
//    public class UploadFileToAzure : IRequest<Response<string>>
//    {
//        public string FileName { get; set; }
//        public string FileExt { get; set; }
//        public string FileBinary { get; set; }

//        public class UploadFileToAzureHandler : IRequestHandler<UploadFileToAzure, Response<string>>
//        {
//            private readonly IFileUploadService _fileUpload;

//            public UploadFileToAzureHandler(IFileUploadService fileUpload)
//            {
//                _fileUpload = fileUpload;
//            }

//            public async Task<Response<string>> Handle(UploadFileToAzure request, CancellationToken cancellationToken)
//            {
//                var uniqueFileName = await _fileUpload.UploadToAzure(request.FileBinary, request.FileName);

//                var fileViewModel = await _fileUpload.GetFileLinkFromAzure(uniqueFileName);

//                return new Response<string>(fileViewModel.FileBinary, "successful");
//            }
//        }
//    }
//}
