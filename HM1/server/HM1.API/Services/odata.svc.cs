//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using AppCore.API.Services;
using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Foundation.DomainModel;
using AppCore.Services.Identity.Services;
using HM1.DomainModel;
using HM1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace HM1.API.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerSession)]
    public class odata : TenantDataService<IHM1Context>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.UseVerboseErrors = true;
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.SetServiceActionAccessRule("*", ServiceActionRights.Invoke);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;

            //config.DataServiceBehavior.
        }

        protected override void InterceptChange(TenantEntity entity, UpdateOperations operations)
        {
            base.InterceptChange(entity, operations);

            if (operations == UpdateOperations.Add)
            {
                var aeProp = entity.GetType().GetProperty("AccountingEntityID");
                if (aeProp != null)
                {
                    var accountingEntity = CurrentDataSource.AccountingEntities.Single(x => x.AppTenantID == TenantFilter.AppTenantID);
                    aeProp.GetSetMethod().Invoke(entity, new object[] { accountingEntity.ID });

                }
            }
        }

        [QueryInterceptor("Parties")]
        public Expression<Func<Party, bool>> OnQueryParties()
        {
            return TenantFilter.BuildFilter<Party>();
        }

        [ChangeInterceptor("Parties")]
        public void OnChangeParty(Party entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }


        [QueryInterceptor("AccountingEntities")]
        public Expression<Func<AccountingEntity, bool>> OnQueryAccountingEntities()
        {
            return TenantFilter.BuildFilter<AccountingEntity>();
        }

        [ChangeInterceptor("AccountingEntities")]
        public void OnChangeAccountingEntity(AccountingEntity entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }


        [QueryInterceptor("Contracts")]
        public Expression<Func<Contract, bool>> OnQueryContracts()
        {
            return TenantFilter.BuildFilter<Contract>();
        }

        [ChangeInterceptor("Contracts")]
        public void OnChangeContract(Contract entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }


        [QueryInterceptor("Journals")]
        public Expression<Func<Journal, bool>> OnQueryJournals()
        {
            return TenantFilter.BuildFilter<Journal>();
        }

        [ChangeInterceptor("Journals")]
        public void OnChangeJournal(Journal entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

        [QueryInterceptor("JournalTxns")]
        public Expression<Func<JournalTxn, bool>> OnQueryJournalTxns()
        {
            return TenantFilter.BuildFilter<JournalTxn>();
        }

        [ChangeInterceptor("JournalTxns")]
        public void OnChangeJournalTxn(JournalTxn entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }





        [QueryInterceptor("JournalTemplates")]
        public Expression<Func<JournalTemplate, bool>> OnQueryJournalTemplates()
        {
            return TenantFilter.BuildFilter<JournalTemplate>();
        }

        [ChangeInterceptor("JournalTemplates")]
        public void OnChangeJournalTemplate(JournalTemplate entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

        [QueryInterceptor("JournalTemplateTxns")]
        public Expression<Func<JournalTemplateTxn, bool>> OnQueryJournalTemplateTxn()
        {
            return TenantFilter.BuildFilter<JournalTemplateTxn>();
        }

        [ChangeInterceptor("JournalTemplateTxns")]
        public void OnChangeJournalTemplateTxn(JournalTemplateTxn entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

        [QueryInterceptor("JournalTemplateTxnPostings")]
        public Expression<Func<JournalTemplateTxnPosting, bool>> OnQueryJournalTemplateTxnPosting()
        {
            return TenantFilter.BuildFilter<JournalTemplateTxnPosting>();
        }

        [ChangeInterceptor("JournalTemplateTxnPostings")]
        public void OnChangeJournalTemplateTxnPosting(JournalTemplateTxnPosting entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

        [QueryInterceptor("LedgerTxns")]
        public Expression<Func<LedgerTxn, bool>> OnQueryLedgerTxn()
        {
            return TenantFilter.BuildFilter<LedgerTxn>();
        }

        [ChangeInterceptor("LedgerTxns")]
        public void OnChange(LedgerTxn entity, UpdateOperations operations)
        {
            //InterceptChange(entity, operations);
            throw new NotSupportedException();
        }



        [QueryInterceptor("ScheduledJournals")]
        public Expression<Func<ScheduledJournal, bool>> OnQueryScheduledJournal()
        {
            return TenantFilter.BuildFilter<ScheduledJournal>();
        }

        [ChangeInterceptor("ScheduledJournals")]
        public void OnChangeScheduledJournal(ScheduledJournal entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }


        [QueryInterceptor("SequenceNumbers")]
        public Expression<Func<SequenceNumber, bool>> OnQuerySequenceNumber()
        {
            return TenantFilter.BuildFilter<SequenceNumber>();
        }

        [ChangeInterceptor("SequenceNumbers")]
        public void OnChangeSequenceNumber(SequenceNumber entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }


        [QueryInterceptor("JournalTemplateInputs")]
        public Expression<Func<JournalTemplateInput, bool>> OnQueryUserInput()
        {
            return TenantFilter.BuildFilter<JournalTemplateInput>();
        }

        [ChangeInterceptor("JournalTemplateInputs")]
        public void OnChangeUserInput(JournalTemplateInput entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }



        [QueryInterceptor("ScheduledJournalInputValues")]
        public Expression<Func<ScheduledJournalInputValue, bool>> OnQueryUserInputValue()
        {
            return TenantFilter.BuildFilter<ScheduledJournalInputValue>();
        }

        [ChangeInterceptor("ScheduledJournalInputValues")]
        public void OnChangeUserInputValue(ScheduledJournalInputValue entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }


        ////

        [QueryInterceptor("Participants")]
        public Expression<Func<Participant, bool>> OnQueryParticipant()
        {
            return TenantFilter.BuildFilter<Participant>();
        }

        [ChangeInterceptor("Participants")]
        public void OnChangeParticipant(Participant entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

        [QueryInterceptor("ProviderAccounts")]
        public Expression<Func<ProviderAccount, bool>> OnQueryProviderAccount()
        {
            return TenantFilter.BuildFilter<ProviderAccount>();
        }

        [ChangeInterceptor("ProviderAccounts")]
        public void OnChangeProviderAccount(ProviderAccount entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

        [QueryInterceptor("ProviderAccountParticipants")]
        public Expression<Func<ProviderAccountParticipant, bool>> OnQueryProviderAccountParticipant()
        {
            return TenantFilter.BuildFilter<ProviderAccountParticipant>();
        }

        [ChangeInterceptor("ProviderAccountParticipants")]
        public void OnChangeProviderAccountParticipant(ProviderAccountParticipant entity, UpdateOperations operations)
        {
            InterceptChange(entity, operations);
        }

    }
}
