import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, ChartConfiguration, ChartType } from 'chart.js';
import { ActivatedRoute, Router } from '@angular/router';
import { RegionService } from '../../services/region.service';
@Component({
  selector: 'app-region',
  templateUrl: './region.component.html',
  styleUrls: ['./region.component.css']
})


export class RegionComponent implements AfterViewInit {
  @ViewChild('myChart') private chartRef!: ElementRef;
  chart: any;



  selectedRegion: any;
  allRegions: any[] = [];
  cityYear: any[] = [];
  salesCityYear: any[] = [];


  periods = ['Mês', 'Trimestre', 'Semestre', 'Ano'];
  selectedPeriod = 'Ano';

  totalSales: { [key: string]: number } = {};

  salesDataMonth: { [key: string]: { amount: number; month: string }[] } = {};

  salesDataQuarter: { [key: string]: number[] } = {};

  salesDatasmester: { [key: string]: number[] } = {};

  constructor(private route: ActivatedRoute, private regRest: RegionService, private router: Router) { }

  ngAfterViewInit(): void {
    this.regRest.GetCountrysYear().subscribe(
      (data: any[]) => {
        data.forEach((item: { country: string, sales_Amount: number }) => {
          const country = item.country;
          const salesAmount = item.sales_Amount;
          this.totalSales[country] = salesAmount;
        });
      },
      (error) => {
        console.error('Error:', error); // Log de erro
      }
    );
    this.regRest.GetLocalsByYear().subscribe(
      (data) => {
        console.log('Success:', data); // Log detalhado da estrutura dos dados recebidos
        this.cityYear = data.map((item: any) => item.city);
        this.allRegions = data.map((item: any) => item.city);
        this.selectedRegion = this.allRegions[0];
        this.salesCityYear = data.map((item: any) => item.sales_Amount);
        this.createChart();
      },
      (error) => {
        console.error('Error:', error); // Log de erro
      }
    );
    this.regRest.GetLocalsByMonth().subscribe(
      (data) => {
        this.processSalesDataMonth(data);
      },
      (error) => {
        console.error('Error:', error); // Log de erro
      }
    );
    this.regRest.GetLocalsByQuarter().subscribe(
      (data) => {
        this.processSalesDataQuarter(data);
      },
      (error) => {
        console.error('Error:', error); // Log de erro
      }
    );
    this.regRest.GetLocalsBySemester().subscribe(
      (data) => {
        this.processSalesDataSemester(data);
      },
      (error) => {
        console.error('Error:', error); // Log de erro
      }
    );

  }

  createChart(): void {
    const ctx = this.chartRef.nativeElement.getContext('2d');
    const data = this.getChartData();
    const labels = this.getLabels();

    const chartType = this.selectedPeriod === 'Ano' ? 'doughnut' : 'bar';
    const backgroundColor = chartType === 'doughnut'
      ? ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40', '#FFCD56', '#4BC0C0']
      : 'rgba(54, 162, 235, 0.2)';
    const borderColor = chartType === 'doughnut'
      ? undefined
      : 'rgba(54, 162, 235, 1)';

    this.chart = new Chart(ctx, {
      type: chartType,
      data: {
        labels: labels,
        datasets: [{
          label: `Faturamento em ${this.selectedRegion}`,
          data: data,
          backgroundColor: backgroundColor,
          borderColor: borderColor,
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        scales: chartType === 'bar' ? {
          y: {
            beginAtZero: true
          }
        } : undefined
      }
    });
  }

  updateChart(): void {
    const data = this.getChartData();
    const labels = this.getLabels();
    const chartType = this.selectedPeriod === 'Ano' ? 'doughnut' : 'bar';
    const backgroundColor = chartType === 'doughnut'
      ? ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40', '#FFCD56', '#4BC0C0']
      : 'rgba(54, 162, 235, 0.2)';
    const borderColor = chartType === 'doughnut'
      ? undefined
      : 'rgba(54, 162, 235, 1)';



    this.chart.destroy();
    this.chart = new Chart(this.chartRef.nativeElement.getContext('2d'), {
      type: chartType,
      data: {
        labels: labels,
        datasets: [{
          label: `Faturamento em ${this.selectedRegion}`,
          data: data,
          backgroundColor: backgroundColor,
          borderColor: borderColor,
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        scales: chartType === 'bar' ? {
          y: {
            beginAtZero: true
          }
        } : undefined
      }
    });
  }

  onRegionChange(event: any): void {
    if (event.target) {
      const region = event.target.value;
      this.selectedRegion = region;
      this.updateChart();
    }
  }

  onPeriodChange(period: string): void {
    this.selectedPeriod = period;
    this.updateChart();
  }

  getChartData(): number[] {
    const city = this.selectedRegion;
    let data: number[] = [];

    if (this.selectedPeriod === 'Ano') {
      data = this.salesCityYear;
    } else if (this.selectedPeriod === 'Semestre') {
      const salesDataForRegion = this.salesDatasmester[this.selectedRegion];
      data.push(...salesDataForRegion);
    } else if (this.selectedPeriod === 'Trimestre') {
      const salesDataForRegion = this.salesDataQuarter[this.selectedRegion];
      data.push(...salesDataForRegion);
    } else if (this.selectedPeriod === 'Mês') {
      const salesDataForRegion = this.salesDataMonth[this.selectedRegion];
      if (salesDataForRegion) {
        salesDataForRegion.forEach(sale => {
          data.push(sale.amount); // Adiciona apenas o valor de venda ao array data
        });
      }
    }

    return data;
  }

  getLabels(): string[] {
    if (this.selectedPeriod === 'Ano') {
      return this.cityYear;
    } else if (this.selectedPeriod === 'Semestre') {
      return ['1º Semestre', '2º Semestre'];
    } else if (this.selectedPeriod === 'Trimestre') {
      return ['1º Trimestre', '2º Trimestre', '3º Trimestre', '4º Trimestre'];
    } else if (this.selectedPeriod === 'Mês') {
      const salesDataForRegion = this.salesDataMonth[this.selectedRegion];
      const monthNames = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
      const monthValues = Object.values(salesDataForRegion).map(sale => Number(sale.month));
      const monthLabels = monthValues.map(monthValue => monthNames[monthValue - 1]); 

      return monthLabels;
    }

    return [];
  }

  processSalesDataSemester(data: any[]): void {
    this.salesDatasmester = {}; 

    data.forEach(item => {
      const city = item.city;
      const salesAmount = item.sales_Amount;

      if (!this.salesDatasmester[city]) {
        this.salesDatasmester[city] = [];
      }
      this.salesDatasmester[city].push(salesAmount);
    });

  }

  processSalesDataQuarter(data: any[]): void {
    this.salesDataQuarter = {};

    data.forEach(item => {
      const city = item.city;
      const salesAmount = item.sales_Amount;

      if (!this.salesDataQuarter[city]) {
        this.salesDataQuarter[city] = [];
      }
      this.salesDataQuarter[city].push(salesAmount);
    });

  }

  processSalesDataMonth(data: any[]): void {
    this.salesDataMonth = {};

    data.forEach(item => {
      const city = item.city;
      const salesAmount = item.sales_Amount;
      const month = item.time; 

      if (!this.salesDataMonth[city]) {
        this.salesDataMonth[city] = [];
      }

      this.salesDataMonth[city].push({ amount: salesAmount, month: month });
    });
  }


  formatCurrency(country: string) {
    if (this.totalSales[country] !== undefined) {
      return this.totalSales[country].toLocaleString('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + ' €';
    }
    return '0,00 €';
  }

}
