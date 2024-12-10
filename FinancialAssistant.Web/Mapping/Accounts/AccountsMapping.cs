﻿using FinancialAssistant.DataAccess.Model;
using FinancialAssistant.DataTransfer.Account;

namespace FinancialAssistant.Web.Mapping.Accounts;

public static class AccountsMapping
{
    public static Account ToModel(this AddFirstAccountDto addFirstAccount)
        => new()
        {
            UserId = addFirstAccount.UserId, 
            Balance = 0, 
            Name = addFirstAccount.Name, 
            IsDefault = true
        };
}