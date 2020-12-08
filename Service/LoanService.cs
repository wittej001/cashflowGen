using System.Collections.Generic;
using Math = System.Math;

class LoanService
{
    string LOAN_TYPE = "loan";
    string AGGREGATE_TYPE = "aggregate";

    public List<Cashflow> CalculateCashflows(List<LoanItem> TagLoans)
    {
        Dictionary<int, double> remainingBalanceById = new Dictionary<int, double>();
        Dictionary<int, double> monthlyPaymentById = new Dictionary<int, double>();
        Dictionary<int, Cashflow> cashflowById = new Dictionary<int, Cashflow>();
        
        int longestLoan = 0;
        foreach (LoanItem loan in TagLoans) {
            remainingBalanceById.Add(loan.Id, loan.Principal);
            monthlyPaymentById.Add(loan.Id, loan.Principal * (loan.Rate/1200) / (1 - Math.Pow((1 + loan.Rate/1200), (-1 * loan.Term))));
            cashflowById.Add(loan.Id, new Cashflow(loan.Id, loan.Principal, loan.Term, loan.Rate, LOAN_TYPE));

            if (loan.Term > longestLoan) {
                longestLoan = loan.Term;
            };
        };

        Cashflow aggregateCashflow = new Cashflow(0, 0, 0, 0, AGGREGATE_TYPE);

        int i = 0;
        while (i < longestLoan) {
            double aggregateInterest = 0;
            double aggregatePrincipal = 0;
            double aggregateRemBalance = 0;

            foreach( LoanItem loan in TagLoans) {

                if (i < loan.Term) {
                    var loanRate = loan.Rate;
                    var loanRemBalance = remainingBalanceById[loan.Id];
                    var loanMonthlyPayment = monthlyPaymentById[loan.Id];
                    var loanCashflow = cashflowById[loan.Id];

                    var interestPayment = loanRemBalance * (loan.Rate / 1200);
                    var principalPayment = loanMonthlyPayment - interestPayment;
                    var newRemBalance = loanRemBalance - principalPayment;

                    aggregateInterest += interestPayment;
                    aggregatePrincipal += principalPayment;
                    aggregateRemBalance += newRemBalance;

                    loanCashflow.Payments.Add(new Period(i + 1, interestPayment, principalPayment, newRemBalance));

                    remainingBalanceById[loan.Id] = newRemBalance;
                }

            };

            aggregateCashflow.Payments.Add(new Period(i + 1, aggregateInterest, aggregatePrincipal, aggregateRemBalance));
            
            i += 1;
        };

        List<Cashflow> cashflowList = new List<Cashflow>();
        foreach (Cashflow cashflow in cashflowById.Values) {
            cashflowList.Add(cashflow);
        };

        // Only add aggregate if there are multiple cashflows
        if (TagLoans.Count != 1) {
            cashflowList.Add(aggregateCashflow);
        };

        return cashflowList;

    }
}