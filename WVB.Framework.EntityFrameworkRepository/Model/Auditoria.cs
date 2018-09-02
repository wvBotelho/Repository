using System;

namespace WVB.Framework.EntityFrameworkRepository.Model
{
    public class Auditoria
    {
        public Guid AuditoriaID { get; set; } = Guid.NewGuid();

        public Guid? EntidadeID { get; set; }

        public string NomeEntidade { get; set; }

        public string AlteradoPor { get; set; }

        public DateTime DataAlteracao { get; set; } = DateTime.Now;

        public AcaoAuditoria UltimaAcao { get; set; }

        public string CamposAlterados { get; set; }

        public string ValoresAdicionados { get; set; }
    }

    public enum AcaoAuditoria : int
    {
        Adicionado = 0,
        Alterado = 1,
        Excluido = 2
    }
}
