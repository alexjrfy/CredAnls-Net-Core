﻿using ApplicationCore.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IPessoaRepository: IRepository<Pessoa>
    {
        Task<Pessoa> ChecarPessoa(double numeroDocumento, string chave);
        Task<Pessoa> GetDadosPessoa(double numeroDocumento, string chave);
        Task<List<Pessoa>> GetDadosPessoasGrupo(Guid idGrupo);
    }
}
