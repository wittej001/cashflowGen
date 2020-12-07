using System.Collections.Generic;


public class Cashflow
{
    public long Principal { get; set; }
    public long Rate { get; set; }
    public int Term { get; set; }
    public List<Period> Payments { get; set; }
}