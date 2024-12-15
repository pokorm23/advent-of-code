﻿using System.Text;

namespace Pokorm.AdventOfCode.Helpers;

public record Grid<T>(Dictionary<Coord, T> Values, long Width, long Height) : Grid(Width, Height)
{
    public Func<T, char> ValueCharFactory { get; set; }

    public override Grid<T> GetSubGrid(Coord coord, long gridWidth, long gridHeight)
    {
        var baseGrid = base.GetSubGrid(coord, gridWidth, gridHeight);

        var newValues = new Dictionary<Coord, T>();

        foreach (var (c, value) in this.Values)
        {
            var nc = new Coord(c.X - coord.X, c.Y - coord.Y);

            if (!baseGrid.IsIn(nc))
            {
                continue;
            }

            newValues.Add(nc, value);
        }

        return this with
        {
            Values = newValues,
            Width = baseGrid.Width,
            Height = baseGrid.Height
        };
    }

    public IEnumerable<string> GetLines()
    {
        for (var y = 0L; y < this.Height; y++)
        {
            var str = new StringBuilder();

            for (var x = 0L; x < this.Width; x++)
            {
                var val = this.Values[new Coord(x, y)];

                if (this.ValueCharFactory is not null)
                {
                    str.Append(this.ValueCharFactory(val));

                    continue;
                }

                if (val is Direction d)
                {
                    str.Append(d.ToChar());

                    continue;
                }

                var valueStr = this.Values[new Coord(x, y)]?.ToString() ?? "";

                var c = valueStr.Length switch
                {
                    0     => '?',
                    var _ => valueStr[0]
                };

                str.Append(c);
            }

            yield return str.ToString();
        }
    }

    public override string ToString() => $"Grid {this.Width}x{this.Height}{Environment.NewLine}{Environment.NewLine}{string.Join(Environment.NewLine, GetLines())}";

    public Grid<T> Copy(Func<T, T>? copy = null)
    {
        if (copy is null)
        {
            return this with
            {
                Values = this.Values.ToDictionary()
            };
        }

        return this with
        {
            Values = this.Values.ToDictionary(x => x.Key, x => copy(x.Value))
        };
    }
}

public record Grid(long Width, long Height)
{
    public bool IsIn(Coord source) => source.Y >= 0 && source.X >= 0 && source.Y < this.Height && source.X < this.Width;

    public Coord? TryGetCoordInDirection(Coord source, Vector vector)
    {
        if (!IsIn(source))
        {
            throw new ArgumentOutOfRangeException(nameof(source), $"{source} range out in {this}");
        }

        var newCoord = new Coord(source.X + vector.X, source.Y + vector.Y);

        if (IsIn(newCoord))
        {
            return newCoord;
        }

        return null;
    }

    public IEnumerable<Coord> GetSiblings(Coord coord, params IEnumerable<Vector> vectors)
    {
        return vectors.Select(v => TryGetCoordInDirection(coord, v))
                      .OfType<Coord>();
    }

    public virtual Grid GetSubGrid(Coord coord, long gridWidth, long gridHeight)
    {
        if (!IsIn(coord))
        {
            throw new ArgumentOutOfRangeException(nameof(coord), $"{coord} range out in {this}");
        }

        return new Grid(gridWidth - coord.X, gridHeight - coord.Y);
    }

    public override string ToString() => $"{this.Width}x{this.Height}";
}
