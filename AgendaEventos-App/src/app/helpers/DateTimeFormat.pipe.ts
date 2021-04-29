import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateFormatPipe'
})
export class DateTimeFormatPipe implements PipeTransform {
  transform(date: string): string {
    const dateOut: moment.Moment = moment(date, 'DD/MM/YYYY HH:mm:ss');
    return dateOut.format('DD/MM/YYYY');
  }
}
