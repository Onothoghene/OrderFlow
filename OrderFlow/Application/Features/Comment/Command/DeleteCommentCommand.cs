using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;

namespace Application.Features.Comment.Command
{
    public class DeleteCommentCommand : IRequest<Response<bool>>
    {
        public int commentId { get; set; }

        public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Response<bool>>
        {
            private readonly ICommentRepositoryAsync _commentRepository;

            public DeleteCommentCommandHandler(IUserProfileRepositoryAsync userProfile,
                                              ICommentRepositoryAsync commentRepository)
            {
                _commentRepository = commentRepository;
            }

            public async Task<Response<bool>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
            {
                using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = await _commentRepository.GetByIdAsync(command.commentId) ?? 
                                                            throw new ApiException($"The requested comment could not be found.");

                    //data.IsDeleted = true;
                    await _commentRepository.DeleteAsync(data);

                    ts.Complete();
                    return new Response<bool>(true, "Food Comment deleted successfully");

                }
            }
        }
    }
}

