import { Component, OnInit } from '@angular/core';
import { ClientsService } from '../../services/clients.service';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css']
})
export class ClientsComponent implements OnInit {

  clients: any[] = [];
  clientSalesDataMonth: { [key: string]: { amount: number; month: string }[] } = {};
  allClients: any[] = [];
  sortDirection: boolean = true;
  chart: any;
  monthlyChart: any;
  productChart: any;
  selectedClient: any = null; // Variável para armazenar o cliente selecionado

  constructor(private clientRest: ClientsService) { }

  ngOnInit(): void {
    this.clientRest.getAllClientsValues().subscribe(
      (data: any) => {
        this.clients = data;
      },
      (error) => {
        console.error('Error fetching clients:', error);
      }
    );
    this.clientRest.getAllMonths().subscribe(
      (data: any) => {
        this.processClientSalesDataMonth(data);
        this.allClients = Array.from(new Set(data.map((item: any) => item.name)));
        this.selectedClient = this.allClients[0];
        this.createMonthlyChart();
      },
      (error) => {
        console.error('Error fetching months:', error);
      }
    );
  }

  sortClients(property: string): void {
    this.clients.sort((a, b) => {
      if (this.sortDirection) {
        return a[property] > b[property] ? 1 : -1;
      } else {
        return a[property] < b[property] ? 1 : -1;
      }
    });
    this.sortDirection = !this.sortDirection;
    this.createMonthlyChart(); // Update the monthly chart when sorting
  }

  getFirstName(name: string): string {
    return name.split(' ')[0];
  }

  createMonthlyChart(): void {
    if (this.monthlyChart) {
      this.monthlyChart.destroy(); // Destroy the previous chart instance if it exists
    }

    const data = this.getChartData();
    const labels = this.getLabels();

    this.monthlyChart = new Chart('clientsMonthlyChart', {
      type: 'line',
      data: {
        labels: labels,
        datasets: [{
          label: `Vendas de ${this.selectedClient}`,
          data: data,
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
          borderColor: 'rgba(75, 192, 192, 1)',
          borderWidth: 1,
          fill: false
        }]
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true,
          }
        },
        plugins: {
          tooltip: {
            callbacks: {
              label: function (context: any) {
                let label = context.dataset.label || '';
                if (label) {
                  label += ': ';
                }
                if (context.parsed.y !== null) {
                  label += '€' + context.parsed.y;
                }
                return label;
              }
            }
          }
        }
      }
    });
  }


  getChartData(): number[] {
    const salesDataForClient = this.clientSalesDataMonth[this.selectedClient];
    let data: number[] = [];

    if (salesDataForClient) {
      data = salesDataForClient.map((sale: any) => sale.amount);
    }

    return data;
  }

  getLabels(): string[] {
    const salesDataForRegion = this.clientSalesDataMonth[this.selectedClient];
    const monthNames = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];

    if (!salesDataForRegion) {
      return [];
    }
    const monthValues = salesDataForRegion.map((sale: any) => sale.month);
    const monthLabels = monthValues.map((monthValue: number) => monthNames[monthValue - 1]);

    return monthLabels;
  }

  onClientChange(event: any): void {
    if (event.target) {
      const client = event.target.value;
      this.selectedClient = client;
      this.createMonthlyChart();
    }
  }

  processClientSalesDataMonth(data: any[]): void {
    this.clientSalesDataMonth = {};

    data.forEach(item => {
      const name = item.name;
      const salesAmount = item.sales_Amount;
      const month = item.time;

      if (!this.clientSalesDataMonth[name]) {
        this.clientSalesDataMonth[name] = [];
      }

      this.clientSalesDataMonth[name].push({ amount: salesAmount, month: month });
    });
  }


}
