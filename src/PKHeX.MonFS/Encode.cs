using PKHeX.Core;
using PKHeX.Facade;

namespace PKHeX.MonFS;

public static class Encode
{
    
    public static byte[] Handle(Game game, PC pc)
    {
        if (pc is null)
            throw new Exception("PC is null!");

        for (int i = 0; i < pc.Mons.Count; i++)
        {
            var mon = pc.Mons[i];
            if (mon is null)
                continue;
            var boxMon = game.Trainer.PokemonBox.All[i];
            boxMon.Pkm.Nickname = Utils.MonStringToPkString(mon.Name);
            boxMon.Pkm.Species = Utils.GenderSpeciesMappingEncode(mon.Species, mon.Gender);

            var ball = Utils.StringToItemId(mon.CapturedBall);
            if (ball is null)
            {
                throw new Exception($"Ball {mon.CapturedBall} not found!");
            }
            boxMon.Pkm.Ball = (byte)ball;

            for (var j = 0; j < mon.PcMark.Count; j++)
            {
                if (boxMon.Pkm is IAppliedMarkings<bool> b)
                {
                    b.SetMarking(j, mon.PcMark[j]);
                }
            }

            boxMon.Pkm.EXP = mon.Exp;
            RibbonApplicator.RemoveAllValidRibbons(boxMon.Pkm);
            Utils.SetRibbonsToN(boxMon.Pkm, mon.Ribbons);

            var otName = mon.OtName;
            boxMon.Pkm.OriginalTrainerName = Utils.MonStringToPkString(otName);
            boxMon.Pkm.OriginalTrainerGender = (byte)(mon.OtGender ? 0 : 1);
            boxMon.Pkm.MetLevel = mon.MetLevel;
            if (mon.Virus)
            {
                boxMon.Pkm.PokerusDays = 4;
                boxMon.Pkm.PokerusStrain = 0b1111111111;
            }
            else
            {
                boxMon.Pkm.PokerusDays = 0;
                boxMon.Pkm.PokerusStrain = 0;
            }

            for (var j = 0; j < mon.MoveSet.Count; j++)
            {
                var move = Utils.StringToMoveId(mon.MoveSet[j]);
                boxMon.Pkm.SetMove(j, move);
            }


            var heldItem = Utils.StringToItemId(mon.Item);
            if (heldItem is null)
            {
                throw new Exception($"Item {mon.Item} not found!");
            }
            boxMon.Pkm.HeldItem = (int)heldItem;

            boxMon.Pkm.TID16 = mon.OtTid;

            var genderByte = (byte)(mon.Gender == "Male" ? 0 : 1);
            while (boxMon.Pkm.Gender != genderByte || (mon.Shiny ? !boxMon.Pkm.IsShiny : boxMon.Pkm.IsShiny))  {
                boxMon.Pkm.SetIsShiny(mon.Shiny);
                boxMon.Pkm.SetGender(genderByte);
            }
        }

        for (int i = pc.Mons.Count; i < game.Trainer.PokemonBox.All.Count; i++)
        {
            var boxMon = game.Trainer.PokemonBox.All[i];
            boxMon.Pkm.Species = 0;
        }

        return game.ToByteArray();
    }
}
