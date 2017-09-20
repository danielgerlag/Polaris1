using AppCore.Modules.Foundation.DomainModel;
using HR1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.DomainModel.Services
{
    public class AccountingEntitySetupManager : IAccountingEntitySetupManager
    {

        public void Run(IHR1Context db, AccountingEntity accountingEntity)
        {
            var amtInput = new JournalTemplateInput()
            {
                AppTenantID = accountingEntity.AppTenantID,
                Name = "Salary",
                UserInputType = db.UserInputTypes.First(x => x.Code == "CURRENCY")
            };

            var template = new JournalTemplate();
            template.AppTenantID = accountingEntity.AppTenantID;
            template.JournalType = db.JournalTypes.First(x => x.Code == "PAYSTUB");
            template.Name = "Salary";
            template.OriginKey = "EmploymentContract";
            template.UserInputs.Add(amtInput);


            var templateTxn = new JournalTemplateTxn();
            templateTxn.AppTenantID = accountingEntity.AppTenantID;
            templateTxn.AmountInput = amtInput;
            templateTxn.Description = "Salary";
            templateTxn.JournalTxnType = template.JournalType.JournalTxnTypes.First(x => x.IsCode == "AMT");

            template.JournalTemplateTxns.Add(templateTxn);

            accountingEntity.JournalTemplates.Add(template);
        }

    }
}
