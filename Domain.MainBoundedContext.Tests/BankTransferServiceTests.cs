using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
using NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
using NLayerApp.Infrastructure.Crosscutting.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Localization;
using NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
using NLayerApp.Infrastructure.Crosscutting.Validator;
using System;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class BankTransferServiceTests
    {
        #region Class Initialize

        public BankTransferServiceTests()
        {
            //Localization
            LocalizationFactory.SetCurrent(new ResourcesManagerFactory());
        }

        #endregion

        [Fact]
        public void PerformTransferThrowExceptionIfSourceCantWithdrawedWithLockedAccount()
        {
            //Arrange

            var customer = GetCustomer();

            var source = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");
            source.Lock();

            var target = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "12312321322", "01"));


            //Act
            var bankTransferService = new BankTransferService();
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankTransferService.PerformTransfer(10, source, target));
            Assert.IsType<InvalidOperationException>(ex);
        }
        [Fact]
        public void PerformTransferThrowExceptionIfTargetBankAccountNumberIsEqualToSourceBankAccountNumber()
        {
            //Arrange
            var customer = GetCustomer();

            var source = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");
            source.Lock();

            var target = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "3333333333", "01"));


            //Act
            var bankTransferService = new BankTransferService();
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankTransferService.PerformTransfer(10, source, target));
            Assert.IsType<InvalidOperationException>(ex);
        }
        [Fact]        
        public void PerformTransferThrowExceptionIfSourceCantWithdrawedWithExceedAmoung()
        {
            //Arrange
            var customer = GetCustomer();

            var source = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");

            var target = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "12312321322", "01"));


            //Act
            var bankTransferService = new BankTransferService();
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankTransferService.PerformTransfer(2000, source, target));
            Assert.IsType<InvalidOperationException>(ex);
        }
        [Fact]
        public void PerformTransferThrowExceptionIfTargetIsLockedAccount()
        {
            //Arrange
            var customer = GetCustomer();

            var source = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");

            var target = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "12312321322", "01"));
            target.Lock();

            //Act
            var bankTransferService = new BankTransferService();
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankTransferService.PerformTransfer(10, source, target));
            Assert.IsType<InvalidOperationException>(ex);            
        }
        [Fact]
        public void PerformTransferCreateActivities()
        {
            //Arrange
            var customer = GetCustomer();

            var source = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");

            var target = BankAccountFactory.CreateBankAccount(customer, new BankAccountNumber("1111", "2222", "12312321322", "01"));


            //Act

            int activities = source.BankAccountActivity.Count;

            var bankTransferService = new BankTransferService();
            bankTransferService.PerformTransfer(50, source, target);

            //Assert
            Assert.NotNull(source.BankAccountActivity);
            Assert.Equal(++activities, source.BankAccountActivity.Count);

        }

        Customer GetCustomer()
        {
            string firstName = "firstName";
            string lastName = "lastName";
            string telephone = string.Empty;
            string company = string.Empty;
            string city = "city";
            string zipCode = "zipCode";
            string addressline1 = "addressline1";
            string addressline2 = "addressline2";

            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            var address = new Address(city, zipCode, addressline1, addressline2);

            var customer = CustomerFactory.CreateCustomer(firstName, lastName, telephone, company, country, address);

            return customer;
        }
    }
}
