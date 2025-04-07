using PKHeX.Core;
using PKHeX.Facade.Repositories;

namespace PKHeX.MonFS;

public class Utils
{
    public static string SplitItemName(string name)
    {
        var item = ItemRepository.GetItemByName(name);
        if (item is null)
        {
            throw new Exception($"Item '{name}' not found in the dictionary.");
        }
        return item.Name;
    }

    // This language is a fucking joke
    public static void SafeAdd(ref byte count, sbyte delta)
    {
        try
        {
            count = checked((byte)(count + delta));
        }
        catch (OverflowException)
        {
            if (delta > 0)
            {
                count = byte.MaxValue;
            }
            else
            {
                count = 0;
            }
        }
    }

    public static void SetRibbonsToN(PKM pk, byte count)
    {
        if (pk is IRibbonSetOnly3 o3)
        {
            o3.RibbonWorld = count > 0;
            SafeAdd(ref count, -1);
            if (count < 0)
                return;

            var coolCount = count > 4 ? 4 : count;
            o3.RibbonCountG3Cool = (byte)coolCount;
            SafeAdd(ref count, (sbyte)-coolCount);

            var beautyCount = count > 4 ? 4 : count;
            o3.RibbonCountG3Beauty = (byte)beautyCount;
            SafeAdd(ref count, (sbyte)-beautyCount);

            var cuteCount = count > 4 ? 4 : count;
            o3.RibbonCountG3Cute = (byte)cuteCount;
            SafeAdd(ref count, (sbyte)-cuteCount);

            var smartCount = count > 4 ? 4 : count;
            o3.RibbonCountG3Smart = (byte)smartCount;
            SafeAdd(ref count, (sbyte)-smartCount);
        }
    }

    public static byte GetRibbonCount(PKM pk)
    {
        byte result = 0;
        if (pk is IRibbonSetOnly3 o3)
        {
            if (o3.RibbonWorld)
                result++;

            result += o3.RibbonCountG3Cool;
            result += o3.RibbonCountG3Beauty;
            result += o3.RibbonCountG3Cute;
            result += o3.RibbonCountG3Smart;
        }

        return result;
    }

    public static string MonStringToPkString(string name)
    {
        return name.Replace("…", "⑬");
    }

    public static string PkStringToMonString(string name)
    {
        return name.Replace("⑬", "…");
    }


    private static Dictionary<string, ushort> StringToItemIdMapping = new();
    private static Dictionary<ushort, string> ItemIdToStringMapping = new();

