using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WVB.Framework.EntityFrameworkRepository;
using WVB.Framework.EntityFrameworkRepository.Interfaces;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Models;
using WVB.Framework.EntityFrameworkRepository.UnitTest.Data;
using WVB.Framework.EntityFrameworkRepository.Enum;
using WVB.Framework.EntityFrameworkRepository.Model;
using WVB.Framework.EntityFrameworkRepository.UnitTest.DTOs;

namespace WVB.Framework.Repository.UnitTest
{
    public class RepositortyTest
    {
        #region Propriedades

        private static TestContext Context { get; set; }

        private static IConfigurationRoot Configuration { get; set; }

        private Project GetNewProject
        {
            get
            {
                return new Project()
                {
                    Name = "WVB.Framework.Repository",
                    Description = "Repositório genérico utilizando a API Entity Framework Core."
                };
            }
        }

        private ProjectDetail GetNewDetail
        {
            get
            {
                return new ProjectDetail()
                {
                    Budget = 50M,
                    Critical = true
                };
            }
        }

        private Customer GetNewCustomer
        {
            get
            {
                return new Customer()
                {
                    Name = "Wagner Vinicius Botelho",
                    Contact = GetNewContact
                };
            }
        }

        private Resource GetNewResource
        {
            get
            {
                return new Resource()
                {
                    Name = "Microsoft",
                    Contact = GetNewContact
                };
            }
        }

        private Technology GetNewTechnology
        {
            get
            {
                return new Technology()
                {
                    Name = "Visual Studio 2017"
                };
            }
        }

        private ContactInformation GetNewContact
        {
            get
            {
                return new ContactInformation()
                {
                    Email = "wagnerbotelho24@gmail.com",
                    Phone = "94550-5151"
                };
            }
        }

        private HashSet<ProjectResource> GetNewProjectResouces
        {
            get
            {
                return new HashSet<ProjectResource>()
                {
                    new ProjectResource()
                    {
                        Resource = GetNewResource,
                        Role = Role.ProjectManager
                    },
                    new ProjectResource()
                    {
                        Resource = new Resource(){ Name = "Visual Studio 2017", Contact = GetNewContact },
                        Role = Role.Developer
                    },
                    new ProjectResource()
                    {
                        Resource = new Resource(){ Name = "SQL Server Management Studio 2017", Contact = GetNewContact },
                        Role = Role.Developer
                    }
                };
            }
        }

        #endregion        

        public RepositortyTest()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false)
                .Build();

            Context = new TestContext(Database.SqlServer, Configuration.GetConnectionString("RepositoryConnectionString"));
        }

        private static GenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(Context);
        }

        [Fact]
        public void Pode_Adicionar_Projeto()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                Project project = GetNewProject;
                project.Detail = GetNewDetail;
                project.Customer = GetNewCustomer;
                project.ProjectResources = GetNewProjectResouces;

                repository.Create(project);

                Assert.True(repository.SaveChanges());
            }
        }

        [Fact]
        public void Pode_Atualizar_Projeto()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                Project projetoCompleto = repository.GetList()
                    .Include(p => p.Detail)
                    .Include(p => p.ProjectResources)
                    .FirstOrDefault();

                projetoCompleto.End = DateTime.Today.AddMonths(5);
                projetoCompleto.Detail.Budget = 150M;
                projetoCompleto.Detail.Critical = true;

                repository.Update(projetoCompleto);

                Assert.True(repository.SaveChanges());
            }
        }

        [Fact]
        public void Pode_Atualizar_2()
        {
            using (IGenericRepository<Customer> repository = GetRepository<Customer>())
            {
                Customer customer = repository.GetList().LastOrDefault();

                customer.Name = "Nome alterado";

                repository.Update(customer);

                Assert.True(repository.SaveChanges());
            }
        }

        [Fact]
        public void Pode_Deletar_Projeto_logicamente()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                Project projectDeleted = repository.GetList()
                    .Include(p => p.ProjectResources)
                    .LastOrDefault();

                repository.Delete(projectDeleted);

                Assert.True(repository.SaveChanges());
            }
        }

        [Fact]
        public void Nao_Pode_Consultar_Projeto_Deletado()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                 IEnumerable<Project> projects = repository.GetList();

                //Assert.Equal(1, projects.Count());
                Assert.Single(projects);
            }
        }

        [Fact]
        public void Pode_Retornar_Todos_Registros()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                IEnumerable<Project> allProjects = repository.GetHistory();

                Assert.Equal(2, allProjects.Count());
            }
        }

        [Fact]
        public void Pode_Auditar_Alteracao_De_Registros()
        {
            using (IGenericRepository<Auditoria> repository = GetRepository<Auditoria>())
            {
                IList<AuditoriaDTO> projetosAlterados = repository
                    .Where(p => p.UltimaAcao == AcaoAuditoria.Alterado)
                    .OrderByDescending(p => p.DataAlteracao)
                    .Select(p => new AuditoriaDTO()
                    {
                        EntidadeID = p.EntidadeID,
                        NomeEntidade = p.NomeEntidade,
                        AlteradoPor = p.AlteradoPor,
                        DataAlteracao = p.DataAlteracao,
                        CamposAlterados = p.CamposAlterados,
                        ValoresAdicionados = p.ValoresAdicionados
                    }).ToList();

                Assert.Equal(2, projetosAlterados.Count);
            }
        }

        [Fact]
        public void Pode_Auditar_Exclusao_De_Registros()
        {
            using (IGenericRepository<Auditoria> repository = GetRepository<Auditoria>())
            {
                var projetosExcluidos = repository
                    .Where(p => p.UltimaAcao == AcaoAuditoria.Excluido)
                    .OrderByDescending(p => p.DataAlteracao)
                    .Select(p => new { p.EntidadeID, p.NomeEntidade, p.AlteradoPor, p.DataAlteracao })
                    .ToList();

                Assert.Equal(1, projetosExcluidos.Count());
            }
        }

        [Fact]
        public void Table_Splitting_Projeto_Sem_Detalhes()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                Project projetoSemDetalhe = repository.GetList().LastOrDefault();

                Assert.Null(projetoSemDetalhe.Detail);
            }
        }

        [Fact]
        public void Table_Splitting_Projeto_Com_detalhes()
        {
            using (IGenericRepository<Project> repository = GetRepository<Project>())
            {
                Project projetoComDetalhe = repository.GetList().Include(p => p.Detail).LastOrDefault();

                Assert.NotNull(projetoComDetalhe.Detail);
            }
        }
    }
}
