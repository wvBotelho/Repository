using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping
{
    public class ProjectDetailMap : IEntityTypeConfiguration<ProjectDetail>
    {
        public void Configure(EntityTypeBuilder<ProjectDetail> builder)
        {
            //Definindo a chave primária
            builder.HasKey(p => p.ProjectID);

            //Definindo o nome da tabela e seus campos
            builder.ToTable("project_wvb_rep");

            builder.Property(p => p.ProjectID)
                .HasColumnName("project_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(p => p.Budget)
                .HasColumnName("budget")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(p => p.Critical)
                .HasColumnName("critical")
                .HasColumnType("bit")
                .HasDefaultValue(false)
                .IsRequired();

            #region Definindo o relacionamento

            //projectDetail - project : one to one
            builder.HasOne(p => p.Project)
                .WithOne(p => p.Detail)
                .HasForeignKey<ProjectDetail>(p => p.ProjectID)
                .HasPrincipalKey<Project>(e => e.ProjectID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}
