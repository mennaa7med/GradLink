import * as signalR from '@microsoft/signalr';

export function createChatConnection(baseUrl) {
  const access = localStorage.getItem('accessToken');
  const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${baseUrl}/hubs/chat`, {
      accessTokenFactory: () => access || ''
    })
    .withAutomaticReconnect()
    .build();
  return connection;
}


