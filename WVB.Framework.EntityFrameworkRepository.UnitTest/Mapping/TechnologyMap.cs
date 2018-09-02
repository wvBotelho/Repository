using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping
{
    public class TechnologyMap : IEntityTypeConfiguration<Technology>
    {
        public void Configure(EntityTypeBuilder<Technology> builder)
        {
            //Definindo a chave primária
            builder.HasKey(t => t.TechnologyID)
                .HasName("pk_technology_id");

            //Definindo o nome da tabela e seus campos
            builder.ToTable("technology_wvb_rep");

            builder.Property(t => t.TechnologyID)
                .HasColumnName("technology_id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .IsRequired();            

            builder.Property(t => t.ResourceID)
                .HasColumnName("resource_id")
                .HasColumnType("uniqueidentifier");            

            builder.Property(t => t.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            //Definindo o relacionamento
            //technology - resource : one to many
            builder.HasOne(t => t.Resource)
                .WithMany(t => t.Technologies)
                .HasForeignKey(t => t.ResourceID)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
