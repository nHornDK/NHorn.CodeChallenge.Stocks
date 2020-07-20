import {
  applyMiddleware,
  combineReducers,
  compose,
  createStore,
  Store,
  AnyAction,
} from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import { History } from 'history';
import * as SignalR from '@aspnet/signalr';
import signalRMiddleware from '../infrastructure/middleware/signalRMiddleware';
import { RootReduxState } from '../infrastructure/redux-extensions';
import stockReducer from './reducers/stockReducer';

export default function configureStore(
  history: History,
  initialState?: RootReduxState
): Store<RootReduxState, AnyAction> {
  const connection = new SignalR.HubConnectionBuilder()
    .withUrl('/Hubs/Stock')
    .build();
  const middleware = [
    thunk,
    routerMiddleware(history),
    signalRMiddleware(connection),
  ];

  const reducers = {
    stock: stockReducer,
  };
  const rootReducer = combineReducers({
    ...reducers,
    router: connectRouter(history),
  });

  const store = createStore(
    rootReducer,
    initialState,
    compose(applyMiddleware(...middleware))
  );
  return store;
}
