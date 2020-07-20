import * as React from 'react';
import './index.scss';
import { connect } from 'react-redux';
import {
  RootReduxState,
  SignalRPromise,
} from '../../infrastructure/redux-extensions';
import * as actions from '../../store/actions';

interface StockTableProps extends RootReduxState {
  pingPong: () => SignalRPromise<Date>;
}
class StockTable extends React.PureComponent<StockTableProps> {
  public render(): JSX.Element {
    const { stock } = this.props;
    return (
      <div className="container">
        <h1>Simple stocks table</h1>
        <table className={'stocks-table'}>
          <thead>
            <tr>
              <th>Symbol</th>
              <th>Ask Price</th>
              <th>Bid Price</th>
            </tr>
          </thead>
          <tbody>
            {stock.stocks.map((s) => (
              <tr key={s.id}>
                <td>{s.symbol}</td>
                <td>{Math.round(s.askPrice * 10) / 10}</td>
                <td>{Math.round(s.bidPrice * 10) / 10}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  }
}

const mapStateToProps = (s: RootReduxState): RootReduxState => s;
export default connect(mapStateToProps, actions)(StockTable);
