import { Component, computed, ElementRef, signal, ViewChild } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideSend, lucideX, lucideUser2, lucideBot } from '@ng-icons/lucide';
import {
  AgentService,
  createTask,
  TaskResponse,
} from '../../BussinessLogic/Services/Agent.Service';
import { ApiResponse } from '../../BussinessLogic/Models/Generics/ApiResponse';
import { StorageService } from '../../BussinessLogic/Services/Storage.Service';
import { Constants } from '../../BussinessLogic/Helpers/Constants';

@Component({
  selector: 'app-agents',
  imports: [NgIcon],
  providers: [AgentService, NgIcon],
  viewProviders: [provideIcons({ lucideSend, lucideX, lucideUser2, lucideBot })],
  templateUrl: './agents.html',
  styleUrl: './agents.css',
})
export class AgentsComponent {
  @ViewChild('chatWindow') chatWindow!: ElementRef<HTMLDivElement>;
  searchText = signal<string>('');
  sessionId = signal<string>('');

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
    private agentService: AgentService,
    private storageService: StorageService,
  ) {}

  ngOnInit(): void {
    var sId = this.storageService.get<string>(Constants.SESSIONID_STORAGE_KEY);
    if (sId != undefined && sId != '') {
      this.sessionId.set(sId);
    } else {
      this.sessionId.set(crypto.randomUUID());
      this.storageService.set(Constants.SESSIONID_STORAGE_KEY, this.sessionId());
    }
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
    this.messages.set([]);
  }

  search() {
    if (this.searchText() != '') {
      this.start();
      this.agentService.Run(this.searchText(), this.sessionId())?.subscribe({
        next: (data) => {
          if (data != undefined && data != null) {
            var res: ApiResponse<TaskResponse> = new ApiResponse<TaskResponse>(data);
            if (
              res != undefined &&
              res != null &&
              res.isSuccess() &&
              res.result != undefined &&
              res.result != null
            ) {
              if (!res.result.message.includes('TooManyRequests')) {
                const task = createTask(res.result);

                var list =
                  task.data.length > 0
                    ? task.data.map((t) => `${t.id}. ${t.name} [${t.description}]`)
                    : '';

                this.updateMessages(this.searchText(), 'user');
                this.updateMessages(`${task.message} ${list}`, 'ai', this.displayTime());
              }
              this.searchText.set('');
            } else {
              this.updateMessages(this.searchText(), 'user');
              this.updateMessages(
                'Too Many Request, please Try Again',
                'ai',
                this.displayTime(),
                true,
              );
            }
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
  }
}
