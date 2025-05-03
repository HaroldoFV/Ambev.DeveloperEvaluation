using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasDefaultValueSql("nextval('\"SaleSequence\"')");

        builder.Property(s => s.SaleDate).IsRequired();
        builder.Property(s => s.TotalValue).HasColumnType("decimal(18,2)");

        builder.HasMany(s => s.Items)
            .WithOne(s => s.Sale)
            .HasForeignKey(i => i.SaleId);


        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(s => s.CreatedAt).IsRequired();

        builder.Ignore(s => s.Events);
    }
}