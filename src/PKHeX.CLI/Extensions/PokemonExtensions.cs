﻿using PKHeX.Facade;
using PKHeX.Facade.Extensions;

namespace PKHeX.CLI.Extensions;

public static class PokemonExtensions
{
    public static string GetPokemonDisplay(this Pokemon pokemon)
    {
        var gender = $"[{pokemon.Gender.Color()} bold]{pokemon.Gender.Symbol}[/]";
        
        return $"{pokemon.NameDisplay()} {gender} Lv. [yellow]{pokemon.Level}[/]{pokemon.ShinyDisplay()}";
    }

    public static string ShinyDisplay(this Pokemon pokemon) => pokemon.IsShiny ? " ✨" : string.Empty;

    private static string Color(this Gender gender)
    {
        if (gender == Gender.Male) return "dodgerblue2";
        if (gender == Gender.Female) return "violet";
        if (gender == Gender.Genderless) return "yellow3";
        
        return string.Empty;
    }
}
