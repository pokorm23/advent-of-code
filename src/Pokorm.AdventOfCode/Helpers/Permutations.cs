namespace Pokorm.AdventOfCode.Helpers;

public static class Permutations
{
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
    {
        var array = enumerable as T[] ?? enumerable.ToArray();

        var factorials = Enumerable.Range(0, array.Length + 1)
                                   .Select(Factorial)
                                   .ToArray();

        for (var i = 0L; i < factorials[array.Length]; i++)
        {
            var sequence = GenerateSequence(i, array.Length - 1, factorials);

            yield return GeneratePermutation(array, sequence);
        }
    }

    private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
    {
        var clone = (T[]) array.Clone();

        for (var i = 0; i < clone.Length - 1; i++)
        {
            Swap(ref clone[i], ref clone[i + sequence[i]]);
        }

        return clone;
    }

    private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
    {
        var sequence = new int[size];

        for (var j = 0; j < sequence.Length; j++)
        {
            var facto = factorials[sequence.Length - j];

            sequence[j] = (int) (number / facto);
            number = (int) (number % facto);
        }

        return sequence;
    }

    private static void Swap<T>(ref T a, ref T b)
    {
        var temp = a;
        a = b;
        b = temp;
    }

    private static long Factorial(int n)
    {
        long result = n;

        for (var i = 1; i < n; i++)
        {
            result = result * i;
        }

        return result;
    }
}
