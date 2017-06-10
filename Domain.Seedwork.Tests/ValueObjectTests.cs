namespace NLayerApp.Domain.Seedwork.Tests
{
    using NLayerApp.Domain.Seedwork.Tests.Classes;
    using Xunit;

    public class ValueObjectTests
    {
        [Fact]
        public void IdenticalDataEqualsIsTrueTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

            //Act
            bool resultEquals = address1.Equals(address2);
            bool resultEqualsSimetric = address2.Equals(address1);
            bool resultEqualsOnThis = address1.Equals(address1);

            //Assert
            Assert.True(resultEquals);
            Assert.True(resultEqualsSimetric);
            Assert.True(resultEqualsOnThis);
        }

        [Fact]
        public void IdenticalDataEqualOperatorIsTrueTest()
        {
            //Arraneg
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

            //Act
            bool resultEquals = (address1 == address2);
            bool resultEqualsSimetric = (address2 == address1);
            bool resultEqualsOnThis = (address1 == address1);

            //Assert
            Assert.True(resultEquals);
            Assert.True(resultEqualsSimetric);
            Assert.True(resultEqualsOnThis);
        }

        [Fact]
        public void IdenticalDataIsNotEqualOperatorIsFalseTest()
        {
            //Arraneg
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine1", "streetLine2", "city", "zipcode");

            //Act
            bool resultEquals = (address1 != address2);
            bool resultEqualsSimetric = (address2 != address1);
            bool resultEqualsOnThis = (address1 != address1);

            //Assert
            Assert.False(resultEquals);
            Assert.False(resultEqualsSimetric);
            Assert.False(resultEqualsOnThis);
        }

        [Fact]
        public void DiferentDataEqualsIsFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

            //Act
            bool result = address1.Equals(address2);
            bool resultSimetric = address2.Equals(address1);

            //Assert
            Assert.False(result);
            Assert.False(resultSimetric);
        }

        [Fact]
        public void DiferentDataIsNotEqualOperatorIsTrueTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

            //Act
            bool result = (address1 != address2);
            bool resultSimetric = (address2 != address1);

            //Assert
            Assert.True(result);
            Assert.True(resultSimetric);
        }

        [Fact]
        public void DiferentDataEqualOperatorIsFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", "city", "zipcode");
            Address address2 = new Address("streetLine2", "streetLine1", "city", "zipcode");

            //Act
            bool result = (address1 == address2);
            bool resultSimetric = (address2 == address1);

            //Assert
            Assert.False(result);
            Assert.False(resultSimetric);
        }

        [Fact]
        public void SameDataInDiferentPropertiesIsEqualsFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine2", "streetLine1", null, null);

            //Act
            bool result = address1.Equals(address2);


            //Assert
            Assert.False(result);
        }

        [Fact]
        public void SameDataInDiferentPropertiesEqualOperatorFalseTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine2", "streetLine1", null, null);

            //Act
            bool result = (address1 == address2);


            //Assert
            Assert.False(result);
        }

        [Fact]
        public void DiferentDataInDiferentPropertiesProduceDiferentHashCodeTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine2", "streetLine1", null, null);

            //Act
            int address1HashCode = address1.GetHashCode();
            int address2HashCode = address2.GetHashCode();


            //Assert
            Assert.NotEqual(address1HashCode, address2HashCode);
        }
        [Fact]
        public void SameDataInDiferentPropertiesProduceDiferentHashCodeTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", null, null, "streetLine1");
            Address address2 = new Address(null, "streetLine1", "streetLine1", null);

            //Act
            int address1HashCode = address1.GetHashCode();
            int address2HashCode = address2.GetHashCode();


            //Assert
            Assert.NotEqual(address1HashCode, address2HashCode);
        }
        [Fact]
        public void SameReferenceEqualsTrueTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", null, null, "streetLine1");
            Address address2 = address1;


            //Act
            if (!address1.Equals(address2))
                Assert.True(false, "Fail");

            if (!(address1 == address2))
                Assert.True(false, "Fail");

        }
        [Fact]
        public void SameDataSameHashCodeTest()
        {
            //Arrange
            Address address1 = new Address("streetLine1", "streetLine2", null, null);
            Address address2 = new Address("streetLine1", "streetLine2", null, null);

            //Act
            int address1HashCode = address1.GetHashCode();
            int address2HashCode = address2.GetHashCode();


            //Assert
            Assert.Equal(address1HashCode, address2HashCode);
        }

        [Fact]
        public void SelfReferenceNotProduceInfiniteLoop()
        {
            //Arrange
            SelfReference aReference = new SelfReference();
            SelfReference bReference = new SelfReference();

            //Act
            aReference.Value = bReference;
            bReference.Value = aReference;

            //Assert

            Assert.NotEqual(aReference, bReference);
        }
    }
}
