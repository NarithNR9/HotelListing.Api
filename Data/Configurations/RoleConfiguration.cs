using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Api.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "a7340aab-5bcd-46db-bbe3-d0761919c9d6",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = "d0017a83-fe1d-49e8-bed4-f2a1042e3065"
            },
            new IdentityRole
            {
                Id = "656fc1d5-c39d-4d35-bafe-88d0c9a2ca1e",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "a2271a5b-35f5-4001-91bc-2ce8391c9553"
            }
        );
    }
}