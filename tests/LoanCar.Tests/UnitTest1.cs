using System;
using System.Linq;
using Xunit;
using LoanCar.Domain;
using LoanCar.Infrastructure;
using LoanCar.Application;

namespace LoanCar.Tests
{
    public class DomainTests
    {
        // Тест 1: Перевіряємо математику обчислення відстані
        [Fact]
        public void Location_CalculateDistance_IsCorrect()
        {
            var loc1 = new Location(0, 0);
            var loc2 = new Location(3, 4);
            
            double distance = loc1.CalculateDistanceTo(loc2);
            
            Assert.Equal(5, distance); // За теоремою Піфагора має бути 5
        }

        // Тест 2: Перевіряємо, чи пускає система досвідчених водіїв у Преміум
        [Fact]
        public void User_CanRentPremium_WithEnoughExperience_ReturnsTrue()
        {
            var user = new User("Профі", 3); // 3 роки стажу
            Assert.True(user.CanRentPremiumOrCargo());
        }

        // Тест 3: Перевіряємо, чи блокує система новачків
        [Fact]
        public void User_CanRentPremium_WithLowExperience_ReturnsFalse()
        {
            var user = new User("Новачок", 1); // 1 рік стажу
            Assert.False(user.CanRentPremiumOrCargo());
        }

        // Тест 4: Перевіряємо поліморфізм (розрахунок запасу ходу електромобіля)
        [Fact]
        public void ElectricCar_CalculateRange_IsCorrect()
        {
            // Батарея 50%, макс запас 400 км. Має вийти 200 км.
            var car = new ElectricCar("AA1111AA", "Tesla", CarClass.Premium, new Location(0,0), 50, 400);
            
            double range = car.CalculateRemainingRangeKm();
            
            Assert.Equal(200, range);
        }

        // Тест 5: Перевіряємо, що після бронювання авто стає недоступним
        [Fact]
        public void Vehicle_Booking_MakesCarUnavailable()
        {
            var car = new CombustionCar("BB2222BB", "Skoda", CarClass.Comfort, new Location(0,0), 40, 8);
            
            Assert.True(car.IsAvailable); // Спочатку доступне
            car.Book();                   // Бронюємо
            Assert.False(car.IsAvailable); // Тепер недоступне
        }
    }
}