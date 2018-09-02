using System;
using System.Collections.Generic;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Models
{
    [Auditar]
    [Historificar]
    public class Technology
    {
        public Guid TechnologyID { get; set; } = Guid.NewGuid();

        public Guid? ResourceID { get; set; }

        public string Name { get; set; }

        #region Propriedade de navegação

        public Resource Resource { get; set; }

        public ICollection<Resource> Resources { get; private set; } = new HashSet<Resource>();

        #endregion

        public override string ToString() => Name;
    }
}