    private static void InitItemMappings()
    {
        if (StringToItemIdMapping.Count == 0)
        {
            var extras = new List<Tuple<string, int>>(){
                Tuple.Create("MasterBall", 1),
                Tuple.Create("UltraBall", 2),
                Tuple.Create("GreatBall", 3),
                Tuple.Create("PokeBall", 4),
                Tuple.Create("SafariBall", 5),
                Tuple.Create("NetBall", 6),
                Tuple.Create("DiveBall", 7),
                Tuple.Create("NestBall", 8),
                Tuple.Create("RepeatBall", 9),
                Tuple.Create("TimerBall", 10),
                Tuple.Create("LuxuryBall", 11),
                Tuple.Create("PremierBall", 12),
                Tuple.Create("Potion", 13),
                Tuple.Create("Antidote", 14),
                Tuple.Create("BurnHeal", 15),
                Tuple.Create("IceHeal", 16),
                Tuple.Create("Awakening", 17),
                Tuple.Create("ParalyzeHeal", 18),
                Tuple.Create("FullRestore", 19),
                Tuple.Create("MaxPotion", 20),
                Tuple.Create("HyperPotion", 21),
                Tuple.Create("SuperPotion", 22),
                Tuple.Create("FullHeal", 23),
                Tuple.Create("Revive", 24),
                Tuple.Create("MaxRevive", 25),
                Tuple.Create("FreshWater", 26),
                Tuple.Create("SodaPop", 27),
                Tuple.Create("Lemonade", 28),
                Tuple.Create("MoomooMilk", 29),
                Tuple.Create("EnergyPowder", 30),
                Tuple.Create("EnergyRoot", 31),
                Tuple.Create("HealPowder", 32),
                Tuple.Create("RevivalHerb", 33),
                Tuple.Create("Ether", 34),
                Tuple.Create("MaxEther", 35),
                Tuple.Create("Elixir", 36),
                Tuple.Create("MaxElixir", 37),
                Tuple.Create("LavaCookie", 38),
                Tuple.Create("BlueFlute", 39),
                Tuple.Create("YellowFlute", 40),
                Tuple.Create("RedFlute", 41),
                Tuple.Create("BlackFlute", 42),
                Tuple.Create("WhiteFlute", 43),
                Tuple.Create("BerryJuice", 44),
                Tuple.Create("SacredAsh", 45),
                Tuple.Create("ShoalSalt", 46),
                Tuple.Create("ShoalShell", 47),
                Tuple.Create("RedShard", 48),
                Tuple.Create("BlueShard", 49),
                Tuple.Create("YellowShard", 50),
                Tuple.Create("GreenShard", 51),
                Tuple.Create("HpUp", 63),
                Tuple.Create("Protein", 64),
                Tuple.Create("Iron", 65),
                Tuple.Create("Carbos", 66),
                Tuple.Create("Calcium", 67),
                Tuple.Create("RareCandy", 68),
                Tuple.Create("PpUp", 69),
                Tuple.Create("Zinc", 70),
                Tuple.Create("PpMax", 71),
                Tuple.Create("GuardSpec", 73),
                Tuple.Create("DireHit", 74),
                Tuple.Create("XAttack", 75),
                Tuple.Create("XDefend", 76),
                Tuple.Create("XSpeed", 77),
                Tuple.Create("XAccuracy", 78),
                Tuple.Create("XSpecial", 79),
                Tuple.Create("PokeDoll", 80),
                Tuple.Create("FluffyTail", 81),
                Tuple.Create("SuperRepel", 83),
                Tuple.Create("MaxRepel", 84),
                Tuple.Create("EscapeRope", 85),
                Tuple.Create("Repel", 86),
                Tuple.Create("SunStone", 93),
                Tuple.Create("MoonStone", 94),
                Tuple.Create("FireStone", 95),
                Tuple.Create("ThunderStone", 96),
                Tuple.Create("WaterStone", 97),
                Tuple.Create("LeafStone", 98),
                Tuple.Create("TinyMushroom", 103),
                Tuple.Create("BigMushroom", 104),
                Tuple.Create("Pearl", 106),
                Tuple.Create("BigPearl", 107),
                Tuple.Create("Stardust", 108),
                Tuple.Create("StarPiece", 109),
                Tuple.Create("Nugget", 110),
                Tuple.Create("HeartScale", 111),
                Tuple.Create("OrangeMail", 121),
                Tuple.Create("HarborMail", 122),
                Tuple.Create("GlitterMail", 123),
                Tuple.Create("MechMail", 124),
                Tuple.Create("WoodMail", 125),
                Tuple.Create("WaveMail", 126),
                Tuple.Create("BeadMail", 127),
                Tuple.Create("ShadowMail", 128),
                Tuple.Create("TropicMail", 129),
                Tuple.Create("DreamMail", 130),
                Tuple.Create("FabMail", 131),
                Tuple.Create("RetroMail", 132),
                Tuple.Create("CheriBerry", 133),
                Tuple.Create("ChestoBerry", 134),
                Tuple.Create("PechaBerry", 135),
                Tuple.Create("RawstBerry", 136),
                Tuple.Create("AspearBerry", 137),
                Tuple.Create("LeppaBerry", 138),
                Tuple.Create("OranBerry", 139),
                Tuple.Create("PersimBerry", 140),
                Tuple.Create("LumBerry", 141),
                Tuple.Create("SitrusBerry", 142),
                Tuple.Create("FigyBerry", 143),
                Tuple.Create("WikiBerry", 144),
                Tuple.Create("MagoBerry", 145),
                Tuple.Create("AguavBerry", 146),
                Tuple.Create("IapapaBerry", 147),
                Tuple.Create("RazzBerry", 148),
                Tuple.Create("BlukBerry", 149),
                Tuple.Create("NanabBerry", 150),
                Tuple.Create("WepearBerry", 151),
                Tuple.Create("PinapBerry", 152),
                Tuple.Create("PomegBerry", 153),
                Tuple.Create("KelpsyBerry", 154),
                Tuple.Create("QualotBerry", 155),
                Tuple.Create("HondewBerry", 156),
                Tuple.Create("GrepaBerry", 157),
                Tuple.Create("TamatoBerry", 158),
                Tuple.Create("CornnBerry", 159),
                Tuple.Create("MagostBerry", 160),
                Tuple.Create("RabutaBerry", 161),
                Tuple.Create("NomelBerry", 162),
                Tuple.Create("SpelonBerry", 163),
                Tuple.Create("PamtreBerry", 164),
                Tuple.Create("WatmelBerry", 165),
                Tuple.Create("DurinBerry", 166),
                Tuple.Create("BelueBerry", 167),
                Tuple.Create("LiechiBerry", 168),
                Tuple.Create("GanlonBerry", 169),
                Tuple.Create("SalacBerry", 170),
                Tuple.Create("PetayaBerry", 171),
                Tuple.Create("ApicotBerry", 172),
                Tuple.Create("LansatBerry", 173),
                Tuple.Create("StarfBerry", 174),
                Tuple.Create("EnigmaBerry", 175),
                Tuple.Create("BrightPowder", 179),
                Tuple.Create("WhiteHerb", 180),
                Tuple.Create("MachoBrace", 181),
                Tuple.Create("ExpShare", 182),
                Tuple.Create("QuickClaw", 183),
                Tuple.Create("SootheBell", 184),
                Tuple.Create("MentalHerb", 185),
                Tuple.Create("ChoiceBand", 186),
                Tuple.Create("KingsRock", 187),
                Tuple.Create("SilverPowder", 188),
                Tuple.Create("AmuletCoin", 189),
                Tuple.Create("CleanseTag", 190),
                Tuple.Create("SoulDew", 191),
                Tuple.Create("DeepSeaTooth", 192),
                Tuple.Create("DeepSeaScale", 193),
                Tuple.Create("SmokeBall", 194),
                Tuple.Create("Everstone", 195),
                Tuple.Create("FocusBand", 196),
                Tuple.Create("LuckyEgg", 197),
                Tuple.Create("ScopeLens", 198),
                Tuple.Create("MetalCoat", 199),
                Tuple.Create("Leftovers", 200),
                Tuple.Create("DragonScale", 201),
                Tuple.Create("LightBall", 202),
                Tuple.Create("SoftSand", 203),
                Tuple.Create("HardStone", 204),
                Tuple.Create("MiracleSeed", 205),
                Tuple.Create("BlackGlasses", 206),
                Tuple.Create("BlackBelt", 207),
                Tuple.Create("Magnet", 208),
                Tuple.Create("MysticWater", 209),
                Tuple.Create("SharpBeak", 210),
                Tuple.Create("PoisonBarb", 211),
                Tuple.Create("NeverMeltIce", 212),
                Tuple.Create("SpellTag", 213),
                Tuple.Create("TwistedSpoon", 214),
                Tuple.Create("Charcoal", 215),
                Tuple.Create("DragonFang", 216),
                Tuple.Create("SilkScarf", 217),
                Tuple.Create("UpGrade", 218),
                Tuple.Create("ShellBell", 219),
                Tuple.Create("SeaIncense", 220),
                Tuple.Create("LaxIncense", 221),
                Tuple.Create("LuckyPunch", 222),
                Tuple.Create("MetalPowder", 223),
                Tuple.Create("ThickClub", 224),
                Tuple.Create("Stick", 225),
                Tuple.Create("RedScarf", 254),
                Tuple.Create("BlueScarf", 255),
                Tuple.Create("PinkScarf", 256),
                Tuple.Create("GreenScarf", 257),
                Tuple.Create("YellowScarf", 258),
                Tuple.Create("MachBike", 259),
                Tuple.Create("CoinCase", 260),
                Tuple.Create("Itemfinder", 261),
                Tuple.Create("OldRod", 262),
                Tuple.Create("GoodRod", 263),
                Tuple.Create("SuperRod", 264),
                Tuple.Create("SsTicket", 265),
                Tuple.Create("ContestPass", 266),
                Tuple.Create("WailmerPail", 268),
                Tuple.Create("DevonGoods", 269),
                Tuple.Create("SootSack", 270),
                Tuple.Create("BasementKey", 271),
                Tuple.Create("AcroBike", 272),
                Tuple.Create("PokeblockCase", 273),
                Tuple.Create("Letter", 274),
                Tuple.Create("EonTicket", 275),
                Tuple.Create("RedOrb", 276),
                Tuple.Create("BlueOrb", 277),
                Tuple.Create("Scanner", 278),
                Tuple.Create("GoGoggles", 279),
                Tuple.Create("Meteorite", 280),
                Tuple.Create("Rm1Key", 281),
                Tuple.Create("Rm2Key", 282),
                Tuple.Create("Rm4Key", 283),
                Tuple.Create("Rm6Key", 284),
                Tuple.Create("StorageKey", 285),
                Tuple.Create("RootFossil", 286),
                Tuple.Create("ClawFossil", 287),
                Tuple.Create("DevonScope", 288),
                Tuple.Create("OaksParcel", 349),
                Tuple.Create("PokeFlute", 350),
                Tuple.Create("SecretKey", 351),
                Tuple.Create("BikeVoucher", 352),
                Tuple.Create("GoldTeeth", 353),
                Tuple.Create("OldAmber", 354),
                Tuple.Create("CardKey", 355),
                Tuple.Create("LiftKey", 356),
                Tuple.Create("HelixFossil", 357),
                Tuple.Create("DomeFossil", 358),
                Tuple.Create("SilphScope", 359),
                Tuple.Create("Bicycle", 360),
                Tuple.Create("TownMap", 361),
                Tuple.Create("VSSeeker", 362),
                Tuple.Create("FameChecker", 363),
                Tuple.Create("TMCase", 364),
                Tuple.Create("BerryPouch", 365),
                Tuple.Create("TeachyTV", 366),
                Tuple.Create("TriPass", 367),
                Tuple.Create("RainbowPass", 368),
                Tuple.Create("Tea", 369),
                Tuple.Create("MysticTicket", 370),
                Tuple.Create("AuroraTicket", 371),
                Tuple.Create("PowderJar", 372),
                Tuple.Create("Ruby", 373),
                Tuple.Create("Sapphire", 374),
                Tuple.Create("MagmaEmblem", 375),
                Tuple.Create("OldSeaMap", 376)
            };

            foreach (var extra in extras)
            {
                StringToItemIdMapping[extra.Item1] = (ushort)extra.Item2;
                ItemIdToStringMapping[(ushort)extra.Item2] = extra.Item1;
            }
        }

    }


