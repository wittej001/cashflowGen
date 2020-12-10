import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { Loan } from './Loan';


@Injectable({
  providedIn: 'root'
})
export class LoanService {

  private url = 'https://localhost:5001/api/LoanItems';  // URL to web api
  private cashflowUrl = 'https://localhost:5001/api/LoanItems/AllCashFlows';
  private specCashflowUrl = 'https://localhost:5001/api/LoanItems/Cashflows';
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }
  
  /** GET loans from the server */
  getAllData(ids: any = null): Observable<Loan[]> {
    let url = ids ?  this.specCashflowUrl : this.cashflowUrl;
    let params;
    if (url === this.specCashflowUrl) {
      params = new HttpParams().append('_ids', ids)
    } else {
      params = new HttpParams()
    }

    return this.http.get<Loan[]>(url, {params: params})
      .pipe(
        tap(_ => this.log('fetched loans')),
        catchError(this.handleError<Loan[]>('getLoans', []))
      );
  }

  /** POST: add a new loan to the server */
  addLoan(loan: Loan): Observable<Loan> {
    return this.http.post<Loan>(this.url, loan, this.httpOptions).pipe(
      tap((newLoan: Loan) => this.log(`added loan w/ id=${newLoan.id}`)),
      catchError(this.handleError<Loan>('addLoan'))
    );
  }


  /** Log a loanService message with the MessageService */
  private log(message: string) {
    console.log(message);
  }

  deleteLoan(loan: Loan | number): Observable<Loan>{
    const id = typeof loan === 'number' ? loan : loan.id;
    const idUrl = `${this.url}/${id}`;

    return this.http.delete<Loan>(idUrl, this.httpOptions).pipe(
      tap(_ => this.log(`deleted loan id=${id}`)),
      catchError(this.handleError<Loan>('deleteLoan'))
    );
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
