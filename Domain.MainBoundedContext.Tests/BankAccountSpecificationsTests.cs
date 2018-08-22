using NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
using NLayerApp.Domain.Seedwork.Specification;
using Xunit;

namespace Domain.MainBoundedContext.Tests
{
    public class BankAccountSpecificationsTests
    {
        [Fact]
        public void BankAccountSpecificationNullBankAccountNumberReturnTrueSpec()
        {
            //Arrange
            ISpecification<BankAccount> spec = null;

            //Act
            spec = BankAccountSpecifications.BankAccountIbanNumber(null);

            //assert
            Assert.IsType<TrueSpecification<BankAccount>>(spec);
        }
        [Fact]
        public void BankAccountSpecificationEmptyBankAccountNumberReturnTrueSpec()
        {
            //Arrange
            ISpecification<BankAccount> spec = null;

            //Act
            spec = BankAccountSpecifications.BankAccountIbanNumber(null);

            //assert
            Assert.IsType<TrueSpecification<BankAccount>>(spec);
        }

        [Fact]
        public void BankAccountSpecificationValiBankAccountNumberReturnAndSpec()
        {
            //Arrange
            ISpecification<BankAccount> spec = null;

            //Act
            spec = BankAccountSpecifications.BankAccountIbanNumber("AB001");

            //assert
            Assert.IsType<AndSpecification<BankAccount>>(spec);

        }
    }
}
