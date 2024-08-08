import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { TimeService } from '../../services/time.service';
import Chart from 'chart.js/auto';
import { data } from '../../models/data';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-data-page',
  templateUrl: './data-page.component.html',
  styleUrls: ['./data-page.component.css']
})


export class DataPageComponent implements AfterViewInit {
  @ViewChild('myChart') private chartRef!: ElementRef;
  chart: any;

   months: string[] = [];
   salesAmounts: number[] = [];


  constructor(private timeRest: TimeService) { }

  ngAfterViewInit(): void {
    this.fetchDataAndCreateChart();
  }
  

  fetchDataAndCreateChart(): void {
    this.timeRest.getAnualAmount(2023).subscribe(
      (data2: any) => {
        const salesAmount = data2.sales_Amount;
      },
      (error: any) => {
        console.error('Error fetching data', error);
      }
    );
    this.timeRest.GetSalesByMonth().subscribe(
      (data2: any) => {
        data2.forEach((item: { month: string; sales_Amount: number; }) => {
          this.months.push(item.month);
          this.salesAmounts.push(item.sales_Amount);
        });
        this.createChart(this.salesAmounts, this.months);
      },
      (error: any) => {
        console.error('Error fetching data', error);
      }
    );
  }
  selectedLabel: string = 'Months';  // Default label

  handleClick(option: string): void {
    this.selectedLabel = option;  // Update label based on selection
    switch (option) {
      case 'Months':
        this.handleMonthsClick();
        break;
      case 'Quarter':
        this.handleQuarterClick();
        break;
      case 'Semester':
        this.handleSemesterClick();
        break;
      default:
        // Handle default case
        break;
    }
  }


  handleMonthsClick(): void {
    this.createChart(this.salesAmounts, this.months);
  }

  handleQuarterClick(): void {
    this.timeRest.GetSalesByQuarter().subscribe(
      (data2: any) => {
        const quarter: string[] = [];
        const salesAmounts: number[] = [];
        data2.forEach((item: { quarter: string; sales_Amount: number; }) => {
          quarter.push(item.quarter);
          salesAmounts.push(item.sales_Amount);
        });
        console.log(salesAmounts)
        this.createChart(salesAmounts, quarter);
      },
      (error: any) => {
        console.error('Error fetching data', error);
      }
    );
  }

  handleSemesterClick(): void {
    this.timeRest.GetSalesBySemester().subscribe(
      (data2: any) => {
        const semester: number[] = [];
        const salesAmount: number[] = [];
        data2.forEach((item: { semester: number; sales_Amount: number; }) => {
          semester.push(item.semester);
          salesAmount.push(item.sales_Amount);
        });
        this.createChart(salesAmount, semester);
      },
      (error: any) => {
        console.error('Error fetching data', error);
      }
    )
  }
 

  createChart(salesAmount: number[], labels: any[]): void {
    const ctx = this.chartRef.nativeElement.getContext('2d');
    console.log(salesAmount)
    if (this.chart) {
      this.chart.destroy();
    }
    this.chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: labels, 
        datasets: [{
          label: 'Sales Amount',
          data: salesAmount, 
          backgroundColor: [
            'rgba(54, 162, 235, 0.2)',        
          ],
          borderColor: [
            'rgba(54, 162, 235, 1)',
          ],
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: true, // Allows chart to take full width/height of container
        aspectRatio: 3, // Customize the ratio
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
         
    });
  }
  
}

