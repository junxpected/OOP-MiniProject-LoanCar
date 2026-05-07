using System;
using System.Collections.Generic;
using System.Linq;
using LoanCar.Domain;

namespace LoanCar.Infrastructure
{
    public interface IUserRepository
    {
        User? GetUserByName(string name);
    }

    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public InMemoryUserRepository()
        {
            // Тестові користувачі
            _users = new List<User>
            {
                new User("Новачок", 1), // 1 рік стажу (не може брати Преміум)
                new User("Профі", 5)    // 5 років стажу (може брати все)
            };
        }

        public User? GetUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
    }

    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAllAvailable(CarClass desiredClass);
        void Update(Vehicle vehicle);
    }

    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles;

        public InMemoryVehicleRepository()
        {
            // наш "автопарк"
            _vehicles = new List<Vehicle>
            {
                new ElectricCar("AA0001AA", "Nissan Leaf", CarClass.Economy, new Location(2, 3), 80, 200),
                new CombustionCar("BB0002BB", "Skoda Octavia", CarClass.Comfort, new Location(5, 5), 40, 7.5),
                new ElectricCar("CC0003CC", "Tesla Model S", CarClass.Premium, new Location(1, 1), 90, 500),
                new CombustionCar("DD0004DD", "Renault Master", CarClass.Cargo, new Location(10, 12), 60, 10.0)
            };
        }

        public IEnumerable<Vehicle> GetAllAvailable(CarClass desiredClass)
        {
            // Повертаємо лише вільні авто потрібного класу
            return _vehicles.Where(v => v.IsAvailable && v.ClassType == desiredClass);
        }

        public void Update(Vehicle vehicle)
        {
            //цей метод потрібен для правильної архітектури майбутніх ітерацій.
        }
    }
}