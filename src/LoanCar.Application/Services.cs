using System.Linq;
using LoanCar.Domain;

namespace LoanCar.Application
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Result<User> Login(string username)
        {
            var user = _userRepository.GetUserByName(username);
            if (user == null)
            {
                return Result<User>.Failure($"Користувача з іменем '{username}' не знайдено.");
            }
            return Result<User>.Success(user);
        }
    }

    public class RentalService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public RentalService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public Result<Vehicle> FindAndBookNearestCar(User user, CarClass desiredClass, Location userLocation)
        {
            if ((desiredClass == CarClass.Premium || desiredClass == CarClass.Cargo) && !user.CanRentPremiumOrCargo())
            {
                return Result<Vehicle>.Failure("Доступ заборонено. Для преміум та вантажних авто потрібен стаж від 2 років.");
            }

            // 2. Отримання доступних авто з репозиторію
            var availableCars = _vehicleRepository.GetAllAvailable(desiredClass).ToList();
            if (!availableCars.Any())
            {
                return Result<Vehicle>.Failure("На жаль, немає доступних авто обраного класу.");
            }

            // 3. Пошук найближчого авто
            var nearestCar = availableCars
                .OrderBy(c => c.CurrentLocation.CalculateDistanceTo(userLocation))
                .First();

            // 4. Зміна статусу та збереження
            nearestCar.Book();
            _vehicleRepository.Update(nearestCar);

            return Result<Vehicle>.Success(nearestCar);
        }
    }
}