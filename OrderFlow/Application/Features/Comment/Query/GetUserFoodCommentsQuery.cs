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
    public class GetUserFoodCommentsQuery : IRequest<Response<CommentVM>>
    {
        public int? userId { get; set; }
        public int foodId { get; set; }

        public class GetUserFoodCommentsQueryHandler : IRequestHandler<GetUserFoodCommentsQuery, Response<CommentVM>>
        {
            private readonly ICommentRepositoryAsync _commentRepository;
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _userService;

            public GetUserFoodCommentsQueryHandler(ICommentRepositoryAsync commentRepository, IMapper mapper,
                                                    IAuthenticatedUserService userService)
            {
                _commentRepository = commentRepository;
                _mapper = mapper;
                _userService = userService;
            }
            public async Task<Response<CommentVM>> Handle(GetUserFoodCommentsQuery query, CancellationToken cancellationToken)
            {
                var user = query.userId.HasValue ? query.userId.Value : _userService.UserId;
                var response = await Task.Run(()=> _commentRepository.GetUserFoodComments(user, query.foodId));
                return new Response<CommentVM>(_mapper.Map<CommentVM>(response), "successful");
            }
        }
    }
}

