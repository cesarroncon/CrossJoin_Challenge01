using System.ComponentModel.Design;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Challenge
{
    public static class ValidationConfig
    {
        public static ValidationRuleManager ValidationRules { get; } = new ValidationRuleManager();

        public static void Configure()
        {
            ValidationRules.SetRequired("Company", "NIF", (obj) => ((Company)obj).Country == "Portugal", true);
            ValidationRules.SetRequired("Company", "NIF", (obj) => ((Company)obj).Country == "Espanha", true);
        }

    }
}
