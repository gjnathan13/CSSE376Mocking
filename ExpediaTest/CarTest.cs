using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestClass]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[TestInitialize]
		public void TestInitialize()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[TestMethod]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[TestMethod]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [TestMethod]
        public void TestThatCarGetsCarLocationFromDatabase(){

            IDatabase mockDB = mocks.DynamicMock<IDatabase>();
            String carOne = "Red Zone Spot 5";
            String carTwo = "Blue Zone Spot 11";
           
            using (mocks.Ordered()) {
            Expect.Call(mockDB.getCarLocation(13)).Return(carOne);
            Expect.Call(mockDB.getCarLocation(244)).Return(carTwo);
            }
            mockDB.Stub(x => x.getCarLocation(Arg<Int32>.Is.Anything)).Return("No car found");

            mocks.ReplayAll();

            Car target = new Car(15);
            target.Database = mockDB;

            String result;
            result = target.getCarLocation(13);
            Assert.AreEqual(carOne, result);
            result = target.getCarLocation(244);
            Assert.AreEqual(carTwo, result);
            result = target.getCarLocation(25);
            Assert.AreEqual("No car found", result);
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.StrictMock<IDatabase>();
            Int32 Miles = 10000;

            Expect.Call(mockDatabase.Miles).PropertyBehavior();

            mocks.ReplayAll();
            mockDatabase.Miles = Miles;
            var target = new Car(10);
            target.Database = mockDatabase;

            int currentMilage = target.Mileage;
            Assert.AreEqual(currentMilage, Miles);
            mocks.VerifyAll();
        }

        [TestMethod()]
        public void TestTheCarIsAssignedWithProperName()
        {
            var targetCar = ObjectMother.BMW();
            String targetName = "BMW X4 Coupe";
            Assert.AreEqual(targetCar.Name, targetName);
        }
	}
}
