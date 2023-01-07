namespace quota.Quota
{
    public class QuotaManager
    {
        public string Name { get; }
        public int QuotaLimit { get; }
        public int DefaultUseCost { get; }
        public int QuotaLeft { get; private set; }
        public TimeSpan ReplinishTime { get; }
        public DateTime LastReplenish { get; private set; }
        public DateTime LastUse { get; private set; }

        private QuotaManager(string name, int quotaLimit, TimeSpan replenishTime, int useCost) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Quota name cannot be null or whitespace");
            if (quotaLimit < 0)
                throw new ArgumentOutOfRangeException("Cannot have negative quota limit");
            if (replenishTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("Cannot have negative quota replenish window");
            if (useCost <= 0)
                throw new ArgumentOutOfRangeException("Cannot have per quota use cost negative or equal to zero");

            Name = name;
            QuotaLimit = quotaLimit;
            ReplinishTime = replenishTime;
            QuotaLeft = QuotaLimit;
            DefaultUseCost = useCost;
        }

        public static QuotaManager Of(string name, int quota, TimeSpan window, int useCost = 1)
        {
            return new QuotaManager(name, quota, window, useCost);
        }

        public void Use()
        {
            UseFor(DefaultUseCost);
        }

        public void UseFor(int quota)
        {
            if (DateTime.Now - LastReplenish > ReplinishTime)
            {
                QuotaLeft = QuotaLimit;
                LastReplenish = DateTime.Now;
            }

            if (QuotaLeft <= 0)
                throw new NotEnoughQuotaException(Name, quota, QuotaLeft);
            
            QuotaLeft -= quota;
            LastUse = DateTime.Now;
        }

        public bool CanUse()
        {
            try
            {
                Use();
                return true;
            }
            catch(NotEnoughQuotaException) 
            {
                return false;
            }
        } 
    }
}
