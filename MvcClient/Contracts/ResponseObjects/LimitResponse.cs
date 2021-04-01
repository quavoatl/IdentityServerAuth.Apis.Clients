namespace MvcClient.Contracts.ResponseObjects
{
    public class LimitResponse
    {
        public int LimitId { get; set; }
        public int Value { get; set; }
        public string CoverType { get; set; }
        public string BrokerId { get; set; }
    }
}