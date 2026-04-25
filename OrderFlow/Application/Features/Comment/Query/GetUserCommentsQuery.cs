using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.DTOs.Comments;
using Application.Interfaces.Repositories;
using Application.Interfaces;

namespace Application.Features.Comment.Query
{
    public class GetUserCommentsQuery : IRequest<Response<CommentVM>>
    {
        public int? userId { get; set; }

        public class GetUserCommentsQueryHandler : IRequestHandler<GetUserCommentsQuery, Response<CommentVM>>
        {
            private readonly ICommentRepositoryAsync _commentRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetUserCommentsQueryHandler(ICommentRepositoryAsync commentRepository, IMapper mapper,
                                                    IAuthenticatedUserService userService)
            {
                _commentRepository = commentRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<CommentVM>> Handle(GetUserCommentsQuery query, CancellationToken cancellationToken)
            {
                var user = query.userId.HasValue ? query.userId.Value : _userService.UserId;
                var response = await Task.Run(()=> _commentRepository.GetUserComments(user));
                return new Response<CommentVM>(_mapper.Map<CommentVM>(response), "successful");
            }
        }
    }
}

