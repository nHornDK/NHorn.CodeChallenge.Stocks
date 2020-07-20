import { Action as ReduxAction, AnyAction } from 'redux';
// import { QualityChangedModel } from '../models/QualityChangedModel';
// import { QCTestModel } from '../models/QCTestModel';
// import { ClientStateModel } from '../models/ClientStateModel';
import { ThunkAction, ThunkDispatch } from 'redux-thunk';
// import { qualityControlState } from '../store/reducers/qualityControl';
// import { OpcFishDataModel } from '../models/OpcFishDataModel';
// import { DeviceModel } from '../models/DeviceModel';
// import { SignalRGroupModel } from '../models/SignalRGroupModel';
import { RouterRootState } from 'connected-react-router';
import { StockDto } from '../models/dto/StockDto';
import { StockReduxState } from '../store/reducers/stockReducer';

export type SignalRPromise<T> = Promise<PayloadResult<T>>;

export interface PayloadResult<TPayload> extends ReduxAction<ActionType> {
  payload?: {
    data: TPayload;
  };
}
// eslint-disable-next-line @typescript-eslint/ban-types
export interface ActionResult<TPayload, TParams = {}>
  extends PayloadResult<TPayload> {
  params?: TParams;
  error?: string;
  signalR?: SignalRType;
}

export enum SignalRType {
  Outgoing,
  Incoming,
}

export enum ActionType {
  StockChanged = 'StockChanged',
  AllStocks = 'AllStocks',
  PingPong = 'PingPong',
}

export interface RootReduxState extends RouterRootState {
  stock: StockReduxState;
}
// eslint-disable-next-line @typescript-eslint/ban-types
export interface ReduxActionCreator<TPayload, TParams = {}> {
  (
    dispatch: ThunkDispatchType<ActionResult<TPayload, TParams>>,
    getState: () => RootReduxState
  ): Promise<ActionResult<TPayload>>;
}

export type AllActionResults =
  | ActionResult<StockDto>
  | ActionResult<StockDto[]>
  | ActionResult<Date>;

export type RootActions = AllActionResults;

// eslint-disable-next-line @typescript-eslint/ban-types
export type ThunkResultType<T> = ThunkAction<T, RootReduxState, {}, AnyAction>;
// eslint-disable-next-line @typescript-eslint/ban-types
export type ThunkDispatchType<T> = ThunkDispatch<T, {}, AnyAction>;
