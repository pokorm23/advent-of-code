using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Pokorm.AdventOfCode.Y2024.Days;

// https://adventofcode.com/2024/day/9
public class Day09
{
    public readonly ILogger<Day09> logger;

    public Day09(ILogger<Day09> logger) => this.logger = logger;

    public long Solve(string input)
    {
        var disk = Parse(input);

        this.logger.LogDebug(disk.ToString());

        var fragDisk = new Disk(disk.ApplyAllFileChanges().ToList());

        this.logger.LogDebug(fragDisk.ToString());

        var checksum = fragDisk.ComputeChecksum();

        return checksum;
    }

    public long SolveBonus(string input)
    {
        var disk = Parse(input);

        this.logger.LogDebug(disk.ToString());

        var fragDisk = new Disk(disk.ApplyAllFileChangesWithoutFragmentation(this.logger).ToList());

        this.logger.LogDebug(fragDisk.ToString());

        var checksum = fragDisk.ComputeChecksum();

        return checksum;
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

    public record SingleFile(int Id) : SingleDiskItem()
    {
        public override string ToString() => this.Id > 10 ? $"[{this.Id}]" : this.Id.ToString();
    }

    public record SingleFreeSpace() : SingleDiskItem()
    {
        public override string ToString() => ".";
    }

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
        public IEnumerable<SingleDiskItem> ApplyAllFileChanges()
        {
            var lastFileIndex = this.Items.FindLastIndex(i => i is SingleFile);

            if (lastFileIndex == -1)
            {
                throw new Exception();
            }

            var firstFreeSpace = this.Items.FindIndex(i => i is SingleFreeSpace);

            if (firstFreeSpace == -1 || firstFreeSpace >= lastFileIndex)
            {
                throw new Exception();
            }

            var appliedFiles = 0;

            for (var (i, j) = (0, this.Items.Count - 1); i < this.Items.Count && j >= 0 && i <= j;)
            {
                var originalI = i;
                var oi = this.Items[i] as SingleFreeSpace;
                var oj = this.Items[j] as SingleFile;

                if (oi is not null && oj is null) // both are free
                {
                    j--;

                    continue;
                }

                i++;

                if (oi is null)
                {
                    oj = (SingleFile) this.Items[originalI];
                }
                else
                {
                    j--;
                }

                appliedFiles++;

                yield return new SingleFile(oj.Id);
            }

            for (var i = 0; i < this.Items.Count - appliedFiles; i++)
            {
                yield return new SingleFreeSpace();
            }
        }

        [Pure]
        public IEnumerable<SingleDiskItem> ApplyAllFileChangesWithoutFragmentation(ILogger logger)
        {
            using var _ = logger.BeginScope("");

            var lastFileIndex = this.Items.FindLastIndex(i => i is SingleFile);

            if (lastFileIndex == -1)
            {
                throw new Exception();
            }

            var firstFreeSpace = this.Items.FindIndex(i => i is SingleFreeSpace);

            if (firstFreeSpace == -1 || firstFreeSpace >= lastFileIndex)
            {
                throw new Exception();
            }

            SingleFile? lastFile = null;
            Range? fileEndRange = null;
            var occu = new Dictionary<int, SingleDiskItem>();

            for (var j = this.Items.Count - 1; j >= 0; j--)
            {
                LogPos(logger, this, occu, fileEndRange, 0, j);

                var file = this.Items[j] as SingleFile;

                if (fileEndRange is null && file is not null)
                {
                    fileEndRange = new Range(j, j + 1);
                }

                if (lastFile is not null)
                {
                    Debug.Assert(fileEndRange.HasValue);

                    if (file is not null && lastFile.Id == file.Id)
                    {
                        fileEndRange = new Range(j, fileEndRange.Value.End);
                    }

                    if (lastFile.Id != file?.Id || j == 0)
                    {
                        var fileBlock = fileEndRange.Value.GetOffsetAndLength(this.Items.Count);

                        SingleFreeSpace? lastFree = null;
                        Range? freeStartRange = null;
                        var found = false;

                        // iterate for empty free space
                        for (var i = 0; i < j; i++)
                        {
                            LogPos(logger, this, occu, fileEndRange, 0, j, i);

                            var free = this.Items[i] as SingleFreeSpace;

                            // simulation that pos is taken
                            if (free is not null && occu.ContainsKey(i))
                            {
                                free = null;
                            }

                            if (freeStartRange is null && free is not null)
                            {
                                freeStartRange = new Range(i, i + 1);
                            }

                            if (lastFree is not null)
                            {
                                Debug.Assert(freeStartRange.HasValue);

                                if (free is not null)
                                {
                                    freeStartRange = new Range(freeStartRange.Value.Start, i + 1);
                                }
                            }

                            if (freeStartRange.HasValue)
                            {
                                var (offset, length) = freeStartRange.Value.GetOffsetAndLength(this.Items.Count);

                                if (length == fileBlock.Length)
                                {
                                    found = true;

                                    for (var l = offset; l < offset + length; l++)
                                    {
                                        occu.Add(l, lastFile);
                                    }

                                    break;
                                }
                            }

                            if (free is null)
                            {
                                freeStartRange = null;
                                lastFree = null;
                            }
                            else
                            {
                                lastFree = free;
                            }
                        }

                        // append to front result
                        /*for (var l = 0; l < fileBlock.Length && found; l++)
                        {
                            front.Add(lastFile);
                            k++;
                        }*/

                        // append to back result
                        for (var l = fileBlock.Offset; l < fileBlock.Offset + fileBlock.Length; l++)
                        {
                            if (!found) // confirmed adding
                            {
                                occu.Add(l, lastFile);

                                // back.Insert(0, lastFile);
                            }
                            else
                            {
                                //back.Insert(0, new SingleFreeSpace());
                            }
                        }
                    }

                    if (file is not null && lastFile.Id != file.Id)
                    {
                        fileEndRange = new Range(j, j + 1);
                    }
                }

                if (file is null)
                {
                    fileEndRange = null;
                    lastFile = null;
                }
                else
                {
                    lastFile = file;
                }
            }

            foreach (var i in Enumerable.Range(0, this.Items.Count).Where(x => !occu.Any(c => c.Key == x)))
            {
                occu.Add(i, new SingleFreeSpace());
            }

            foreach (var (index, value) in occu.OrderBy(x => x.Key))
            {
                yield return value;
            }
        }

        private void LogPos(ILogger l,
            Disk s,
            Dictionary<int, SingleDiskItem> front,
            Range? fileEndRange,
            int k,
            int j)
        {
           /* var prependJ = new string(' ', j);

            var c = new char[s.Items.Count];

            for (var i1 = 0; i1 < c.Length; i1++)
            {
                c[i1] = ' ';
            }

            foreach (var d in front)
            {
                c[d.Key] = d.Value is SingleFile f ? (char) ('0' + f.Id) : '.';
            }

            var result = new string(c).TrimEnd();

            l.LogTrace(result);
            l.LogDebug(s.ToString());

            if (fileEndRange is not null)
            {
                var (o, le) = fileEndRange.Value.GetOffsetAndLength(s.Items.Count);
                l.LogTrace($"{new string(' ', o)}{string.Join("", Enumerable.Range(o, le).Select(x => s.Items[x].ToString()))}");
            }

            l.LogTrace($"{prependJ}^");
            l.LogTrace($"{prependJ}|");
            l.LogTrace($"{prependJ}j");*/
        }

        private void LogPos(ILogger l,
            Disk s,
            Dictionary<int, SingleDiskItem> front,
            Range? fileEndRange,
            int k,
            int j,
            int i)
        {
            /*var c = new char[s.Items.Count];

            for (var i1 = 0; i1 < c.Length; i1++)
            {
                c[i1] = ' ';
            }

            foreach (var d in front)
            {
                c[d.Key] = d.Value is SingleFile f ? (char) ('0' + f.Id) : '.';
            }

            var result = new string(c).TrimEnd();

            l.LogTrace(result);

            l.LogDebug(s.ToString());

            if (fileEndRange is not null)
            {
                var (o, le) = fileEndRange.Value.GetOffsetAndLength(s.Items.Count);
                l.LogTrace($"{new string(' ', o)}{string.Join("", Enumerable.Range(o, le).Select(x => s.Items[x].ToString()))}");
            }

            if (i == j)
            {
                var prependI = new string(' ', i);
                l.LogTrace($"{prependI}^");
                l.LogTrace($"{prependI}|");
                l.LogTrace($"{prependI}i,j");
            }
            else if (i < j)
            {
                var prependI = new string(' ', i);
                var prependJ = new string(' ', j - i - 1);
                l.LogTrace($"{prependI}^{prependJ}^");
                l.LogTrace($"{prependI}|{prependJ}|");
                l.LogTrace($"{prependI}i{prependJ}j");
            }
            else if (j < i)
            {
                var prependI = new string(' ', j);
                var prependJ = new string(' ', i - j - 1);
                l.LogTrace($"{prependI}^{prependJ}^");
                l.LogTrace($"{prependI}|{prependJ}|");
                l.LogTrace($"{prependI}j{prependJ}i");
            }*/
        }

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

        public override string ToString()
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
                    continue;
                }

                result += i * f.Id;
            }

            return result;
        }
    }
}
