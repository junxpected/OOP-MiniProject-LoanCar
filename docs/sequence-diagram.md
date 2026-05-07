---
config:
  theme: dark
---
sequenceDiagram
    autonumber
    actor User as Користувач
    participant UI as Консоль (Інтерфейс)
    participant Auth as AuthService (Авторизація)
    participant Rental as RentalService (Оренда)
    participant DB as Сховище (In-Memory)

    %% Блок логіну
    Note over User, DB: Етап 1: Авторизація
    User->>UI: Вводить логін
    UI->>Auth: LoginUser(username)
    Auth->>DB: GetUser(username)
    DB-->>Auth: Дані користувача (включаючи стаж)
    Auth-->>UI: Успішний вхід

    %% Блок пошуку та перевірок
    Note over User, DB: Етап 2: Вибір авто та перевірка стажу
    User->>UI: Обирає клас "Premium" та вводить координати
    UI->>Rental: GetNearestCars(user, "Premium", location)
    Rental->>Rental: Перевірка: чи стаж > 2 років?
    alt Стаж менше 2 років
        Rental-->>UI: Помилка: "Доступ заборонено. Малий стаж."
        UI-->>User: Виводить повідомлення про помилку
    else Стаж більше 2 років
        Rental->>DB: GetAllAvailableCars("Premium")
        DB-->>Rental: Список авто
        
        Note right of Rental: Розрахунок відстані<br/>та запасу ходу (км)
        Rental-->>UI: Відсортований список авто
        UI-->>User: Показує список авто на екрані
        
        %% Блок бронювання
        Note over User, DB: Етап 3: Бронювання
        User->>UI: Обирає авто зі списку
        UI->>Rental: BookCar(user, carId)
        Rental->>DB: Змінити статус авто на "Booked"
        Rental-->>UI: Успіх
        UI-->>User: "Авто успішно заброньовано!"
    end