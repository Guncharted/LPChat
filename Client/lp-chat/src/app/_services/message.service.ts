import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable, Subject } from 'rxjs';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  chatApiEp = environment.apiUrl + '/chat';

  messageSubject: Subject<Message[]> = new Subject();

  constructor(private http: HttpClient) { }

  sendMessage(message: Message) {
    return this.http.post<Message>(this.chatApiEp + '/add', message);
  }

  pollMessages(dateTime: Date): Observable<Message[]> {
    let pollUrlPart = '/poll'

    if (dateTime) {
      pollUrlPart = pollUrlPart + '?since=' + dateTime.toISOString();
    }
    return this.http.get<Message[]>(this.chatApiEp + pollUrlPart);
  }
}
