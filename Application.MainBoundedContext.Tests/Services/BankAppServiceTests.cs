namespace NLayerApp.Application.MainBoundedContext.Tests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using NLayerApp.Application.MainBoundedContext.BankingModule.Services;
    using NLayerApp.Application.MainBoundedContext.DTO;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
    using NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Collection("Our Test Collection #2")]

    public class BankAppServiceTests 
    {
        protected TestsInitialize fixture;

        public BankAppServiceTests(TestsInitialize fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void LockBankAccountReturnFalseIfIdentifierIsEmpty()
        {
            //Arrange
            var bankAccountRepository = new Mock<IBankAccountRepository>();

            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => guid == Guid.Empty ?
                                          null :
                                          new BankAccount { });

            var customerRepository = new Mock<ICustomerRepository>();

            IBankTransferService transferService = new BankTransferService();

            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.LockBankAccount(Guid.Empty);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void LockBankAccountReturnFalseIfBankAccountNotExist()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            var bankAccountRepository = new Mock<IBankAccountRepository>();

            bankAccountRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => null);

            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.LockBankAccount(Guid.NewGuid());

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void LockBankAccountReturnTrueIfBankAccountIsLocked()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            bankAccountRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) =>
                        {
                            var customer = new Customer();
                            customer.GenerateNewIdentity();

                            var bankAccount = new BankAccount();
                            bankAccount.GenerateNewIdentity();

                            bankAccount.SetCustomerOwnerOfThisBankAccount(customer);

                            return bankAccount;
                        });

            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.LockBankAccount(Guid.NewGuid());

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void AddBankAccountThrowArgumentNullExceptionWhenBankAccountDTOIsNull()
        {
            //Arrange
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            Exception ex = Assert.Throws<ArgumentException>(() =>
                {
                    //Act
                    var result = bankingService.AddBankAccount(null);

                    //Assert

                    Assert.Null(result);
                }
            );

            Assert.IsType(typeof(ArgumentException), ex);

        }
        [Fact]
        public void AddBankAccountReturnNullWhenCustomerIdIsEmpty()
        {
            //Arrange
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var customerRepository = new Mock <ICustomerRepository>();

            IBankTransferService transferService = new BankTransferService();

            var dto = new BankAccountDTO()
            {
                CustomerId = Guid.Empty
            };
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            Exception ex = Assert.Throws<ArgumentException>(() =>
                {
                    //Act
                    var result = bankingService.AddBankAccount(dto);
                }
            );

            Assert.IsType(typeof(ArgumentException), ex);

        }
        [Fact]
        public void AddBankAccountThrowInvalidOperationExceptionWhenCustomerNotExist()
        {
            //Arrange
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            var customerRepository = new Mock<ICustomerRepository>();

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => null);

            IBankTransferService transferService = new BankTransferService();

            var dto = new BankAccountDTO()
            {
                CustomerId = Guid.NewGuid()
            };

            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            Exception ex = Assert.Throws<InvalidOperationException>(() =>
            {
                //Act
                bankingService.AddBankAccount(dto);
            }
            );

            Assert.IsType(typeof(InvalidOperationException), ex);

        }

        [Fact]
        public void AddBankAccountReturnDTOWhenSaveSucceed()
        {
            //Arrange
            IBankTransferService transferService = new BankTransferService();

            var customerRepository = new Mock<ICustomerRepository>();

            customerRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) =>
                    {
                        var customer = new Customer()
                        {
                            FirstName = "Jhon",
                            LastName = "El rojo"
                        };

                        customer.ChangeCurrentIdentity(guid);

                        return customer;
                    }
                );

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            var bankAccountRepository = new Mock<IBankAccountRepository>();
            bankAccountRepository.Setup(x => x.Add(It.IsAny<BankAccount>()));
            
            bankAccountRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            var dto = new BankAccountDTO()
            {
                CustomerId = Guid.NewGuid(),
                BankAccountNumber = "BA"
            };

            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.AddBankAccount(dto);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void FindBankAccountsReturnAllItems()
        {
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            bankAccountRepository.Setup(x => x.GetAll()).Returns(() =>
                {
                    var customer = new Customer();
                    customer.GenerateNewIdentity();

                    var bankAccount = new BankAccount()
                    {
                        BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02"),
                    };
                    bankAccount.SetCustomerOwnerOfThisBankAccount(customer);
                    bankAccount.GenerateNewIdentity();

                    var accounts = new List<BankAccount>() { bankAccount };

                    return accounts;
                }
                );
            
            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.FindBankAccounts();

            Assert.NotNull(result);
            Assert.True(result.Count == 1);

        }

        [Fact]
        public void FindBankAccountActivitiesReturnNullWhenBankAccountIdIsEmpty()
        {
            //Arrange

            var bankAccountRepository = new Mock<IBankAccountRepository>();
            
            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => guid == Guid.Empty ?
                                          null :
                                          new BankAccount { });
            
            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.FindBankAccountActivities(Guid.Empty);


            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void FindBankAccountActivitiesReturnNullWhenBankAccountNotExists()
        {
            //Arrange
            var bankAccountRepository = new Mock<IBankAccountRepository>();
            
            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => null);

            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.FindBankAccountActivities(Guid.NewGuid());

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void FindBankAccountActivitiesReturnAllItems()
        {
            //Arrange
            var bankAccountRepository = new Mock<IBankAccountRepository>();

            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    var bActivity1 = new BankAccountActivity() { Date = DateTime.Now, Amount = 1000 };
                    bActivity1.GenerateNewIdentity();

                    var bActivity2 = new BankAccountActivity() { Date = DateTime.Now, Amount = 1000 };
                    bActivity2.GenerateNewIdentity();

                    var bankAccount = new BankAccount()
                    {
                        BankAccountActivity = new HashSet<BankAccountActivity>() { bActivity1, bActivity2 }
                    };
                    bankAccount.GenerateNewIdentity();

                    return bankAccount;
                });

            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            var result = bankingService.FindBankAccountActivities(Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }

        [Fact]
        public void PerformBankTransfer()
        {
            //Arrange

            //--> source bank account data

            var sourceId = new Guid("3481009C-A037-49DB-AE05-44FF6DB67DEC");
            var bankAccountNumberSource = new BankAccountNumber("4444", "5555", "3333333333", "02");
            var sourceCustomer = new Customer();
            sourceCustomer.GenerateNewIdentity();

            var source = BankAccountFactory.CreateBankAccount(sourceCustomer, bankAccountNumberSource);
            source.ChangeCurrentIdentity(sourceId);
            source.DepositMoney(1000, "initial");

            var sourceBankAccountDTO = new BankAccountDTO()
            {
                Id = sourceId,
                BankAccountNumber = source.Iban
            };

            //--> target bank account data
            var targetCustomer = new Customer();
            targetCustomer.GenerateNewIdentity();
            var targetId = new Guid("8A091975-F783-4730-9E03-831E9A9435C1");
            var bankAccountNumberTarget = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var target = BankAccountFactory.CreateBankAccount(targetCustomer, bankAccountNumberTarget);
            target.ChangeCurrentIdentity(targetId);


            var targetBankAccountDTO = new BankAccountDTO()
            {
                Id = targetId,
                BankAccountNumber = target.Iban
            };

            var accounts = new List<BankAccount>() { source, target };

            var bankAccountRepository = new Mock<IBankAccountRepository>();

            bankAccountRepository
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid guid) => {
                    return accounts.Where(ba => ba.Id == guid).SingleOrDefault();
                });

            Mock<MainBCUnitOfWork> _mockContext = new Mock<MainBCUnitOfWork>();
            _mockContext.Setup(c => c.Commit());

            bankAccountRepository
                .Setup(x => x.UnitOfWork).Returns(_mockContext.Object);

            var customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            IBankAppService bankingService = new BankAppService(bankAccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);

            //Act
            bankingService.PerformBankTransfer(sourceBankAccountDTO, targetBankAccountDTO, 100M);

            //Assert
            Assert.Equal(source.Balance, 900);
            Assert.Equal(target.Balance, 100);
        }

        [Fact]
        public void ConstructorThrowExceptionIfBankTransferServiceDependencyIsNull()
        {
            //Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var bankAcccountRepository = new Mock<IBankAccountRepository>();
            IBankTransferService transferService = null;
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();
            
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                IBankAppService bankingService = new BankAppService(bankAcccountRepository.Object, customerRepository.Object, transferService, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
        [Fact]
        public void ConstructorThrowExceptionIfCustomerRepositoryDependencyIsNull()
        {
            //Arrange
            Mock<IBankAccountRepository> bankAcccountRepository = new Mock<IBankAccountRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                IBankAppService bankingService = new BankAppService(bankAcccountRepository.Object, null, transferService, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);

        }
        [Fact]
        public void ConstructorThrowExceptionIfBankAccountRepositoryDependencyIsNull()
        {
            //Arrange
            Mock<ICustomerRepository> customerRepository = new Mock<ICustomerRepository>();
            IBankTransferService transferService = new BankTransferService();
            Mock<ILogger<BankAppService>> _mockLogger = new Mock<ILogger<BankAppService>>();

            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                IBankAppService bankingService = new BankAppService(null, customerRepository.Object, transferService, _mockLogger.Object);
            }
            );

            Assert.IsType(typeof(ArgumentNullException), ex);
        }
    }
}
