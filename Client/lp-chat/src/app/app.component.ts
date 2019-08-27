import { Component, OnInit } from '@angular/core';
import { MessageService } from './_services/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'lp-chat';

  constructor(private messageService: MessageService) {}

  ngOnInit() {

  }
}
