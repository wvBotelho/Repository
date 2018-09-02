using System;

namespace WVB.Framework.EntityFrameworkRepository.UnitTest.DTOs
{
    public class AuditoriaDTO
    {
        public Guid? EntidadeID { get; set; }

        public string NomeEntidade { get; set; }

        public string AlteradoPor { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string CamposAlterados { get; set; }

        public string ValoresAdicionados { get; set; }
    }
}
