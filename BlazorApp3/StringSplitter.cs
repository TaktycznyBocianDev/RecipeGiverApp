using System.Text.RegularExpressions;

public static class StringSplitter
{
    /// <summary>
    /// Takes a single input string and returns a list of strings, each starting with a sequence "number.".
    /// </summary>
    /// <param name="input">String, most common use for recipe instructions.</param>
    /// <returns>List of strings, where each of them is a separate instruction.</returns>
    public static List<string> SplitStringByNumberedSections(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        // Define a regular expression pattern to match numbers followed by a period and a space (e.g., "1. ", "2. ", etc.)
        string pattern = @"(\d+\.)";

        // Use Regex.Matches to find all numbered sections in the input string
        var matches = Regex.Matches(input, pattern);
        var result = new List<string>();

        // Loop through all matches to construct the resulting list
        for (int i = 0; i < matches.Count; i++)
        {
            // Find the start index of the current and next match
            int startIndex = matches[i].Index;
            int endIndex = (i < matches.Count - 1) ? matches[i + 1].Index : input.Length;

            // Extract the section, including the number and its associated text
            string section = input.Substring(startIndex, endIndex - startIndex).Trim();

            result.Add(section);
        }

        return result;
    }
}
