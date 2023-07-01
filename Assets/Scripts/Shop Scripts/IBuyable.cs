public interface IBuyable
{
    int Buy(int curMoney);
    bool CanBuy(int curMoney);
    int GetPrice();
    bool IsBought();
}
