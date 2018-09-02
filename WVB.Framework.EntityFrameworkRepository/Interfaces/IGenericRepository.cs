using System;

namespace WVB.Framework.EntityFrameworkRepository.Interfaces
{
    /// <summary>
    /// Classe base para todas as operações básicas de consulta e manipulação no banco de dados
    /// </summary>
    /// <typeparam name="TEntity">Classe que representa a entidade de dominio que será manipulada no banco</typeparam>
    public interface IGenericRepository<TEntity> : IGenericRepositoryWriteOnly<TEntity>, IRepositoryGenericReadOnly<TEntity>, IDisposable where TEntity : class
    {
    }
}
