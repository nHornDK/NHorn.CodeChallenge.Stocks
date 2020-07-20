import { StockDto } from '../../models/dto/StockDto';
import {
  ActionType,
  AllActionResults,
  ActionResult,
} from '../../infrastructure/redux-extensions';
import { Reducer } from 'redux';
export interface StockReduxState {
  stocks: StockDto[];
  clientPing: number | undefined;
  serverDate: Date | undefined;
  clientDate: Date | undefined;
}

const intialState: StockReduxState = {
  stocks: [],
  clientPing: undefined,
  serverDate: undefined,
  clientDate: undefined,
};

const stockReducer: Reducer<StockReduxState, AllActionResults> = (
  state: StockReduxState = intialState,
  action: AllActionResults
) => {
  console.log(action);
  switch (action.type) {
    case ActionType.StockChanged: {
      return ((a: ActionResult<StockDto>): StockReduxState => {
        if (!a.payload) return state;
        const { data } = a.payload;
        return {
          ...state,
          stocks: state.stocks.map((s) => {
            if (s.id === data.id) {
              return data;
            }
            return s;
          }),
        };
      })(action as ActionResult<StockDto>);
    }
    case ActionType.AllStocks: {
      return ((a: ActionResult<StockDto[]>): StockReduxState => {
        if (!a.payload) return state;
        return {
          ...state,
          stocks: a.payload.data,
        };
      })(action as ActionResult<StockDto[]>);
    }
    case ActionType.PingPong: {
      return ((a: ActionResult<Date>): StockReduxState => {
        if (!a.payload) return state;
        const data = a.payload.data;

        if (data !== undefined) {
          const clientDate = new Date();
          const serverDate = new Date(data);
          return {
            ...state,
            clientPing: Math.round(
              (clientDate.getTime() - serverDate.getTime()) / 2
            ),
            clientDate,
            serverDate,
          };
        }
        return state;
      })(action as ActionResult<Date>);
    }
    default:
      return state;
  }
};

export default stockReducer;
