namespace Evently.Modules.Ticketing.Application.Customers.ChangeCustomerName;

public record ChangeCustomerNameCommand(Guid CustomerId, string FirstName, string LastName) : ICommand;