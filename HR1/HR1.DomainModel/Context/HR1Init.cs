using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.DomainModel.Context
{
    public class HR1Init : CreateDatabaseIfNotExists<HR1Context>
    {

        protected override void Seed(HR1Context context)
        {

            UserInputType dtText = new UserInputType() { Description = "Text", Code = "TEXT" };
            UserInputType dtWholeNumber = new UserInputType() { Description = "WholeNumber", Code = "INT" };
            UserInputType dtDecimal = new UserInputType() { Description = "Currency", Code = "CURRENCY" };
            UserInputType dtPercentage = new UserInputType() { Description = "Percentage", Code = "PCT" };
            UserInputType dtDate = new UserInputType() { Description = "Date", Code = "DATE" };
            UserInputType dtList = new UserInputType() { Description = "List", Code = "LIST" };
            UserInputType dtYear = new UserInputType() { Description = "Year", Code = "YEAR" };
            UserInputType dtBoolean = new UserInputType() { Description = "Boolean", Code = "BOOL" };            

            context.UserInputTypes.Add(dtText);
            context.UserInputTypes.Add(dtWholeNumber);
            context.UserInputTypes.Add(dtDecimal);
            context.UserInputTypes.Add(dtPercentage);
            context.UserInputTypes.Add(dtDate);
            context.UserInputTypes.Add(dtList);
            context.UserInputTypes.Add(dtYear);
            context.UserInputTypes.Add(dtBoolean);            


            var jtGeneral = new JournalType() { Name = "General", Code = "GEN" };
            var jtPaystub = new JournalType() { Name = "Pay Stub", Code = "PAYSTUB" };

            context.JournalTypes.Add(jtGeneral);
            context.JournalTypes.Add(jtPaystub);
            context.JournalTypes.Add(new JournalType() { Name = "Payment", Code = "PAY" });

            
            jtGeneral.JournalTxnTypes.Add(new JournalTxnType() { Name = "Defined Amount", IsCode = "AMT", OfCode = "null" });
            jtGeneral.JournalTxnTypes.Add(new JournalTxnType() { Name = "Percentage of ledger account", IsCode = "PERC", OfCode = "LEDGER" });


            jtPaystub.JournalTxnTypes.Add(new JournalTxnType() { Name = "Defined Amount", IsCode = "AMT", OfCode = "null" });


            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "Defined Amount", IsDailyCalc = false, IsDefinedAmount = true, IsPercentage = false, OfContextParameter = false, OfCoveragePremium = false, OfLedgerAccount = false });
            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "% of Coverage Premium", IsDailyCalc = false, IsDefinedAmount = false, IsPercentage = true, OfContextParameter = false, OfCoveragePremium = true, OfLedgerAccount = false });
            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "% of Ledger Account", IsDailyCalc = false, IsDefinedAmount = false, IsPercentage = true, OfContextParameter = false, OfCoveragePremium = false, OfLedgerAccount = true });
            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "% of Contextual Parameter", IsDailyCalc = false, IsDefinedAmount = false, IsPercentage = true, OfContextParameter = true, OfCoveragePremium = false, OfLedgerAccount = false });




            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Current Asset", IsAsset = true, IsCurrent = true, CreditPositive = false });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Non-Current Asset", IsAsset = true, IsCurrent = false, CreditPositive = false });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Current Liability", IsLiability = true, IsCurrent = true, CreditPositive = true });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Non-Current Liability", IsLiability = true, IsCurrent = false, CreditPositive = true });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Expense", IsExpense = true, IsCurrent = true, CreditPositive = false });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Income", IsIncome = true, IsCurrent = true, CreditPositive = true });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Debtors", IsAsset = true, IsDebtor = true, IsCurrent = true, CreditPositive = false });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Creditors", IsLiability = true, IsCredior = true, IsCurrent = true, CreditPositive = true });
            //context.LedgerAccountTypes.Add(new LedgerAccountType() { Name = "Equity", IsEquity = true, CreditPositive = true });


            base.Seed(context);
        }


                
    }
}

