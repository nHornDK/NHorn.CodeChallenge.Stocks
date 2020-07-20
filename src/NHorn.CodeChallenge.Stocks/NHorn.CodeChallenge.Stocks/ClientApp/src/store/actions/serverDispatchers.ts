import {
  AllActionResults,
  ActionType,
  ActionResult,
} from '../../infrastructure/redux-extensions';

const serverDispatchers: ActionResult<AllActionResults>[] = [
  {
    type: ActionType.AllStocks,
  },
  {
    type: ActionType.StockChanged,
  },
];
export default serverDispatchers;
