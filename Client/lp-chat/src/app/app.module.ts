import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ChatComponent } from './chat/chat.component';
import { MessageListComponent } from './chat/message-list/message-list.component';
import { MessageEditorComponent } from './chat/message-editor/message-editor.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MessageCardComponent } from './chat/message-card/message-card.component';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { from } from 'rxjs';

@NgModule({
  declarations: [
    AppComponent,
    ChatComponent,
    MessageListComponent,
    MessageEditorComponent,
    MessageCardComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
