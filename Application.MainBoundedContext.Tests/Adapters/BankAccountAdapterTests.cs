namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Infrastructure.Crosscutting.Adapter;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class BankAccountAdapterTests 
    {
        protected TestsInitialize fixture;

        public BankAccountAdapterTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }
        [Fact]
        public void AdaptBankActivityToBankActivityDTO()
        {
            //Arrange
            BankAccountActivity activity = new BankAccountActivity();

            activity.GenerateNewIdentity();
            activity.Date = DateTime.Now;
            activity.Amount = 1000;
            activity.ActivityDescription = "transfer...";


            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var activityDTO = adapter.Adapt<BankAccountActivity, BankActivityDTO>(activity);

            //Assert
            Assert.Equal(activity.Date, activityDTO.Date);
            Assert.Equal(activity.Amount, activityDTO.Amount);
            Assert.Equal(activity.ActivityDescription, activityDTO.ActivityDescription);
        }
        [Fact]
        public void AdaptEnumerableBankActivityToListBankActivityDTO()
        {
            //Arrange
            BankAccountActivity activity = new BankAccountActivity();

            activity.GenerateNewIdentity();
            activity.Date = DateTime.Now;
            activity.Amount = 1000;
            activity.ActivityDescription = "transfer...";

            IEnumerable<BankAccountActivity> activities = new List<BankAccountActivity>() { activity };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var activitiesDTO = adapter.Adapt<IEnumerable<BankAccountActivity>, List<BankActivityDTO>>(activities);

            //Assert
            Assert.NotNull(activitiesDTO);
            Assert.True(activitiesDTO.Count() == 1);

            Assert.Equal(activity.Date, activitiesDTO[0].Date);
            Assert.Equal(activity.Amount, activitiesDTO[0].Amount);
            Assert.Equal(activity.ActivityDescription, activitiesDTO[0].ActivityDescription);
        }
        [Fact]
        public void AdaptBankAccountToBankAccountDTO()
        {
            //Arrange
            var country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo","+3441","company", country, new Address("", "", "", ""));
            customer.GenerateNewIdentity();

            BankAccount account = new BankAccount();
            account.GenerateNewIdentity();
            account.BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02");
            account.SetCustomerOwnerOfThisBankAccount(customer);
            account.DepositMoney(1000, "reason");
            account.Lock();

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var bankAccountDTO = adapter.Adapt<BankAccount, BankAccountDTO>(account);


            //Assert
            Assert.Equal(account.Id, bankAccountDTO.Id);
            Assert.Equal(account.Iban, bankAccountDTO.BankAccountNumber);
            Assert.Equal(account.Balance, bankAccountDTO.Balance);
            Assert.Equal(account.Customer.FirstName, bankAccountDTO.CustomerFirstName);
            Assert.Equal(account.Customer.LastName, bankAccountDTO.CustomerLastName);
            Assert.Equal(account.Locked, bankAccountDTO.Locked);
        }
        [Fact]
        public void AdaptEnumerableBankAccountToListBankAccountListDTO()
        {
            //Arrange

            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo","+341232","company", country, new Address("", "", "", ""));
            customer.GenerateNewIdentity();

            BankAccount account = new BankAccount();
            account.GenerateNewIdentity();
            account.BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02");
            account.SetCustomerOwnerOfThisBankAccount(customer);
            account.DepositMoney(1000, "reason");
            var accounts = new List<BankAccount>() { account };

            //Act
            ITypeAdapter adapter = TypeAdapterFactory.CreateAdapter();
            var bankAccountsDTO = adapter.Adapt<IEnumerable<BankAccount>, List<BankAccountDTO>>(accounts);


            //Assert
            Assert.NotNull(bankAccountsDTO);
            Assert.True(bankAccountsDTO.Count == 1);

            Assert.Equal(account.Id, bankAccountsDTO[0].Id);
            Assert.Equal(account.Iban, bankAccountsDTO[0].BankAccountNumber);
            Assert.Equal(account.Balance, bankAccountsDTO[0].Balance);
            Assert.Equal(account.Customer.FirstName, bankAccountsDTO[0].CustomerFirstName);
            Assert.Equal(account.Customer.LastName, bankAccountsDTO[0].CustomerLastName);
        }
    }
}
