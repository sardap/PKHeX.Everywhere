using System.Text.Json;
using PKHeX.Core;
using PKHeX.Facade;
using PKHeX.Facade.Repositories;
using Gender = PKHeX.Facade.Gender;

namespace PKHeX.MonFS;

public static class Decode
{
    public static PC Handle(Game game)
    {
        var pc = new PC();

        // Ribbon decoding is wrong

        foreach(var boxMon in game.Trainer.PokemonBox.All) {
            if (boxMon.Species == Species.None) {
                break;
            }
            var name = Utils.PkStringToMonString(boxMon.Pkm.Nickname);
            var species = Utils.GenderSpeciesMappingDecode(SpeciesName.GetSpeciesName(boxMon.Pkm.Species, 2));
            var gender = boxMon.Gender == Gender.Male ? "Male" : "Female";
            var pcMark = new List<bool>();
            if (boxMon.Pkm is IAppliedMarkings<bool> b)
            {
                for (var j = 0; j < b.MarkingCount; j++)
                {
                    var mark = b.GetMarking(j);
                    pcMark.Add(mark);
                }
            }
            var capturedBall = Utils.ItemIdToString(boxMon.Pkm.Ball);
            if (capturedBall is null) {
                throw new Exception("Captured ball is null");
            }
            var moveSet = new List<string>();
            for (var j = 0; j < boxMon.Pkm.Moves.Length; j++)
            {
                var move = Utils.MoveIdToString(boxMon.Pkm.Moves[j]);
                if (move is null) {
                    throw new Exception("Move is null");
                }
                moveSet.Add(move);
            }
            var heldItem = Utils.ItemIdToString((ushort)boxMon.Pkm.HeldItem);
            if (heldItem is null) {
                throw new Exception("Held item is null");
            }
            var ribbons = Utils.GetRibbonCount(boxMon.Pkm);
            var otTid = boxMon.Pkm.TID16;
            var shiny = boxMon.Pkm.IsShiny;
            var otName = Utils.PkStringToMonString(boxMon.Pkm.OriginalTrainerName);
            var exp = boxMon.Pkm.EXP;
            var metLevel = boxMon.Pkm.MetLevel;
            var virus = boxMon.Pkm.IsPokerusInfected;
            var otGender = boxMon.Pkm.OriginalTrainerGender == 0 ? true : false;

            pc.Mons.Add(new StringsMon
            {
                Name = name,
                Species = species,
                Gender = gender,
                PcMark = pcMark,
                CapturedBall = capturedBall,
                MoveSet = moveSet,
                Item = heldItem,
                Ribbons = ribbons,
                OtTid = otTid,
                Shiny = shiny,
                OtName = otName,
                Exp = exp,
                MetLevel = metLevel,
                Virus = virus,
                OtGender = otGender
            });
        }

        return pc;
    }
}
