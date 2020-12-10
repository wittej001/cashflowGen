using System.Collections.Generic;
using Math = System.Math;

/**
 * Contains all business logic to deal with loan objects 
 */
class LoanService
{
    /**
     * Constants to represent the loan types
     */
    string LOAN_TYPE = "loan";
    string AGGREGATE_TYPE = "aggregate";
    
    /**
     * Maps loan ID to remaining balance of loan
     *
     * Format: INT -> DOUBLE
     */
    Dictionary<int, double> remainingBalanceById = new Dictionary<int, double>();
    
    /**
     * Maps loan ID to the base monthly payment for the loan
     *
     * Format: INT -> DOUBLE
     */
    Dictionary<int, double> monthlyPaymentById = new Dictionary<int, double>();
    
    /**
     * Maps loan ID to cashflow object for the loan
     *
     * Format: INT -> CASHFLOW
     */
    Dictionary<int, Cashflow> cashflowById = new Dictionary<int, Cashflow>();

    /**
     * Params: TagLoans - List of LoanItems
     * 
     * Given a list of loans, calculates a cashflow for each, as well
     * as an aggregate cashflow for the group of loans
     */
    public List<Cashflow> CalculateCashflows(List<LoanItem> TagLoans)
    {
        int longestLoan = FindLongestLoan(TagLoans);

        Cashflow aggregateCashflow = new Cashflow(0, 0, 0, 0, AGGREGATE_TYPE);

        // i represents each month
        int i = 0;
        while (i < longestLoan) {
            // Reset aggregate for each loop/month of the loans
            double aggregateInterest = 0;
            double aggregatePrincipal = 0;
            double aggregateRemBalance = 0;

            foreach( LoanItem loan in TagLoans) {
                
                // Only want to do cashflow calculations if loan is not paid off
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
                    
                    loanCashflow.Payments.Add(new Period(i + 1, 
                        Math.Round(interestPayment, 2), Math.Round(principalPayment, 2), 
                        Math.Round(newRemBalance, 2)));

                    remainingBalanceById[loan.Id] = newRemBalance;
                }

            };

            aggregateCashflow.Payments.Add(new Period(i + 1, 
                Math.Round(aggregateInterest, 2), Math.Round(aggregatePrincipal, 2), 
                Math.Round(aggregateRemBalance, 2)));
            
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

    /**
     * Params: TagLoans - List of LoanItems
     * 
     * Loops through all loans one time to initialize information dicts
     * that will be used to calculate cashflows
     */
    public int FindLongestLoan(List<LoanItem> TagLoans){
        int longestLoan = 0;
        foreach (LoanItem loan in TagLoans) {
            remainingBalanceById.Add(loan.Id, loan.Principal);
            monthlyPaymentById.Add(loan.Id, loan.Principal * (loan.Rate/1200) / (1 - Math.Pow((1 + loan.Rate/1200), (-1 * loan.Term))));
            cashflowById.Add(loan.Id, new Cashflow(loan.Id, loan.Principal, loan.Term, loan.Rate, LOAN_TYPE));
            
            // piggy back off of initializing the dictionaries to
            // also find the longest loan, which will be the length of the 
            // aggregate
            if (loan.Term > longestLoan) {
                longestLoan = loan.Term;
            };
        };
        return longestLoan;
    }
}