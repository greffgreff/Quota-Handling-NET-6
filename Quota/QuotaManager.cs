namespace quota.Quota
{
    public class QuotaManager
    {
        public string Name { get; }
        public int QuotaLimit { get; }
        public int QuotaLeft { get; private set; }
        public TimeSpan ReplinishTime { get; }
        public DateTime LastReplenish { get; private set; }
        public DateTime LastUse { get; private set; }

        private QuotaManager(string name, int quotaLimit, TimeSpan replenishTime) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Quota name cannot be null or whitespace");
            if (quotaLimit < 0)
                throw new ArgumentOutOfRangeException("Cannot have negative quota limit");
            if (replenishTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("Cannot have negative quota replenish window");

            Name = name;
            QuotaLimit = quotaLimit;
            ReplinishTime = replenishTime;
            QuotaLeft = QuotaLimit;
        }

        public static QuotaManager Of(string name, int quota, TimeSpan window)
        {
            return new QuotaManager(name, quota, window);
        }

        public void Use(int quota)
        {
            if (DateTime.Now - LastReplenish > ReplinishTime)
            {
                QuotaLeft = QuotaLimit;
                LastReplenish = DateTime.Now;
            }

            if (QuotaLeft <= 0)
            {
                throw new NotEnoughQuotaException(Name, quota, QuotaLeft);
            }
            
            QuotaLeft -= quota;
            LastUse = DateTime.Now;
        }

        public bool CanUse(int quota = 1)
        {
            try
            {
                Use(quota);
                return true;
            }
            catch(NotEnoughQuotaException) 
            {
                return false;
            }
        }
    }
}
