using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Models
{
    [Auditar]
    [Historificar]
    public class Project : IValidatableObject
    {
        public Guid ProjectID { get; set; } = Guid.NewGuid();

        public Guid? CustomerID { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; } = DateTime.Now;

        public DateTime? End { get; set; }

        public IEnumerable<ProjectResource> Testers => ProjectResources.Where(t => t.Role == Role.Tester);

        public IEnumerable<ProjectResource> Developers => ProjectResources.Where(d => d.Role == Role.Developer);

        public ProjectResource ProjectManager => ProjectResources.SingleOrDefault(p => p.Role == Role.ProjectManager);

        #region Propriedades de navegação

        public ProjectDetail Detail { get; set; }

        public Customer Customer { get; set; }

        public ICollection<ProjectResource> ProjectResources { get; set; } = new HashSet<ProjectResource>();

        #endregion

        public override string ToString() => Name;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProjectManager == null)
                yield return new ValidationResult("Nenhum gerente de projeto definido.");

            if (!Developers.Any())
                yield return new ValidationResult("Nenhum desenvolvedore definido.");

            if (End != null && End <= DateTime.MinValue)
                yield return new ValidationResult("Data de fechamento do projeto deve ser uma data válida.");

            if (End != null && End < Start)
                yield return new ValidationResult("Data de fechamento do projeto não pode ser menor que a data de entrada.", new[] { "End" });
        }
    }

    public class ProjectDetail : IValidatableObject
    {
        public Guid ProjectID { get; set; }

        public decimal Budget { get; set; }

        public bool Critical { get; set; }

        #region Propriedade de Navegação

        public Project Project { get; set; }

        #endregion

        public override string ToString() => $"{Project.Name} - R$ {Budget}, Critical: {Critical}";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Budget < 0)
                yield return new ValidationResult("Valor de despesas não pode ser menor que zero.", new[] { "Budget" });
        }
    }
}
