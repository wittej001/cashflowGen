using System.Collections.Generic;


public class Cashflow
{
    public int Id { get; set;}
    public double Principal { get; set; }
    public double Rate { get; set; }
    public int Term { get; set; }
    public List<Period> Payments { get; set; }
    public string Type { get; set;}

    public Cashflow(int id, double principal, int term, double rate, string type){
        Id = id;
        Principal = principal;
        Term = term;
        Rate = rate;
        Payments = new List<Period>();
        Type = type;
    } 
    
}
