using System.Diagnostics.Contracts;
using System.Text;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/9
public class Day09
{
    /*public readonly ILogger<Day09> logger;

    public Day09(ILogger<Day09> logger)
    {
        this.logger = logger;
    }*/

    public long Solve(string input)
    {
        var data = Parse(input);

        FileChange? change = null;

        do
        {
            change = data.TryToMoveNextFile();

            if (change is null)
            {
                break;
            }

            var newItems = data.ApplyFileChange(change);

            data = new Disk(newItems);
        } while (true);

        var checksum = data.ComputeChecksum();

        return checksum;
    }

    public long SolveBonus(string input)
    {
        var data = Parse(input);
        var result = 0;

        return result;
    }

    public static Disk Parse(string input)
    {
        var fileIndex = 0;
        var items = new List<DiskItem>();

        foreach (var (i, c) in input.Index())
        {
            if (i % 2 == 0)
            {
                items.Add(new File(fileIndex, c));

                fileIndex++;
            }
            else
            {
                items.Add(new FreeSpace(c));
            }
        }

        return new Disk(ExpandToSingleItems(items).ToList());
    }

    public record struct CharNum(char Char, int Num)
    {
        public static implicit operator int(CharNum num) => num.Num;

        public static implicit operator CharNum(char num) => new CharNum(num, int.Parse(num.ToString()));
    }

    public abstract record DiskItem(CharNum BlockCount);

    public abstract record SingleDiskItem() : DiskItem('1');

    public record SingleFile(int Id) : SingleDiskItem();

    public record SingleFreeSpace() : SingleDiskItem();

    public record File(int Id, CharNum BlockCount) : DiskItem(BlockCount);

    public record FreeSpace(CharNum BlockCount) : DiskItem(BlockCount);

    public record FileChange(int FileId, int OriginalIndex, int NewIndex);

    public static IEnumerable<SingleDiskItem> ExpandToSingleItems(List<DiskItem> items)
    {
        foreach (var diskItem in items)
        {
            foreach (var _ in Enumerable.Range(0, diskItem.BlockCount))
            {
                if (diskItem is FreeSpace)
                {
                    yield return new SingleFreeSpace();
                }
                else if (diskItem is File f)
                {
                    yield return new SingleFile(f.Id);
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }

    public record Disk(List<SingleDiskItem> Items)
    {
        [Pure]
        public List<SingleDiskItem> ApplyFileChange(FileChange change)
        {
            var newItems = this.Items.ToList();

            newItems[change.OriginalIndex] = new SingleFreeSpace();

            newItems[change.NewIndex] = new SingleFile(change.FileId);

            return newItems;
        }

        public FileChange? TryToMoveNextFile()
        {
            var lastFileIndex = this.Items.FindLastIndex(i => i is SingleFile);

            if (lastFileIndex == -1)
            {
                return null;
            }

            var firstFreeSpace = this.Items.FindIndex(i => i is SingleFreeSpace);

            if (firstFreeSpace == -1 || firstFreeSpace >= lastFileIndex)
            {
                return null;
            }

            var fileId = ((SingleFile) this.Items[lastFileIndex]).Id;

            return new FileChange(fileId, lastFileIndex, firstFreeSpace);
        }

        public string ToBlockString()
        {
            var result = new StringBuilder();

            foreach (var diskItem in this.Items)
            {
                if (diskItem is SingleFreeSpace)
                {
                    result.Append('.');
                }
                else if (diskItem is SingleFile f)
                {
                    if (f.Id >= 10)
                    {
                        result.Append('[');
                        result.Append(f.Id);
                        result.Append(']');
                    }
                    else
                    {
                        result.Append(f.Id);
                    }
                }
                else
                {
                    throw new Exception();
                }
            }

            return result.ToString();
        }

        public long ComputeChecksum()
        {
            var result = 0L;

            foreach (var (i, item) in this.Items.Index())
            {
                if (item is not SingleFile f)
                {
                    break;
                }

                result += i * f.Id;
            }

            return result;
        }
    }
}
