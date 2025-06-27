using System.Collections.Frozen;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/22
public class Day22(ILogger<Day22> logger)
{
    public long Solve(string[] lines)
    {
        var data = Parse(lines);

        var result = 0L;

        foreach (var secret in data.Secrets)
        {
            var lastSecret = GetNextSecret(secret, 2000);

            result += lastSecret;
        }

        return result;
    }

    public long SolveBonus(string[] lines)
    {
        var data = Parse(lines);

        var allChanges = new List<List<(short Price, List<SecretRowSequence> Sequence)>>();

        foreach (var secret in data.Secrets.Select(x => new SecretRow(x, GetLastDigit(x), 0)))
        {
            var rows = new List<SecretRow>(); // without the first
            var changes = new HashSet<SecretRowSequence>();
            var cursorSecret = secret;

            for (var i = 0; i < 2000; i++)
            {
                var nextSecretRow = GetNextSecretRow(cursorSecret);

                rows.Add(nextSecretRow);

                if (rows.Count > 4)
                {
                    changes.Add(new SecretRowSequence(rows[^4], rows[^3], rows[^2], rows[^1]));
                }

                cursorSecret = nextSecretRow;
            }

            var orderedChanges = changes.GroupBy(x => x.Change4.LastDigit)
                                        .OrderByDescending(x => x.Key)
                                        .Select(x => (x.Key, x.Distinct().ToList()))
                                        .ToList();

            allChanges.Add(orderedChanges);
        }

        var all = allChanges.SelectMany(x => x)
                            .SelectMany(x => x.Sequence)
                            .ToList();

        var unique = all.Distinct().ToList();

        var lookup = allChanges.SelectMany((x, i) => x.Select(y => (Index: i, y.Price, y.Sequence)))
                               .SelectMany(x => x.Sequence.Select(y => (x.Index, x.Price, Sequence: y)))
                               .GroupBy(x => (x.Index, x.Sequence))
                               .Select(x => (x.Key.Index, x.Key.Sequence, x.First().Price))
                               .ToFrozenDictionary(x => (x.Index, x.Sequence), x => x.Item3);

        var max = 0L;
        SecretRowSequence? seq = null;

        foreach (var ss in unique)
        {
            var sum = 0L;

            foreach (var (i, _) in data.Secrets.Index())
            {
                if (!lookup.TryGetValue((i, ss), out var maxPrice))
                {
                    continue;
                }

                sum += maxPrice;
            }

            if (sum > max)
            {
                max = sum;
                seq = ss;
            }
        }

        return max;
    }

    public static long GetNextSecret(long secret, int iterations)
    {
        for (var i = 0; i < iterations; i++)
        {
            secret = GetNextSecret(secret);
        }

        return secret;
    }

    public static short GetLastDigit(long secret) => (short) Math.DivRem(secret, 10).Remainder;

    public static long GetNextSecret(long secret)
    {
        secret = Prune(Mix(secret * 64, secret));
        secret = Prune(Mix(secret / 32, secret));
        secret = Prune(Mix(secret * 2048, secret));

        return secret;
    }

    public static SecretRow GetNextSecretRow(SecretRow secret)
    {
        var nextSecret = GetNextSecret(secret.Secret);
        var d = GetLastDigit(nextSecret);
        var diff = d - secret.LastDigit;

        return new SecretRow(nextSecret, d, diff);
    }

    private static long Mix(long value, long secret) => value ^ secret;

    private static long Prune(long secret) => secret % 16777216;

    private static DayData Parse(string[] lines)
    {
        var secrets = lines.Select(x => long.Parse(x));

        return new DayData(secrets.ToList());
    }

    private record DayData(List<long> Secrets);

    public record SecretRow(long Secret, short LastDigit, int Change)
    {
        public override string ToString() => $"{this.Secret.ToString(),8}: {this.LastDigit} ({this.Change})";
    }

    public record SecretRowSequence(SecretRow Change1, SecretRow Change2, SecretRow Change3, SecretRow Change4)
    {
        public virtual bool Equals(SecretRowSequence? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Change1.Change.Equals(other.Change1.Change)
                   && this.Change2.Change.Equals(other.Change2.Change)
                   && this.Change3.Change.Equals(other.Change3.Change)
                   && this.Change4.Change.Equals(other.Change4.Change);
        }

        public override int GetHashCode() => HashCode.Combine(this.Change1.Change, this.Change2.Change, this.Change3.Change, this.Change4.Change);

        public override string ToString() => $"{this.Change1.Change},{this.Change2.Change},{this.Change3.Change},{this.Change4.Change}";
    }
}
