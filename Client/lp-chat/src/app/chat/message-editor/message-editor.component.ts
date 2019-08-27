import { Component, OnInit } from '@angular/core';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-message-editor',
  templateUrl: './message-editor.component.html',
  styleUrls: ['./message-editor.component.css']
})
export class MessageEditorComponent implements OnInit {

  constructor(private messageService: MessageService) { }

  message: Message = new Message();

  ngOnInit() {
  }

  onSend() {
    this.messageService.sendMessage(this.message)
    .subscribe(() => {
      console.log('good');
      this.message = new Message();
    }, error => {
      console.error('ZHOPA');
    })
  }

}
