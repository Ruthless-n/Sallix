import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from './services/api';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('SALLIX');
  activeTab: string = 'comissoes';
  
  //Comissões
  vendas: any[] = [];
  comissoesPorVendedor: any[] = [];
  
  //Estoque
  estoque: any[] = [];
  movimentacoes: any[] = [];
  novaMovimentacao = {
    codigoProduto: '',
    tipo: '',
    quantidade: 0,
    descricao: ''
  };
  
  //Juros
  simulacaoJuros = {
    valor: 0,
    dataVencimento: ''
  };
  resultadoJuros: any = null;

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.carregarDados();
  }

  carregarDados() {
    this.apiService.getVendas().subscribe(
      (data: any) => {
        this.vendas = data || [];
        this.calcularComissoesPorVendedor();
      },
      (error) => console.error('Erro ao carregar vendas:', error)
    );

    this.apiService.getEstoque().subscribe(
      (data: any) => {
        this.estoque = data.map((e: any) => ({
          ...e,
          estoqueAtual: e.estoque
        })) || [];
      },
      (error) => console.error('Erro ao carregar estoque:', error)
    );

    this.apiService.getMovimentacoes().subscribe(
      (data: any) => {
        this.movimentacoes = data || [];
      },
      (error) => console.error('Erro ao carregar movimentações:', error)
    );
  }

  calcularComissoesPorVendedor() {
    const mapa = new Map();
    
    this.vendas.forEach(venda => {
      if (!mapa.has(venda.vendedor)) {
        mapa.set(venda.vendedor, {
          vendedor: venda.vendedor,
          vendas: [],
          totalVendas: 0,
          totalComissao: 0
        });
      }
      
      const seller = mapa.get(venda.vendedor);
      seller.vendas.push(venda);
      seller.totalVendas += venda.totalVendas;
      seller.totalComissao += venda.totalComissao;
    });
    
    this.comissoesPorVendedor = Array.from(mapa.values());
  }

  adicionarMovimentacao() {
    if (!this.novaMovimentacao.codigoProduto || !this.novaMovimentacao.tipo || this.novaMovimentacao.quantidade <= 0) {
      alert('Preencha todos os campos corretamente!');
      return;
    }

    this.apiService.adicionarMovimentacao({
      codigoProduto: parseInt(this.novaMovimentacao.codigoProduto),
      tipo: this.novaMovimentacao.tipo,
      quantidade: this.novaMovimentacao.quantidade,
      descricao: this.novaMovimentacao.descricao
    }).subscribe(
      (response: any) => {
        alert('Movimentação registrada com sucesso!');
        this.novaMovimentacao = { codigoProduto: '', tipo: '', quantidade: 0, descricao: '' };
        this.carregarDados();
      },
      (error) => {
        console.error('Erro ao registrar movimentação:', error);
        alert('Erro ao registrar movimentação');
      }
    );
  }

  calcularJuros() {
    if (this.simulacaoJuros.valor <= 0 || !this.simulacaoJuros.dataVencimento) {
      alert('Preencha o valor e a data de vencimento!');
      return;
    }

    this.apiService.calcularJuros(this.simulacaoJuros
    ).subscribe({
      next: (res) => {
        this.resultadoJuros = res;
        console.log("Resultado juros: ", res);
      },
      error: (err) => console.error(err)
      }
    );
  }
}
