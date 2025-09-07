namespace WheelSell.DAL.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}