    public static ushort? StringToItemId(string itemName)
    {
        InitItemMappings();

        // Check if the item is a TM
        if (itemName.StartsWith("TM", StringComparison.OrdinalIgnoreCase))
        {
            // Extract the TM number from the item name
            string tmNumber = itemName.Substring(2, 2).Trim();
            if (ushort.TryParse(tmNumber, out ushort tmId))
            {
                return (ushort)(tmId + 288);
            }
        }

        if (itemName.StartsWith("HM", StringComparison.OrdinalIgnoreCase))
        {
            // Extract the HM number from the item name
            string hmNumber = itemName.Substring(2, 2).Trim();
            if (ushort.TryParse(hmNumber, out ushort hmId))
            {
                return (ushort)(hmId + 339);
            }
        }

        return StringToItemIdMapping.TryGetValue(itemName, out ushort itemId) ? itemId : null;
    }

    public static string? ItemIdToString(ushort itemId)
    {
        InitItemMappings();

        // Check if the item is a TM
        if (itemId >= 339)
        {
            return "Hm" + (itemId - 339).ToString("D2");
        }

        if (itemId >= 288)
        {
            return "Tm" + (itemId - 288).ToString("D2");
        }


        return ItemIdToStringMapping.TryGetValue(itemId, out string? itemName) ? itemName : null;
    }


