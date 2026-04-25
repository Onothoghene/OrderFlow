using Application.Interfaces.Repositories;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Application.Exceptions;

namespace Application.Features.Comment.Command
{
    public class AddOrUpdateCommentCommand : IRequest<Response<bool>>
    {
        public int? Id { get; set; }
        public int FoodId { get; set; }
        public string CommentText { get; set; }
        public double Rating { get; set; }

        public class AddOrUpdateCommentCommandHandler : IRequestHandler<AddOrUpdateCommentCommand, Response<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IAuthenticatedUserService _user;
            private readonly ICommentRepositoryAsync _commentRepository;

            public AddOrUpdateCommentCommandHandler(IMapper mapper, IAuthenticatedUserService user,
                                                   ICommentRepositoryAsync commentRepository)
            {
                _mapper = mapper;
                _user = user;
                _commentRepository = commentRepository;
            }

            public async Task<Response<bool>> Handle(AddOrUpdateCommentCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userId = _user.UserId;

                    //Update functionality
                    if (command.Id.HasValue && command.Id.Value > 0)
                    {
                        var comment = await _commentRepository.GetByIdAsync(command.Id.Value);
                        if (comment == null)
                            throw new ApiException($"The requested comment could not be found.");

                        comment.Rating = command.Rating;
                        comment.CommentText = command.CommentText;

                        await _commentRepository.UpdateAsync(comment);
                    }
                    else //Create Functionality
                    {
                        var data = _mapper.Map<Domain.Entities.Comments>(command);

                        await _commentRepository.AddAsync(data);
                    }

                    ts.Complete();
                }

                return new Response<bool>(true, "Request excuted successfully.");
            }
        }
    }
}

