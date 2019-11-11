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

  messageText: string = '';

  ngOnInit() {
  }

  onSend() {
    const message = this.getMessage();
    if (message !== null) {
      this.messageService.sendMessage(message)
        .subscribe(() => {
          this.messageText = '';
        }, error => {
          console.error(error);
        })
    }
  }

  getMessage(): Message {
    if (!this.canSead()) {
      return null;
    }
    const message: Message = {
      text: this.messageText
    };
    return message;
  }

  canSead() {
    if (this.messageText !== null &&
      this.messageText !== undefined &&
      this.messageText.trim().length !== 0) {
      return true;
    }
    else {
      return false;
    }
  }

}
