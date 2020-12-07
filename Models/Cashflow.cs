using System.Collections.Generic;


public class Cashflow
{
    public int Id { get; set;}
    public long Principal { get; set; }
    public long Rate { get; set; }
    public int Term { get; set; }
    public List<Period> Payments { get; set; }

    public CashFlow(int id, long principal, int term, long rate){
        Id = id;
        Principal = principal;
        Term = term;
        Rate = rate;
        Payments = new List<Period>();
    }
    
}
