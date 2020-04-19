namespace FluentDeepCompare.Steps
{
    internal static class ResultMessage
    {
        internal static string FormatTrackingMessage(string name, int level, char stepSymbol, bool areEqual, object leftValue, object rightValue)
        {
            return $"{AreAqualText(areEqual)} {LevelText(level)} ({stepSymbol}) {name} |{leftValue ?? "null"}|{rightValue ?? "null"}|";
        }

        internal static string ObjectMessage(string name, int level, char stepSymbol)
        {
            return $"    {LevelText(level)} ({stepSymbol}) {name}:";
        }

        internal static string CollecionsDiffLengthMessage(string name, int level, char stepSymbol, int leftLength, int rightLength)
        {
            return $"{AreAqualText(false)} {LevelText(level)} ({stepSymbol}) {name} |Length={leftLength}|Length={rightLength}|";
        }

        internal static string CollecionsLengthMessage(string name, int level, char stepSymbol, int? length)
        {
            return $"    {LevelText(level)} ({stepSymbol}) {name} |Length={length?.ToString() ?? "?"}|";
        }

        private static string AreAqualText(bool areEqual) => (areEqual) ? "[=]" : "[X]";

        private static string LevelText(int level) => "".PadLeft(3 * level);
    }
}