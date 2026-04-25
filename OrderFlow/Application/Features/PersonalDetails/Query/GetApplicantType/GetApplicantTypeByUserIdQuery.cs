//using Application.Interfaces;
//using Application.Interfaces.Repositories;
//using Application.Wrappers;
//using AutoMapper;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Application.Features.PersonalDetails.Query.GetApplicantType
//{
//    public class GetApplicantTypeByUserIdQuery : IRequest<Response<bool>>
//    {
//        public int Id { get; set; }

//        public class GetApplicantTypeByUserIdQueryHandler : IRequestHandler<GetApplicantTypeByUserIdQuery, Response<bool>>
//        {
//            private readonly IUserProfileRepositoryAsync _userProfile;
//            private readonly IMapper _mapper;
//            private readonly IAuthenticatedUserService _user;
//            //private readonly IApplicationStageRepositoryAsync _applicationStages;

//            public GetApplicantTypeByUserIdQueryHandler(IUserProfileRepositoryAsync userProfile, IMapper mapper,
//                IAuthenticatedUserService user/*, IApplicationStageRepositoryAsync applicationStages*/)
//            {
//                _userProfile = userProfile;
//                _mapper = mapper;
//                _user = user;
//                //_applicationStages = applicationStages;
//            }

//            public async Task<Response<bool>> Handle(GetApplicantTypeByUserIdQuery request, CancellationToken cancellationToken)
//            {


//                return new Response<bool>(false, "successful");
//            }
//        }
//    }
//}
