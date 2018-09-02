using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping
{
    public class ProjectResourceMap : IEntityTypeConfiguration<ProjectResource>
    {
        public void Configure(EntityTypeBuilder<ProjectResource> builder)
        {
            //Definindo a chave primária
            builder.HasKey(p => new { p.ProjectID, p.ResourceID })
                .HasName("pk_project_resource_id");

            //Definindo o nome da tabela e seus campos
            builder.ToTable("project_resource_wvb_rep");

            builder.Property(p => p.ProjectID)
                .HasColumnName("project_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(p => p.ResourceID)
                .HasColumnName("resource_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(p => p.Role)
                .HasColumnName("role")
                .HasColumnType("int")
                .IsRequired();

            #region Definindo os relacionamentos

            //projectResource - project : one to many
            builder.HasOne(p => p.Project)
                .WithMany(pr => pr.ProjectResources)
                .HasForeignKey(p => p.ProjectID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //projectResource - resource : one to many
            builder.HasOne(p => p.Resource)
                .WithMany(r => r.ProjectResources)
                .HasForeignKey(p => p.ResourceID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            #endregion
        }
    }
}
