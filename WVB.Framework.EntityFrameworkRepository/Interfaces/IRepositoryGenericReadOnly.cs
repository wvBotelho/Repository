using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WVB.Framework.EntityFrameworkRepository.Interfaces
{
    /// <summary>
    /// interface base para as operações básicas de leitura no banco de dados
    /// </summary>
    /// <typeparam name="TEntity">Classe que representa a entidade de dominio que será manipulada no banco</typeparam>
    public interface IRepositoryGenericReadOnly<TEntity> where TEntity : class
    {
        /// <summary>
        /// Encontra um registro de uma tabela no banco de dados
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        /// <returns>Retorna, se existir, o registro no banco de dados</returns>
        TEntity Find(dynamic entityID);

        /// <summary>
        /// Encontra um registro de uma tabela no banco de dados assincronamente
        /// </summary>
        /// <param name="entityID">Chave primária que será usada para encontrar a entidade no banco</param>
        /// <returns>Retorna, se existir, o registro no banco de dados</returns>
        Task<TEntity> FindAsync(dynamic entityID);

        /// <summary>
        /// Retorna todos os registro de uma tabela no banco de dados
        /// </summary>
        /// <returns>Retorna uma lista de registros</returns>
        IQueryable<TEntity> GetList();

        /// <summary>
        /// Retorna todos os registro de uma tabela historificada (registros normais e os que foram excluidos lógicamente). 
        /// Para isso a entidade tem que ter o atributo Historificar definido na sua classe, caso contrário, o método retorna nulo
        /// </summary>
        /// <returns>Retorna uma lista com todos os registros</returns>
        IEnumerable<TEntity> GetHistory();

        /// <summary>
        /// Retorna todos os registros de uma tabela, mediante condição  
        /// </summary>
        /// <param name="condicao">Condição que será usada para filtrar os registros</param>
        /// <returns>Retorna uma lista de registros</returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> condicao);

        /// <summary>
        /// Executa uma store procedure do banco
        /// </summary>
        /// <param name="storeProcedure">Nome da Store Procedure</param>
        /// <param name="parameters">Parâmetros da procedure. Opcional</param>
        /// <returns>Retorna uma lista de registros</returns>
        IEnumerable<TEntity> ExecuteStoreProcedure(string storeProcedure, params object[] parameters);
    }
}
