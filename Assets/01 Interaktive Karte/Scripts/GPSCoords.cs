namespace InteractiveMap
{
    [System.Serializable]
    public class GPSCoords
    {
        [System.Serializable]
        public class DMS
        {
            public float degrees;
            public float minutes;
            public float seconds;

            public double ToDegreeDecimal()
            {
                return (((seconds / 60) + minutes) / 60) + degrees;
            }
        }

        public DMS latitude = new DMS();
        public DMS longitude = new DMS();
    }
}