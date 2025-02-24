import { Component, ViewEncapsulation } from '@angular/core';
import { WeatherForecast, WeatherForecastClient } from 'app/web-api-client';

@Component({
    selector     : 'example',
    templateUrl  : './example.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ExampleComponent
{
    public forecasts: WeatherForecast[] = [];
    
    constructor(private client: WeatherForecastClient) {
        client.get().subscribe(result => {
          this.forecasts = result;
        }, error => console.error(error));
      }
}
