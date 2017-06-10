namespace NLayerApp.Application.MainBoundedContext.BankingModule.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Application.Seedwork;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Infrastructure.Crosscutting.Validator;
    using NLayerApp.Infrastructure.Crosscutting.Localization;
    using Microsoft.Extensions.Logging;
    using System.Transactions;

    /// <summary>
    /// The bank management service implementation
    /// </summary>
    public class BankAppService
        : IBankAppService
    {
        #region Members

        readonly IBankAccountRepository _bankAccountRepository;
        readonly ICustomerRepository _customerRepository;
        readonly IBankTransferService _transferService;

        readonly ILogger _logger;
        readonly ILocalization _resources;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance 
        /// </summary>
        public BankAppService(IBankAccountRepository bankAccountRepository, // the bank account repository dependency
                              ICustomerRepository customerRepository, // the customer repository dependency
                              IBankTransferService transferService,
                              ILogger<BankAppService> logger)
        {
            //check preconditions
            if (bankAccountRepository == null)
                throw new ArgumentNullException("bankAccountRepository");

            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");

            if (transferService == null)
                throw new ArgumentNullException("trasferService");

            _bankAccountRepository = bankAccountRepository;
            _customerRepository = customerRepository;
            _transferService = transferService;

            _logger = logger;
            _resources = LocalizationFactory.CreateLocalResources();

        }

        #endregion

        #region IBankAppService Members

        public BankAccountDTO AddBankAccount(BankAccountDTO bankAccountDTO)
        {
            if (bankAccountDTO == null || bankAccountDTO.CustomerId == Guid.Empty)
                throw new ArgumentException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotAddNullBankAccountOrInvalidCustomer));

            //check if exists the customer for this bank account
            var associatedCustomer = _customerRepository.Get(bankAccountDTO.CustomerId);

            if (associatedCustomer != null) // if the customer exist
            {
                //Create a new bank account  number
                var accountNumber = CalculateNewBankAccountNumber();

                //Create account from factory 
                var account = BankAccountFactory.CreateBankAccount(associatedCustomer, accountNumber);

                //save bank account
                SaveBankAccount(account);

                return account.ProjectedAs<BankAccountDTO>();
            }
            else //the customer for this bank account not exist, cannot create a new bank account
                throw new InvalidOperationException(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotCreateBankAccountForNonExistingCustomer));


        }

        public bool LockBankAccount(Guid bankAccountId)
        {
            //recover bank account, lock and commit changes
            var bankAccount = _bankAccountRepository.Get(bankAccountId);

            if (bankAccount != null)
            {
                bankAccount.Lock();

                _bankAccountRepository.UnitOfWork.Commit();

                return true;
            }
            else // if not exist the bank account return false
            {
                _logger.LogWarning(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotLockNonExistingBankAccount), bankAccountId);

                return false;
            }
        }

        public List<BankAccountDTO> FindBankAccounts()
        {
            var bankAccounts = _bankAccountRepository.GetAll();

            if (bankAccounts != null
                &&
                bankAccounts.Any())
            {
                return bankAccounts.ProjectedAsCollection<BankAccountDTO>();
            }
            else // no results
                return null;
        }

        public void PerformBankTransfer(BankAccountDTO fromAccount, BankAccountDTO toAccount, decimal amount)
        {
            //Application-Logic Process: 
            // 1º Get Accounts objects from Repositories
            // 2º Start Transaction
            // 3º Call PerformTransfer method in Domain Service
            // 4º If no exceptions, commit the unit of work and complete transaction

            if (BankAccountHasIdentity(fromAccount)
                &&
                BankAccountHasIdentity(toAccount))
            {
                var source = _bankAccountRepository.Get(fromAccount.Id);
                var target = _bankAccountRepository.Get(toAccount.Id);

                if (source != null & target != null) // if all accounts exist
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        //perform transfer
                        _transferService.PerformTransfer(amount, source, target);

                        //comit unit of work
                        _bankAccountRepository.UnitOfWork.Commit();

                        //complete transaction
                        scope.Complete();
                    }
                }
                else
                    _logger.LogError(_resources.GetStringResource(LocalizationKeys.Application.error_CannotPerformTransferInvalidAccounts));
            }
            else
                _logger.LogError(_resources.GetStringResource(LocalizationKeys.Application.error_CannotPerformTransferInvalidAccounts));

        }
        /// <summary>
        /// <see cref="NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/>
        /// </summary>
        /// <param name="bankAccountId"><see cref="NLayerApp.Application.MainBoundedContext.ERPModule.Services.IBankManagementService"/></param>
        /// <returns><see cref="NLayerApp.Application.MainBoundedContext.ERPModule.Services.IBankManagementService"/></returns>
        public List<BankActivityDTO> FindBankAccountActivities(Guid bankAccountId)
        {
            var account = _bankAccountRepository.Get(bankAccountId);

            if (account != null)
            {
                return account.BankAccountActivity
                              .ProjectedAsCollection<BankActivityDTO>();
            }
            else // the bank account not exist
            {
                _logger.LogWarning(_resources.GetStringResource(LocalizationKeys.Application.warning_CannotGetActivitiesForInvalidOrNotExistingBankAccount));
                return null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //dispose all resources
            _bankAccountRepository.Dispose();
            _customerRepository.Dispose();
        }

        #endregion

        #region Private Methods

        void SaveBankAccount(BankAccount bankAccount)
        {
            //validate bank account
            var validator = EntityValidatorFactory.CreateValidator();

            if (validator.IsValid<BankAccount>(bankAccount)) // save entity
            {
                _bankAccountRepository.Add(bankAccount);
                _bankAccountRepository.UnitOfWork.Commit();
            }
            else //throw validation errors
                throw new ApplicationValidationErrorsException(validator.GetInvalidMessages(bankAccount));
        }

        BankAccountNumber CalculateNewBankAccountNumber()
        {
            var bankAccountNumber = new BankAccountNumber();

            //simulate bank account number creation....

            bankAccountNumber.OfficeNumber = "2354";
            bankAccountNumber.NationalBankCode = "2134";
            bankAccountNumber.CheckDigits = "02";
            bankAccountNumber.AccountNumber = new Random().Next(1, Int32.MaxValue).ToString();

            return bankAccountNumber;

        }

        bool BankAccountHasIdentity(BankAccountDTO bankAccountDTO)
        {
            //return true is bank account dto has identity
            return (bankAccountDTO != null && bankAccountDTO.Id != Guid.Empty);
        }

        #endregion
    }
}
