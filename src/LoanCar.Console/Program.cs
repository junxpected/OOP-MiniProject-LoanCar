using System;
using LoanCar.Domain;
using LoanCar.Infrastructure;
using LoanCar.Application;

namespace LoanCar.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Ініціалізація інфраструктури та сервісів
            IUserRepository userRepository = new InMemoryUserRepository();
            IVehicleRepository vehicleRepository = new InMemoryVehicleRepository();

            AuthService authService = new AuthService(userRepository);
            RentalService rentalService = new RentalService(vehicleRepository);

            Console.WriteLine("=== Вітаємо в NextGen CarSharing ===");

            // 2. Логін
            Console.WriteLine("Доступні тестові акаунти: 'Новачок' (стаж 1 рік) або 'Профі' (стаж 5 років)");
            Console.Write("Введіть ваше ім'я: ");
            string? username = Console.ReadLine();

            var loginResult = authService.Login(username ?? "");

            if (!loginResult.IsSuccess)
            {
                Console.WriteLine($"\n[Помилка] {loginResult.ErrorMessage}");
                return; // Зупиняємо програму, якщо логін не вдався
            }

            User currentUser = loginResult.Value!;
            Console.WriteLine($"\nУспішний вхід! Вітаємо, {currentUser.Name} (Стаж: {currentUser.DrivingExperienceYears} років).");

            // 3. Введення даних для пошуку
            Console.WriteLine("\n--- Пошук авто ---");
            Console.Write("Введіть вашу координату X (число, наприклад 3): ");
            double x = Convert.ToDouble(Console.ReadLine());

            Console.Write("Введіть вашу координату Y (число, наприклад 4): ");
            double y = Convert.ToDouble(Console.ReadLine());
            Location userLocation = new Location(x, y);

            Console.WriteLine("\nОберіть клас авто:");
            Console.WriteLine("1 - Економ (Economy)");
            Console.WriteLine("2 - Комфорт (Comfort)");
            Console.WriteLine("3 - Преміум (Premium)");
            Console.WriteLine("4 - Вантажне (Cargo)");
            Console.Write("Ваш вибір (1-4): ");
            
            int classChoice = Convert.ToInt32(Console.ReadLine());
            CarClass desiredClass = (CarClass)classChoice;

            // 4. Виконання бізнес-логіки
            Console.WriteLine("\nШукаємо найближче авто...");
            var rentalResult = rentalService.FindAndBookNearestCar(currentUser, desiredClass, userLocation);

            // 5. Виведення результату
            if (rentalResult.IsSuccess)
            {
                Vehicle bookedCar = rentalResult.Value!;
                Console.WriteLine("\n[УСПІХ] Авто успішно заброньовано!");
                Console.WriteLine($"Модель: {bookedCar.Model} ({bookedCar.LicensePlate})");
                
                Console.WriteLine($"Запас ходу: {bookedCar.CalculateRemainingRangeKm():F1} км");
                
                Console.WriteLine($"Відстань до авто: {bookedCar.CurrentLocation.CalculateDistanceTo(userLocation):F1} од.");
            }
            else
            {
                Console.WriteLine($"\n[ВІДМОВА] {rentalResult.ErrorMessage}");
            }
        }
    }
}