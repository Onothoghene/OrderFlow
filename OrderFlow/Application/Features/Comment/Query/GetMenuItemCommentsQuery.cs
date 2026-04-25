using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces.Repositories;
using System.Collections.Generic;
using Application.DTOs.Comments;

namespace Application.Features.Comment.Query
{
    public class GetMenuItemCommentsQuery : IRequest<Response<List<CommentVM>>>
    {
        public int menuItemId { get; set; }

        public class GetMenuItemCommentsQueryHandler : IRequestHandler<GetMenuItemCommentsQuery, Response<List<CommentVM>>>
        {
            private readonly ICommentRepositoryAsync _commentRepository;
            private readonly IMapper _mapper;

            public GetMenuItemCommentsQueryHandler(ICommentRepositoryAsync commentRepository, IMapper mapper)
            {
                _commentRepository = commentRepository;
                _mapper = mapper;
            }
            public async Task<Response<List<CommentVM>>> Handle(GetMenuItemCommentsQuery query, CancellationToken cancellationToken)
            {
                var response = await Task.Run(() => _commentRepository.GetFoodComments(query.menuItemId));
                return new Response<List<CommentVM>>(_mapper.Map<List<CommentVM>>(response), "successful");
            }
        }
    }
}

