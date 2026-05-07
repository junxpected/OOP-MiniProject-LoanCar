classDiagram
    direction TB

    %% --- ШАР ДОМЕНУ (Domain Layer) ---
    %% Сутності та правила, які не залежать від інших шарів

    class Location {
        +double X
        +double Y
        +CalculateDistanceTo(other: Location) double
    }

    class CarClass {
        <<enumeration>>
        Economy
        Comfort
        Premium
        Cargo
    }

    class User {
        +Guid Id
        +string Name
        +int DrivingExperienceYears
        +bool HasActiveRental
        +CanRentPremiumOrCargo() bool
    }

    class Vehicle {
        <<abstract>>
        +string LicensePlate
        +string Model
        +CarClass ClassType
        +Location CurrentLocation
        +bool IsAvailable
        +Book() Result
        +CompleteRental(newLocation: Location)
        +CalculateRemainingRangeKm() double*
    }

    class ElectricCar {
        +int BatteryPercentage
        +double MaxRangeKm
        +CalculateRemainingRangeKm() double
    }

    class CombustionCar {
        +double CurrentFuelLiters
        +double FuelConsumptionPer100Km
        +CalculateRemainingRangeKm() double
    }

    class Result~T~ {
        +bool IsSuccess
        +T Value
        +string ErrorMessage
        +Success(value: T) Result
        +Failure(error: string) Result
    }

    %% Зв'язки Домену
    Vehicle <|-- ElectricCar : Успадкування
    Vehicle <|-- CombustionCar : Успадкування
    Vehicle --> Location : Містить
    Vehicle --> CarClass : Використовує

    %% --- ШАР ЗАСТОСУНКУ (Application Layer) ---
    %% Бізнес-логіка та координація

    class IRentalService {
        <<interface>>
        +FindAndBookCar(userId: Guid, desiredClass: CarClass, userLocation: Location) Result~Vehicle~
    }

    class RentalService {
        -IUserRepository _userRepo
        -IVehicleRepository _vehicleRepo
        +FindAndBookCar(...) Result~Vehicle~
    }

    %% --- ШАР ІНФРАСТРУКТУРИ (Infrastructure Layer) ---
    %% Робота з даними

    class IUserRepository {
        <<interface>>
        +GetUserById(id: Guid) User
    }

    class IVehicleRepository {
        <<interface>>
        +GetAllAvailable() List~Vehicle~
        +Update(vehicle: Vehicle)
    }

    class InMemoryVehicleRepo {
        -List~Vehicle~ _cars
        +GetAllAvailable() List~Vehicle~
        +Update(vehicle: Vehicle)
    }

    %% Зв'язки між шарами
    IRentalService <|.. RentalService : Реалізує
    IVehicleRepository <|.. InMemoryVehicleRepo : Реалізує
    RentalService --> IUserRepository : Залежить (через інтерфейс)
    RentalService --> IVehicleRepository : Залежить (через інтерфейс)
    RentalService --> User : Працює з
    RentalService --> Vehicle : Працює з