using System;
using System.Collections.Generic;
using System.Text;

namespace iExpr.Exprs.Math
{
    public struct RealNumber : IEquatable<RealNumber>
    {
        double Value;

        public RealNumber(double value)
        {
            Value = value;
        }

        public static implicit operator RealNumber(double value)
        {
            return new RealNumber(value);
        }
        public static implicit operator double(RealNumber value)
        {
            return value.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is RealNumber && Equals((RealNumber)obj);
        }

        public bool Equals(RealNumber other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            var hashCode = -783812246;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(RealNumber number1, RealNumber number2)
        {
            return number1.Equals(number2);
        }

        public static bool operator !=(RealNumber number1, RealNumber number2)
        {
            return !(number1 == number2);
        }
    }
}
