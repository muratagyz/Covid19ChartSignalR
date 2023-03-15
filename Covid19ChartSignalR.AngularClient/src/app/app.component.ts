import { Component, OnInit } from '@angular/core';
import { CovidService } from './Services/covid.service';
import { ChartType } from 'angular-google-charts';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  columnNames = ['Tarih', 'İstanbul', 'Ankara', 'İzmir', 'Konya', 'Antalya'];
  charType:any = 'LineChart';
  options: any = {
    legend: {
      position: 'Bottom',
      title: 'Covid19 Chart',
    },
  };
  title = 'Covid19ChartSignalRClient';
  constructor(public covidService: CovidService) {}
  ngOnInit(): void {
    this.covidService.startConnection();
    this.covidService.startListener();
  }
}
