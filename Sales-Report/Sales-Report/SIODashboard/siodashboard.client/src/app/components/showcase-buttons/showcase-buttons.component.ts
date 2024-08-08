import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from '../../models/data';
import { TimeService } from '../../services/time.service';
import { ClientsService } from '../../services/clients.service'

@Component({
  selector: 'app-showcase-buttons',
  templateUrl: './showcase-buttons.component.html',
  styleUrls: ['./showcase-buttons.component.css']
})
export class ShowcaseButtonsComponent implements OnInit {

  anualTotalSale?: data
  year?: number = 2023
  clientsCount?: number

  constructor(private route: ActivatedRoute, private timeRest: TimeService, private clientRest: ClientsService,private router: Router,) { }

  ngOnInit(): void {
    this.timeRest.getAnualAmount(this.year).subscribe(
      (data: data) => {
        this.anualTotalSale = data;
        console.log(this.anualTotalSale);
      },
      (error) => {
        console.error('Error fetching annual amount:', error);
      }
    );
    this.clientRest.getClientsCount().subscribe(
      (data: number) => {
        this.clientsCount = data;
      },
      (error) => {
        console.error('Error fetching clients count:', error);
      }
    );

  }

  formatCurrency(): string {
    return this.anualTotalSale?.sales_Amount.toLocaleString('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + ' €';
  }


}
