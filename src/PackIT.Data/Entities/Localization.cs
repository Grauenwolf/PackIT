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

        public Localization(string value)
        {
            var splitLocalization = value.Split(',');
            City = splitLocalization.First();
            Country = splitLocalization.Last();
        }

        public override string ToString() => $"{City},{Country}";
    }
}
