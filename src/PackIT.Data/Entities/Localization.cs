using System.Linq;

namespace PackIT.Data.Entities
{
    public class Localization
    {
        public Localization()
        {
        }

        public Localization(string city, string country)
        {
            City = city;
            Country = country;
        }

        public string City { get; set; }
        public string Country { get; set; }

        public static Localization Create(string value)
        {
            var splitLocalization = value.Split(',');
            return new Localization
            {
                City = splitLocalization.First(),
                Country = splitLocalization.Last()
            };
        }

        public override string ToString()
            => $"{City},{Country}";
    }
}
