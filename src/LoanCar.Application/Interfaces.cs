using System.Collections.Generic;
using LoanCar.Domain;

namespace LoanCar.Application
{
    // Інтерфейс для роботи з користувачами
    public interface IUserRepository
    {
        User? GetUserByName(string name); 
    }

    // Інтерфейс для роботи з автомобілями
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAllAvailable(CarClass desiredClass);
        void Update(Vehicle vehicle);
    }
}