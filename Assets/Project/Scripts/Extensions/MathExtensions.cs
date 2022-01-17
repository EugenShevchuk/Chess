namespace Project.Extensions
{
    public static class MathExtensions
    {
        public static bool Odd(this int i)
        {
            return (i & 1) == 0;
        }
    }
}