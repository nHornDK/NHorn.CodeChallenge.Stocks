import {
  ReduxActionCreator,
  ActionType,
  SignalRType,
} from '../../infrastructure/redux-extensions';
interface PingPongAction {
  (): ReduxActionCreator<Date, Date>;
}

const pingPong: PingPongAction = () => async (dispatch) =>
  dispatch({
    type: ActionType.PingPong,
    signalR: SignalRType.Outgoing,
    params: new Date(),
  });
