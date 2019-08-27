import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { retry, repeat, switchMap, startWith, concatMap, tap } from 'rxjs/operators';
import { Subject, Subscription } from 'rxjs';

@Component({
  selector: 'app-message-list',
  templateUrl: './message-list.component.html',
  styleUrls: ['./message-list.component.css']
})
export class MessageListComponent implements OnInit, OnDestroy, AfterViewChecked {

  @ViewChild('scrollMe') private scrollContainer: ElementRef;

  messages: Message[] = [];
  dateSince: Date = null;
  pollSubs: Subscription;

  constructor(private messageService: MessageService) { }

  ngOnInit() {
    this.pollSubs = this.messageService.messageSubject
      .pipe(startWith([]), 
          concatMap(() => this.messageService.pollMessages(this.dateSince)), 
          tap<Message[]>((messages) => this.messageService.messageSubject.next(messages)))
      .subscribe(messages => {
        this.messages = [...this.messages, ...messages];
        this.dateSince = new Date();
      });

      this.scrollToBottom();
  }

  ngOnDestroy(): void {
    this.pollSubs.unsubscribe();    
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
        this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    } catch(err) { console.log(err) }                 
}
}
