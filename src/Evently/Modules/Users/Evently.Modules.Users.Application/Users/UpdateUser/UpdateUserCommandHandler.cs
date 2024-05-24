using Evently.Common.Domain;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserRepository users,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await users.GetAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId));
        
        user.Update(request.FirstName, request.LastName);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (UserResponse)user;
    }
}