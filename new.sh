#!/bin/bash
current_year=$(date +%Y)
year=${2:-$current_year}
num=$(printf "%02d" "$1")

dotnet new aoc -q $num -y $year

PUZZLE_URL="https://adventofcode.com/$year/day/$1/input"
PUZZLE_FILE="src/Pokorm.AdventOfCode/Y$year/Inputs/$num.txt"

curl "${PUZZLE_URL}" -H "cookie: session=${AOC_SESSION_COOKIE}" -o "${PUZZLE_FILE}"
