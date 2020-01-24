namespace SecretSanta.Data.Tests
{
    public class SampleData
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


        public static Gift CreateCheeseBurgerGift() => new Gift
        {
            Title = CheeseBurgerTitle,
            Description = CheeseBurgerDescription,
            Url = CheeseBurgerUrl,
            User = CreateCheeseBurgerUser()
        };

        public static Gift CreateChickenGift() => new Gift
        {
            Title = ChickenTitle,
            Description = ChickenDescription,
            Url = ChickenUrl,
            User = CreateChickenUser()
        };

        public static User CreateChickenUser() => new User
        {
            FirstName = ChickenFirstName,
            LastName = ChickenLastName
        };


        public static User CreateCheeseBurgerUser() => new User
        {
            FirstName = CheeseBurgerFirstName,
            LastName = CheeseBurgerLastName
        };
    }
}
