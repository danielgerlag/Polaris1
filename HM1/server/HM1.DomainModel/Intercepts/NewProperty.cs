using AppCore.DomainModel.Abstractions.Intercepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;

namespace HM1.DomainModel.Intercepts
{
    public class NewProperty : EntityIntercept<AccountingEntity>
    {
        public NewProperty()
        {

        }

        public override void Run(AccountingEntity entity, IDbContext dataContext)
        {

            var debtorAcct = new LedgerAccount()
            {
                AppTenantID = entity.AppTenantID,
                Ledger = dataContext.Set<Ledger>().Single(x => x.Code == "FIN"),
                LedgerAccountType = dataContext.Set<LedgerAccountType>().Single(x => x.Code == "D"),
                Name = "Contributions Due"
            };

            var creditorAcct = new LedgerAccount()
            {
                AppTenantID = entity.AppTenantID,
                Ledger = dataContext.Set<Ledger>().Single(x => x.Code == "FIN"),
                LedgerAccountType = dataContext.Set<LedgerAccountType>().Single(x => x.Code == "C"),
                Name = "Services Payable"
            };

            var expenseAcct = new LedgerAccount()
            {
                AppTenantID = entity.AppTenantID,
                Ledger = dataContext.Set<Ledger>().Single(x => x.Code == "FIN"),
                LedgerAccountType = dataContext.Set<LedgerAccountType>().Single(x => x.Code == "E"),
                Name = "General Expense"
            };
            
            //entity.LedgerAccounts.Add();

            var expenseTemplate = CreateExpenseTemplate(entity, dataContext, creditorAcct, expenseAcct);
            entity.JournalTemplates.Add(expenseTemplate);
            
        }



        private JournalTemplate CreateExpenseTemplate(AccountingEntity entity, IDbContext dataContext, LedgerAccount payableAcct, LedgerAccount expenseAcct)
        {
            var template = new JournalTemplate();
            template.AppTenantID = entity.AppTenantID;
            template.JournalType = dataContext.Set<JournalType>().Single(x => x.Code == "SEXP");
            template.OriginKey = "ProviderAccount";
            template.Name = "Service Expense";
            
            JournalTemplateInput refInput = new JournalTemplateInput();
            refInput.Name = "Reference";
            refInput.AppTenantID = entity.AppTenantID;
            refInput.Description = "";
            refInput.UserInputType = dataContext.Set<UserInputType>().Single(x => x.Code == "TEXT");            
            template.UserInputs.Add(refInput);

            JournalTemplateInput amtInput = new JournalTemplateInput();
            amtInput.Name = "Amount";
            amtInput.AppTenantID = entity.AppTenantID;
            amtInput.Description = "";
            amtInput.UserInputType = dataContext.Set<UserInputType>().Single(x => x.Code == "CURRENCY");
            template.UserInputs.Add(amtInput);
            
            //template.ReferenceInput = refInput;

            JournalTemplateTxn mainTxn = new JournalTemplateTxn();
            mainTxn.AppTenantID = entity.AppTenantID;
            mainTxn.AmountInput = amtInput;
            mainTxn.Description = "Service Expense";
            mainTxn.PrimaryFactorSource = "INPUT";
            mainTxn.IncludeInTotal = true;
            mainTxn.JournalTxnType = template.JournalType.JournalTxnTypes.Single(x => x.PrimaryFactorCode == "AMT" && x.SecondaryFactorCode == null);
            mainTxn.Postings.Add(new JournalTemplateTxnPosting() { AppTenantID = entity.AppTenantID, AddBaseAmount = true, LedgerAccount = payableAcct, PostType = "C" });
            mainTxn.Postings.Add(new JournalTemplateTxnPosting() { AppTenantID = entity.AppTenantID, AddBaseAmount = true, LedgerAccount = expenseAcct, PostType = "D" });
            template.JournalTemplateTxns.Add(mainTxn);


            JournalTemplateTxn splitTxn = new JournalTemplateTxn();
            splitTxn.AppTenantID = entity.AppTenantID;
            splitTxn.AmountInput = amtInput;
            splitTxn.Description = "Split";
            splitTxn.SecondaryFactorSource = "INPUT";
            splitTxn.IncludeInTotal = false;
            splitTxn.JournalTxnType = template.JournalType.JournalTxnTypes.Single(x => x.PrimaryFactorCode == "PART" && x.SecondaryFactorCode == "AMT");
            template.JournalTemplateTxns.Add(splitTxn);

            dataContext.Set<JournalTemplate>().Add(template);
            return template;
        } 

    }
}
