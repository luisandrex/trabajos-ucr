﻿using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories.UserAdministration
{
    public interface IFuncionarioRepository
    {
        Task<List<Funcionario>> GetAllAsync();
    }
}
