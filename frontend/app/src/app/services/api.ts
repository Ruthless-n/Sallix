import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private baseUrl: string = 'https://localhost:7064/api';

  constructor(private http: HttpClient) { }

  getEndpointUrl(endpoint: string): string {
    return `${this.baseUrl}/${endpoint}`;
  }

  // Comissões
  getVendas(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Comissao/resumo`);
  }

  // Estoque
  getEstoque(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Estoque/produtos`);
  }

  getMovimentacoes(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Estoque/historico`);
  }

  adicionarMovimentacao(dados: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/estoque/movimentar`, dados);
  }

  // Juros
  calcularJuros(dados: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Juros/calcular-juros`, dados);
  }
}

