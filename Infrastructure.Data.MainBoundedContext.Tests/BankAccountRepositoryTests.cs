namespace NLayerApp.Infrastructure.Data.MainBoundedContext.Tests
{
    using Microsoft.EntityFrameworkCore;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.BankingModule.Repositories;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #1")]
    /// <summary>
    /// Summary description for BankAccountRepositoryTests
    /// </summary>
    public class BankAccountRepositoryTests : IClassFixture<MainBCUnitOfWorkInitializer>
    {
        protected MainBCUnitOfWorkInitializer fixture;

        public BankAccountRepositoryTests(MainBCUnitOfWorkInitializer fixture
            )
        {
            this.fixture = fixture;
        }
        [Fact]
        public void BankAccountRepositoryGetMethodReturnMaterializedEntityById()
        {
            //Arrange
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);

            var selectedBankAccount = new Guid("0343C0B0-7C40-444A-B044-B463F36A1A1F");

            //Act
            var bankAccount = bankAccountRepository.Get(selectedBankAccount);

            //Assert
            Assert.NotNull(bankAccount);
            Assert.True(bankAccount.Id == selectedBankAccount);
        }

        [Fact]
        public void BankAccountRepositoryGetMethodReturnNullWhenIdIsEmpty()
        {
            //Arrange
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);

            //Act
            var bankAccount = bankAccountRepository.Get(Guid.Empty);

            //Assert
            Assert.Null(bankAccount);
        }
        [Fact]
        public void BankAccountRepositoryAddNewItemSaveItem()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);
           
            var customer = customerRepository.Get(new Guid("43A38AC8-EAA9-4DF0-981F-2685882C7C45"));
            
            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            var newBankAccount = BankAccountFactory.CreateBankAccount(customer,bankAccountNumber);
            

            //Act
            bankAccountRepository.Add(newBankAccount);

            try
            {
                fixture.unitOfWork.Commit();
            }
            catch (DbUpdateException ex)
            {
                var entry = ex.Entries.First();
            }
        }

        [Fact]
        public void BankAccountRepositoryGetAllReturnMaterializedBankAccountsAndCustomers()
        {
            //Arrange
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);

            //Act
            var allItems = bankAccountRepository.GetAll();

            //Assert
            Assert.NotNull(allItems);
            Assert.True(allItems.Any());
            Assert.True(allItems.All(ba => ba.Customer != null));
        }
        [Fact]
        public void BankAccountRepositoryAllMatchingMethodReturnEntitiesWithSatisfiedCriteria()
        {
            //Arrange
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);

            string iban = string.Format("ES{0} {1} {2} {0}{3}","02","4444","5555","3333333333");

            var spec = BankAccountSpecifications.BankAccountIbanNumber(iban);

            //Act
            var result = bankAccountRepository.AllMatching(spec);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.All(b => b.Iban == iban));
        }
        [Fact]
        public void BankAccountRepositoryFilterMethodReturnEntitisWithSatisfiedFilter()
        {
            //Arrange
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);

            string iban = string.Format("ES{0} {1} {2} {0}{3}", "02", "4444", "5555", "3333333333");
            
            
            //Act
            var allItems = bankAccountRepository.GetFiltered(ba => ba.Iban == iban);

            //Assert
            Assert.NotNull(allItems);
            Assert.True(allItems.All(b => b.Iban == iban));
        }
        [Fact]
        public void BankAccountRepositoryPagedMethodReturnEntitiesInPageFashion()
        {
            //Arrange
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);
           
            //Act
            var pageI = bankAccountRepository.GetPaged(0, 1, b => b.Id, false);
            var pageII = bankAccountRepository.GetPaged(1, 1, b => b.Id, false);

            //Assert
            Assert.NotNull(pageI);
            Assert.True(pageI.Count() == 1);

            Assert.NotNull(pageII);
            Assert.True(pageII.Count() == 1);

            Assert.False(pageI.Intersect(pageII).Any());
        }

        [Fact]
        public void BankAccountRepositoryRemoveItemDeleteIt()
        {
            //Arrange
            var customerRepository = new CustomerRepository(fixture.unitOfWork, fixture.customerLogger);
            var bankAccountRepository = new BankAccountRepository(fixture.unitOfWork, fixture.bankAccountLogger);

            var customer = customerRepository.Get(new Guid("43A38AC8-EAA9-4DF0-981F-2685882C7C45"));
           
            var bankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02");

            var newBankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);
            

            bankAccountRepository.Add(newBankAccount);
            fixture.unitOfWork.Commit();

            //Act
            bankAccountRepository.Remove(newBankAccount);
            fixture.unitOfWork.Commit();

          
        }
    }
}
