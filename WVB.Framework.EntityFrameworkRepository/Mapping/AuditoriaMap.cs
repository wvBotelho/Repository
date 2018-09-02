using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WVB.Framework.EntityFrameworkRepository.Model;

namespace WVB.Framework.EntityFrameworkRepository.Mapping
{
    public class AuditoriaMap : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            //Define a chave primária
            builder.HasKey(a => a.AuditoriaID)
                .HasName("pk_auditoria_id");

            //Define o nome da tabela e seus campos
            builder.ToTable("auditoria_wvb_rep");

            builder.Property(a => a.AuditoriaID)
                .HasColumnName("id_auditoria")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(a => a.EntidadeID)
                .HasColumnName("id_entidade")
                .HasColumnType("uniqueidentifier");

            builder.Property(a => a.NomeEntidade)
                .HasColumnName("nome_entidade")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(a => a.AlteradoPor)
                .HasColumnName("alterado_por")
                .HasColumnType("varchar(50)");

            builder.Property(a => a.DataAlteracao)
                .HasColumnName("data_alteracao")
                .HasColumnType("datetime");

            builder.Property(a => a.UltimaAcao)
                .HasColumnName("ultima_acao")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(a => a.CamposAlterados)
                .HasColumnName("campos_alterados")
                .HasColumnType("nvarchar(4000)");

            builder.Property(a => a.ValoresAdicionados)
                .HasColumnName("valores_adicionados")
                .HasColumnType("nvarchar(4000)");

            #region Gerando Indice

            builder.HasIndex(a => new { a.EntidadeID, a.NomeEntidade })
                .HasName("ix_entidade");

            #endregion
        }
    }
}
