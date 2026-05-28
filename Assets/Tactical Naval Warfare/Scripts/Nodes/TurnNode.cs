public class TurnNode
{
    public ShipController ship;

    public TurnNode next;

    public TurnNode previous;

    public TurnNode(ShipController newShip)
    {
        ship = newShip;
    }
}