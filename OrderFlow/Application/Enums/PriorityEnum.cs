namespace Application.Enums
{
    public enum PriorityEnum
    {
        Critical = 1,
        High = 2,
        Normal = 3
    }

    public enum OrderEnum
    {
        Pending = 1,
        Completed = 2,
        Canceled = 3
    }

    public enum PaymentStausEnum
    {
        Pending = 1,
        Recieved = 2,
        Canceled = 3
    }

    public enum PaymentOptionEnum
    {
        Stripe = 1,
        Cash = 2,
    }

}
