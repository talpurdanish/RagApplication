interface Message {
  type: 'user' | 'ai';
  message: string;
  time: string;
  isError: boolean;
}
