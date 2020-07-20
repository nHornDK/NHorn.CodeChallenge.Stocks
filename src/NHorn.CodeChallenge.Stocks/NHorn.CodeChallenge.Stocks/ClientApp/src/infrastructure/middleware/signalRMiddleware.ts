/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
/* eslint-disable @typescript-eslint/no-explicit-any */
import * as SignalR from '@aspnet/signalr';
import { Middleware } from 'redux';
import serverDispatchers from '../../store/actions/serverDispatchers';
import { AllActionResults, SignalRType, ActionType } from '../redux-extensions';

let serverDispatchersInitialized = false;

const signalRMiddleware = (connection: SignalR.HubConnection): Middleware => (
  api
) => {
  if (!serverDispatchersInitialized) {
    serverDispatchers.forEach((dispatcher) => {
      connection.on(dispatcher.type, (data) => {
        const a: AllActionResults = {
          type: dispatcher.type,
          payload: {
            data,
          },
        };
        api.dispatch(a);
      });
    });
    if (connection.state === SignalR.HubConnectionState.Disconnected) {
      const startCon = (c: SignalR.HubConnection): void => {
        console.info('starting signalR connection');
        connection
          .start()
          .then((): void => {
            console.warn('SignalR connection established');
          })
          .catch(() => {
            if (c.state === SignalR.HubConnectionState.Disconnected) {
              console.warn('lost signalr connection. trying to reconnect');
              setTimeout(() => {
                startCon(c);
              }, 500);
            }
          });
      };
      startCon(connection);
      connection.onclose(() => {
        console.warn('SignalR connection closed');
        startCon(connection);
      });
    }
    serverDispatchersInitialized = true;
  }

  return (next) => (action) => {
    const invoke = (con: SignalR.HubConnection) =>
      con.invoke(action.type.toString(), action.params);
    if (action.signalR === SignalRType.Outgoing) {
      const delayPromise = (c: SignalR.HubConnection): any => {
        if (c.state === SignalR.HubConnectionState.Connected) {
          return invoke(c).then((data) =>
            next({
              type: action.type,
              payload: {
                data,
              },
            })
          );
        }
        setTimeout(() => {
          return delayPromise(c);
        }, 500);
      };
      return delayPromise(connection);
    }
    return next(action);
  };
};

export default signalRMiddleware;
