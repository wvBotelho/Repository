using System;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.Models
{
    public class ProjectResource
    {
        public Guid ProjectID { get; set; }

        public Guid ResourceID { get; set; }

        public Role Role { get; set; }

        #region Propriedades de navegação

        public Resource Resource { get; set; }

        public Project Project { get; set; }

        #endregion
    }
}
