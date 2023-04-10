using System.Numerics;

namespace Math;

public class BigFraction
{

	public BigInteger Numerator { get; }
	public BigInteger Denominator { get; }

	public BigFraction(BigInteger numerator, BigInteger denominator)
	{
		Numerator = numerator;
		Denominator = denominator;

		if(denominator == 0)
		{
			throw new DivideByZeroException($"{this} is not a valid {GetType()}.");
		}
	}

	public BigFraction Simplify()
	{
		BigInteger gcd = BigInteger.GreatestCommonDivisor(Numerator, Denominator);

		int sign = Denominator < 0 ? -1 : 1;
		return new BigFraction(sign * Numerator / gcd, sign * Denominator / gcd);
	}

	public static BigFraction operator +(BigFraction a)
	{
		return a;
	}

	public static BigFraction operator -(BigFraction a)
	{
		return new(-a.Numerator, a.Denominator);
	}

	public static BigFraction operator +(BigFraction a, BigFraction b)
	{
		return new((a.Numerator * b.Denominator) + (b.Numerator * a.Denominator), a.Denominator * b.Denominator);
	}

	public static BigFraction operator -(BigFraction a, BigFraction b)
	{
		return a + (-b);
	}

	public static BigFraction operator *(BigFraction a, BigFraction b)
	{
		return new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
	}

	public static BigFraction operator /(BigFraction a, BigFraction b)
	{
		return new(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
	}

	public static implicit operator BigFraction(BigInteger i)
	{
		return new(i, 1);
	}

	public static implicit operator BigFraction(int i)
	{
		return new(i, 1);
	}

	public static implicit operator BigFraction(uint i)
	{
		return new(i, 1);
	}

	public static implicit operator BigFraction(long l)
	{
		return new(l, 1);
	}

	public static implicit operator BigFraction(ulong l)
	{
		return new(l, 1);
	}

	public override string ToString()
	{
		return Denominator.Equals(BigInteger.One) ? Numerator.ToString() : $"{Numerator}/{Denominator}";
	}

	public bool Equals(BigFraction other)
	{
		return Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);
	}

	public override bool Equals(object? obj)
	{
		return obj is BigFraction frac && Equals(frac);
	}
}