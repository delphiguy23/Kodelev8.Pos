namespace Point.Of.Sale.Shared.Enums;

public enum OutBoxEventStatus
{
    Cancelled = -1,
    ReadyForProcessing,
    CurrentlyProcessing,
    CompletedSuccessful,
    CompletedWithFailure,
}
