namespace ShowMoBud.Models
{
    public class Dispensary
    {

        public string Name { get; set; } = "";
        public List<Address> Addresses { get; set; } = new List<Address>();

    }
}
