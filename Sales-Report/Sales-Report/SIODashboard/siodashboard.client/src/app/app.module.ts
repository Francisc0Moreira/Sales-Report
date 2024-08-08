import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { GraphsComponent } from './components/graphs/graphs.component';
import { ShowcaseButtonsComponent } from './components/showcase-buttons/showcase-buttons.component';
import { ClientsComponent } from './components/clients/clients.component';
import { FormsModule } from '@angular/forms';
import { DataPageComponent } from './components/data-page/data-page.component';
import { FooterComponent } from './components/footer/footer.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { ProductsComponent } from './components/products/products.component';
import { RegionComponent } from './components/region/region.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    GraphsComponent,
    ShowcaseButtonsComponent,   
    ClientsComponent,
    DataPageComponent,
    FooterComponent,
    SidebarComponent,
    ProductsComponent,
    RegionComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
