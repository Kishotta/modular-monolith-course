using Evently.Common.Domain;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IIdentityProviderService identityProvider,
    IUserRepository users,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await identityProvider.RegisterUserAsync(
            new UserModel(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName), cancellationToken);
        if (result.IsFailure)
            return Result.Failure<UserResponse>(result.Error);
        
        var user = User.Create(request.Email, request.FirstName, request.LastName, result.Value);
        
        users.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (UserResponse)user;
    }
}