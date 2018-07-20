namespace DataLogistics.Model
{
    public class RiverDtoStoredEvent
    {
        public RiverDtoStoredEvent(BlobStorageLocation blobLocation)
        {
            BlobLocation = blobLocation;
        }

        public BlobStorageLocation BlobLocation { get; }
    }
}