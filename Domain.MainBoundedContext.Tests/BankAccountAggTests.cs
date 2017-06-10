namespace NLayerApp.Domain.MainBoundedContext.Tests
{
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Xunit;

    public class BankAccountAggTests : TestsInitialize
    {
        [Fact]
        public void BankAccountCannotSetATransientCustomer()
        {
            //Arrange
            var customer = new Customer()
            {
                FirstName = "Unai",
                LastName ="Zorrilla",
            };

            var bankAccount = new BankAccount();

            //Act
            Exception ex = Assert.Throws<ArgumentException>(() => bankAccount.SetCustomerOwnerOfThisBankAccount(customer));
            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void BankAccountSetCustomerFixCustomerId()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            
            //Act
            BankAccount bankAccount = new BankAccount();
            bankAccount.SetCustomerOwnerOfThisBankAccount(customer);

            //Assert
            Assert.Equal(customer.Id, bankAccount.CustomerId);
        }
        [Fact]
        public void BankAccountFactoryCreateValidBankAccount()
        {
            //Arrange
            var country = new Country("Spain","es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company",country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            BankAccount bankAccount = null;

            //Act
            bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.NotNull(bankAccount);
            Assert.True(bankAccount.BankAccountNumber == bankAccountNumber);
            Assert.False(bankAccount.Locked);
            Assert.True(bankAccount.CustomerId == customer.Id);

            Assert.False(validationResults.Any());
        }

        [Fact]
        public void BankAccountLockSetLocked()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.Lock();

            //Assert
            Assert.True(bankAccount.Locked);

        }
        [Fact]
        public void BankAccountUnLockSetUnLocked()
        {
            //Arrange
            var country = new Country("spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.Lock();
            bankAccount.UnLock();

            //Assert
            Assert.False(bankAccount.Locked);
        }
        [Fact]
        public void BankAccountCannotWithDrawMoneyInLockedAccount()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);
            bankAccount.Lock();

            //Act
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankAccount.WithdrawMoney(200, "the reason.."));
            Assert.IsType(typeof(InvalidOperationException), ex);
        }
        [Fact]
        public void BankAccountCannotWithDrawMoneyIfBalanceIsLessThanAmountAccount()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankAccount.WithdrawMoney(200, "the reason.."));
            Assert.IsType(typeof(InvalidOperationException), ex);
        }
        [Fact]
        public void BankAccountCannotDepositMoneyInLockedAccount()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);
            bankAccount.Lock();

            //Act
            Exception ex = Assert.Throws<InvalidOperationException>(() => bankAccount.DepositMoney(200, "the reason.."));
            Assert.IsType(typeof(InvalidOperationException), ex);
        }
        [Fact]
        public void BankAccountDepositMoneyAnotateActivity()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.DepositMoney(1000, activityReason);

            //Assert
            Assert.True(bankAccount.Balance == 1000);
            Assert.NotNull(bankAccount.BankAccountActivity);
            Assert.NotNull(bankAccount.BankAccountActivity.Any());
            Assert.True(bankAccount.BankAccountActivity.Single().Amount == 1000);
            Assert.True(bankAccount.BankAccountActivity.Single().ActivityDescription == activityReason);
        }
        [Fact]
        public void BankAccountWithdrawMoneyAnotateActivity()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";
            
            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));

            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.DepositMoney(1000, activityReason);
            bankAccount.WithdrawMoney(1000, activityReason);

            //Assert
            Assert.True(bankAccount.Balance == 0);
            Assert.NotNull(bankAccount.BankAccountActivity);
            Assert.NotNull(bankAccount.BankAccountActivity.Any());
            Assert.True(bankAccount.BankAccountActivity.Last().Amount == -1000);
            Assert.True(bankAccount.BankAccountActivity.Last().ActivityDescription == activityReason);
        }
        [Fact]
        public void BankAccountDepositMaxDecimalThrowOverflowBalance()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.GenerateNewIdentity();


            BankAccount bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            bankAccount.DepositMoney(1, activityReason);

            Exception ex = Assert.Throws<OverflowException>(() => bankAccount.DepositMoney(Decimal.MaxValue, activityReason));
            Assert.IsType(typeof(OverflowException), ex);
        }
        [Fact]
        public void BankAccountCannotDepositMoneyLessThanZero()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            Exception ex = Assert.Throws<ArgumentException>(() => bankAccount.DepositMoney(-100, activityReason));
            Assert.IsType(typeof(ArgumentException), ex);
        }
        [Fact]
        public void BankAccountDepositAndWithdrawSetBalance()
        {
            //Arrange
            Country country = new Country("Spain", "es-ES");
            country.GenerateNewIdentity();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", "+3422", "company", country, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));

            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            Assert.Equal(bankAccount.Balance,0);

            bankAccount.DepositMoney(1000, activityReason);
            Assert.Equal(bankAccount.Balance, 1000);

            bankAccount.WithdrawMoney(250, activityReason);
            Assert.Equal(bankAccount.Balance, 750);
        }
        [Fact]
        public void BankAccountWithNullBankAccountNumberProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = null;

            //act
            var validationContext = new ValidationContext(bankAccount,null,null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("BankAccountNumber"));
        }
        [Fact]
        public void BankAccountWithNullOfficeNumberProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber(null, "2222", "3333333333", "01");

            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("OfficeNumber"));
        }
        [Fact]
        public void BankAccountWithEmptyOfficeNumberProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber(string.Empty, "2222", "3333333333", "01");

            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("OfficeNumber"));
        }
        [Fact]
        public void BankAccountWithNullNationalBankCodeNumberProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111",null, "3333333333", "01");
            
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("NationalBankCode"));
        }
        [Fact]
        public void BankAccountWithEmptyNationalBankCodeProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111",string.Empty, "3333333333", "01");

            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("NationalBankCode"));
        }
        [Fact]
        public void BankAccountWithNullAccountNumberProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111","2222", null, "01");
            
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("AccountNumber"));
        }
        [Fact]
        public void BankAccountWithEmptyAccountNumberProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111", string.Empty, string.Empty, "01");

            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("AccountNumber"));
        }
        [Fact]
        public void BankAccountWithCheckDigistsProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333",null);

            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("CheckDigits"));
        }
        [Fact]
        public void BankAccountWithEmptyCheckDigistsProduceValidationError()
        {
            //Arrange
            var bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", string.Empty);

            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.NotNull(validationResults);
            Assert.True(validationResults.Any());
            Assert.True(validationResults.First().MemberNames.Contains("CheckDigits"));
        }
    }
}
