using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IUserRepository users,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Email, request.FirstName, request.LastName);
        
        users.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return (UserResponse)user;
    }
}