using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WVB.Framework.EntityFrameworkRepository.Configuration;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes;
using WVB.Framework.EntityFrameworkRepository.Interfaces;

namespace WVB.Framework.EntityFrameworkRepository
{
    /// <summary>
    /// Classe base para todas as operações básicas de consulta e manipulação no banco de dados
    /// </summary>
    /// <typeparam name="TEntity">Classe que representa a entidade de dominio que será manipulada no banco</typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public ProjectContext Context { get; set; }

        protected DbSet<TEntity> Entities { get; set; }

        /// <summary>
        /// Construtor que recebe na assinatura um DBContext
        /// </summary>
        /// <param name="context">Contexto do banco</param>
        public GenericRepository(ProjectContext context)
        {
            Context = context;
            Entities = context.Set<TEntity>();
        }

        /// <summary>
        /// Adiciona a entidade no banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que será adicionada</param>
        public virtual void Create(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException($"Entidade '{nameof(TEntity)}' nula.");

            Entities.Add(entity);
        }

        /// <summary>
        /// Adiciona a entidade no banco de dados assincronamente. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que será adicionado</param>
        public async Task CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException($"Entidade '{nameof(TEntity)}' nula.");

            await Entities.AddAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove a entidade do banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que será removida</param>
        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException($"Entidade '{nameof(TEntity)}' nula.");

            Entities.Remove(entity);
        }

        /// <summary>
        /// Remove a entidade do banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        public void Delete(dynamic entityID)
        {
            TEntity entity = Find(entityID);

            Delete(entity);
        }

        /// <summary>
        /// Remove a entidade do banco de dados assincronamente. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        public async Task DeleteAsync(dynamic entityID)
        {
            TEntity entity = await FindAsync(entityID).ConfigureAwait(false);

            Delete(entity);
        }

        /// <summary>
        /// Atualiza as informações da entidade no banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que terá as informações de registro atualizadas</param>
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException($"Entidade '{nameof(TEntity)}' nula.");

            Entities.Update(entity);
        }

        /// <summary>
        /// Persiste todas as alterações no banco de dados
        /// </summary>
        /// <returns>Retorna true se operação foi realizada com sucesso</returns>
        public bool SaveChanges()
        {
            try
            {
                return Context.SaveChanges() > 0;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
        }

        /// <summary>
        /// Persiste todas as alterações no banco de dados assincronamente
        /// </summary>
        /// <returns>Retorna true se operação foi realizada com sucesso</returns>
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (DbUpdateException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
        }

        /// <summary>
        /// Encontra um registro de uma tabela no banco de dados
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        /// <returns>Retorna, se existir, o registro no banco de dados</returns>
        public TEntity Find(dynamic entityID)
        {
            return Entities.Find(entityID);
        }

        /// <summary>
        /// Encontra um registro de uma tabela no banco de dados assincronamente
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        /// <returns>Retorna, se existir, o registro no banco de dados</returns>
        public async Task<TEntity> FindAsync(dynamic entityID)
        {
            return await Entities.FindAsync(entityID).ConfigureAwait(false);
        }

        /// <summary>
        /// Retorna todos os registro de uma tabela no banco de dados
        /// </summary>
        /// <returns>Retorna uma lista de registros</returns>
        public IQueryable<TEntity> GetList()
        {
            return Entities;
        }

            /// <summary>
            /// Retorna todos os registro de uma tabela historificada (registros normais e os que foram excluidos lógicamente). 
            /// Para isso a entidade tem que ter o atributo Historificar definido na sua classe, caso contrário, o método retorna a lista inteira
            /// </summary>
            /// <returns>Retorna uma lista com todos os registros</returns>
        public IEnumerable<TEntity> GetHistory()
        {
            HistorificarAttribute historificarAttribute = (HistorificarAttribute)Attribute.GetCustomAttribute(Entities.FirstOrDefault().GetType(),
                typeof(HistorificarAttribute));

            if (historificarAttribute == null)
                return Entities;

            return Where(e => EF.Property<bool>(e, "deletado") || !EF.Property<bool>(e, "deletado")).IgnoreQueryFilters();
        }

        /// <summary>
        /// Retorna todos os registros de uma tabela, mediante condição  
        /// </summary>
        /// <param name="condicao">Condição que será usada para filtrar os registros</param>d
        /// <returns>Retorna uma lista de registros</returns>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> condicao)
        {
            return Entities.Where(condicao);
        }

        /// <summary>
        /// Executa uma store procedure do banco
        /// </summary>
        /// <param name="storeProcedure">Nome da Store Procedure</param>
        /// <param name="parameters">Parâmetros da procedure. Opcional</param>
        /// <returns>Retorna uma lista de registros</returns>
        public IEnumerable<TEntity> ExecuteStoreProcedure(string storeProcedure, params object[] parameters)
        {
            return Entities.FromSql(storeProcedure, parameters);
        }

        /// <summary>
        /// Libera os recursos do objeto
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera os recursos do objeto
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Context.Dispose();
        }
    }
}
