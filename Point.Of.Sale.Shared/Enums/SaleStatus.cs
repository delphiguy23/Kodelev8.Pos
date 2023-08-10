namespace Point.Of.Sale.Shared.Enums;

public enum SaleStatus
{
    OrderPlaced = -1,
    Cancelled = 0,
    Processed,
    Served,
    UnPaid,
    Paid,
    Reversed
}
