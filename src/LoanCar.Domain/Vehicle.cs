using System;

namespace LoanCar.Domain
{
    // Абстрактний базовий клас
    public abstract class Vehicle
    {
        public string LicensePlate { get; }
        public string Model { get; }
        public CarClass ClassType { get; }
        public Location CurrentLocation { get; private set; }
        public bool IsAvailable { get; private set; }

        protected Vehicle(string licensePlate, string model, CarClass classType, Location location)
        {
            LicensePlate = licensePlate;
            Model = model;
            ClassType = classType;
            CurrentLocation = location;
            IsAvailable = true; // За замовчуванням авто вільне
        }

        public void Book()
        {
            IsAvailable = false;
        }

        public void CompleteRental(Location newLocation)
        {
            CurrentLocation = newLocation;
            IsAvailable = true;
        }

        // Абстрактний метод, який змушує спадкоємців рахувати запас ходу
        public abstract double CalculateRemainingRangeKm();
    }

    // Спадкоємець 1: Електромобіль
    public class ElectricCar : Vehicle
    {
        public int BatteryPercentage { get; private set; }
        public double MaxRangeKm { get; }

        public ElectricCar(string licensePlate, string model, CarClass classType, Location location, int battery, double maxRange) 
            : base(licensePlate, model, classType, location)
        {
            BatteryPercentage = battery;
            MaxRangeKm = maxRange;
        }

        public override double CalculateRemainingRangeKm()
        {
            return (BatteryPercentage / 100.0) * MaxRangeKm;
        }
    }

    // Спадкоємець 2: Бензинове авто
    public class CombustionCar : Vehicle
    {
        public double CurrentFuelLiters { get; private set; }
        public double FuelConsumptionPer100Km { get; }

        public CombustionCar(string licensePlate, string model, CarClass classType, Location location, double fuel, double consumption) 
            : base(licensePlate, model, classType, location)
        {
            CurrentFuelLiters = fuel;
            FuelConsumptionPer100Km = consumption;
        }

        public override double CalculateRemainingRangeKm()
        {
            return (CurrentFuelLiters / FuelConsumptionPer100Km) * 100.0;
        }
    }
}