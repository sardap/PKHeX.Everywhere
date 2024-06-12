namespace PKHeX.Facade;

public class Money
{
    private readonly Game _game;

    public Money(Game game)
    {
        _game = game;
    }

    public decimal Amount => _game.SaveFile.Money;

    public void Set(uint amount)
    {
        _game.SaveFile.Money = (uint)Math.Min(amount, _game.SaveFile.MaxMoney);
    }

    public void SetMax()
    {
        _game.SaveFile.Money = Convert.ToUInt32(_game.SaveFile.MaxMoney);
    }

    public override string ToString() => Amount.ToString("C");
}