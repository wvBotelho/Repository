using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping
{
    public class ResourceMap : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            //Definindo a chave primária
            builder.HasKey(r => r.ResourceID)
                .HasName("pk_resource_id");

            //Definindo o nome da tabela e os seus campos
            builder.ToTable("resource_wvb_rep");

            builder.Property(r => r.ResourceID)
                .HasColumnName("resource_id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(r => r.TechnologyID)
                .HasColumnName("technology_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            //Owned type
            builder.OwnsOne(r => r.Contact, r =>
            {
                r.Property(a => a.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(100)")
                .IsRequired();

                r.Property(a => a.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(14)")
                    .IsRequired();
            });

            #region Definindo os relacionamentos

            //resource - projectResources : many to one
            builder.HasMany(r => r.ProjectResources)
                .WithOne(r => r.Resource)
                .OnDelete(DeleteBehavior.Restrict);

            //resource - technology : one to many
            builder.HasMany(r => r.Technologies)
                .WithOne(r => r.Resource)
                .OnDelete(DeleteBehavior.ClientSetNull);

            #endregion
        }
    }
}
