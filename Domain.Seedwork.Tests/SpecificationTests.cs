namespace NLayerApp.Domain.Seedwork.Tests
{
    using NLayerApp.Domain.Seedwork.Specification;
    using NLayerApp.Domain.Seedwork.Tests.Classes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;
    public class SpecificationTests
    {

        [Fact]
        public void CreateNewDirectSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> adHocSpecification;
            Expression<Func<SampleEntity, bool>> spec = s => s.Id == Guid.NewGuid();

            //Act
            adHocSpecification = new DirectSpecification<SampleEntity>(spec);

            //Assert
            //Assert.ReferenceEquals(new PrivateObject(adHocSpecification).GetField("_MatchingCriteria"), spec);
        }
        [Fact]
        public void CreateDirectSpecificationNullSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> adHocSpecification;
            Expression<Func<SampleEntity, bool>> spec = null;

            //Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => adHocSpecification = new DirectSpecification<SampleEntity>(spec));
            Assert.IsType<ArgumentNullException>(ex);

        }
        [Fact]
        public void CreateAndSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            AndSpecification<SampleEntity> composite = new AndSpecification<SampleEntity>(leftAdHocSpecification, rightAdHocSpecification);

            //Assert
            Assert.NotNull(composite.SatisfiedBy());
            leftAdHocSpecification.Equals(composite.LeftSideSpecification);
            rightAdHocSpecification.Equals(composite.RightSideSpecification);

            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.ChangeCurrentIdentity(identifier);

            list.AddRange(new SampleEntity[] { sampleA, sampleB });


            var result = list.AsQueryable().Where(composite.SatisfiedBy()).ToList();

            Assert.True(result.Count == 1);
        }
        [Fact]
        public void CreateOrSpecificationTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            OrSpecification<SampleEntity> composite = new OrSpecification<SampleEntity>(leftAdHocSpecification, rightAdHocSpecification);

            //Assert
            Assert.NotNull(composite.SatisfiedBy());
            leftAdHocSpecification.Equals(composite.LeftSideSpecification);
            rightAdHocSpecification.Equals(composite.RightSideSpecification);

            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            list.AddRange(new SampleEntity[] { sampleA, sampleB });


            var result = list.AsQueryable().Where(composite.SatisfiedBy()).ToList();

            Assert.True(result.Count() == 2);
        }
        [Fact]
        public void CreateAndSpecificationNullLeftSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            AndSpecification<SampleEntity> composite;

            Exception ex = Assert.Throws<ArgumentNullException>(() => composite = new AndSpecification<SampleEntity>(null, rightAdHocSpecification));

            Assert.IsType<ArgumentNullException>(ex);

        }
        [Fact]
        public void CreateAndSpecificationNullRightSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> rightSpec = s => s.Id == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            AndSpecification<SampleEntity> composite;

            Exception ex = Assert.Throws<ArgumentNullException>(() => composite = new AndSpecification<SampleEntity>(leftAdHocSpecification, null));
            Assert.IsType<ArgumentNullException>(ex);

        }
        [Fact]
        public void CreateOrSpecificationNullLeftSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            OrSpecification<SampleEntity> composite;

            Exception ex = Assert.Throws<ArgumentNullException>(() => composite = new OrSpecification<SampleEntity>(null, rightAdHocSpecification));
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void CreateOrSpecificationNullRightSpecThrowArgumentNullExceptionTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            Expression<Func<SampleEntity, bool>> rightSpec = s => s.Id == Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.SampleProperty.Length > 2;

            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            //Act
            OrSpecification<SampleEntity> composite;

            Exception ex = Assert.Throws<ArgumentNullException>(() => composite = new OrSpecification<SampleEntity>(leftAdHocSpecification, null));
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void UseSpecificationLogicAndOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            ISpecification<SampleEntity> andSpec = leftAdHocSpecification & rightAdHocSpecification;

            //Assert

            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            var sampleC = new SampleEntity() { SampleProperty = "the sample property" };
            sampleC.ChangeCurrentIdentity(identifier);

            list.AddRange(new SampleEntity[] { sampleA, sampleB, sampleC });

            var result = list.AsQueryable().Where(andSpec.SatisfiedBy()).ToList();

            Assert.True(result.Count == 1);
        }
        [Fact]
        public void UseSpecificationAndOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            ISpecification<SampleEntity> andSpec = leftAdHocSpecification && rightAdHocSpecification;

            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            var sampleC = new SampleEntity() { SampleProperty = "the sample property" };
            sampleC.ChangeCurrentIdentity(identifier);

            list.AddRange(new SampleEntity[] { sampleA, sampleB, sampleC });


            var result = list.AsQueryable().Where(andSpec.SatisfiedBy()).ToList();

            Assert.True(result.Count == 1);

        }
        [Fact]
        public void UseSpecificationBitwiseOrOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            var orSpec = leftAdHocSpecification | rightAdHocSpecification;


            //Assert
            var list = new List<SampleEntity>();

            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            list.AddRange(new SampleEntity[] { sampleA, sampleB });

            var result = list.AsQueryable().Where(orSpec.SatisfiedBy()).ToList();
        }
        [Fact]
        public void UseSpecificationOrOperatorTest()
        {
            //Arrange
            DirectSpecification<SampleEntity> leftAdHocSpecification;
            DirectSpecification<SampleEntity> rightAdHocSpecification;

            var identifier = Guid.NewGuid();
            Expression<Func<SampleEntity, bool>> leftSpec = s => s.Id == identifier;
            Expression<Func<SampleEntity, bool>> rightSpec = s => s.SampleProperty.Length > 2;


            //Act
            leftAdHocSpecification = new DirectSpecification<SampleEntity>(leftSpec);
            rightAdHocSpecification = new DirectSpecification<SampleEntity>(rightSpec);

            var orSpec = leftAdHocSpecification || rightAdHocSpecification;


            //Assert
            var list = new List<SampleEntity>();
            var sampleA = new SampleEntity() { SampleProperty = "1" };
            sampleA.ChangeCurrentIdentity(identifier);

            var sampleB = new SampleEntity() { SampleProperty = "the sample property" };
            sampleB.GenerateNewIdentity();

            list.AddRange(new SampleEntity[] { sampleA, sampleB });

            var result = list.AsQueryable().Where(orSpec.SatisfiedBy()).ToList();

            Assert.True(result.Count() == 2);
        }
        [Fact]
        public void CreateNotSpecificationithSpecificationTest()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();
            DirectSpecification<SampleEntity> specification = new DirectSpecification<SampleEntity>(specificationCriteria);

            //Act
            NotSpecification<SampleEntity> notSpec = new NotSpecification<SampleEntity>(specification);
            //Expression<Func<SampleEntity, bool>> resultCriteria = new PrivateObject(notSpec).GetField("_OriginalCriteria") as Expression<Func<SampleEntity, bool>>;

            //Assert
            Assert.NotNull(notSpec);
            //Assert.NotNull(resultCriteria);
            Assert.NotNull(notSpec.SatisfiedBy());

        }
        [Fact]
        public void CreateNotSpecificationWithCriteriaTest()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();


            //Act
            NotSpecification<SampleEntity> notSpec = new NotSpecification<SampleEntity>(specificationCriteria);

            //Assert
            Assert.NotNull(notSpec);
            //Assert.NotNull(new PrivateObject(notSpec).GetField("_OriginalCriteria"));
        }
        [Fact]
        public void CreateNotSpecificationFromNegationOperator()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();


            //Act
            DirectSpecification<SampleEntity> spec = new DirectSpecification<SampleEntity>(specificationCriteria);
            ISpecification<SampleEntity> notSpec = !spec;

            //Assert
            Assert.NotNull(notSpec);

        }
        [Fact]
        public void CheckNotSpecificationOperators()
        {
            //Arrange
            Expression<Func<SampleEntity, bool>> specificationCriteria = t => t.Id == Guid.NewGuid();


            //Act
            Specification<SampleEntity> spec = new DirectSpecification<SampleEntity>(specificationCriteria);
            Specification<SampleEntity> notSpec = !spec;
            ISpecification<SampleEntity> resultAnd = notSpec && spec;
            ISpecification<SampleEntity> resultOr = notSpec || spec;

            //Assert
            Assert.NotNull(notSpec);
            Assert.NotNull(resultAnd);
            Assert.NotNull(resultOr);

        }
        [Fact]
        public void CreateNotSpecificationNullSpecificationThrowArgumentNullExceptionTest()
        {
            //Arrange
            NotSpecification<SampleEntity> notSpec;

            //Act
            
            Exception ex = Assert.Throws<ArgumentNullException>(() => notSpec = new NotSpecification<SampleEntity>((ISpecification<SampleEntity>)null));
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void CreateNotSpecificationNullCriteriaThrowArgumentNullExceptionTest()
        {
            //Arrange
            NotSpecification<SampleEntity> notSpec;

            //Act
            Exception ex = Assert.Throws<ArgumentNullException>(() => notSpec = new NotSpecification<SampleEntity>((Expression<Func<SampleEntity, bool>>)null));
            Assert.IsType<ArgumentNullException>(ex);
        }
        [Fact]
        public void CreateTrueSpecificationTest()
        {
            //Arrange
            ISpecification<SampleEntity> trueSpec = new TrueSpecification<SampleEntity>();
            bool expected = true;
            bool actual = trueSpec.SatisfiedBy().Compile()(new SampleEntity());
            //Assert
            Assert.NotNull(trueSpec);
            expected.Equals(actual);
        }
    }
}
