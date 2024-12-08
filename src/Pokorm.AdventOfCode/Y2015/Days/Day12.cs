namespace Pokorm.AdventOfCode.Y2015.Days;

// https://adventofcode.com/2015/day/12
public class Day12
{
    public long Solve(string input)
    {
        var data = Parse(input);

        var result = 0;

        var visitor = new JVisitor();

        visitor.Visit(data.Root, c =>
        {
            result += c is JInteger i ? i.Value : 0;
        });

        return result;
    }

    public long SolveBonus(string input)
    {
        var data = Parse(input);

        var result = 0;

        return result;
    }

    public static DayData Parse(string input)
    {
        var root = ParseJson(input);

        return new DayData(root);
    }

    public static JToken ParseJson(string input) => ParseJToken(input).JToken;

    public static (JToken JToken, string Other) ParseJToken(string input)
    {
        input = input.Trim();

        if (input.StartsWith("["))
        {
            input = input[1..];

            var items = new List<JToken>();

            while (true)
            {
                (var next, input) = ParseJToken(input);

                items.Add(next);

                input = input.Trim();

                if (input.StartsWith(","))
                {
                    input = input[1..];

                    continue;
                }

                if (input.StartsWith("]"))
                {
                    input = input[1..];

                    break;
                }

                throw new Exception();
            }

            return (new JArray(items), input);
        }

        if (input.StartsWith("{"))
        {
            input = input[1..];

            var items = new Dictionary<string, JToken>();

            while (true)
            {
                (var propNameToken, input) = ParseJToken(input);

                if (propNameToken is not JString prop)
                {
                    throw new Exception();
                }

                input = input.Trim();

                if (!input.StartsWith(":"))
                {
                    throw new Exception();
                }

                input = input[1..];

                (var next, input) = ParseJToken(input);

                items.Add(prop.Value, next);

                input = input.Trim();

                if (input.StartsWith(","))
                {
                    input = input[1..];

                    continue;
                }

                if (input.StartsWith("}"))
                {
                    input = input[1..];

                    break;
                }

                throw new Exception(input);
            }

            return (new JObject(items), input);
        }

        if (input.StartsWith("\""))
        {
            input = input[1..];

            var text = new string(input.TakeWhile(c => c != '"').ToArray());

            input = input[(text.Length + 1)..];

            return (new JString(text), input);
        }

        if (input[0] == '-' || char.IsDigit(input[0]))
        {
            var negative = input[0] == '-';

            if (negative)
            {
                input = input[1..];
            }

            var text = new string(input.TakeWhile(char.IsDigit).ToArray());

            input = input[(text.Length - 1)..];

            var value = int.Parse(text);

            if (negative)
            {
                value = -value;
            }

            return (new JInteger(value), input[1..]);
        }

        return ParseJToken(input[1..]);
    }

    public class JVisitor
    {
        public void Visit(JToken token, Action<JToken> action)
        {
            if (token is JArray a)
            {
                Visit(a, action);
            }
            else if (token is JObject b)
            {
                Visit(b, action);
            }
            else if (token is JInteger c)
            {
                Visit(c, action);
            }
            else if (token is JString d)
            {
                Visit(d, action);
            }
            else
            {
                throw new Exception();
            }
        }

        private void Visit(JArray token, Action<JToken> action)
        {
            foreach (var tokenItem in token.Items)
            {
                Visit(tokenItem, action);
            }
        }

        private void Visit(JString token, Action<JToken> action)
        {
            action(token);
        }

        private void Visit(JObject token, Action<JToken> action)
        {
            foreach (var tokenItem in token.Properties.Values)
            {
                Visit(tokenItem, action);
            }
        }

        private void Visit(JInteger token, Action<JToken> action)
        {
            action(token);
        }
    }

    public record JToken;

    public record JArray(List<JToken> Items) : JToken;

    public record JObject(Dictionary<string, JToken> Properties) : JToken;

    public record JInteger(int Value) : JToken;

    public record JString(string Value) : JToken;

    public record DayData(JToken Root) { }
}
