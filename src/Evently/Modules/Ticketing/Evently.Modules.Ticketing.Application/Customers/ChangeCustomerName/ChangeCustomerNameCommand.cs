namespace Evently.Modules.Ticketing.Application.Customers.ChangeCustomerName;

public sealed record ChangeCustomerNameCommand(Guid CustomerId, string FirstName, string LastName) : ICommand;