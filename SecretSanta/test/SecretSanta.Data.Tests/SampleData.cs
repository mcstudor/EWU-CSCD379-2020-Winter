
using SecretSanta.Data;

namespace SecretSanta.Data.Tests
{
    public static class SampleData
    {
        public const string CheeseBurgerTitle = "Cheese Burger";
        public const string CheeseBurgerDescription = "Burger, bun, cheese, and special sauce";
        public const string CheeseBurgerUrl = "https://www.cheesebur.ger";

        public const string ChickenTitle = "8-Wing Bucket";
        public const string ChickenDescription = "8 breaded chicken wings with some herbs and a spice";
        public const string ChickenUrl = "https://www.friedchick.en";

        public const string CheeseBurgerFirstName = "Ronald";
        public const string CheeseBurgerLastName = "McDonald";

        public const string ChickenFirstName = "Colonel";
        public const string ChickenLastName = "Sanders";

        public const string FastFoodTitle = "Fast Food";


        public static Gift CreateCheeseBurgerGift() =>
            new Gift(CheeseBurgerTitle, CheeseBurgerDescription, CheeseBurgerUrl, CreateChickenUser());

        public static Gift CreateChickenGift() =>
            new Gift(ChickenTitle, ChickenDescription, CheeseBurgerUrl, CreateCheeseBurgerUser());

        public static Group CreateFastFoodGroup() => new Group(FastFoodTitle);

        public static User CreateChickenUser() => 
            new User(ChickenFirstName, ChickenLastName);

        public static User CreateCheeseBurgerUser() => 
            new User(CheeseBurgerFirstName, CheeseBurgerLastName);

        public static string GetCheeseUsername() =>
            $"{CheeseBurgerFirstName.Substring(0, 1)}{CheeseBurgerLastName}";

        public static string GetChickenUsername() =>
            $"{ChickenFirstName.Substring(0, 1)}{ChickenLastName}";
    }
}