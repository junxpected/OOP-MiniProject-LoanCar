using System;

namespace LoanCar.Domain
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public int DrivingExperienceYears { get; }
        public bool HasActiveRental { get; private set; }

        public User(string name, int drivingExperienceYears)
        {
            Id = Guid.NewGuid();
            Name = name;
            DrivingExperienceYears = drivingExperienceYears;
            HasActiveRental = false;
        }

        // Бізнес-правило: чи можна орендувати преміум/вантажівку?
        public bool CanRentPremiumOrCargo()
        {
            return DrivingExperienceYears >= 2;
        }

        public void StartRental() => HasActiveRental = true;
        public void EndRental() => HasActiveRental = false;
    }
}