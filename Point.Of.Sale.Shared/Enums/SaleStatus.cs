namespace Point.Of.Sale.Shared.Enums;

public enum SaleStatus
{
    Cancelled = -1,
    OrderPlaced = 0,
    Processed,
    Served,
    UnPaid,
    Paid,
    Reversed
}
