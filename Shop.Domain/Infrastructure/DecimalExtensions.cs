namespace Shop.Domain.Infrastructure
{
    public static class DecimalExtensions
    {
        public static string GetValueString(this decimal value) => $"${value:N2}";
    }
}
