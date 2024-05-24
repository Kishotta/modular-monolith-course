using Evently.Common.Domain;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.ChangeUserName;

internal sealed class ChangeUserNameCommandHandler(
    IUserRepository users,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeUserNameCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(ChangeUserNameCommand request, CancellationToken cancellationToken)
    {
        var user = await users.GetAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        
        user.ChangeName(request.FirstName, request.LastName);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (UserResponse)user;
    }
}