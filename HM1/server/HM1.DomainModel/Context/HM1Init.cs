using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel.Context
{
    public class HM1Init : CreateDatabaseIfNotExists<HM1Context>
    {

        protected override void Seed(HM1Context context)
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
            var jtSplitExpense = new JournalType() { Name = "Split Expense", Code = "SEXP" };

            context.JournalTypes.Add(jtGeneral);
            context.JournalTypes.Add(jtSplitExpense);
            context.JournalTypes.Add(new JournalType() { Name = "Payment", Code = "PAY" });


            jtGeneral.JournalTxnTypes.Add(new JournalTxnType() { Name = "Defined Amount", PrimaryFactorCode = "AMT", SecondaryFactorCode = null });
            jtGeneral.JournalTxnTypes.Add(new JournalTxnType() { Name = "Percentage of ledger account", PrimaryFactorCode = "PERC", SecondaryFactorCode = "LEDGER" });


            jtSplitExpense.JournalTxnTypes.Add(new JournalTxnType() { Name = "Defined Amount", PrimaryFactorCode = "AMT", SecondaryFactorCode = null });
            jtSplitExpense.JournalTxnTypes.Add(new JournalTxnType() { Name = "Participation", PrimaryFactorCode = "PART", SecondaryFactorCode = "AMT" });


            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "Defined Amount", IsDailyCalc = false, IsDefinedAmount = true, IsPercentage = false, OfContextParameter = false, OfCoveragePremium = false, OfLedgerAccount = false });
            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "% of Coverage Premium", IsDailyCalc = false, IsDefinedAmount = false, IsPercentage = true, OfContextParameter = false, OfCoveragePremium = true, OfLedgerAccount = false });
            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "% of Ledger Account", IsDailyCalc = false, IsDefinedAmount = false, IsPercentage = true, OfContextParameter = false, OfCoveragePremium = false, OfLedgerAccount = true });
            //context.JournalTxnClasses.Add(new JournalTxnClass() { Description = "% of Contextual Parameter", IsDailyCalc = false, IsDefinedAmount = false, IsPercentage = true, OfContextParameter = true, OfCoveragePremium = false, OfLedgerAccount = false });




            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "CA", Name = "Current Asset", IsAsset = true, IsCurrent = true, CreditPositive = false });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "NCA", Name = "Non-Current Asset", IsAsset = true, IsCurrent = false, CreditPositive = false });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "CL", Name = "Current Liability", IsLiability = true, IsCurrent = true, CreditPositive = true });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "NCL", Name = "Non-Current Liability", IsLiability = true, IsCurrent = false, CreditPositive = true });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "E", Name = "Expense", IsExpense = true, IsCurrent = true, CreditPositive = false });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "I", Name = "Income", IsIncome = true, IsCurrent = true, CreditPositive = true });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "D", Name = "Debtors", IsAsset = true, IsDebtor = true, IsCurrent = true, CreditPositive = false });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "C", Name = "Creditors", IsLiability = true, IsCredior = true, IsCurrent = true, CreditPositive = true });
            context.LedgerAccountTypes.Add(new LedgerAccountType() { Code = "EQ", Name = "Equity", IsEquity = true, CreditPositive = true });

            context.Regions.Add(new Region() { Code = "BC", Name = "British Columbia" });

            context.Ledgers.Add(new Ledger() { Code = "FIN", Name = "Financial Ledger" });

            base.Seed(context);
        }



    }
}
