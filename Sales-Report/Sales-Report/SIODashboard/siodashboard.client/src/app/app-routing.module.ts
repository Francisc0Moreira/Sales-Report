import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ClientsComponent } from './components/clients/clients.component';
import { DataPageComponent } from './components/data-page/data-page.component';
import { ProductsComponent } from './components/products/products.component';
import { RegionComponent } from './components/region/region.component';

 

const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
  },
  {
    path: "clients",
    component: ClientsComponent,
  },
  {
    path: "calendar",
    component: DataPageComponent,
  },
  {
    path: "products",
    component: ProductsComponent,
  },
  {
    path: "region",
    component: RegionComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
