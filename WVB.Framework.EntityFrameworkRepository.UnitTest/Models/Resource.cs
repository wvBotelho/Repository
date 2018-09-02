using System;
using System.Collections.Generic;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Models
{
    [Auditar]
    [Historificar]
    public class Resource
    {
        public Guid ResourceID { get; set; } = Guid.NewGuid();

        public Guid? TechnologyID { get; set; }

        public string Name { get; set; }

        #region Propriedades de navegação

        public ContactInformation Contact { get; set; }

        public ICollection<ProjectResource> ProjectResources { get; } = new HashSet<ProjectResource>();

        public ICollection<Technology> Technologies { get; set; } = new HashSet<Technology>();

        #endregion

        public override string ToString() => Name;
    }
}
