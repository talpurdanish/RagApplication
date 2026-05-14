import { Component, Input, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-progress',
  imports: [],
  templateUrl: './progress.html',
  styleUrl: './progress.css',
})
export class Progress {
  @Input('color') Color?: 'red' | 'blue' | 'green' | 'yellow' | 'gray' | 'theme' = 'yellow';
}
