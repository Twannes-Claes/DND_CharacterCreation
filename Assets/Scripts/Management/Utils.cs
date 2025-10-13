using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    public static int ToModifier(int score) => Mathf.FloorToInt((score - 10) / 2f);
    public static int ToModifier(string text) => int.TryParse(text, out var score) ? ToModifier(score) : -99;
    public static string ToSignedNumber(int value) => value >= 0 ? $"+{value}" : value.ToString();
    public static string ToSignedNumber(string text) => ToSignedNumber(ToModifier(text));

    //2d8+3, modifier changed to +4 for example => 2d8+4
    public static string UpdateDiceModifier(int score, string diceText)
    {
        diceText = diceText.Replace(" ", "");

        var match = Regex.Match(diceText, @"^([0-9]*d[0-9]+)([+-]\d+)?$");

        if (!match.Success)
            return string.Empty; ;

        string dicePart = match.Groups[1].Value;
        string modifier = ToSignedNumber(score);

        if (score == 0)
        {
            modifier = string.Empty;
        }

        return dicePart + modifier;
    }
}
