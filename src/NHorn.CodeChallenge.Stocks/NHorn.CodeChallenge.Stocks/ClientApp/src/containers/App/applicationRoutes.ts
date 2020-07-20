import { RouteProps } from 'react-router';
import Home from '../Home';
import StockTable from '../StockTable';

export interface ApplicationRoute extends RouteProps {
  label: string;
  path: string;
  icon?: React.ReactElement;
}
const applicationRoutes: ApplicationRoute[] = [
  { label: 'Home', path: '/', component: Home, exact: true },
  { label: 'Stocks', path: '/Stocks', component: StockTable, exact: true },
];
export { applicationRoutes };
