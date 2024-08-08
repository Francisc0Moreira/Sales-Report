import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Company } from '../../models/company';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  ready: boolean = false;
  company?: Company;

  constructor(private route: ActivatedRoute, private companyRest: CompanyService, private router: Router,) { }

  ngOnInit(): void {
    const readyValue = localStorage.getItem("ready");
    if (readyValue !== null) {
      this.ready = readyValue === 'true';
      this.companyRest.getCompany().subscribe(
        (data) => {
          console.log('Success:', data);
          this.company = data;
        },
        (error) => {
          console.error('Error:', error);
        }
      );
    }
  }

  


}
