namespace OOPTask4.Core;

internal static class Utility
{
    public static void CheckIfGreaterThanZero(int parameterValue, string parameterName)
    {
        if (parameterValue <= 0)
        {
            throw new ArgumentException("Must be greater than zero", parameterName);
        }
    }
}