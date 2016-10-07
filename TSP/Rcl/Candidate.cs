namespace TSP.Rcl
{
    public class Candidate
    {
        public int Delta { get; set; }
        public int CityIndex { get; set; }
        public int InsertAt { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null || GetType() != obj.GetType() || GetHashCode() != obj.GetHashCode())
                return false;

            return Equals((Candidate)obj);
        }

        public bool Equals(Candidate obj)
        {
            return Delta == obj.Delta && CityIndex == obj.CityIndex && InsertAt == obj.InsertAt;
        }

        public override int GetHashCode()
        {
            return new { Delta, CityIndex, InsertAt }.GetHashCode();
        }
    }
}
