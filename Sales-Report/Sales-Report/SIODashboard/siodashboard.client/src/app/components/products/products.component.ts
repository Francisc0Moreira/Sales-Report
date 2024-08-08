import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, ChartConfiguration, ChartType } from 'chart.js';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements AfterViewInit {

  constructor(private route: ActivatedRoute, private prodRest: ProductService, private router: Router) { }

  @ViewChild('mychart') private chartRef!: ElementRef;
  chart: any;
  mostSoldProductName: string = '';
  mostSoldProductAmount: number = 0;
  productCityData: any[] = [];
  productList: string[] = []; // Lista de produtos

  ngAfterViewInit(): void {
    this.prodRest.getProductsByCity().subscribe(
      (data) => {
        console.log('Success:', JSON.stringify(data, null, 2)); // Log detalhado da estrutura dos dados recebidos
        this.productCityData = data;
        this.populateProductList(); // Popula a lista de produtos
        this.createChart(this.productList[0]); // Inicialmente, exibir dados para o primeiro produto
      },
      (error) => {
        console.error('Error:', error); // Log de erro
      }
    );
  }

  private populateProductList(): void {
    const productSet = new Set(this.productCityData.map(item => item.id));
    this.productList = Array.from(productSet);
    console.log('Product List:', this.productList); // Log para verificar a lista de produtos
  }

  private createChart(selectedProduct: string): void {
    const productData = this.getProductData(selectedProduct);
    console.log('Product Data:', productData); // Log para verificar os dados do produto

    if (!productData.cities.length || !productData.sales.length) {
      console.warn('No data available for the selected product');
      return;
    }

    const data = {
      labels: productData.cities,
      datasets: [{
        label: 'Sales Amount (€)',
        data: productData.sales,
        backgroundColor: ['rgba(75, 192, 192, 0.2)'],
        borderColor: ['rgba(75, 192, 192, 1)'],
        borderWidth: 1
      }]
    };

    const config: ChartConfiguration = {
      type: 'bar' as ChartType,
      data,
      options: {
        scales: {
          x: {
            ticks: {
              align: 'center'
            }
          },
          y: {
            beginAtZero: true,
            ticks: {
              callback: function (value) {
                return '€' + value;
              }
            }
          }
        }
      }
    };

    if (this.chart) {
      this.chart.destroy();
    }

    this.chart = new Chart(this.chartRef.nativeElement, config);

    // Determina o produto mais vendido globalmente e atualiza os dados da card
    const maxUnits = this.getMaxUnitsSold();
    this.mostSoldProductName = this.getProductName(maxUnits.productIndex);
    this.mostSoldProductAmount = maxUnits.unitsSold;
  }

  onProductSelect(productId: string): void {
    this.createChart(productId);
  }

  getProductData(productId: string): { cities: string[], sales: number[] } {
    const filteredData = this.productCityData.filter(item => item.id === productId);
    console.log('Filtered Data for', productId, ':', filteredData); // Log detalhado dos dados filtrados

    const cities = filteredData.map(item => item.city);
    const sales = filteredData.map(item => item.sales_Amount || 0);

    return { cities, sales };
  }

  getProductName(productId: string): string {
    return productId; // Usando o ID do produto como nome
  }

  getMaxUnitsSold(): { productIndex: string, unitsSold: number } {
    let maxUnits = 0;
    let maxId = '';

    for (const product of this.productCityData) {
      const productData = this.getProductData(product.id).sales;
      const totalUnits = productData.reduce((acc, curr) => acc + curr, 0);
      if (totalUnits > maxUnits) {
        maxUnits = totalUnits;
        maxId = product.id;
      }
    }

    return { productIndex: maxId, unitsSold: maxUnits };
  }
}
