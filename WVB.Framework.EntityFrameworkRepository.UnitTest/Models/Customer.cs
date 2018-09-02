using System;
using System.Collections.Generic;
using WVB.Framework.EntityFrameworkRepository.CustomAttributes;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Models
{
    [Auditar]
    [Historificar]
    public class Customer
    {
        public Guid CustomerID { get; set; } = Guid.NewGuid();

        public Guid ProjectID { get; set; }

        public string Name { get; set; }

        #region Propriedades de navegação

        public ContactInformation Contact { get; set; }

        public ICollection<Project> Projects { get; private set; } = new HashSet<Project>();

        #endregion

        public override string ToString() => Name;
    }
}
