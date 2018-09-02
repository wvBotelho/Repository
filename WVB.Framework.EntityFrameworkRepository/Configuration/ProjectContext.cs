using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes;
using WVB.Framework.EntityFrameworkRepository.Enum;
using WVB.Framework.EntityFrameworkRepository.Mapping;
using WVB.Framework.EntityFrameworkRepository.Model;

namespace WVB.Framework.EntityFrameworkRepository.Configuration
{
    public abstract class ProjectContext : DbContext
    {
        //TODO: verificar um melhor jeito de pegar o usuário genéricamente. Ex: Linux e Mac
        protected string GetUsuarioWindows { get; } = WindowsIdentity.GetCurrent().Name;

        public DbSet<Auditoria> Auditorias { get; set; }

        protected ProjectContext(DbContextOptions<ProjectContext> options) : base (options)
        {
        }

        protected ProjectContext(Database database, string connectionString) : base (GetOptions(database, connectionString))
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                HistorificarAttribute historificarAttribute = (HistorificarAttribute)Attribute.GetCustomAttribute(entity.ClrType, typeof(HistorificarAttribute));

                if (historificarAttribute != null)
                    entity.AddProperty("deletado", typeof(bool));
            }

            //Adiciona a tabela auditar no banco
            modelBuilder.ApplyConfiguration(new AuditoriaMap());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached);

            IServiceProvider serviceProvider = this.GetService<IServiceProvider>();

            List<Auditoria> auditorias = new List<Auditoria>();

            foreach (EntityEntry entry in entries)
            {
                AuditarAttribute auditarAttribute = (AuditarAttribute)Attribute.GetCustomAttribute(entry.Entity.GetType(), typeof(AuditarAttribute));
                HistorificarAttribute historificarAttribute = (HistorificarAttribute)Attribute.GetCustomAttribute(entry.Entity.GetType(),
                    typeof(HistorificarAttribute));

                ValidationContext validationContext = new ValidationContext(entry.Entity, serviceProvider, new Dictionary<object, object>());
                List<ValidationResult> results = new List<ValidationResult>();

                if (auditarAttribute != null)
                    AuditarAcao(entry, auditorias);

                if (historificarAttribute != null)
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.Property("deletado").CurrentValue = true;
                        entry.State = EntityState.Modified;
                    }
                }

                if (!Validator.TryValidateObject(entry.Entity, validationContext, results, true))
                {
                    foreach (ValidationResult result in results)
                    {
                        if (result != ValidationResult.Success)
                            throw new ValidationException(result.ErrorMessage);
                    }
                }
            }

            Auditorias.AddRange(auditorias);

            return base.SaveChanges();
        }

        //TODO: Falta adicionar o provider do oracle sql
        private static DbContextOptions GetOptions(Database typeDB, string connectionString)
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<ProjectContext>();

            ILoggerFactory loggerFactory = new LoggerFactory()
                        .AddConsole((category, level) => (level == LogLevel.Information) && (category == DbLoggerCategory.Database.Command.Name))
                        .AddDebug((category, level) => (level == LogLevel.Information) && (category == DbLoggerCategory.Database.Command.Name));

            switch (typeDB)
            {
                case Enum.Database.SqlServer:
                    optionsBuilder.UseLoggerFactory(loggerFactory)
                        .UseSqlServer(connectionString, providerOptions => providerOptions.CommandTimeout(60))                        
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //avisa para o entity framework não guardar no cache as entidades de consulta
                    break;
                case Enum.Database.MySql:
                    optionsBuilder.UseLoggerFactory(loggerFactory)
                        .UseMySQL(connectionString, providerOptions => providerOptions.CommandTimeout(60))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
                case Enum.Database.SqLite:
                    optionsBuilder.UseLoggerFactory(loggerFactory)
                        .UseSqlite(connectionString, providerOptions => providerOptions.CommandTimeout(60))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
                case Enum.Database.PostgreSql:
                    optionsBuilder.UseLoggerFactory(loggerFactory)
                        .UseNpgsql(connectionString, providerOptions => providerOptions.CommandTimeout(60))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
                default:
                    optionsBuilder.UseLoggerFactory(loggerFactory)
                        .UseSqlServer(connectionString, providerOptions => providerOptions.CommandTimeout(60))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    break;
            }

            return optionsBuilder.Options;
        }
        
        private void AuditarAcao(EntityEntry entry, IList<Auditoria> auditorias)
        {
            if (entry.State == EntityState.Added)
                AuditarEntidadeAdicionada(entry, auditorias);

            if (entry.State == EntityState.Modified)
                AuditarEntidadeAtualizada(entry, auditorias);

            if (entry.State == EntityState.Deleted)
                AuditarEntidadeDeletada(entry, auditorias);
        }

        protected virtual void AuditarEntidadeAdicionada(EntityEntry entry, IList<Auditoria> auditorias)
        {
            Guid entidadeID = (Guid)entry.Property($"{entry.Entity.GetType().Name}ID").CurrentValue;

            Dictionary<string, object> propertiesAndValues = GetCurrentPropertiesAndValues(entry);

            Auditoria auditoria = new Auditoria()
            {
                EntidadeID = entidadeID,
                AlteradoPor = GetUsuarioWindows,
                NomeEntidade = GetEntityName(entry),
                UltimaAcao = AcaoAuditoria.Adicionado,
                CamposAlterados = string.Join(", ", propertiesAndValues.Keys),
                ValoresAdicionados = string.Join(", ", propertiesAndValues.Values)
            };

            auditorias.Add(auditoria);
        }

        protected virtual void AuditarEntidadeAtualizada(EntityEntry entry, IList<Auditoria> auditorias)
        {
            Guid entidadeID = (Guid)entry.Property($"{entry.Entity.GetType().Name}ID").CurrentValue;

            Dictionary<string, object> propertiesAndValues = GetModifiedPropertiesAndValues(entry);

            Auditoria auditoria = new Auditoria()
            {
                EntidadeID = entidadeID,
                AlteradoPor = GetUsuarioWindows,
                NomeEntidade = GetEntityName(entry),
                UltimaAcao = AcaoAuditoria.Alterado,
                CamposAlterados = string.Join(", ", propertiesAndValues.Keys),
                ValoresAdicionados = string.Join(", ", propertiesAndValues.Values)
            };

            auditorias.Add(auditoria);
        }

        //TODO: Colocar uma maneira de pegar o id da entidade por um atributo key ou ver se o entity framework tem
        protected virtual void AuditarEntidadeDeletada(EntityEntry entry, IList<Auditoria> auditorias)
        {
            //IKey entidadeID = entry.Metadata.FindPrimaryKey();
            Guid entidadeID = (Guid)entry.Property($"{entry.Entity.GetType().Name}ID").CurrentValue;

            Dictionary<string, object> propertiesAndValues = GetOriginalPropertiesAndValues(entry);

            Auditoria auditoria = new Auditoria()
            {
                EntidadeID = entidadeID,
                AlteradoPor = GetUsuarioWindows,
                NomeEntidade = GetEntityName(entry),
                UltimaAcao = AcaoAuditoria.Excluido,
                CamposAlterados = string.Join(", ", propertiesAndValues.Keys),
                ValoresAdicionados = string.Join(", ", propertiesAndValues.Values)
            };

            auditorias.Add(auditoria);
        }

        /// <summary>
        /// Retorna um key pair com o nome das propriedades e os valores atuais da entidade
        /// </summary>
        /// <param name="entry">Entidade que vai ser manipulada no banco</param>
        /// <returns>Retorna todas as proprieades e valores atuais</returns>
        private Dictionary<string, object> GetCurrentPropertiesAndValues(EntityEntry entry)
        {
            Dictionary<string, object> propertiesAndValues = new Dictionary<string, object>();

            foreach (IProperty property in entry.CurrentValues.Properties)
            {
                propertiesAndValues.Add(property.Name, entry.CurrentValues[property]);
            }

            return propertiesAndValues;
        }

        /// <summary>
        /// Retorna um key pair com o nome das propriedades e os valores que foram modificados da entidade
        /// </summary>
        /// <param name="entry">Entidade que vai ser manipulada no banco</param>
        /// <returns>Retorna todas as propriedades e valores modificados</returns>
        private Dictionary<string, object> GetModifiedPropertiesAndValues(EntityEntry entry)
        {
            Dictionary<string, object> propertiesAndValues = new Dictionary<string, object>();

            foreach (PropertyEntry property in entry.Properties.Where(p => p.IsModified))
            {
                propertiesAndValues.Add(property.Metadata.Name, entry.CurrentValues[property.Metadata]);
            }

            return propertiesAndValues;
        }

        /// <summary>
        /// Retorna um key pair com o nome das propriedades e os valores que vieram do banco com a entidade
        /// </summary>
        /// <param name="entry">Entidade que vai ser manipulada no banco</param>
        /// <returns>Retorna todas as propriedaes e valores recuperados do banco</returns>
        private Dictionary<string, object> GetOriginalPropertiesAndValues(EntityEntry entry)
        {
            Dictionary<string, object> propertiesAndValues = new Dictionary<string, object>();

            foreach (IProperty property in entry.OriginalValues.Properties)
            {
                propertiesAndValues.Add(property.Name, entry.OriginalValues[property]);
            }

            return propertiesAndValues;
        }

        /// <summary>
        /// Pega o nome da Entidade atual
        /// </summary>
        /// <param name="entry">Entidade que vai ser manipulada no banco</param>
        /// <returns>Retorna o nome da entidade</returns>
        private string GetEntityName(EntityEntry entry)
        {
            return entry.Entity.GetType().Name ?? entry.Metadata.Name;
        }
    }
}
