```mermaid
classDiagram
    class Location {
        +double X
        +double Y
        +CalculateDistanceTo(other Location) double
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
        +CalculateRemainingRangeKm() double
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
    class IRentalService {
        <<interface>>
        +FindAndBookCar(userId Guid, desiredClass CarClass, userLocation Location) Result
    }
    class RentalService {
        -IUserRepository _userRepo
        -IVehicleRepository _vehicleRepo
        +FindAndBookCar() Result
    }
    class IUserRepository {
        <<interface>>
        +GetUserById(id Guid) User
    }
    class IVehicleRepository {
        <<interface>>
        +GetAllAvailable() List
        +Update(vehicle Vehicle)
    }
    class InMemoryVehicleRepo {
        -List _cars
        +GetAllAvailable() List
        +Update(vehicle Vehicle)
    }
    classDiagram
    class Result {
        +bool IsSuccess
        +string ErrorMessage
        +Success(value) Result
        +Failure(error string) Result
    }
    Vehicle --> Result
    Vehicle <|-- CombustionCar
    Vehicle <|-- ElectricCar
    Vehicle --> Location
    Vehicle --> CarClass
    RentalService ..|> IRentalService
    InMemoryVehicleRepo ..|> IVehicleRepository
    RentalService --> IUserRepository
    RentalService --> IVehicleRepository
```