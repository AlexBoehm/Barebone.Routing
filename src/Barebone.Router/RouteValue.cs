namespace Barebone.Routing
{
    /// <summary>
    /// Wrapper around the value string to enable Methods like IsInteger and other extension methods
    /// </summary>
    public struct RouteValue
    {
        string _value;

        public RouteValue(string value)
        {
            _value = value;
        }

        public string Value { get { return _value; } }

        public bool IsInteger()
        {
            int parsed;
            return int.TryParse(_value, out parsed);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ((RouteValue)obj)._value.Equals(this._value);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static implicit operator string(RouteValue routeValue)
        {
            return routeValue._value;
        }
    }
}