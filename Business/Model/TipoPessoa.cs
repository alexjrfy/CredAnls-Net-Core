using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Model
{
    public class TipoPessoa : ExternalEntity
    {
        public String Descricao { get; set; }
        public String Chave { get; set; }
    }
}
