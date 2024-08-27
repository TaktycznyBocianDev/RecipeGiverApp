using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class StringSplitter
{
    public static List<string> SplitStringByNumberedSections(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();

        // Define a regular expression pattern to match numbers followed by a period and space (e.g., "1. ", "2. ", etc.)
        string pattern = @"(?=\d+\.\s)";

        // Use Regex.Split to divide the input string based on the pattern
        string[] parts = Regex.Split(input, pattern);

        // Filter out any empty strings that may result from the split and return as a list
        var result = new List<string>(parts);
        result.RemoveAll(string.IsNullOrWhiteSpace);

        return result;
    }
}
