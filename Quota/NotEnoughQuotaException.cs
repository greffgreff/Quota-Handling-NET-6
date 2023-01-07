namespace quota.Quota
{
    public class NotEnoughQuotaException : Exception
    { 
        public NotEnoughQuotaException(string name, int quotaUse, int quotaLeft) 
            : base($"Not enough quota left for '{name}'. {quotaLeft} quota left for {quotaUse} use")
        {

        }
    }
}
