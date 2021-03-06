﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Contracts;
using CarRental.Business.Entities;

namespace CarRental.Data.Contracts
{
    public interface IAccountRepository : IDatRepository<Account>
    {
        Account GetByLogin(string login);
    }
}
