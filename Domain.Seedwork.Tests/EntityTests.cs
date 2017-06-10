namespace NLayerApp.Domain.Seedwork.Tests
{
    using NLayerApp.Domain.Seedwork.Tests.Classes;
    using System;
    using Xunit;

    public class EntityTests
    {
        [Fact]
        public void SameIdentityProduceEqualsTrueTest()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var entityLeft = new SampleEntity();
            var entityRight = new SampleEntity();

            entityLeft.ChangeCurrentIdentity(id);
            entityRight.ChangeCurrentIdentity(id);

            //Act
            bool resultOnEquals = entityLeft.Equals(entityRight);
            bool resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.True(resultOnEquals);
            Assert.True(resultOnOperator);

        }
        [Fact]
        public void DiferentIdProduceEqualsFalseTest()
        {
            //Arrange

            var entityLeft = new SampleEntity();
            var entityRight = new SampleEntity();

            entityLeft.GenerateNewIdentity();
            entityRight.GenerateNewIdentity();


            //Act
            bool resultOnEquals = entityLeft.Equals(entityRight);
            bool resultOnOperator = entityLeft == entityRight;

            //Assert
            Assert.False(resultOnEquals);
            Assert.False(resultOnOperator);

        }
        [Fact]
        public void CompareUsingEqualsOperatorsAndNullOperandsTest()
        {
            //Arrange

            SampleEntity entityLeft = null;
            SampleEntity entityRight = new SampleEntity();

            entityRight.GenerateNewIdentity();

            //Act
            if (!(entityLeft == (Entity)null))//this perform ==(left,right)
                Assert.True(false, "Fail");

            if (!(entityRight != (Entity)null))//this perform !=(left,right)
                Assert.True(false, "Fail");

            entityRight = null;

            //Act
            if (!(entityLeft == entityRight))//this perform ==(left,right)
                Assert.True(false, "Fail");

            if (entityLeft != entityRight)//this perform !=(left,right)
                Assert.True(false, "Fail");


        }
        [Fact]
        public void CompareTheSameReferenceReturnTrueTest()
        {
            //Arrange
            var entityLeft = new SampleEntity();
            SampleEntity entityRight = entityLeft;


            //Act
            if (!entityLeft.Equals(entityRight))
                Assert.True(false, "Fail");

            if (!(entityLeft == entityRight))
                Assert.True(false, "Fail");

        }
        [Fact]
        public void CompareWhenLeftIsNullAndRightIsNullReturnFalseTest()
        {
            //Arrange

            SampleEntity entityLeft = null;
            var entityRight = new SampleEntity();

            entityRight.GenerateNewIdentity();

            //Act
            if (!(entityLeft == (Entity)null))//this perform ==(left,right)
                Assert.True(false, "Fail");

            if (!(entityRight != (Entity)null))//this perform !=(left,right)
                Assert.True(false, "Fail");
        }

        [Fact]
        public void SetIdentitySetANonTransientEntity()
        {
            //Arrange
            var entity = new SampleEntity();

            //Act
            entity.GenerateNewIdentity();

            //Assert
            Assert.False(entity.IsTransient());
        }

        [Fact]
        public void ChangeIdentitySetNewIdentity()
        {
            //Arrange
            var entity = new SampleEntity();
            entity.GenerateNewIdentity();

            //act
            Guid expected = entity.Id;
            entity.ChangeCurrentIdentity(Guid.NewGuid());

            //assert
            Assert.NotEqual(expected, entity.Id);
        }
    }
}
