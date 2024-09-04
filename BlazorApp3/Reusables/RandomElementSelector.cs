using RecipeGiverApp.Web.Reusables;

namespace RecipeGiverApp.Web.Reusables
{
    public static class RandomElementSelector
    {
        private static readonly Random _random = new Random();

        public static T GetRandomElement<T>(List<T> items)
        {
            if (items == null || items.Count == 0)
            {
                throw new ArgumentException("The list cannot be null or empty.", nameof(items));
            }

            int index = _random.Next(items.Count);
            return items[index];
        }
    }
}
