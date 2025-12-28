namespace MyMedia.Domain.Entities.enums;

public enum OrderStatus
{
    InCart, 
    Cancelled,
    PendingPayment,
    PaymentFailed,
    PaymentAccepted,
    Shipped,
    Completed
}