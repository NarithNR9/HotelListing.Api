using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Api.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.HotelId);
            builder.HasIndex(x => new { x.CheckInDate, x.CheckOutDate });
        }
    }
}
