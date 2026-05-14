import {
  AfterViewChecked,
  Component,
  computed,
  ElementRef,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { DocumentService } from '../../BussinessLogic/Services/Document.Service';
import { ApiResponse } from '../../BussinessLogic/Models/Generics/ApiResponse';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideBot, lucideSend, lucideUser2, lucideX } from '@ng-icons/lucide';

import { Constants } from '../../BussinessLogic/Helpers/Constants';
import { StorageService } from '../../BussinessLogic/Services/Storage.Service';

interface Message {
  type: 'user' | 'ai';
  message: string;
  time: string;
  isError: boolean;
}

@Component({
  selector: 'app-document',
  imports: [NgIcon],
  providers: [DocumentService, NgIcon],
  viewProviders: [provideIcons({ lucideSend, lucideX, lucideUser2, lucideBot })],
  templateUrl: './document.chat.html',
  styleUrl: './document.chat.css',
})
export class DocumentChatComponent implements OnInit, AfterViewChecked {
  @ViewChild('chatWindow') chatWindow!: ElementRef<HTMLDivElement>;
  searchText = signal<string>('');

  previousMessages = signal<string[]>([]);
  messages = signal<Message[]>([]);

  time = signal(0.0);
  isRunning = signal(false);
  private intervalId: any = null;
  displayTime = computed(() => this.time().toFixed(1));

  start() {
    if (this.isRunning()) return;
    this.time.set(0.0);
    this.isRunning.set(true);

    // Run every 100ms for 0.1s increments
    this.intervalId = setInterval(() => {
      this.time.update((t) => t + 0.1);
    }, 100);
  }

  stop() {
    this.isRunning.set(false);
    if (this.intervalId) {
      clearInterval(this.intervalId);
      this.intervalId = null;
    }
  }

  constructor(
    private documentService: DocumentService,
    private storageService: StorageService,
  ) {}

  ngOnInit(): void {
    const pms = this.storageService.get<string[]>(Constants.MESSAGES_STORAGE_KEY);
    if (pms != null) this.previousMessages.set(pms);

    const chat = this.storageService.get<Message[]>(Constants.CHAT_STORAGE_KEY);
    if (chat != null) this.messages.set(chat);
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  scrollToBottom() {
    if (this.chatWindow) {
      this.chatWindow.nativeElement.scrollTop = this.chatWindow.nativeElement.scrollHeight;
    }
  }

  reset() {
    this.storageService.remove(Constants.CHAT_STORAGE_KEY);
    this.storageService.remove(Constants.MESSAGES_STORAGE_KEY);
    this.previousMessages.set([]);
    this.messages.set([]);
  }

  search() {
    if (this.searchText() != '') {
      this.start();
      this.documentService.SearchDocument(this.searchText(), this.previousMessages())?.subscribe({
        next: (data) => {
          if (data != undefined && data != null) {
            var res: ApiResponse<string> = new ApiResponse<string>(data);
            if (
              res != undefined &&
              res != null &&
              res.isSuccess() &&
              res.result != undefined &&
              res.result != ''
            ) {
              this.updateMessages(this.searchText(), 'user');
              this.updateMessages(res.result?.toString(), 'ai', this.displayTime());
              this.updatePreviousMessages(this.searchText());
            }
            this.searchText.set('');
            this.stop();
          }
        },
        error: (err) => {
          this.updateMessages(this.searchText(), 'user');
          this.updateMessages(
            'Sorry no reponse was recieved, please try again!!',
            'ai',
            this.displayTime(),
            true,
          );

          this.searchText.set('');
          this.stop();
        },
      });
    }
  }

  updatePreviousMessages(message: string) {
    this.previousMessages.update((arr) => {
      if (!arr.includes(message)) {
        return [...arr, message];
      }
      return arr;
    });
    this.storageService.set<string[]>(Constants.MESSAGES_STORAGE_KEY, this.previousMessages());
  }

  updateMessages(
    message: string,
    type: 'user' | 'ai',
    time: string = '',
    isError: boolean = false,
  ) {
    this.messages.update((arr) => {
      var m = { type: type, message: message, time: time, isError: isError };
      return [...arr, m];
    });
    this.storageService.set<Message[]>(Constants.CHAT_STORAGE_KEY, this.messages());
  }
}