    private static Dictionary<string, ushort> StringToMoveMapping = new();
    private static Dictionary<ushort, string> MoveToStringMapping = new();

    private static void InitMoveMappings()
    {
        if (StringToMoveMapping.Count == 0)
        {
            StringToMoveMapping = GameInfo.Strings.movelist
            .Select((moveName, id) => (id: Convert.ToUInt16(id), moveName))
            .ToDictionary(x => x.moveName.Replace(" ", "").Replace("-", ""), x => Convert.ToUInt16(x.id));
            StringToMoveMapping.Add("ViceGrip", 11);
            StringToMoveMapping.Add("FaintAttack", 185);
            StringToMoveMapping.Add("HiJumpKick", 136);

            foreach (var mapping in StringToMoveMapping)
            {
                MoveToStringMapping[mapping.Value] = mapping.Key;
            }
        }

    }


    public static ushort StringToMoveId(string moveName)
    {
        InitMoveMappings();

        if (StringToMoveMapping.TryGetValue(moveName, out ushort moveId))
        {
            return moveId;
        }

        throw new ArgumentException($"Move '{moveName}' not found in the dictionary.");

    }


    public static string MoveIdToString(ushort moveId)
    {
        InitMoveMappings();

        if (MoveToStringMapping.TryGetValue(moveId, out string? moveName))
        {
            return moveName;
        }

        throw new ArgumentException($"Move '{moveId}' not found in the dictionary.");

    }

    public static ushort GenderSpeciesMappingEncode(string species, string gender)
    {
        if (gender == "Female")
        {
            switch (species)
            {
                case "Volbeat":
                    // Illumise
                    return 314;
                case "Nidorino":
                    // Nidorina
                    return 30;
                case "Latios":
                    // Latias
                    return 380;
                case "Nidoking":
                    // Nidoqueen
                    return 31;
                case "Tyrogue":
                    // Chansey
                    return 113;
                case "Hitmonlee":
                    // Blissey
                    return 242;
                case "Tauros":
                    // Miltank
                    return 241;
            }
        }

        if (!SpeciesName.TryGetSpecies(species, 2, out ushort result))
        {
            throw new Exception($"Species {species} not found!");
        }

        return result;
    }

    public static string GenderSpeciesMappingDecode(string species)
    {
        switch (species)
        {
            case "Illumise":
                return "Volbeat";
            case "Nidorina":
                return "Nidorino";
            case "Latias":
                return "Latios";
            case "Nidoqueen":
                return "Nidoking";
            case "Chansey":
                return "Tyrogue";
            case "Blissey":
                return "Hitmonlee";
            case "Miltank":
                return "Tauros";
        }

        return species;
    }

}
