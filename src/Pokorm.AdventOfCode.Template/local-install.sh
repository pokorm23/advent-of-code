#!/bin/bash
ver="1.0.0"
name="Pokorm.AdventOfCode.Template"

dotnet pack -v m -c Release -p:Version=$ver "$name.csproj"
dotnet new uninstall $name
dotnet new install ".\bin\Release\\${name}.${ver}.nupkg"
