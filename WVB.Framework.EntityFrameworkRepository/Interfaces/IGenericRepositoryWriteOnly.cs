using System.Threading.Tasks;

namespace WVB.Framework.EntityFrameworkRepository.Interfaces
{
    /// <summary>
    /// Interface base para as operações básicas de escrita no banco de dados
    /// </summary>
    /// <typeparam name="TEntity">Classe que representa a entidade de dominio que será manipulada no banco</typeparam>
    public interface IGenericRepositoryWriteOnly<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adiciona a entidade no banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que será adicionada</param>
        void Create(TEntity entity);

        /// <summary>
        /// Adiciona a entidade no banco de dados assincronamente. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que será adicionado</param>
        Task CreateAsync(TEntity entity);

        /// <summary>
        /// Remove a entidade do banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que será removida</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Remove a entidade do banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        void Delete(dynamic entityID);

        /// <summary>
        /// Remove a entidade do banco de dados assincronamente. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        Task DeleteAsync(dynamic entityID);

        /// <summary>
        /// Atualiza as informações da entidade no banco de dados. É necessário invocar o Método SaveChanges para a alteração ser executada definitivamente no banco
        /// </summary>
        /// <param name="entity">Entidade que terá as informações de registro atualizadas</param>
        void Update(TEntity entity);

        /// <summary>
        /// Persiste todas as alterações no banco de dados
        /// </summary>
        /// <returns>Retorna true se operação foi realizada com sucesso</returns>
        bool SaveChanges();

        /// <summary>
        /// Persiste todas as alterações no banco de dados assincronamente
        /// </summary>
        /// <returns>Retorna true se operação foi realizada com sucesso</returns>
        Task<bool> SaveChangesAsync();
    }
}
