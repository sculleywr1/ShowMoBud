using ShowMoBud.Models;

namespace ShowMoBud.Services
{
    public class DispensaryService
    {

        private static readonly List<Dispensary> _all = new()
        {

            new() {Name = "Heya Wellness", Address = "4300 N Service Rd, St. Peters, MO 63376", Latitude = 38.798870, Longitude = -90.583379},
            new() {Name = "Root 66", Address = "3004 S St Peters Pkwy, St. Peters, MO 63303", Latitude = 38.772820, Longitude = -90.629570},
            new() {Name = "Kind Goods", Address = "3899 Veterans Memorial,, Ste J, St Peters, MO 63376", Longitude = -90.576820, Latitude = 38.799030},
            new() {Name = "Mint Cannabis", Address = "150 Mid Rivers Mall Cir, St. Peters, MO 63376", Longitude = -90.576820, Latitude = 38.799030}
        };

        public IReadOnlyList<Dispensary> GetAll() => _all;
    }
}
