public class Period
{
    public int Month { get; set; }
    public double InterestPayment { get; set; }
    public double PrincipalPayment { get; set; }
    public double RemainingBalance { get; set; }

    public Period(int month, double interestPayment, double principalPayment, double remainingBalance)
    {
        Month = month;
        InterestPayment = interestPayment;
        PrincipalPayment = principalPayment;
        RemainingBalance = remainingBalance;
    }
}