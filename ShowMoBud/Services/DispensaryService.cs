using ShowMoBud.Models;

namespace ShowMoBud.Services
{
    public class DispensaryService
    {

        private static readonly List<Dispensary> _all = new()
        {
            new()
            {
                Name = "Greenlight Dispensary",
                Addresses =
                {
                    new Address
                    {
                        FullAddress = "6497 Chippewa St, St. Louis, MO 63109",
                        Latitude = 38.5928571,
                        Longitude = -90.3015986
                    },
                    new Address
                    {
                        FullAddress = "9800 Manchester Rd, Suite C, St. Louis, MO 63119",
                        Latitude = 38.606623,
                        Longitude = -90.372594
                    }
                }
            }
        };

        public IReadOnlyList<Dispensary> GetAll() => _all;
    }
}
