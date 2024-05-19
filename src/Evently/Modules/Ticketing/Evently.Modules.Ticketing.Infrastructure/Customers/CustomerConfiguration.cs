using Evently.Modules.Ticketing.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Ticketing.Infrastructure.Customers;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.Email).HasMaxLength(300);
        builder.Property(customer => customer.FirstName).HasMaxLength(200);
        builder.Property(customer => customer.LastName).HasMaxLength(200);
    }
}