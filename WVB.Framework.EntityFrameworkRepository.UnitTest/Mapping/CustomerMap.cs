using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            //Definindo a chave primária
            builder.HasKey(c => c.CustomerID)
                .HasName("pk_customer_id");

            //Definindo o nome da tabela e seus campos
            builder.ToTable("customer_wvb_rep");

            builder.Property(c => c.CustomerID)
                .HasColumnName("customer_id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(c => c.ProjectID)
                .HasColumnName("project_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            //Owned Type
            builder.OwnsOne(c => c.Contact, c =>
            {
                c.Property(a => a.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(100)")
                .IsRequired();

                c.Property(a => a.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(14)")
                    .IsRequired();
            });

            #region Definindo o relacionamento

            //customer - project : many to one
            builder.HasMany(c => c.Projects)
                .WithOne(c => c.Customer)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion
        }
    }
}
