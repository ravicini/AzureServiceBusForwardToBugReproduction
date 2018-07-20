namespace DataLogistics.Model
{
    public class BlobStorageLocation
    {
        public BlobStorageLocation(string container, string name)
        {
            Container = container;
            Name = name;
        }

        public string Container { get; set; }

        public string Name { get; set; }
    }
}