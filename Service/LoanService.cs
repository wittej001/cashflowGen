using System.Collections.Generic;



class LoanService
{

    public List<Cashflow> CalculateCashflows(List<LoanItems> TagLoans)
    {
        List<Cashflow> CashflowList = new List<Cashflow>();
        Cashflow aggregateCashflow = new Cashflow();
        int longestLoan = 0;
        foreach( LoanItem loan in TagLoans){
            if(loan.Term > longestLoan){
                longestLoan = loan.Term;
            }
        }
        int i = longestLoan;
        while(i != 0){
            foreach( LoanItem loan in TagLoans){
                var loanRate = loan.Rate;
                
            }
            i -= 1;
        }
        if(TagLoans.Count == 1){
            Cashflow cashflow = new Cashflow()
        }

    }



}