namespace DAL
{
    public class Carousel : EntityBase
    {
        public string Title { get; set; }
        public byte[] Img { get; set; }
        public string ContentType { get; set; }
    }
}