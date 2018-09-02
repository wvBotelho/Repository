using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Mapping
{
    public class ProjectMap : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            //Definindo chave primária
            builder.HasKey(p => p.ProjectID)
                .HasName("pk_project_id");

            //Definindo nome da tabela e seus campos
            builder.ToTable("project_wvb_rep");

            builder.Property(p => p.ProjectID)
                .HasColumnName("project_id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(p => p.CustomerID)
                .HasColumnName("customer_id")
                .HasColumnType("uniqueidentifier");

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("description")
                .HasColumnType("NVARCHAR(1000)")
                .IsRequired();

            builder.Property(p => p.Start)
                .HasColumnName("start")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(p => p.End)
                .HasColumnName("end")
                .HasColumnType("datetime");

            #region Definindo os relacionamentos

            //projects - projectDetail : one to one
            builder.HasOne(p => p.Detail)
                .WithOne(p => p.Project)
                .HasForeignKey<ProjectDetail>(d => d.ProjectID)
                .HasPrincipalKey<Project>(p => p.ProjectID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //projects - customers : one to many
            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Projects)
                .HasForeignKey(p => p.CustomerID)
                .OnDelete(DeleteBehavior.SetNull);

            //projects - projectsResource : many to one
            builder.HasMany(p => p.ProjectResources)
                .WithOne(p => p.Project)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion            
        }
    }
}
